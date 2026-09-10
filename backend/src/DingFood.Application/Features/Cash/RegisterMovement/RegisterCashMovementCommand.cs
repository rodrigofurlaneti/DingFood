using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Cash.RegisterMovement;

public sealed record RegisterCashMovementCommand(
    long CashSessionId,
    long CashMovementTypeId,
    long EmployeeId,
    decimal Amount,
    string? Description) : ICommand<long>;
