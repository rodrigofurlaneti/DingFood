using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Admin;

// Fase 10 — consulta a versão do catálogo do merchant (GET catalog/v2.0/merchants/{merchantId}/catalogVersion) —
// "v1" ou "v2". Ver UpgradeIfoodCatalogVersionCommand/DowngradeIfoodCatalogVersionCommand pra migrar.
public sealed record IfoodCatalogVersionResponse(string? Version);

public sealed record CheckIfoodCatalogVersionQuery(long BranchId) : IQuery<IfoodCatalogVersionResponse>;
