using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Merchant;

public sealed record GetIfoodInterruptionsQuery(long BranchId) : IQuery<IReadOnlyCollection<IfoodInterruptionResponse>>;
