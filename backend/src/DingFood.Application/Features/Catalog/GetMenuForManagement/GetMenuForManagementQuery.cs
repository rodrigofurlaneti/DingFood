using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.GetMenuForManagement;

public sealed record GetMenuForManagementQuery(long CompanyId) : IQuery<IReadOnlyCollection<ProductManagementResponse>>;
