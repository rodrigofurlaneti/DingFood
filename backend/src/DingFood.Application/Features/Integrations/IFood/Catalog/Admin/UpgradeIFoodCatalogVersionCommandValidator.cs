using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Admin;

public sealed class UpgradeIfoodCatalogVersionCommandValidator : AbstractValidator<UpgradeIfoodCatalogVersionCommand>
{
    public UpgradeIfoodCatalogVersionCommandValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
    }
}
