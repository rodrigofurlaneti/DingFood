using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActive;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActiveByCompanyId
{
    public sealed record GetAllActiveByCompanyIdBranchPaymentMethodSettingQuery(
        long CompanyId) : IQuery<IReadOnlyList<BranchPaymentMethodSettingResponse>>;
}