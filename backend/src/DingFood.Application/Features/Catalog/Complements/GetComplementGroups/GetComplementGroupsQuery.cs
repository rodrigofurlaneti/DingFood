using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.Complements.GetComplementGroups;

public sealed record GetComplementGroupsQuery(long CompanyId) : IQuery<IReadOnlyCollection<ComplementGroupResponse>>;
