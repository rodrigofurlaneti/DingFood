using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.Delete
{
    public sealed record DeleteKeetaIntegrationRefundDisputeCommand(
        long Id,
        long CompanyId) : ICommand;
}
