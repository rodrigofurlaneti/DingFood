using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Authorization.ProcessAuthorizationWebhook
{
    public sealed record ProcessKeetaAuthorizationWebhookCommand(string RawPayload, string? Signature) : ICommand;
}
