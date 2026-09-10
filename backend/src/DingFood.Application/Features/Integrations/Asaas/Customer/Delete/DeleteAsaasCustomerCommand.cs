using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Customer.Delete
{
    public sealed record DeleteAsaasCustomerCommand(long CustomerId, long CompanyId) : ICommand;
}
