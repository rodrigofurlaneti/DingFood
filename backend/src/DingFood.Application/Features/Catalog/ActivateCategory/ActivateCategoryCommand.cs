using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.ActivateCategory;

public sealed record ActivateCategoryCommand(long CategoryId) : ICommand;
