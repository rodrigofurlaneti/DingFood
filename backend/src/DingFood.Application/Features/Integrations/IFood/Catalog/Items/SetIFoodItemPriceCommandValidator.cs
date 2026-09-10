using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Items;

public sealed class SetIfoodItemPriceCommandValidator : AbstractValidator<SetIfoodItemPriceCommand>
{
    public SetIfoodItemPriceCommandValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.Value).GreaterThanOrEqualTo(0);
    }
}
