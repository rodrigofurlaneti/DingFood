using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.MarkReady;

public sealed record MarkOrderReadyCommand(long CustomerOrderId) : ICommand;
