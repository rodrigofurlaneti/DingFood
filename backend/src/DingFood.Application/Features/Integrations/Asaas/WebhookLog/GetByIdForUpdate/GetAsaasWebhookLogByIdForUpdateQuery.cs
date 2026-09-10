using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.WebhookLog.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.WebhookLog.GetByIdForUpdate
{
    public sealed record GetAsaasWebhookLogByIdForUpdateQuery(
        long Id,
        long CompanyId) : IQuery<AsaasWebhookLogResponse>;
}
