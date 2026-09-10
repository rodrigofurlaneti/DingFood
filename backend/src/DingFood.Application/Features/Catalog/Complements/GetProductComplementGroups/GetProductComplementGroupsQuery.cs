using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.GetProductComplementGroups;

public sealed record GetProductComplementGroupsQuery(long ProductId) : IQuery<IReadOnlyCollection<ProductComplementGroupResponse>>;
