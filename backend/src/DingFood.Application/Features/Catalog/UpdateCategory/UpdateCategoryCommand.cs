using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.UpdateCategory;

public sealed record UpdateCategoryCommand(long CategoryId, string Name, int DisplayOrder) : ICommand;
