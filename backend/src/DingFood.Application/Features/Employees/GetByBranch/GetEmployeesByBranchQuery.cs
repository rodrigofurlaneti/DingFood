using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Employees.GetByBranch;

public sealed record GetEmployeesByBranchQuery(long BranchId) : IQuery<IReadOnlyCollection<EmployeeResponse>>;
