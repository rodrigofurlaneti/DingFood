using FluentValidation;

namespace DingFood.Application.Features.Reservations.Cancel;

public sealed class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
{
    public CancelReservationCommandValidator()
    {
        RuleFor(x => x.ReservationId).GreaterThan(0);
    }
}
