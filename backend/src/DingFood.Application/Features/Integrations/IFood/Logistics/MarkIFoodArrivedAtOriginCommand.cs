using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Logistics;

// Entregador chegou na loja (origem) para retirar o pedido.
public sealed record MarkIfoodArrivedAtOriginCommand(long IfoodOrderId) : ICommand;
