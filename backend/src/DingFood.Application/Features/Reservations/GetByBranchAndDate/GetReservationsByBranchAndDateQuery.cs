using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Reservations.GetByBranchAndDate;

public sealed record GetReservationsByBranchAndDateQuery(long BranchId, DateTime From, DateTime To)
    : IQuery<IReadOnlyCollection<ReservationResponse>>;
