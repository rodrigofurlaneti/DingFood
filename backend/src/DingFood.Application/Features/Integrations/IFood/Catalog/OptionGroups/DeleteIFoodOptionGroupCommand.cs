using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.OptionGroups;

// Fase 10 — exclui um grupo de opções (DELETE catalog/v2.0/merchants/{merchantId}/optionGroups/{optionGroupId}).
public sealed record DeleteIfoodOptionGroupCommand(long BranchId, Guid OptionGroupId) : ICommand;
