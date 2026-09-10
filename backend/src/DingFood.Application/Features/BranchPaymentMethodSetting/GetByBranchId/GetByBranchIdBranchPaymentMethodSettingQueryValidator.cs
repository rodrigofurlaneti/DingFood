using FluentValidation;

namespace DingFood.Application.Features.BranchPaymentMethodSetting.GetByBranchId
{
    public sealed class GetByBranchIdBranchPaymentMethodSettingQueryValidator
        : AbstractValidator<GetByBranchIdBranchPaymentMethodSettingQuery>
    {
        public GetByBranchIdBranchPaymentMethodSettingQueryValidator()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .WithMessage("O identificador da filial (BranchId) deve ser maior que zero.");
        }
    }
}