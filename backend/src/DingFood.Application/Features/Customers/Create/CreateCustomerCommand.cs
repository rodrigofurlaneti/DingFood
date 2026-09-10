using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Customers.Create;

public sealed record CreateCustomerCommand(
    long CompanyId,
    string Name,
    string? Phone,
    string? Cpf,
    string? Email) : ICommand<long>;
