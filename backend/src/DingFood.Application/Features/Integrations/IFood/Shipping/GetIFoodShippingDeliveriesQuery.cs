using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Shipping;

// Entregas via malha do Ifood (fase 8) ainda abertas (não canceladas) de uma filial — pra tela
// "Entregas Ifood" (envio de pedidos de outros canais).
public sealed record GetIfoodShippingDeliveriesQuery(long BranchId) : IQuery<IReadOnlyCollection<IfoodShippingDeliveryResponse>>;
