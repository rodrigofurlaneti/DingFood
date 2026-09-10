using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetByOrderId
{
    public sealed record GetKeetaRefundDisputeByOrderIdQuery(
        string OrderId) : IQuery<KeetaIntegrationRefundDisputeResponse>;
}
