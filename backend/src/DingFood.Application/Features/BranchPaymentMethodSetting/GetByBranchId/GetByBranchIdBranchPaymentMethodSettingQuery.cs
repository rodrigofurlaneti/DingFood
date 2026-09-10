using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActive;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetByBranchId
{
    public sealed record GetByBranchIdBranchPaymentMethodSettingQuery(
        long BranchId) : IQuery<BranchPaymentMethodSettingResponse?>;
}