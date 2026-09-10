using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActive;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetByBranchOrCompanyFallback
{
    public sealed record GetByBranchOrCompanyFallbackBranchPaymentMethodSettingQuery(
        long CompanyId,
        long? BranchId) : IQuery<BranchPaymentMethodSettingResponse?>;
}