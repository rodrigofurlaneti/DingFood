using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Review;

public sealed record GetIfoodReviewByIdQuery(long BranchId, string ReviewId) : IQuery<IfoodReviewDetailResponse>;
