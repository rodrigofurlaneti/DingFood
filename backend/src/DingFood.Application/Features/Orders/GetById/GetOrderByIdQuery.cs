using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Orders.GetById;

public sealed record GetOrderByIdQuery(long CustomerOrderId) : IQuery<OrderResponse>;
