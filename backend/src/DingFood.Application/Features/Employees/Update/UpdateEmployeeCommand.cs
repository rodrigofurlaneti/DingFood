using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Employees.Update;

public sealed record UpdateEmployeeCommand(
    long EmployeeId,
    long JobTitleId,
    string Name,
    string? Email,
    string? Phone,
    decimal? Salary) : ICommand;
