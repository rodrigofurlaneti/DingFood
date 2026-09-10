using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentIdForUpdate
{
    public sealed record GetByAsaasPaymentIdForUpdateQuery(
        string AsaasPaymentId) : IQuery<AsaasIntegrationPaymentResponse>;
}
