using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActive;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetById
{
    public sealed record GetByIdBranchPaymentMethodSettingQuery(
        long Id) : IQuery<BranchPaymentMethodSettingResponse?>;
}