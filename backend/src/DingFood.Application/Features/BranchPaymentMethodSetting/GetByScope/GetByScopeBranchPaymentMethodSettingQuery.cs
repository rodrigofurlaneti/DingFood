using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActive;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetByScope
{
    public sealed record GetByScopeBranchPaymentMethodSettingQuery(
        long CompanyId,
        long? BranchId) : IQuery<BranchPaymentMethodSettingResponse?>;
}