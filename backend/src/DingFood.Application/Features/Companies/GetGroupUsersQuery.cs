using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Security;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Companies;

public sealed record GroupUserResponse(long Id, string UserName, long HomeCompanyId);
public sealed record GetGroupUsersQuery : IQuery<IReadOnlyCollection<GroupUserResponse>>;
internal sealed class GetGroupUsersQueryHandler(ICompanyAccessService access, ICurrentUserService user,
    ICurrentTenantService tenant, IBusinessGroupRepository groups) : IQueryHandler<GetGroupUsersQuery, IReadOnlyCollection<GroupUserResponse>>
{
    public async Task<Result<IReadOnlyCollection<GroupUserResponse>>> Handle(GetGroupUsersQuery request, CancellationToken ct)
    {
        var current = await access.ResolveAsync(user.UserId, tenant.CompanyId ?? 0, ct);
        if (current is null || !current.Roles.Contains("Administrador"))
            return Result.Failure<IReadOnlyCollection<GroupUserResponse>>(new Error("Company.Forbidden", "Acesso restrito ao administrador."));
        IReadOnlyCollection<GroupUserResponse> result = (await groups.GetUsersAsync(current.BusinessGroupId, ct))
            .Select(x => new GroupUserResponse(x.Id, x.UserName, x.CompanyId)).ToArray();
        return Result.Success(result);
    }
}
