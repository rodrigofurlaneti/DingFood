using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Review;

public sealed record ReplyIfoodReviewCommand(long BranchId, string ReviewId, string Text) : ICommand<IfoodReviewReplyResponse>;
