using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Products;

public sealed class CreateIfoodProductCommandValidator : AbstractValidator<CreateIfoodProductCommand>
{
    public CreateIfoodProductCommandValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
