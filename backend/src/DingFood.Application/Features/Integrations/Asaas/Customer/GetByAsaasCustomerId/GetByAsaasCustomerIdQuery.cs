using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Customer.GetAllByCompanyId;
namespace DingFood.Application.Features.Integrations.Asaas.Customer.GetByAsaasCustomerId
{
    public sealed record GetByAsaasCustomerIdQuery(
        string AsaasCustomerId) : IQuery<AsaasIntegrationCustomerResponse>;
}
