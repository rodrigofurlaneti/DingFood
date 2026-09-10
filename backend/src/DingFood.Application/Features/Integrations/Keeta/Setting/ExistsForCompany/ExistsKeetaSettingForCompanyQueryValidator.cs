using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.ExistsForCompany
{
    public sealed class ExistsKeetaSettingForCompanyQueryValidator
        : AbstractValidator<ExistsKeetaSettingForCompanyQuery>
    {
        public ExistsKeetaSettingForCompanyQueryValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0)
                .WithMessage("O identificador da empresa (CompanyId) deve ser maior que zero.");
        }
    }
}
