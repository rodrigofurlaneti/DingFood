using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Customer.GetByCustomerIdAndCompanyId
{
    public sealed record GetByCustomerIdAndCompanyIdQuery(
        long CustomerId,
        long CompanyId) : IQuery<AsaasIntegrationCustomerResponse>;
}
