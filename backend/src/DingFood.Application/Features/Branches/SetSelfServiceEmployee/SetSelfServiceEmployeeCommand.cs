using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Branches.SetSelfServiceEmployee;

public sealed record SetSelfServiceEmployeeCommand(long BranchId, long? EmployeeId) : ICommand;
