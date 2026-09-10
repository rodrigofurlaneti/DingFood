using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Setting.GetAllActive;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.GetByBranchIdForUpdate
{
    public sealed record GetAsaasSettingByBranchIdForUpdateQuery(
        long CompanyId,
        long BranchId) : IQuery<AsaasIntegrationSettingResponse>;
}
