using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Review;

public sealed record GetIfoodReviewsQuery(
    long BranchId,
    int Page,
    int PageSize,
    DateTime? DateFrom,
    DateTime? DateTo,
    string Sort,
    string SortBy) : IQuery<IfoodReviewListResponse>;
