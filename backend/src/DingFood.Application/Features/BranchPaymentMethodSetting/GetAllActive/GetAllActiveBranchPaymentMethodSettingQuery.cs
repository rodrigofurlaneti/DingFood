using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActive
{
    public sealed record GetAllActiveBranchPaymentMethodSettingQuery() : IQuery<IReadOnlyList<BranchPaymentMethodSettingResponse>>;
}