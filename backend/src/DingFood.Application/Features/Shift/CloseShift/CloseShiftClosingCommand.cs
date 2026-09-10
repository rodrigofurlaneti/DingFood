using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Shift.CloseShift;

public sealed record CloseShiftClosingCommand(
    long ShiftClosingId,
    long ClosedByEmployeeId,
    string? Notes) : ICommand<ShiftClosingResponse>;
