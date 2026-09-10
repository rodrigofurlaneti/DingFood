using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

public sealed record CancelIfoodOrderCommand(long IfoodOrderId, string ReasonCode) : ICommand;
