using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Stock.SetLimits;

public sealed record SetStockLimitsCommand(
    long StockItemId,
    decimal MinimumQuantity,
    decimal? MaximumQuantity) : ICommand;
