using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Catalog.ProductExtras;

public sealed record ProductOptionalExtraResponse(long Id, long ProductId, string OptionalExtraName, int DisplayOrder)
{
    public static ProductOptionalExtraResponse From(ProductOptionalExtra item) => new(item.Id, item.ProductId, item.OptionalExtraName, item.DisplayOrder);
}

