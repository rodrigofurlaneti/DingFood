using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Integrations.Asaas.WebhookLog.Receive
{
    public sealed record ReceiveAsaasWebhookCommand(
        string RawPayload,
        string? AccessToken,
        string? IpAddress) : ICommand;
}
