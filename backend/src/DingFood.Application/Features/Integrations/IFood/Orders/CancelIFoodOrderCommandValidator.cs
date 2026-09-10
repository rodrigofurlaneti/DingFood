using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

public sealed class CancelIfoodOrderCommandValidator : AbstractValidator<CancelIfoodOrderCommand>
{
    public CancelIfoodOrderCommandValidator()
    {
        RuleFor(x => x.IfoodOrderId).GreaterThan(0);
        RuleFor(x => x.ReasonCode).NotEmpty().MaximumLength(20);
    }
}
