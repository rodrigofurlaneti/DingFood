using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Security;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Companies;

public sealed record SetCompanyAccessCommand(long AppUserId, long? RoleId, bool Enabled, long? EmployeeId = null) : ICommand;

internal sealed class SetCompanyAccessCommandHandler(ICompanyAccessService access, ICurrentUserService user,
    ICurrentTenantService tenant, IBusinessGroupRepository groups, ICompanyRepository companies, IRoleRepository roles,
    IEmployeeRepository employees, IBranchRepository branches)
    : ICommandHandler<SetCompanyAccessCommand>
{
    public async Task<Result> Handle(SetCompanyAccessCommand request, CancellationToken ct)
    {
        var current = await access.ResolveAsync(user.UserId, tenant.CompanyId ?? 0, ct);
        if (current is null || !current.Roles.Contains("Administrador") || user.UserId == request.AppUserId)
            return Result.Failure(new Error("Company.Forbidden", "Somente o administrador pode alterar o acesso de outros usuários."));
        var target = await groups.GetUserHomeAsync(request.AppUserId, ct);
        var company = await companies.GetByIdAsync(current.CompanyId, ct);
        if (target is null || company is null)
            return Result.Failure(new Error("Company.Forbidden", "Usuário indisponível neste grupo."));
        var grant = AppUserCompany.Create(target.Value.User, target.Value.Home, company);
        if (grant.IsFailure) return Result.Failure(grant.Error);
        if (!request.Enabled)
        {
            if (target.Value.Home.Id == company.Id)
                return Result.Failure(new Error("Company.HomeAccess", "Desative o usuário para remover o acesso à empresa de origem."));
            await groups.RevokeAccessAsync(request.AppUserId, company.Id, ct);
            return Result.Success();
        }
        if (request.RoleId is { } roleId)
        {
            var role = await roles.GetByIdAsync(roleId, ct);
            if (role is null || role.CompanyId != company.Id || !role.IsActive)
                return Result.Failure(new Error("Company.InvalidRole", "Perfil não pertence à empresa ativa."));
        }
        if (request.EmployeeId is { } employeeId)
        {
            var employee = await employees.GetByIdAsync(employeeId, ct);
            if (employee is null) return Result.Failure(new Error("Company.InvalidEmployee", "Funcionário indisponível."));
            var branch = await branches.GetByIdAsync(employee.BranchId, ct);
            var assigned = grant.Value.AssignEmployee(employee, branch);
            if (assigned.IsFailure) return assigned;
        }
        await groups.SetAccessAsync(grant.Value, request.RoleId, ct);
        return Result.Success();
    }
}
