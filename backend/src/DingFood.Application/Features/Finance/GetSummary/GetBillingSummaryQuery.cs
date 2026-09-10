using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Finance.GetSummary;

public sealed record GetBillingSummaryQuery(
    long BranchId,
    int ReferenceYear,
    int ReferenceMonth) : IQuery<BillingSummaryResponse>;
