using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.GetCategoryById;

public sealed record GetCategoryByIdQuery(long CategoryId) : IQuery<CategoryResponse>;
