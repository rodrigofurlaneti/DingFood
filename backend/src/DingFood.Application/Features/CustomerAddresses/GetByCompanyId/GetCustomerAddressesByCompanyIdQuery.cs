using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.CustomerAddresses.GetByBranchId;
namespace DingFood.Application.Features.CustomerAddresses.GetByCompanyId
{
    public sealed record GetCustomerAddressesByCompanyIdQuery(long CompanyId) : IQuery<IEnumerable<CustomerAddressResponse>>;
}
