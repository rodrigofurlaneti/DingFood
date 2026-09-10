using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.ExistsForCompany
{
    public sealed record ExistsAsaasSettingForCompanyQuery(
        long CompanyId) : IQuery<bool>;
}
