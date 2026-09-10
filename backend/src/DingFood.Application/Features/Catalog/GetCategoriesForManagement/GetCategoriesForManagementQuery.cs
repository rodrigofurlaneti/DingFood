using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.GetCategoriesForManagement;

public sealed record GetCategoriesForManagementQuery(long CompanyId) : IQuery<IReadOnlyCollection<CategoryManagementResponse>>;
