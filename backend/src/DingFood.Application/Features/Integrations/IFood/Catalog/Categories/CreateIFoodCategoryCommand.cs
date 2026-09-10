using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Categories;

// Fase 10 — cria uma categoria (POST catalog/v2.0/merchants/{merchantId}/catalogs/{catalogId}/categories).
public sealed record IfoodCategoryCreateResponse(string? IfoodCategoryId);

public sealed record CreateIfoodCategoryCommand(long BranchId, string CatalogId, string Name)
    : ICommand<IfoodCategoryCreateResponse>;
