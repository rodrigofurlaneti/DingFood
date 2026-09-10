using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Purchases.GetByBranch;

public sealed record GetPurchasesByBranchQuery(long BranchId) : IQuery<IReadOnlyCollection<PurchaseResponse>>;
