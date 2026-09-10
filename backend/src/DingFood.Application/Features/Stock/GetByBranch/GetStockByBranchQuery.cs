using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Stock.GetByBranch;

public sealed record GetStockByBranchQuery(long BranchId) : IQuery<IReadOnlyCollection<StockItemResponse>>;
