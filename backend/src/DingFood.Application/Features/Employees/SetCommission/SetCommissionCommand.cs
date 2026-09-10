using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Employees.SetCommission;

public sealed record SetCommissionCommand(long EmployeeId, decimal? CommissionPercent) : ICommand;
