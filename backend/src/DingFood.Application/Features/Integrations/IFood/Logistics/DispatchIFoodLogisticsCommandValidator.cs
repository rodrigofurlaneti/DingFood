using FluentValidation;

namespace DingFood.Application.Features.Integrations.Ifood.Logistics;

public sealed class DispatchIfoodLogisticsCommandValidator : AbstractValidator<DispatchIfoodLogisticsCommand>
{
    public DispatchIfoodLogisticsCommandValidator()
    {
        RuleFor(x => x.IfoodOrderId).GreaterThan(0);
    }
}
