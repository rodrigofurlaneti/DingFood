using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetPendingDisputesByBranch
{
    public sealed record GetPendingKeetaRefundDisputesByBranchQuery(
        long BranchId) : IQuery<IReadOnlyList<KeetaIntegrationRefundDisputeResponse>>;
}
