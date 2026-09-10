using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.GetByBranchId
{
    public sealed record GetByBranchIdQuery(
        long BranchId) : IQuery<IReadOnlyList<AsaasIntegrationPaymentResponse>>;
}
