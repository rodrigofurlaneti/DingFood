using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

public sealed class StartIfoodOrderPreparationCommandValidator : AbstractValidator<StartIfoodOrderPreparationCommand>
{
    public StartIfoodOrderPreparationCommandValidator()
    {
        RuleFor(x => x.IfoodOrderId).GreaterThan(0);
    }
}
