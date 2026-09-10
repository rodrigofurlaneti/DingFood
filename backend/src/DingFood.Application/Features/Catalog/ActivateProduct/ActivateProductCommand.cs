using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.ActivateProduct;

public sealed record ActivateProductCommand(long ProductId) : ICommand;
