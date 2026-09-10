using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Setting.GetAllActive;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.GetByBranchOrCompanyFallback
{
    public sealed record GetByBranchOrCompanyFallbackQuery(
        long CompanyId,
        long? BranchId = null) : IQuery<AsaasIntegrationSettingResponse>;
}
