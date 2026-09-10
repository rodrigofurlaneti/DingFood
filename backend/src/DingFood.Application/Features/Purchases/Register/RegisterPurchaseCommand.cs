using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Purchases.Register;

public sealed record PurchaseItemInput(long ProductId, decimal Quantity, decimal UnitCost);

public sealed record RegisterPurchaseCommand(
    long BranchId,
    long SupplierId,
    long EmployeeId,
    string? DocumentNumber,
    DateTime PurchasedAt,
    string? Notes,
    IReadOnlyCollection<PurchaseItemInput> Items) : ICommand<long>;
