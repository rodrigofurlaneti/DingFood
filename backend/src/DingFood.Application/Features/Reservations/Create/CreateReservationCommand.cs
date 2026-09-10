using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Reservations.Create;

public sealed record CreateReservationCommand(
    long BranchId,
    string CustomerName,
    string? CustomerPhone,
    int PartySize,
    DateTime ReservedFor,
    string? Notes) : ICommand<long>;
