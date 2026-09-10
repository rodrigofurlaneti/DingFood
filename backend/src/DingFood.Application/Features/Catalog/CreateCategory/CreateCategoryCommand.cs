using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.CreateCategory;

public sealed record CreateCategoryCommand(long CompanyId, string Name, int DisplayOrder) : ICommand<long>;
