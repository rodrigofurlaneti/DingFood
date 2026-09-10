using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Catalog.DeactivateProduct;

public sealed record DeactivateProductCommand(long ProductId) : ICommand;
