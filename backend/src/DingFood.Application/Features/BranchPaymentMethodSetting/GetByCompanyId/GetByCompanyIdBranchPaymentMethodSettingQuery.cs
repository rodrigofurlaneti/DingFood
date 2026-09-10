using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActive;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetByCompanyId
{
    public sealed record GetByCompanyIdBranchPaymentMethodSettingQuery(
        long CompanyId) : IQuery<BranchPaymentMethodSettingResponse?>;
}