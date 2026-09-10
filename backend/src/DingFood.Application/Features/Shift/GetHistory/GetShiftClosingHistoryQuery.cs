using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Shift.GetHistory;

public sealed record GetShiftClosingHistoryQuery(
    long BranchId,
    int ReferenceYear,
    int ReferenceMonth) : IQuery<IReadOnlyCollection<ShiftClosingResponse>>;
