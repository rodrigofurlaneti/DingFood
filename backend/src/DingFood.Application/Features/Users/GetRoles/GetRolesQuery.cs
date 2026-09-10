using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Users.GetRoles;

public sealed record GetRolesQuery(long CompanyId) : IQuery<IReadOnlyCollection<RoleResponse>>;
