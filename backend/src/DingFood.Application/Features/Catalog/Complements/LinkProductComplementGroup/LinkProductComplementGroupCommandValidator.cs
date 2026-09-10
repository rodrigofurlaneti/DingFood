using FluentValidation;

namespace DingFood.Application.Features.Catalog.Complements.LinkProductComplementGroup;

public sealed class LinkProductComplementGroupCommandValidator : AbstractValidator<LinkProductComplementGroupCommand>
{
    public LinkProductComplementGroupCommandValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.ComplementGroupId).GreaterThan(0);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0);
    }
}
