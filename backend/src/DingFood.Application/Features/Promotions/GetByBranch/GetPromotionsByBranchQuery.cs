using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Promotions.GetByBranch;

public sealed record GetPromotionsByBranchQuery(long BranchId) : IQuery<IReadOnlyCollection<PromotionResponse>>;
