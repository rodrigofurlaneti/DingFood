using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Stock.GetLedger;

public sealed record GetStockLedgerQuery(long StockItemId) : IQuery<IReadOnlyCollection<StockMovementResponse>>;
