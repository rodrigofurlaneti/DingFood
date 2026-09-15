using System.Text.Json;
using DingFood.Application.Abstractions.Integrations.Cnpja;
using DingFood.Domain.Constants;
using DingFood.Domain.Entities;

namespace DingFood.Application.Features.Cnpj;

/// <summary>
/// Conversões entre o contrato da CNPJá, a entidade persistida e o contrato exposto pela API.
/// </summary>
internal static class CnpjMapper
{
    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>Converte a resposta da CNPJá no snapshot que alimenta Create/Refresh.</summary>
    public static CnpjQuerySnapshot ToSnapshot(CnpjaOfficeResponse source, string rawJson)
    {
        ArgumentNullException.ThrowIfNull(source);

        var company = source.Company;
        var address = source.Address;
        var phone = source.Phones.FirstOrDefault();
        var email = source.Emails.FirstOrDefault();

        return new CnpjQuerySnapshot(
            LegalName: string.IsNullOrWhiteSpace(company?.Name) ? source.TaxId : company!.Name!,
            TradeName: source.Alias,
            FoundedOn: source.Founded,
            IsHeadOffice: source.Head,
            StatusId: source.Status?.Id,
            StatusText: source.Status?.Text,
            StatusDate: source.StatusDate,
            ReasonText: source.Reason?.Text,
            NatureId: company?.Nature?.Id,
            NatureText: company?.Nature?.Text,
            SizeAcronym: company?.Size?.Acronym,
            SizeText: company?.Size?.Text,
            Equity: company?.Equity,
            SimplesOptant: company?.Simples?.Optant,
            SimplesSince: company?.Simples?.Since,
            SimeiOptant: company?.Simei?.Optant,
            SimeiSince: company?.Simei?.Since,
            MainActivityCode: source.MainActivity?.Id,
            MainActivityText: source.MainActivity?.Text,
            AddressStreet: address?.Street,
            AddressNumber: address?.Number,
            AddressDetails: address?.Details,
            AddressDistrict: address?.District,
            AddressCity: address?.City,
            AddressState: address?.State,
            AddressZip: CnpjValidator.Normalize(address?.Zip),
            MunicipalityCode: address?.Municipality,
            PrimaryPhone: BuildPhone(phone),
            PrimaryEmail: email?.Address,
            SourceUpdatedAt: source.Updated,
            RawJson: rawJson);
    }

    /// <summary>
    /// Monta o contrato de saída. Quando <paramref name="payload"/> é nulo (resposta vinda do
    /// cache), o JSON salvo é desserializado para reconstruir sócios, CNAEs e contatos.
    /// Se o RawJson estiver corrompido, devolve os campos colunares e coleções vazias em vez de
    /// derrubar a requisição.
    /// </summary>
    public static CnpjResponse ToResponse(
        CnpjQuery entity,
        CnpjaOfficeResponse? payload,
        bool fromCache,
        bool staleData)
    {
        ArgumentNullException.ThrowIfNull(entity);

        payload ??= TryDeserialize(entity.RawJson);

        var members = payload?.Company?.Members
            .Select(m => new CnpjMemberResponse(
                m.Person?.Name,
                m.Person?.TaxId,
                m.Person?.Type,
                m.Role?.Text,
                m.Since,
                m.Person?.Age,
                m.Person?.Country?.Name))
            .ToList() ?? [];

        var sideActivities = payload?.SideActivities
            .Select(a => new CnpjActivityResponse(a.Id, a.Text))
            .ToList() ?? [];

        var phones = payload?.Phones
            .Select(p => new CnpjPhoneResponse(p.Type, p.Area, p.Number))
            .ToList() ?? [];

        var emails = payload?.Emails
            .Select(e => new CnpjEmailResponse(e.Ownership, e.Address, e.Domain))
            .ToList() ?? [];

        var mainActivity = entity.MainActivityCode is null && entity.MainActivityText is null
            ? null
            : new CnpjActivityResponse(entity.MainActivityCode, entity.MainActivityText);

        var address = new CnpjAddressResponse(
            entity.AddressStreet,
            entity.AddressNumber,
            entity.AddressDetails,
            entity.AddressDistrict,
            entity.AddressCity,
            entity.AddressState,
            entity.AddressZip,
            entity.MunicipalityCode);

        return new CnpjResponse(
            TaxId: entity.TaxId,
            TaxIdFormatted: CnpjValidator.Format(entity.TaxId),
            LegalName: entity.LegalName,
            TradeName: entity.TradeName,
            FoundedOn: entity.FoundedOn,
            IsHeadOffice: entity.IsHeadOffice,
            StatusId: entity.StatusId,
            StatusText: entity.StatusText,
            StatusDate: entity.StatusDate,
            ReasonText: entity.ReasonText,
            NatureText: entity.NatureText,
            SizeAcronym: entity.SizeAcronym,
            SizeText: entity.SizeText,
            Equity: entity.Equity,
            SimplesOptant: entity.SimplesOptant,
            SimplesSince: entity.SimplesSince,
            SimeiOptant: entity.SimeiOptant,
            SimeiSince: entity.SimeiSince,
            MainActivity: mainActivity,
            SideActivities: sideActivities,
            Members: members,
            Address: address,
            Phones: phones,
            Emails: emails,
            SourceUpdatedAt: entity.SourceUpdatedAt,
            QueriedAt: entity.QueriedAt,
            AgeInDays: entity.AgeInDays,
            FromCache: fromCache,
            StaleData: staleData);
    }

    private static CnpjaOfficeResponse? TryDeserialize(string? rawJson)
    {
        if (string.IsNullOrWhiteSpace(rawJson))
            return null;

        try
        {
            return JsonSerializer.Deserialize<CnpjaOfficeResponse>(rawJson, JsonOptions);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string? BuildPhone(CnpjaPhone? phone)
    {
        if (phone is null)
            return null;

        var digits = $"{phone.Area}{phone.Number}";
        return string.IsNullOrWhiteSpace(digits) ? null : CnpjValidator.Normalize(digits);
    }
}
