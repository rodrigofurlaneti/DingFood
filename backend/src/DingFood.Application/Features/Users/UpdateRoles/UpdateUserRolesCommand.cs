using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Users.UpdateRoles;

public sealed record UpdateUserRolesCommand(
    long AppUserId,
    IReadOnlyCollection<long> RoleIds) : ICommand;
