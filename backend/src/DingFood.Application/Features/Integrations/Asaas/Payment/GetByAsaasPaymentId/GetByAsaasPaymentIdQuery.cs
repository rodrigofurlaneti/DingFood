using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentId
{
    public sealed record GetByAsaasPaymentIdQuery(
        string AsaasPaymentId) : IQuery<AsaasIntegrationPaymentResponse>;
}
