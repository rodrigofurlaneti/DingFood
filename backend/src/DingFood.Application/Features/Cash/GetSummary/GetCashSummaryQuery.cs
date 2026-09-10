using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Cash.GetSummary;

public sealed record GetCashSummaryQuery(long CashSessionId) : IQuery<CashSummaryResponse>;
