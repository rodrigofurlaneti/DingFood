using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Logistics;

// Entregador saiu em direção à loja (origem) para retirar o pedido.
public sealed record MarkIfoodGoingToOriginCommand(long IfoodOrderId) : ICommand;
