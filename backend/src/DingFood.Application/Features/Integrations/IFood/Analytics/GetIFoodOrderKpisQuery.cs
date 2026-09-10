using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Ifood.Analytics;

public sealed record GetIfoodOrderKpisQuery(long BranchId, DateTime? PeriodStart, DateTime? PeriodEnd, int Page)
    : IQuery<IfoodOrderKpisResponse>;
