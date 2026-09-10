using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Reservations.Cancel;

public sealed record CancelReservationCommand(long ReservationId) : ICommand;
