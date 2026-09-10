using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Merchant;

public sealed record CreateIfoodInterruptionCommand(
    long BranchId, string Description, DateTime Start, DateTime End) : ICommand;
