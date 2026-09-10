using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.WebhookLog.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.WebhookLog.GetById
{
    public sealed record GetAsaasWebhookLogByIdQuery(
        long Id,
        long CompanyId) : IQuery<AsaasWebhookLogResponse>;
}
