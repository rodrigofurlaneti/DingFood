using DingFood.Domain.Enums;
using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.WebhookLog.Update
{
    public sealed record UpdateAsaasWebhookLogStatusCommand(
        long Id,
        long CompanyId,
        WebhookLogStatus Status,
        string? ErrorMessage = null) : ICommand;
}
