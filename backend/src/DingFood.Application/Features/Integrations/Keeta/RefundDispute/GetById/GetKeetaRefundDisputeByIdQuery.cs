using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.GetById
{
    public sealed record GetKeetaRefundDisputeByIdQuery(
        long Id) : IQuery<KeetaIntegrationRefundDisputeResponse>;
}
