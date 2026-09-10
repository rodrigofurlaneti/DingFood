using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Review;

public sealed record GetIfoodReviewsSummaryQuery(long BranchId) : IQuery<IfoodReviewSummaryResponse>;
