using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.GetAllActive
{
    public sealed record GetAllActiveAsaasSettingsQuery(
        long CompanyId) : IQuery<IReadOnlyList<AsaasIntegrationSettingResponse>>;
}
