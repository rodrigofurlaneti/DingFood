using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Admin;

public sealed class DowngradeIfoodCatalogVersionCommandValidator : AbstractValidator<DowngradeIfoodCatalogVersionCommand>
{
    public DowngradeIfoodCatalogVersionCommandValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
    }
}
