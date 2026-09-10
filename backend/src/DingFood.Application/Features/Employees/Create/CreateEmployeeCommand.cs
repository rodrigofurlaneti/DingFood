using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Employees.Create;

public sealed record CreateEmployeeCommand(
    long BranchId,
    long JobTitleId,
    string Name,
    string Cpf,
    string? Email,
    string? Phone,
    DateTime HiredAt,
    decimal? Salary) : ICommand<long>;
