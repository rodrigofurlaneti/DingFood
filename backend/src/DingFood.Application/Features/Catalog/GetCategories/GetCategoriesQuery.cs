using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.GetCategories;

public sealed record GetCategoriesQuery(long CompanyId) : IQuery<IReadOnlyCollection<CategoryResponse>>;
