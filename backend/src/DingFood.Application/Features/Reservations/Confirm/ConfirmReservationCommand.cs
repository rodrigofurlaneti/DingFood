using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Reservations.Confirm;

public sealed record ConfirmReservationCommand(long ReservationId, long DiningTableId) : ICommand;
