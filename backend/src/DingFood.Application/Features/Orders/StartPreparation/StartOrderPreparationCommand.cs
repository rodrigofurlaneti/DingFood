using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.StartPreparation;

public sealed record StartOrderPreparationCommand(long CustomerOrderId) : ICommand;
