using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.RaiseComandaLimit;

public sealed record RaiseComandaLimitCommand(long CustomerOrderId, decimal NewLimitAmount) : ICommand;
