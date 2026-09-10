using FluentValidation;

namespace DingFood.Application.Features.Finance.DeactivateCost;

public sealed class DeactivateOperatingCostCommandValidator : AbstractValidator<DeactivateOperatingCostCommand>
{
    public DeactivateOperatingCostCommandValidator()
    {
        RuleFor(x => x.OperatingCostId).GreaterThan(0);
    }
}
