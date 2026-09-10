using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentId;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.GetPendingByBranchId
{
    public sealed record GetPendingAsaasPaymentsByBranchIdQuery(
        long BranchId) : IQuery<IReadOnlyList<AsaasIntegrationPaymentResponse>>;
}
