using FluentValidation;

namespace DingFood.Application.Features.Orders.StartPreparation;

public sealed class StartOrderPreparationCommandValidator : AbstractValidator<StartOrderPreparationCommand>
{
    public StartOrderPreparationCommandValidator()
    {
        RuleFor(x => x.CustomerOrderId).GreaterThan(0);
    }
}
