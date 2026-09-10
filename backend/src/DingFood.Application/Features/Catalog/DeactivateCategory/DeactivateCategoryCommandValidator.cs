using FluentValidation;

namespace DingFood.Application.Features.Catalog.DeactivateCategory;

public sealed class DeactivateCategoryCommandValidator : AbstractValidator<DeactivateCategoryCommand>
{
    public DeactivateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}
