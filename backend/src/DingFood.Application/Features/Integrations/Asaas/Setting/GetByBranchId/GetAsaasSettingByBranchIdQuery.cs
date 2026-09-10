using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Setting.GetAllActive;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.GetByBranchId
{
    public sealed record GetAsaasSettingByBranchIdQuery(
        long CompanyId,
        long BranchId) : IQuery<AsaasIntegrationSettingResponse>;
}
