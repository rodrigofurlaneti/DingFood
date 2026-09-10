using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Catalog.ProductExtras;

public sealed record ProductBoostResponse(long Id, long ProductId, string BoostName, decimal IncrementalValue, int DisplayOrder)
{
    public static ProductBoostResponse From(ProductBoost item) => new(item.Id, item.ProductId, item.BoostName, item.IncrementalValue, item.DisplayOrder);
}

