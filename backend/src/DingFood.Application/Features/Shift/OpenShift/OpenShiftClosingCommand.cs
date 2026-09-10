using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Shift.OpenShift;

public sealed record OpenShiftClosingCommand(
    long BranchId,
    long OpenedByEmployeeId) : ICommand<long>;
