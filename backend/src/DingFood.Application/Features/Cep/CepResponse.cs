namespace DingFood.Application.Features.Cep;

/// <summary>Contrato devolvido pela API do DingFood em GET api/cep/{cep}.</summary>
public sealed record CepResponse(
    string Cep,
    string CepFormatted,
    string? Street,
    string? Complement,
    string? Unit,
    string? District,
    string? City,
    string? StateAbbreviation,
    string? StateName,
    string? Region,
    int? IbgeCode,
    string? GiaCode,
    string? AreaCode,
    string? SiafiCode,
    DateTime QueriedAt,
    int AgeInDays,
    /// <summary>true quando a resposta veio do banco, sem bater no ViaCEP.</summary>
    bool FromCache,
    /// <summary>true quando o ViaCEP estava indisponível e servimos um dado vencido.</summary>
    bool StaleData);
