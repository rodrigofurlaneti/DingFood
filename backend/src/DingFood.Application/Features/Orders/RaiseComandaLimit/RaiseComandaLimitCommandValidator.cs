using FluentValidation;

namespace DingFood.Application.Features.Orders.RaiseComandaLimit;

public sealed class RaiseComandaLimitCommandValidator : AbstractValidator<RaiseComandaLimitCommand>
{
    public RaiseComandaLimitCommandValidator()
    {
        RuleFor(x => x.CustomerOrderId).GreaterThan(0);
        RuleFor(x => x.NewLimitAmount).GreaterThan(0);
    }
}
