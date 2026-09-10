using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.GetById
{
    public sealed record GetAsaasPaymentByIdQuery(
        long Id) : IQuery<AsaasIntegrationPaymentResponse>;
}
