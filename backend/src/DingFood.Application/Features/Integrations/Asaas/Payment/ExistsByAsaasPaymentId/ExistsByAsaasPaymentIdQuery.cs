using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.ExistsByAsaasPaymentId
{
    public sealed record ExistsByAsaasPaymentIdQuery(
        string AsaasPaymentId) : IQuery<bool>;
}
