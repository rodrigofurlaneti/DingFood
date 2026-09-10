using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.GetComplementItems;

public sealed record GetComplementItemsQuery(long CompanyId) : IQuery<IReadOnlyCollection<ComplementItemResponse>>;
