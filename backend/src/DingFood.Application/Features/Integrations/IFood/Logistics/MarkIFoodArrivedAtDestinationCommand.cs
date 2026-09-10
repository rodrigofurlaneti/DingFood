using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Logistics;

// Entregador chegou no endereço do cliente (destino).
public sealed record MarkIfoodArrivedAtDestinationCommand(long IfoodOrderId) : ICommand;
