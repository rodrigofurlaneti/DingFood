using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.OptionGroups;

// Fase 10 — exclui uma opção de um grupo
// (DELETE catalog/v2.0/merchants/{merchantId}/optionGroups/{optionGroupId}/options/{productId}).
public sealed record DeleteIfoodOptionCommand(long BranchId, Guid OptionGroupId, Guid ProductId, string? CatalogContext) : ICommand;
