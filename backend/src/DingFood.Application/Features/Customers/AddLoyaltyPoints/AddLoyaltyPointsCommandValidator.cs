using FluentValidation;

namespace DingFood.Application.Features.Customers.AddLoyaltyPoints;

public sealed class AddLoyaltyPointsCommandValidator : AbstractValidator<AddLoyaltyPointsCommand>
{
    public AddLoyaltyPointsCommandValidator()
    {
        RuleFor(x => x.CustomerId).GreaterThan(0);
        RuleFor(x => x.Points).GreaterThan(0);
    }
}
