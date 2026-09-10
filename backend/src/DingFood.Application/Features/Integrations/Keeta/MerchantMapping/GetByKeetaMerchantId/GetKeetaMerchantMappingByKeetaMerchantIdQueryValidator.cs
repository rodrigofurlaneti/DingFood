using FluentValidation;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetByKeetaMerchantId
{
    public sealed class GetKeetaMerchantMappingByKeetaMerchantIdQueryValidator
        : AbstractValidator<GetKeetaMerchantMappingByKeetaMerchantIdQuery>
    {
        public GetKeetaMerchantMappingByKeetaMerchantIdQueryValidator()
        {
            RuleFor(x => x.KeetaMerchantId).GreaterThan(0)
                .WithMessage("O KeetaMerchantId deve ser maior que zero.");
        }
    }
}
