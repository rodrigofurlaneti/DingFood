using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.GetByIdForUpdate
{
    public sealed record GetAsaasPaymentByIdForUpdateQuery(
        long Id) : IQuery<AsaasIntegrationPaymentResponse>;
}
