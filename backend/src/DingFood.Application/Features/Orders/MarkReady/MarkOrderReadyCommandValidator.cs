using FluentValidation;

namespace DingFood.Application.Features.Orders.MarkReady;

public sealed class MarkOrderReadyCommandValidator : AbstractValidator<MarkOrderReadyCommand>
{
    public MarkOrderReadyCommandValidator()
    {
        RuleFor(x => x.CustomerOrderId).GreaterThan(0);
    }
}
