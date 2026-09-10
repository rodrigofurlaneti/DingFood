using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Customer.Update
{
    public sealed record UpdateAsaasIntegrationCustomerCommand(
        long Id,
        string NewAsaasCustomerId) : ICommand;
}
