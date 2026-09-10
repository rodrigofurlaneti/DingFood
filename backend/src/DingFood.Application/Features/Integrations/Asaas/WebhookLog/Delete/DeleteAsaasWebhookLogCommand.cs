using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.WebhookLog.Delete
{
    public sealed record DeleteAsaasWebhookLogCommand(
        long Id,
        long CompanyId) : ICommand;
}
