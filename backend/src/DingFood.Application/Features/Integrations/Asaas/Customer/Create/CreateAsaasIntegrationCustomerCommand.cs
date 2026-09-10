using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Customer.Create
{
    public sealed record CreateAsaasIntegrationCustomerCommand(
        long CustomerId,
        long CompanyId,
        string AsaasCustomerId) : ICommand<long>;
}
