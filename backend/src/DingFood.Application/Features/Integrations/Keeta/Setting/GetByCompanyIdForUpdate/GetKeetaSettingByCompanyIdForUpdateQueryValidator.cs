using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByCompanyIdForUpdate
{
    public sealed class GetKeetaSettingByCompanyIdForUpdateQueryValidator
        : AbstractValidator<GetKeetaSettingByCompanyIdForUpdateQuery>
    {
        public GetKeetaSettingByCompanyIdForUpdateQueryValidator()
        {
            RuleFor(x => x.CompanyId)
                .GreaterThan(0)
                .WithMessage("O identificador da empresa (CompanyId) deve ser maior que zero.");
        }
    }
}
