using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Setting.GetAllActive;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.GetByCompanyIdForUpdate
{
    public sealed record GetAsaasSettingByCompanyIdForUpdateQuery(
        long CompanyId) : IQuery<AsaasIntegrationSettingResponse>;
}
