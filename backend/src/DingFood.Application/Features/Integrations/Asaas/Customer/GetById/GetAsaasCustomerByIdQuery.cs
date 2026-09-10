using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Customer.GetAllByCompanyId;
namespace DingFood.Application.Features.Integrations.Asaas.Customer.GetById
{
    public sealed record GetAsaasCustomerByIdQuery(
        long Id) : IQuery<AsaasIntegrationCustomerResponse>;
}
