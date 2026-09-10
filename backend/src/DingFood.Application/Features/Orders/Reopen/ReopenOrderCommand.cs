using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.Reopen;

public sealed record ReopenOrderCommand(long CustomerOrderId) : ICommand;
