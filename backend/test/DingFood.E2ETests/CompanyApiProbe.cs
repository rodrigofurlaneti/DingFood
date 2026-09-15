using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace DingFood.E2ETests;

/// <summary>
/// Cliente HTTP dos endpoints que a tela de cadastro usa. Serve para duas coisas:
/// exercitar os cenários de API sem navegador (rápido) e confirmar, de uma sessão HTTP nova,
/// que o que a UI fez realmente persistiu.
/// </summary>
internal sealed class CompanyApiProbe(CompanyTestSettings settings, ITestOutputHelper output) : IDisposable
{
    private readonly HttpClient client = new() { BaseAddress = settings.BaseUrl, Timeout = TimeSpan.FromSeconds(60) };

    // ---------- Cadastro ----------

    public async Task<(HttpStatusCode Status, JsonElement Body)> TryRegister(object payload)
        => await Send(HttpMethod.Post, "/api/slideup/register", payload);

    public async Task<(long CompanyId, long BranchId, long AdminUserId)> Register(CompanyTestData data)
    {
        var (status, body) = await TryRegister(Payload(data));
        Assert.True(status == HttpStatusCode.OK,
            $"Cadastro falhou ({(int)status}): {body}. Dados: {data.Describe()}");

        var companyId = body.GetProperty("companyId").GetInt64();
        var branchId = body.GetProperty("branchId").GetInt64();
        var adminUserId = body.GetProperty("adminUserId").GetInt64();

        Assert.True(companyId > 0, "Cadastro não devolveu CompanyId.");
        Assert.True(branchId > 0, "Cadastro não devolveu BranchId — a filial Matriz deve ser criada junto.");
        Assert.True(adminUserId > 0, "Cadastro não devolveu AdminUserId.");

        output.WriteLine($"Empresa criada: companyId={companyId}; branchId={branchId}; {data.Describe()}");
        return (companyId, branchId, adminUserId);
    }

    /// <summary>Monta o corpo exatamente como o signupApi.ts do frontend.</summary>
    public static object Payload(CompanyTestData data, string? cnpjOverride = null, string? userNameOverride = null) => new
    {
        legalName = data.LegalName,
        tradeName = data.TradeName,
        cnpj = cnpjOverride ?? data.Cnpj,
        companyEmail = data.AdminEmail,
        companyPhone = data.CompanyPhone,
        branchName = "Matriz",
        branchCnpj = (string?)null,
        addressStreet = "Rua Brasilio Lombardi",
        addressNumber = "261",
        addressDistrict = "Vila Augusta",
        addressCity = "Guarulhos",
        addressState = "SP",
        addressZipCode = CompanyTestData.ZipCode,
        adminName = data.AdminName,
        adminCpf = data.AdminCpf,
        adminUserName = userNameOverride ?? data.AdminUserName,
        adminEmail = data.AdminEmail,
        adminPassword = data.AdminPassword,
    };

    // ---------- Login do administrador recém-criado ----------

    public async Task VerifyAdminCanLogin(CompanyTestData data, long expectedCompanyId)
    {
        var (status, login) = await Send(HttpMethod.Post, "/api/auth/login", new
        {
            userName = data.AdminUserName,
            password = data.AdminPassword,
        });

        Assert.True(status == HttpStatusCode.OK,
            $"Administrador criado não conseguiu entrar ({(int)status}): {login}. {data.Describe()}");

        Assert.False(string.IsNullOrWhiteSpace(login.GetProperty("accessToken").GetString()),
            "Login não devolveu accessToken.");
        Assert.Equal(expectedCompanyId, login.GetProperty("companyId").GetInt64());
    }

    // ---------- Consultas que alimentam o autopreenchimento ----------

    public async Task<(HttpStatusCode Status, JsonElement Body)> LookupCnpj(string taxId)
        => await Send(HttpMethod.Get, $"/api/cnpj/{taxId}");

    public async Task<(HttpStatusCode Status, JsonElement Body)> LookupCep(string cep)
        => await Send(HttpMethod.Get, $"/api/cep/{cep}");

    // ---------- Infraestrutura ----------

    private async Task<(HttpStatusCode, JsonElement)> Send(HttpMethod method, string path, object? body = null)
    {
        using var request = new HttpRequestMessage(method, path);
        if (body is not null) request.Content = JsonContent.Create(body);

        using var response = await client.SendAsync(request);
        var text = await response.Content.ReadAsStringAsync();

        JsonElement parsed;
        try
        {
            parsed = string.IsNullOrWhiteSpace(text)
                ? default
                : JsonDocument.Parse(text).RootElement.Clone();
        }
        catch (JsonException)
        {
            // Resposta não-JSON (HTML de erro do Nginx, por exemplo) precisa aparecer no log.
            output.WriteLine($"{method} {path} -> {(int)response.StatusCode}; corpo não-JSON: {Truncate(text)}");
            throw;
        }

        output.WriteLine($"{method} {path} -> {(int)response.StatusCode}");
        return (response.StatusCode, parsed);
    }

    /// <summary>Extrai o Error.Code do ProblemDetails (que a API põe em "title").</summary>
    public static string ErrorCode(JsonElement problem)
        => problem.ValueKind == JsonValueKind.Object && problem.TryGetProperty("title", out var title)
            ? title.GetString() ?? string.Empty
            : string.Empty;

    private static string Truncate(string value) => value.Length <= 400 ? value : value[..400] + "…";

    public void Dispose() => client.Dispose();
}
