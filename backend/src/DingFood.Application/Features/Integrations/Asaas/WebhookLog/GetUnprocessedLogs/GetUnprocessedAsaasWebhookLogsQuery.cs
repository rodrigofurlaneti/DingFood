using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.WebhookLog.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.WebhookLog.GetUnprocessedLogs
{
    public sealed record GetUnprocessedAsaasWebhookLogsQuery(
        long CompanyId,
        int Limit = 50) : IQuery<IReadOnlyList<AsaasWebhookLogResponse>>;
}
