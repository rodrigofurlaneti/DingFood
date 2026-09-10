using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAddresses.GetByCustomerId
{
    public sealed record GetCustomerAddressesByCustomerIdQuery(long CustomerId) : IQuery<IEnumerable<CustomerAddressResponse>>;
}
