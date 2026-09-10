using FluentValidation;

namespace DingFood.Application.Features.Orders.Close;

public sealed class CloseOrderCommandValidator : AbstractValidator<CloseOrderCommand>
{
    public CloseOrderCommandValidator()
    {
        RuleFor(x => x.CustomerOrderId).GreaterThan(0);
        RuleFor(x => x.ServiceFeeRate).InclusiveBetween(0, 1);
    }
}
