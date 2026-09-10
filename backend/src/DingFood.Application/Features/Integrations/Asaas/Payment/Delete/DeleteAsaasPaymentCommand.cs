using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Payment.Delete
{
    public sealed record DeleteAsaasPaymentCommand(long PaymentId) : ICommand;
}
