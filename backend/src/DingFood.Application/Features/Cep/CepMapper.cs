using System.Globalization;
using DingFood.Application.Abstractions.Integrations.ViaCep;
using DingFood.Domain.Constants;
using DingFood.Domain.Entities;

namespace DingFood.Application.Features.Cep;

/// <summary>
/// Conversões entre o contrato do ViaCEP, a entidade persistida e o contrato exposto pela API.
/// </summary>
internal static class CepMapper
{
    /// <summary>Converte a resposta do ViaCEP no snapshot que alimenta Create/Refresh.</summary>
    public static CepQuerySnapshot ToSnapshot(ViaCepAddressResponse source, string rawJson)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new CepQuerySnapshot(
            Street: source.Logradouro,
            Complement: source.Complemento,
            Unit: source.Unidade,
            District: source.Bairro,
            City: source.Localidade,
            StateAbbreviation: source.Uf,
            StateName: source.Estado,
            Region: source.Regiao,
            IbgeCode: ParseCode(source.Ibge),
            GiaCode: source.Gia,
            AreaCode: source.Ddd,
            SiafiCode: source.Siafi,
            RawJson: rawJson);
    }

    public static CepResponse ToResponse(CepQuery entity, bool fromCache, bool staleData)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CepResponse(
            Cep: entity.Cep,
            CepFormatted: CepValidator.Format(entity.Cep),
            Street: entity.Street,
            Complement: entity.Complement,
            Unit: entity.Unit,
            District: entity.District,
            City: entity.City,
            StateAbbreviation: entity.StateAbbreviation,
            StateName: entity.StateName,
            Region: entity.Region,
            IbgeCode: entity.IbgeCode,
            GiaCode: entity.GiaCode,
            AreaCode: entity.AreaCode,
            SiafiCode: entity.SiafiCode,
            QueriedAt: entity.QueriedAt,
            AgeInDays: entity.AgeInDays,
            FromCache: fromCache,
            StaleData: staleData);
    }

    /// <summary>O ViaCEP manda os códigos como texto, e vazio quando não se aplica.</summary>
    private static int? ParseCode(string? value)
        => int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : null;
}
