using System.Text.Json.Serialization;

namespace DingFood.Application.Abstractions.Integrations.Cnpja;

// Contrato da API pública da CNPJá: GET https://open.cnpja.com/office/{cnpj}
// Documentação: https://cnpja.com/api/open
//
// Observação deliberada: o campo "company.id" NÃO é mapeado. A CNPJá o devolve ora como string
// ("14486046") ora como número (7526557) conforme o endpoint/exemplo, e desserializar os dois
// formatos exigiria um converter só para um dado que é a raiz do próprio CNPJ — que já temos.
// O System.Text.Json ignora propriedades desconhecidas por padrão, e o JSON íntegro fica salvo
// em CnpjQuery.RawJson de qualquer forma.

public sealed record CnpjaOfficeResponse
{
    [JsonPropertyName("updated")]
    public DateTime? Updated { get; init; }

    [JsonPropertyName("taxId")]
    public string TaxId { get; init; } = string.Empty;

    [JsonPropertyName("alias")]
    public string? Alias { get; init; }

    [JsonPropertyName("founded")]
    public DateOnly? Founded { get; init; }

    [JsonPropertyName("head")]
    public bool Head { get; init; }

    [JsonPropertyName("company")]
    public CnpjaCompany? Company { get; init; }

    [JsonPropertyName("statusDate")]
    public DateOnly? StatusDate { get; init; }

    [JsonPropertyName("status")]
    public CnpjaNamedId? Status { get; init; }

    [JsonPropertyName("reason")]
    public CnpjaNamedId? Reason { get; init; }

    [JsonPropertyName("address")]
    public CnpjaAddress? Address { get; init; }

    [JsonPropertyName("phones")]
    public IReadOnlyList<CnpjaPhone> Phones { get; init; } = [];

    [JsonPropertyName("emails")]
    public IReadOnlyList<CnpjaEmail> Emails { get; init; } = [];

    [JsonPropertyName("mainActivity")]
    public CnpjaActivity? MainActivity { get; init; }

    [JsonPropertyName("sideActivities")]
    public IReadOnlyList<CnpjaActivity> SideActivities { get; init; } = [];

    [JsonPropertyName("suframa")]
    public IReadOnlyList<CnpjaSuframa> Suframa { get; init; } = [];
}

public sealed record CnpjaCompany
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("equity")]
    public decimal? Equity { get; init; }

    [JsonPropertyName("nature")]
    public CnpjaNamedId? Nature { get; init; }

    [JsonPropertyName("size")]
    public CnpjaSize? Size { get; init; }

    [JsonPropertyName("simples")]
    public CnpjaSimplesOption? Simples { get; init; }

    [JsonPropertyName("simei")]
    public CnpjaSimplesOption? Simei { get; init; }

    [JsonPropertyName("members")]
    public IReadOnlyList<CnpjaMember> Members { get; init; } = [];
}

public sealed record CnpjaMember
{
    [JsonPropertyName("since")]
    public DateOnly? Since { get; init; }

    [JsonPropertyName("person")]
    public CnpjaPerson? Person { get; init; }

    [JsonPropertyName("role")]
    public CnpjaNamedId? Role { get; init; }
}

public sealed record CnpjaPerson
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>"NATURAL" (pessoa física) ou "LEGAL" (pessoa jurídica).</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>CPF já mascarado na origem: "***765288**".</summary>
    [JsonPropertyName("taxId")]
    public string? TaxId { get; init; }

    [JsonPropertyName("age")]
    public string? Age { get; init; }

    [JsonPropertyName("country")]
    public CnpjaCountry? Country { get; init; }
}

public sealed record CnpjaAddress
{
    /// <summary>Código IBGE do município.</summary>
    [JsonPropertyName("municipality")]
    public int? Municipality { get; init; }

    [JsonPropertyName("street")]
    public string? Street { get; init; }

    [JsonPropertyName("number")]
    public string? Number { get; init; }

    [JsonPropertyName("district")]
    public string? District { get; init; }

    [JsonPropertyName("city")]
    public string? City { get; init; }

    [JsonPropertyName("state")]
    public string? State { get; init; }

    [JsonPropertyName("details")]
    public string? Details { get; init; }

    [JsonPropertyName("zip")]
    public string? Zip { get; init; }

    [JsonPropertyName("country")]
    public CnpjaCountry? Country { get; init; }
}

public sealed record CnpjaPhone
{
    /// <summary>"LANDLINE" ou "MOBILE".</summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("area")]
    public string? Area { get; init; }

    [JsonPropertyName("number")]
    public string? Number { get; init; }
}

public sealed record CnpjaEmail
{
    /// <summary>"CORPORATE", "PERSONAL", "ACCOUNTING".</summary>
    [JsonPropertyName("ownership")]
    public string? Ownership { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("domain")]
    public string? Domain { get; init; }
}

/// <summary>Par id/texto usado por status, motivo, natureza jurídica e qualificação de sócio.</summary>
public sealed record CnpjaNamedId
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }
}

/// <summary>CNAE: o "id" é o próprio código da atividade (ex.: 6209100).</summary>
public sealed record CnpjaActivity
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }
}

public sealed record CnpjaSize
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("acronym")]
    public string? Acronym { get; init; }

    [JsonPropertyName("text")]
    public string? Text { get; init; }
}

public sealed record CnpjaSimplesOption
{
    [JsonPropertyName("optant")]
    public bool? Optant { get; init; }

    [JsonPropertyName("since")]
    public DateOnly? Since { get; init; }
}

public sealed record CnpjaCountry
{
    [JsonPropertyName("id")]
    public int? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

public sealed record CnpjaSuframa
{
    [JsonPropertyName("number")]
    public string? Number { get; init; }

    [JsonPropertyName("since")]
    public DateOnly? Since { get; init; }

    [JsonPropertyName("approved")]
    public bool? Approved { get; init; }

    [JsonPropertyName("approvalDate")]
    public DateOnly? ApprovalDate { get; init; }

    [JsonPropertyName("status")]
    public CnpjaNamedId? Status { get; init; }

    [JsonPropertyName("incentives")]
    public IReadOnlyList<CnpjaSuframaIncentive> Incentives { get; init; } = [];
}

public sealed record CnpjaSuframaIncentive
{
    [JsonPropertyName("tribute")]
    public string? Tribute { get; init; }

    [JsonPropertyName("benefit")]
    public string? Benefit { get; init; }

    [JsonPropertyName("purpose")]
    public string? Purpose { get; init; }

    [JsonPropertyName("basis")]
    public string? Basis { get; init; }
}

/// <summary>
/// Resposta da consulta junto com o corpo HTTP original, preservado byte a byte para gravação
/// em CnpjQuery.RawJson (reserializar o DTO perderia campos que não mapeamos).
/// </summary>
public sealed record CnpjaOfficeResult(CnpjaOfficeResponse Payload, string RawJson);
