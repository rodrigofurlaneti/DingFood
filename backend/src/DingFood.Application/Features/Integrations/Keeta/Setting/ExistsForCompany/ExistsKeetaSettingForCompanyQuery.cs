using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.ExistsForCompany
{
    public sealed record ExistsKeetaSettingForCompanyQuery(
        long CompanyId) : IQuery<bool>;
}
