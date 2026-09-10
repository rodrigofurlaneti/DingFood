using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Employees.Dismiss;

public sealed record DismissEmployeeCommand(long EmployeeId) : ICommand;
