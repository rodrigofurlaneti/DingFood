using FluentValidation;
using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAddresses.RegisterOrder
{
    public sealed record RegisterCustomerAddressOrderCommand(
        long AddressId,
        long OrderId
    ) : ICommand;
}
