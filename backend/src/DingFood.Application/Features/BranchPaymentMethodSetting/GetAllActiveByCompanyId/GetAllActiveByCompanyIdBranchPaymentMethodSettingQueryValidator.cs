using FluentValidation;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetAllActiveByCompanyId
{
    public sealed class GetAllActiveByCompanyIdBranchPaymentMethodSettingQueryValidator
        : AbstractValidator<GetAllActiveByCompanyIdBranchPaymentMethodSettingQuery>
    {
        public GetAllActiveByCompanyIdBranchPaymentMethodSettingQueryValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0)
                .WithMessage("O identificador da empresa (CompanyId) deve ser maior que zero.");
        }
    }
}