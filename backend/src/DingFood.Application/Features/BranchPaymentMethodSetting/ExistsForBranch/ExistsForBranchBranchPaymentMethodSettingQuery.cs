using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.ExistsForBranch
{
    public sealed record ExistsBranchPaymentMethodSettingForBranchQuery(
        long CompanyId,
        long BranchId) : IQuery<bool>;
}