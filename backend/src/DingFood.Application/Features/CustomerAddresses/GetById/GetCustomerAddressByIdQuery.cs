using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAddresses.GetById
{
    public sealed record GetCustomerAddressByIdQuery(long Id) : IQuery<CustomerAddressResponse>;
}
