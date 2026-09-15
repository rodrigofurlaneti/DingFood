namespace DingFood.Application.Features.Cnpj;

/// <summary>
/// Contrato devolvido pela API do DingFood em GET api/cnpj/{taxId}.
/// Combina as colunas indexadas de CnpjQuery com as coleções extraídas do RawJson.
/// </summary>
public sealed record CnpjResponse(
    string TaxId,
    string TaxIdFormatted,
    string LegalName,
    string? TradeName,
    DateOnly? FoundedOn,
    bool IsHeadOffice,
    int? StatusId,
    string? StatusText,
    DateOnly? StatusDate,
    string? ReasonText,
    string? NatureText,
    string? SizeAcronym,
    string? SizeText,
    decimal? Equity,
    bool? SimplesOptant,
    DateOnly? SimplesSince,
    bool? SimeiOptant,
    DateOnly? SimeiSince,
    CnpjActivityResponse? MainActivity,
    IReadOnlyCollection<CnpjActivityResponse> SideActivities,
    IReadOnlyCollection<CnpjMemberResponse> Members,
    CnpjAddressResponse? Address,
    IReadOnlyCollection<CnpjPhoneResponse> Phones,
    IReadOnlyCollection<CnpjEmailResponse> Emails,
    DateTime? SourceUpdatedAt,
    DateTime QueriedAt,
    int AgeInDays,
    /// <summary>true quando a resposta veio do banco, sem bater na CNPJá.</summary>
    bool FromCache,
    /// <summary>true quando servimos um dado vencido porque a CNPJá estava indisponível.</summary>
    bool StaleData);

public sealed record CnpjActivityResponse(int? Code, string? Text);

public sealed record CnpjMemberResponse(
    string? Name,
    string? TaxId,
    string? PersonType,
    string? Role,
    DateOnly? Since,
    string? AgeRange,
    string? Country);

public sealed record CnpjAddressResponse(
    string? Street,
    string? Number,
    string? Details,
    string? District,
    string? City,
    string? State,
    string? Zip,
    int? MunicipalityCode);

public sealed record CnpjPhoneResponse(string? Type, string? Area, string? Number);

public sealed record CnpjEmailResponse(string? Ownership, string? Address, string? Domain);
