using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.WebhookLog.GetByAsaasPaymentId
{
    public sealed record GetAsaasWebhookLogsByPaymentIdQuery(
        long CompanyId,
        string PaymentId) : IQuery<IReadOnlyList<AsaasWebhookLogResponse>>;
}
