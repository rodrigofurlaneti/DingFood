using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.UpdateProduct;

public sealed record UpdateProductCommand(
    long ProductId,
    long CategoryId,
    long UnitOfMeasureId,
    string Name,
    string? Description,
    string? Barcode,
    decimal SalePrice,
    decimal? CostPrice,
    bool IsStockControlled,
    int? PreparationTimeMinutes) : ICommand;
