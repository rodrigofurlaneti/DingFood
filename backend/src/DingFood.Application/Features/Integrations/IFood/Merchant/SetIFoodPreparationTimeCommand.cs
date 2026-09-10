using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Merchant;

// Nulo = remove a customização (volta pra estimativa automática do Ifood).
public sealed record SetIfoodPreparationTimeCommand(long BranchId, int? Minutes) : ICommand;
