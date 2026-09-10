using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.RemoveServiceFee;

public sealed record RemoveServiceFeeCommand(long CustomerOrderId) : ICommand;
