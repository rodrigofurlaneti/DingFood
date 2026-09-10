using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Finance.SetTarget;

public sealed record SetRevenueTargetCommand(
    long BranchId,
    int ReferenceYear,
    int ReferenceMonth,
    decimal TargetAmount) : ICommand<long>;
