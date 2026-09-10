using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.Cancel;

public sealed record CancelOrderCommand(long CustomerOrderId) : ICommand;
