using FluentValidation;

namespace DingFood.Application.Features.Shift.OpenShift;

public sealed class OpenShiftClosingCommandValidator : AbstractValidator<OpenShiftClosingCommand>
{
    public OpenShiftClosingCommandValidator()
    {
        RuleFor(x => x.BranchId).GreaterThan(0);
        RuleFor(x => x.OpenedByEmployeeId).GreaterThan(0);
    }
}
