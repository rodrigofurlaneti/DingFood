using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Promotions.GetActive;

public sealed record GetActivePromotionsQuery(long BranchId) : IQuery<IReadOnlyCollection<ActivePromotionResponse>>;
