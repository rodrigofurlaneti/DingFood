using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.DeactivateCategory;

public sealed record DeactivateCategoryCommand(long CategoryId) : ICommand;
