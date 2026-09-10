using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Merchant;

public sealed record DeleteIfoodInterruptionCommand(long BranchId, string InterruptionId) : ICommand;
