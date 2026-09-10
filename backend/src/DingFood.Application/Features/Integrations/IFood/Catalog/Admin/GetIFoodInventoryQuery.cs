using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Admin;

// Fase 10 — consulta o estoque de um produto (GET catalog/v2.0/merchants/{merchantId}/inventory/{productId}).
public sealed record IfoodInventoryResponse(string? ProductId, string? OwnerId, int? Amount, bool? InStock);

public sealed record GetIfoodInventoryQuery(long BranchId, Guid ProductId) : IQuery<IfoodInventoryResponse>;
