using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Users.Create;

public sealed record CreateUserCommand(
    long CompanyId,
    long? EmployeeId,
    string UserName,
    string Email,
    string Password,
    IReadOnlyCollection<long> RoleIds) : ICommand<long>;
