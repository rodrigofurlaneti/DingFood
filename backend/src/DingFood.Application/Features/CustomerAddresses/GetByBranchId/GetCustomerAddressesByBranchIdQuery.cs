using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAddresses.GetByBranchId
{
    public sealed record GetCustomerAddressesByBranchIdQuery(long BranchId) : IQuery<IEnumerable<CustomerAddressResponse>>;
}
