using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Shift.GetById;

public sealed record GetShiftClosingByIdQuery(long ShiftClosingId) : IQuery<ShiftClosingResponse>;
