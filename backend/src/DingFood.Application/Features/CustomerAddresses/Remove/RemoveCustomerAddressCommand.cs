using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAddresses.Remove
{
    public sealed record RemoveCustomerAddressCommand(long Id) : ICommand;
}
