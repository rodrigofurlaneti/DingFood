using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.RefundDispute.Update
{
    public sealed record UpdateKeetaIntegrationRefundDisputeCommand(
        long Id,
        long CompanyId,
        bool Accepted,
        string? DenialReasonCode = null,
        string? DenialReasonText = null) : ICommand;
}
