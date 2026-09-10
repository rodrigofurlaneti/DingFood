using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;

namespace DingFood.Application.Features.CustomerAppUser.Create;

public sealed record CreateCustomerAppUserCommand(
    long CompanyId,
    long? BranchId,
    long? CustomerId,
    string Cpf,
    string UserName,
    string Email,
    string Password,
    string? Phone = null
) : ICommand<long>;