using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.RemoveItemComplement;

public sealed record RemoveOrderItemComplementCommand(
    long CustomerOrderId,
    long OrderItemId,
    long OrderItemComplementId,
    long? EmployeeId) : ICommand;
