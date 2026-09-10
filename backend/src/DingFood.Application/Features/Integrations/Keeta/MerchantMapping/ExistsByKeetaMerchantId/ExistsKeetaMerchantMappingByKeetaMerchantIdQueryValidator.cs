using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.ExistsByKeetaMerchantId
{
    public sealed class ExistsKeetaMerchantMappingByKeetaMerchantIdQueryValidator
        : AbstractValidator<ExistsKeetaMerchantMappingByKeetaMerchantIdQuery>
    {
        public ExistsKeetaMerchantMappingByKeetaMerchantIdQueryValidator()
        {
            RuleFor(x => x.KeetaMerchantId).GreaterThan(0)
                .WithMessage("O KeetaMerchantId deve ser maior que zero.");
        }
    }
}
