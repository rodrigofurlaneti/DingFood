using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.ExistsByAfterSaleOrderId
{
    public sealed record ExistsKeetaRefundDisputeByAfterSaleOrderIdQuery(
        long AfterSaleOrderId) : IQuery<bool>;
}
