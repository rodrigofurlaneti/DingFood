using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.Create
{
    public sealed record CreateKeetaIntegrationRefundDisputeCommand(
        long CompanyId,
        long BranchId,
        string OrderId,
        long AfterSaleOrderId,
        decimal RefundAmount,
        string ApplyReason) : ICommand<CreateKeetaIntegrationRefundDisputeResponse>;
}
