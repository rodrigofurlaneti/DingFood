using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Customer.Exists
{
    public sealed record ExistsAsaasCustomerQuery(
        long CustomerId,
        long CompanyId) : IQuery<bool>;
}
