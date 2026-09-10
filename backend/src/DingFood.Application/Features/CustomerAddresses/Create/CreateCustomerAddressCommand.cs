using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.CustomerAddresses.Create
{
    public sealed record CreateCustomerAddressCommand(
        long CompanyId,
        long? BranchId,
        long? CustomerId,
        string Street,
        string Number,
        string Supplement,
        string?ZipCode
    ) : ICommand<long>;
}
