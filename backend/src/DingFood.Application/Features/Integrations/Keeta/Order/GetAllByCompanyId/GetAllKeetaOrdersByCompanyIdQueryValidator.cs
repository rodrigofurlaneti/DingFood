using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.Order.GetAllByCompanyId
{
    public sealed class GetAllKeetaOrdersByCompanyIdQueryValidator
        : AbstractValidator<GetAllKeetaOrdersByCompanyIdQuery>
    {
        public GetAllKeetaOrdersByCompanyIdQueryValidator()
        {
            RuleFor(x => x.CompanyId).GreaterThan(0)
                .WithMessage("O identificador da empresa (CompanyId) deve ser maior que zero.");
        }
    }
}
