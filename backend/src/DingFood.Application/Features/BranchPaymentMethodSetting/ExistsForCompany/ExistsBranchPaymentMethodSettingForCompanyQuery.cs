using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.ExistsForCompany
{
    public sealed record ExistsBranchPaymentMethodSettingForCompanyQuery(
        long CompanyId) : IQuery<bool>;
}