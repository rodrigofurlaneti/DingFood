using FluentValidation;

namespace DingFood.Application.Features.Cash.ReviewSession;

public sealed class ReviewCashSessionCommandValidator : AbstractValidator<ReviewCashSessionCommand>
{
    public ReviewCashSessionCommandValidator()
    {
        RuleFor(x => x.CashSessionId).GreaterThan(0);
    }
}
