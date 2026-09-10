using FluentValidation;

namespace DingFood.Application.Features.Users.Deactivate;

public sealed class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator()
    {
        RuleFor(x => x.AppUserId).GreaterThan(0);
    }
}
