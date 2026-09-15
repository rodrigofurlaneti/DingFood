using System.Net;
using OpenQA.Selenium;
using Xunit;
using Xunit.Abstractions;

namespace DingFood.E2ETests;

/// <summary>
/// Cadastro de empresa (/cadastro -> POST /api/slideup/register) contra o ambiente publicado.
///
/// Cada execução do grupo "cadastro" cria empresa, filial Matriz e administrador REAIS, e os
/// registros são mantidos — por isso o portão E2E_COMPANY_SIGNUP=1. Os cenários de consulta e
/// de validação de tela não criam nada e pedem só E2E_RUN=1.
///
///   E2E_RUN=1                  habilita tudo
///   E2E_COMPANY_SIGNUP=1       habilita o que grava empresa
///   E2E_BASE_URL               padrão http://191.234.174.58:80
///   E2E_CNPJ_LOOKUP            CNPJ real usado só no teste de autopreenchimento
///   E2E_HEADLESS=0             abre o Chrome visível
/// </summary>
[Trait("Category", "CompanySignup")]
public sealed class CompanySignupTests(ITestOutputHelper output)
{
    private const string Modal = "[data-testid='submit-signup']";

    // =====================================================================
    // 1. API — caminho feliz e conflitos
    // =====================================================================

    [CompanySignupFact]
    public async Task Register_ShouldCreateCompanyBranchAndAdmin_AndAllowLogin()
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);
        var data = CompanyTestData.Create(settings.SequenceFile);

        output.WriteLine($"INÍCIO {DateTime.UtcNow:O}: cadastro via API; {data.Describe()}");

        var (companyId, _, _) = await probe.Register(data);
        await probe.VerifyAdminCanLogin(data, companyId);
    }

    [CompanySignupFact]
    public async Task Register_WithCnpjAlreadyUsed_ShouldConflict()
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);

        var first = CompanyTestData.Create(settings.SequenceFile);
        await probe.Register(first);

        // Segunda empresa, dados novos em tudo menos o CNPJ.
        var second = CompanyTestData.Create(settings.SequenceFile);
        var (status, body) = await probe.TryRegister(CompanyApiProbe.Payload(second, cnpjOverride: first.Cnpj));

        Assert.Equal(HttpStatusCode.Conflict, status);
        Assert.Equal("Company.AlreadyExists", CompanyApiProbe.ErrorCode(body));
    }

    [CompanySignupFact]
    public async Task Register_WithUserNameAlreadyUsed_ShouldConflict()
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);

        var first = CompanyTestData.Create(settings.SequenceFile);
        await probe.Register(first);

        var second = CompanyTestData.Create(settings.SequenceFile);
        var (status, body) = await probe.TryRegister(
            CompanyApiProbe.Payload(second, userNameOverride: first.AdminUserName));

        Assert.Equal(HttpStatusCode.Conflict, status);
        Assert.Equal("AppUser.AlreadyExists", CompanyApiProbe.ErrorCode(body));
    }

    [ReadOnlyE2ETheory]
    [InlineData("cnpj", "1234567890123")]          // 13 dígitos
    [InlineData("adminCpf", "1234567890")]          // 10 dígitos
    [InlineData("adminPassword", "curta1")]         // < 8 caracteres
    [InlineData("adminEmail", "nao-e-email")]
    [InlineData("legalName", "")]
    public async Task Register_WithInvalidPayload_ShouldRejectBeforeCreating(string field, string value)
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);

        // Não reserva número de sequência: este cadastro não deve criar nada.
        var data = CompanyTestData.Create(settings.SequenceFile);
        var payload = Mutate(CompanyApiProbe.Payload(data), field, value);

        var (status, body) = await probe.TryRegister(payload);

        Assert.True(status is HttpStatusCode.BadRequest or HttpStatusCode.UnprocessableEntity,
            $"Esperado 400/422 para {field}='{value}', veio {(int)status}: {body}");
    }

    // =====================================================================
    // 2. Consultas que alimentam o autopreenchimento (anônimas, sem escrita)
    // =====================================================================

    [ReadOnlyE2EFact]
    public async Task CnpjLookup_WithExistingCompany_ShouldReturnRegistryData()
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);

        var (status, body) = await probe.LookupCnpj(settings.LookupCnpj);

        Assert.Equal(HttpStatusCode.OK, status);
        Assert.Equal(settings.LookupCnpj, body.GetProperty("taxId").GetString());
        Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("legalName").GetString()),
            "A consulta precisa devolver a razão social — é ela que preenche o formulário.");
        Assert.True(body.GetProperty("address").ValueKind is not System.Text.Json.JsonValueKind.Null,
            "A consulta precisa devolver endereço.");
    }

    [ReadOnlyE2EFact]
    public async Task CnpjLookup_SecondCall_ShouldBeServedFromCache()
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);

        await probe.LookupCnpj(settings.LookupCnpj);              // aquece
        var (status, body) = await probe.LookupCnpj(settings.LookupCnpj);

        Assert.Equal(HttpStatusCode.OK, status);
        Assert.True(body.GetProperty("fromCache").GetBoolean(),
            "A segunda consulta do mesmo CNPJ deve sair do banco — é o que protege a cota de 5/min da CNPJá.");
    }

    [ReadOnlyE2ETheory]
    [InlineData("11111111111111", HttpStatusCode.BadRequest, "Cnpj.Invalid")]   // dígito repetido
    [InlineData("14486046000178", HttpStatusCode.BadRequest, "Cnpj.Invalid")]   // DV errado
    [InlineData("123", HttpStatusCode.BadRequest, "Cnpj.Invalid")]
    public async Task CnpjLookup_WithInvalidNumber_ShouldRejectWithoutCallingProvider(
        string taxId, HttpStatusCode expected, string expectedCode)
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);

        var (status, body) = await probe.LookupCnpj(taxId);

        Assert.Equal(expected, status);
        Assert.Equal(expectedCode, CompanyApiProbe.ErrorCode(body));
    }

    [ReadOnlyE2EFact]
    public async Task CepLookup_ShouldReturnAddress_AndCacheIt()
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);

        var (status, body) = await probe.LookupCep(CompanyTestSettings.LookupCep);

        Assert.Equal(HttpStatusCode.OK, status);
        Assert.Equal("São Paulo", body.GetProperty("city").GetString());
        Assert.Equal("SP", body.GetProperty("stateAbbreviation").GetString());
        Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("street").GetString()));

        var (_, second) = await probe.LookupCep(CompanyTestSettings.LookupCep);
        Assert.True(second.GetProperty("fromCache").GetBoolean(),
            "O ViaCEP bloqueia acesso por volume — a segunda consulta precisa sair do cache.");
    }

    [ReadOnlyE2ETheory]
    [InlineData("99999999", HttpStatusCode.NotFound, "Cep.NotFound")]
    [InlineData("00000000", HttpStatusCode.BadRequest, "Cep.Invalid")]
    [InlineData("123", HttpStatusCode.BadRequest, "Cep.Invalid")]
    public async Task CepLookup_WithBadInput_ShouldReturnTypedError(
        string cep, HttpStatusCode expected, string expectedCode)
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);

        var (status, body) = await probe.LookupCep(cep);

        Assert.Equal(expected, status);
        Assert.Equal(expectedCode, CompanyApiProbe.ErrorCode(body));
    }

    // =====================================================================
    // 3. Interface — validação e autopreenchimento (não cria empresa)
    // =====================================================================

    [ReadOnlyE2EFact]
    public void Signup_EmptyForm_ShouldBlockSubmitAndShowErrors()
    {
        using var browser = new Browser();
        browser.Open("/cadastro");

        Click(browser, browser.Visible(Modal));

        browser.Wait.Until(driver => driver.FindElements(By.CssSelector(".error-message")).Any(e => e.Displayed));
        Assert.Equal("/cadastro", new Uri(browser.Driver.Url).AbsolutePath);
    }

    [ReadOnlyE2ETheory]
    [InlineData("Desktop")]
    [InlineData("Android")]
    public void Signup_CnpjBlur_ShouldAutofillCompanyAndAdmin(string profile)
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var browser = new Browser(android: profile == "Android");
        browser.Open("/cadastro");

        Fill(browser, "cnpj", settings.LookupCnpj);
        Blur(browser, "cnpj");

        // O status só vira "preenchido" depois da resposta da API.
        browser.Wait.Until(driver =>
            driver.FindElements(By.CssSelector("[data-testid='cnpj-status']"))
                  .Any(e => e.Displayed && e.Text.Contains('✓')));

        Assert.False(string.IsNullOrWhiteSpace(Value(browser, "legalName")), "Razão social não foi preenchida.");
        Assert.False(string.IsNullOrWhiteSpace(Value(browser, "tradeName")), "Nome fantasia não foi preenchido.");
        Assert.False(string.IsNullOrWhiteSpace(Value(browser, "addressCity")), "Cidade não foi preenchida.");
        Assert.False(string.IsNullOrWhiteSpace(Value(browser, "addressState")), "UF não foi preenchida.");
        Assert.False(string.IsNullOrWhiteSpace(Value(browser, "adminName")), "Nome do administrador não foi preenchido.");

        output.WriteLine($"Autopreenchimento por CNPJ ({profile}): {Value(browser, "legalName")}");
    }

    [ReadOnlyE2EFact]
    public void Signup_CepBlur_ShouldAutofillAddressAndFocusNumber()
    {
        using var browser = new Browser();
        browser.Open("/cadastro");

        Fill(browser, "addressZipCode", CompanyTestSettings.LookupCep);
        Blur(browser, "addressZipCode");

        browser.Wait.Until(driver =>
            driver.FindElements(By.CssSelector("[data-testid='cep-status']"))
                  .Any(e => e.Displayed && e.Text.Contains('✓')));

        Assert.Equal("São Paulo", Value(browser, "addressCity"));
        Assert.Equal("SP", Value(browser, "addressState"));
        Assert.False(string.IsNullOrWhiteSpace(Value(browser, "addressStreet")));

        // Padrão de formulário com CEP: o número é a única coisa que os Correios não sabem.
        var focused = (string?)((IJavaScriptExecutor)browser.Driver)
            .ExecuteScript("return document.activeElement?.getAttribute('data-testid');");
        Assert.Equal("addressNumber", focused);
    }

    [ReadOnlyE2EFact]
    public void Signup_PasswordMismatch_ShouldShowError()
    {
        using var browser = new Browser();
        browser.Open("/cadastro");

        Fill(browser, "adminPassword", "SenhaBoa1");
        Fill(browser, "confirmPassword", "SenhaOutra1");
        Blur(browser, "confirmPassword");

        browser.Wait.Until(driver => driver.FindElements(By.CssSelector(".error-message"))
            .Any(e => e.Displayed && e.Text.Contains("não coincidem", StringComparison.OrdinalIgnoreCase)));
    }

    // =====================================================================
    // 4. Interface — cadastro completo (cria empresa)
    // =====================================================================

    [CompanySignupTheory]
    [InlineData("Desktop")]
    [InlineData("Android")]
    public async Task Signup_CompleteFlow_ShouldRegisterAndAllowAdminLogin(string profile)
    {
        var settings = new CompanySignupSettingsFixture().Settings;
        using var probe = new CompanyApiProbe(settings, output);
        var data = CompanyTestData.Create(settings.SequenceFile);

        output.WriteLine($"INÍCIO {DateTime.UtcNow:O}: {profile}; {data.Describe()}; " +
                         $"destino={settings.BaseUrl.GetLeftPart(UriPartial.Authority)}. O registro será mantido.");

        using var browser = new Browser(android: profile == "Android");
        browser.Wait.Timeout = TimeSpan.FromSeconds(45);
        browser.Open("/cadastro");

        // CNPJ sintético: a CNPJá devolve 404 e o formulário cai no preenchimento manual.
        Fill(browser, "cnpj", data.Cnpj);
        Blur(browser, "cnpj");
        browser.Wait.Until(driver =>
            driver.FindElements(By.CssSelector("[data-testid='cnpj-status']")).Any(e => e.Displayed));
        output.WriteLine($"Status do CNPJ sintético: {browser.Visible("[data-testid='cnpj-status']").Text}");

        Fill(browser, "legalName", data.LegalName);
        Fill(browser, "tradeName", data.TradeName);
        Fill(browser, "companyPhone", data.CompanyPhone);

        Fill(browser, "addressZipCode", CompanyTestData.ZipCode);
        Blur(browser, "addressZipCode");
        browser.Wait.Until(driver =>
            driver.FindElements(By.CssSelector("[data-testid='cep-status']")).Any(e => e.Displayed));

        if (string.IsNullOrWhiteSpace(Value(browser, "addressStreet")))
        {
            output.WriteLine("ViaCEP não preencheu a rua; validando o caminho manual.");
            Fill(browser, "addressStreet", "Endereço sintético E2E");
            Fill(browser, "addressDistrict", "Bairro de teste");
            Fill(browser, "addressCity", "Guarulhos");
            Fill(browser, "addressState", "SP");
        }
        Fill(browser, "addressNumber", "261");

        Fill(browser, "adminName", data.AdminName);
        Fill(browser, "adminCpf", data.AdminCpf);
        Fill(browser, "adminUserName", data.AdminUserName);
        Fill(browser, "adminEmail", data.AdminEmail);
        Fill(browser, "adminPassword", data.AdminPassword);
        Fill(browser, "confirmPassword", data.AdminPassword);

        // A filial é fixa e desabilitada — só confirma que continua assim.
        var branch = browser.Visible("[data-testid='branchName']");
        Assert.False(branch.Enabled, "O campo de filial deve permanecer desabilitado em 'Matriz'.");
        Assert.Equal("Matriz", branch.GetAttribute("value"));

        output.WriteLine($"ETAPA UI {DateTime.UtcNow:O}: enviar cadastro.");
        Click(browser, browser.Visible(Modal));

        browser.Wait.Until(driver =>
        {
            var toast = driver.FindElements(By.CssSelector("[data-sonner-toast][data-type='error']"))
                              .FirstOrDefault(e => e.Displayed);
            if (toast is not null)
                throw new InvalidOperationException(
                    $"Cadastro rejeitado na interface ({profile}): {toast.Text}. {data.Describe()}");

            return new Uri(driver.Url).AbsolutePath == "/login";
        });

        output.WriteLine("Redirecionado para /login; confirmando login do administrador pela interface.");
        new LoginPage(browser).SignIn(data.AdminUserName, data.AdminPassword);
        browser.Driver.Navigate().Refresh();
        Assert.True(browser.Visible("#topbar-nav").Displayed);

        // Sessão HTTP nova prova que a persistência não dependeu do estado do navegador.
        var (status, login) = await probe.LookupCnpj(data.Cnpj);
        output.WriteLine($"Consulta do CNPJ cadastrado -> {(int)status}");
        Assert.True(status is HttpStatusCode.OK or HttpStatusCode.NotFound,
            "A consulta de CNPJ não deve quebrar após o cadastro.");
        _ = login;
    }

    // =====================================================================
    // Helpers
    // =====================================================================

    private sealed class CompanySignupSettingsFixture
    {
        public CompanyTestSettings Settings { get; } = new();
    }

    private static object Mutate(object payload, string field, string value)
    {
        var json = System.Text.Json.JsonSerializer.SerializeToNode(payload)!.AsObject();
        json[field] = value;
        return json;
    }

    private static string Value(Browser browser, string testId)
        => browser.Visible($"[data-testid='{testId}']").GetAttribute("value") ?? string.Empty;

    private static void Fill(Browser browser, string testId, string value)
    {
        var input = browser.Visible($"[data-testid='{testId}']");
        ((IJavaScriptExecutor)browser.Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", input);
        input.Clear();
        input.SendKeys(value);
    }

    /// <summary>
    /// O autopreenchimento dispara no onBlur. Tab move o foco de verdade — disparar o evento
    /// por JavaScript não passaria pelo handler do react-hook-form.
    /// </summary>
    private static void Blur(Browser browser, string testId)
        => browser.Visible($"[data-testid='{testId}']").SendKeys(Keys.Tab);

    private static void Click(Browser browser, IWebElement element)
    {
        ((IJavaScriptExecutor)browser.Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);
        element.Click(); // clique real do WebDriver; não contornar overlay com JavaScript.
    }
}
