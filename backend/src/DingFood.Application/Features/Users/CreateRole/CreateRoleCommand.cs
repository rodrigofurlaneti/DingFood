using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Users.CreateRole;

public sealed record CreateRoleCommand(
    long CompanyId,
    string Name,
    string? Description) : ICommand<long>;
