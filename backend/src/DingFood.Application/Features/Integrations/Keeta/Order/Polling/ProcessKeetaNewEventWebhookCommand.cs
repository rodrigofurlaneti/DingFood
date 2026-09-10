using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Polling
{
    public sealed record ProcessKeetaNewEventWebhookCommand(
        string RawPayload,
        string? AppId,
        long? KeetaMerchantId,
        string? Signature) : ICommand;
}
