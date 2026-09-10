using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.GetOpenByBranch;

public sealed record GetOpenOrdersByBranchQuery(long BranchId) : IQuery<IReadOnlyCollection<OrderResponse>>;
