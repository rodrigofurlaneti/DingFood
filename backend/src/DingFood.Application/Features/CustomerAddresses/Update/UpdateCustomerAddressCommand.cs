using FluentValidation;
using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.CustomerAddresses.Update
{
    public sealed record UpdateCustomerAddressCommand(
        long Id,
        long CompanyId,
        long? BranchId,
        long? CustomerId,
        string Street,
        string Number,
        string Supplement,
        string? ZipCode
    ) : ICommand;
}
