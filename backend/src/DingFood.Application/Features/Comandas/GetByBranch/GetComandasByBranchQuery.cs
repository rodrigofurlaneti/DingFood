using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Comandas.GetByBranch;

public sealed record GetComandasByBranchQuery(long BranchId) : IQuery<IReadOnlyCollection<ComandaResponse>>;
