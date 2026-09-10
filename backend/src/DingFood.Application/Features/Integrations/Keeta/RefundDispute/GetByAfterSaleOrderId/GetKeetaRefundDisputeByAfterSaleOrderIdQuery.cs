using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetByAfterSaleOrderId
{
    public sealed record GetKeetaRefundDisputeByAfterSaleOrderIdQuery(
        long AfterSaleOrderId) : IQuery<KeetaIntegrationRefundDisputeResponse>;
}
