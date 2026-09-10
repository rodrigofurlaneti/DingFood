using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Customer.GetAllByCompanyId
{
    public sealed record GetAllAsaasCustomersByCompanyIdQuery(
        long CompanyId) : IQuery<IReadOnlyList<AsaasIntegrationCustomerResponse>>;
}
