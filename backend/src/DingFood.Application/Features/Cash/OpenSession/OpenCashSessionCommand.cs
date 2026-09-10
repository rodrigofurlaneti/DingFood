using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Cash.OpenSession;

public sealed record OpenCashSessionCommand(
    long CashRegisterId,
    long OpenedByEmployeeId,
    decimal OpeningAmount) : ICommand<long>;
