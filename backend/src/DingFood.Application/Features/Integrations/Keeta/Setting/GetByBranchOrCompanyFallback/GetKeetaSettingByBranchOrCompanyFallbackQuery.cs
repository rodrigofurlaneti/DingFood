using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Setting.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByBranchOrCompanyFallback
{
    public sealed record GetKeetaSettingByBranchOrCompanyFallbackQuery(
        long CompanyId,
        long? BranchId) : IQuery<KeetaIntegrationSettingResponse>;
}
