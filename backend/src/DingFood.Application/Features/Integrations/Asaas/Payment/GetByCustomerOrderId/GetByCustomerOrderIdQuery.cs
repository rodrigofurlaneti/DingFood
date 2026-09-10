using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.GetByCustomerOrderId
{
    public sealed record GetByCustomerOrderIdQuery(
        long CustomerOrderId) : IQuery<AsaasIntegrationPaymentResponse>;
}
