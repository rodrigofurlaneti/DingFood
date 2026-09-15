using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace DingFood.E2ETests;

/// <summary>
/// Portão dos testes de cadastro de empresa. Cada execução cria uma empresa, uma filial e um
/// usuário administrador REAIS no destino — por isso o opt-in explícito.
/// </summary>
public sealed class CompanySignupFactAttribute : FactAttribute
{
    public CompanySignupFactAttribute() => this.GateCompanySignup();
}

public sealed class CompanySignupTheoryAttribute : TheoryAttribute
{
    public CompanySignupTheoryAttribute() => this.GateCompanySignup();
}

internal static class CompanySignupGate
{
    /// <summary>Consultas de CNPJ/CEP não criam nada — bastam E2E_RUN=1.</summary>
    internal static void GateReadOnly(this FactAttribute attribute)
    {
        if (Environment.GetEnvironmentVariable("E2E_RUN") != "1")
            attribute.Skip = "Defina E2E_RUN=1 para executar contra o ambiente publicado.";
    }

    internal static void GateReadOnly(this TheoryAttribute attribute)
    {
        if (Environment.GetEnvironmentVariable("E2E_RUN") != "1")
            attribute.Skip = "Defina E2E_RUN=1 para executar contra o ambiente publicado.";
    }

    internal static void GateCompanySignup(this FactAttribute attribute)
    {
        if (Environment.GetEnvironmentVariable("E2E_RUN") != "1" ||
            Environment.GetEnvironmentVariable("E2E_COMPANY_SIGNUP") != "1")
            attribute.Skip = "Defina E2E_RUN=1 e E2E_COMPANY_SIGNUP=1 para cadastrar empresas reais de teste.";
    }

    internal static void GateCompanySignup(this TheoryAttribute attribute)
    {
        if (Environment.GetEnvironmentVariable("E2E_RUN") != "1" ||
            Environment.GetEnvironmentVariable("E2E_COMPANY_SIGNUP") != "1")
            attribute.Skip = "Defina E2E_RUN=1 e E2E_COMPANY_SIGNUP=1 para cadastrar empresas reais de teste.";
    }
}

public sealed class ReadOnlyE2EFactAttribute : FactAttribute
{
    public ReadOnlyE2EFactAttribute() => this.GateReadOnly();
}

public sealed class ReadOnlyE2ETheoryAttribute : TheoryAttribute
{
    public ReadOnlyE2ETheoryAttribute() => this.GateReadOnly();
}

internal sealed class CompanyTestSettings
{
    public Uri BaseUrl { get; } = new(Environment.GetEnvironmentVariable("E2E_BASE_URL") ?? "http://191.234.174.58:80");

    /// <summary>
    /// CNPJ real usado só para exercitar o autopreenchimento (consulta na CNPJá). Nunca é
    /// submetido no cadastro — se já existir, o registro falharia com Company.AlreadyExists.
    /// </summary>
    public string LookupCnpj { get; } = Environment.GetEnvironmentVariable("E2E_CNPJ_LOOKUP") ?? "14486046000177";

    /// <summary>CEP estável dos Correios: Praça da Sé, São Paulo.</summary>
    public const string LookupCep = "01001000";

    public string SequenceFile { get; }

    public CompanyTestSettings()
    {
        if (BaseUrl.Scheme is not ("http" or "https") || !string.IsNullOrEmpty(BaseUrl.UserInfo))
            throw new InvalidOperationException("E2E_BASE_URL deve ser HTTP(S), sem credenciais na URL.");

        var target = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(BaseUrl.GetLeftPart(UriPartial.Authority))))[..16];

        SequenceFile = Environment.GetEnvironmentVariable("E2E_COMPANY_SEQUENCE_FILE") ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "DingFood", "E2E", $"company-sequence-{target}.txt");
    }
}

/// <summary>
/// Dados sintéticos de uma empresa de teste. Não é record: o ToString padrão não pode
/// expor a senha do administrador em log de falha.
/// </summary>
internal sealed class CompanyTestData
{
    public const string ZipCode = "07025030";

    public required long Sequence { get; init; }
    public required string LegalName { get; init; }
    public required string TradeName { get; init; }
    public required string Cnpj { get; init; }
    public required string CompanyPhone { get; init; }
    public required string AdminName { get; init; }
    public required string AdminCpf { get; init; }
    public required string AdminUserName { get; init; }
    public required string AdminEmail { get; init; }
    public required string AdminPassword { get; init; }

    public static CompanyTestData Create(string sequenceFile)
    {
        var number = CustomerTestData.NextNumber(sequenceFile);
        var unique = $"{number}{Guid.NewGuid():N}"[..12];

        return new CompanyTestData
        {
            Sequence = number,
            LegalName = $"Furlaneti Teste E2E {number} LTDA",
            TradeName = $"Bar Teste E2E {number}",
            Cnpj = ValidCnpj(),
            CompanyPhone = "1147889520",
            AdminName = $"Joao Furlaneti Teste {number}",
            AdminCpf = CustomerTestData.ValidCpf(),
            // O schema limita AdminUserName a 100 e o formulário a 20 caracteres.
            AdminUserName = $"e2e.{unique}"[..Math.Min(20, 4 + unique.Length)],
            AdminEmail = $"e2e.company.{number}.{Guid.NewGuid():N}@example.com",
            AdminPassword = $"E2e!{Guid.NewGuid():N}"[..16],
        };
    }

    /// <summary>
    /// CNPJ sintético com dígitos verificadores corretos — número matematicamente válido,
    /// não uma identidade real. A consulta na CNPJá vai devolver 404, o que exercita
    /// justamente o caminho de preenchimento manual do formulário.
    /// </summary>
    internal static string ValidCnpj()
    {
        var digits = Enumerable.Range(0, 8).Select(_ => RandomNumberGenerator.GetInt32(10)).ToList();
        digits.AddRange([0, 0, 0, 1]); // matriz

        int[] first = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] second = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        foreach (var weights in new[] { first, second })
        {
            var sum = weights.Select((weight, index) => digits[index] * weight).Sum();
            var remainder = sum % 11;
            digits.Add(remainder < 2 ? 0 : 11 - remainder);
        }

        return digits.Distinct().Count() == 1 ? ValidCnpj() : string.Concat(digits);
    }

    public string Describe() =>
        $"seq={Sequence}; cnpj={Cnpj}; usuario={AdminUserName}; email={AdminEmail}";
}
