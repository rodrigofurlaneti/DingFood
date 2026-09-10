using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

public sealed class RequestIfoodOrderDriverCommandValidator : AbstractValidator<RequestIfoodOrderDriverCommand>
{
    public RequestIfoodOrderDriverCommandValidator()
    {
        RuleFor(x => x.IfoodOrderId).GreaterThan(0);
    }
}
