using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByCompanyId
{
    public sealed class GetKeetaSettingByCompanyIdQueryValidator
        : AbstractValidator<GetKeetaSettingByCompanyIdQuery>
    {
        public GetKeetaSettingByCompanyIdQueryValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0)
                .WithMessage("O identificador da empresa (CompanyId) deve ser maior que zero.");
        }
    }
}
