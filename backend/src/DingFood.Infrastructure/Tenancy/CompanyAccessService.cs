using DingFood.Application.Abstractions.Tenancy;
using DingFood.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DingFood.Infrastructure.Tenancy;

internal sealed class CompanyAccessService(AppDbContext db) : ICompanyAccessService
{
    // These queries cross the active company only to resolve explicit administrative access.
    // Operational repositories must keep their company filters.
    public Task<IReadOnlyCollection<CompanyAccess>> GetAllowedAsync(long userId, CancellationToken ct)
        => LoadAllowedAsync(userId, null, ct);

    private async Task<IReadOnlyCollection<CompanyAccess>> LoadAllowedAsync(long userId, long? selectedCompanyId, CancellationToken ct)
    {
        var user = await db.AppUsers.IgnoreQueryFilters().AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == userId && x.IsActive, ct);
        if (user is null) return [];
        var home = await db.Companies.IgnoreQueryFilters().AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == user.CompanyId && x.IsActive, ct);
        if (home is null) return [];
        var companies = await db.Companies.IgnoreQueryFilters().AsNoTracking()
            .Where(x => (!selectedCompanyId.HasValue || x.Id == selectedCompanyId) && x.IsActive && x.BusinessGroupId == home.BusinessGroupId && x.BusinessGroup.IsActive &&
                db.AppUserCompanies.IgnoreQueryFilters().Any(g =>
                    g.AppUserId == userId && g.CompanyId == x.Id && g.IsActive))
            .Select(x => new { x.Id, x.BusinessGroupId, GroupName = x.BusinessGroup.Name, x.TradeName, x.Cnpj })
            .ToListAsync(ct);
        var result = new List<CompanyAccess>();
        foreach (var company in companies)
        {
            var membershipEmployee = await db.AppUserCompanies.IgnoreQueryFilters().AsNoTracking()
                .Where(x => x.AppUserId == userId && x.CompanyId == company.Id && x.IsActive)
                .Select(x => x.EmployeeId).SingleOrDefaultAsync(ct);
            var employeeId = membershipEmployee ?? (company.Id == user.CompanyId ? user.EmployeeId : null);
            if (employeeId.HasValue && !await db.Employees.IgnoreQueryFilters().AnyAsync(e => e.Id == employeeId && e.IsActive &&
                db.Branchs.IgnoreQueryFilters().Any(b => b.Id == e.BranchId && b.CompanyId == company.Id && b.IsActive), ct))
                employeeId = null;
            var roles = await db.Roles.IgnoreQueryFilters().AsNoTracking()
                .Where(r => r.CompanyId == company.Id && r.IsActive && (
                    db.UserRoles.IgnoreQueryFilters().Any(ur => ur.RoleId == r.Id && ur.AppUserId == userId && ur.CompanyId == company.Id && ur.IsActive) ||
                    db.Set<DingFood.Domain.Entities.AppUserBranch>().IgnoreQueryFilters().Any(g => g.AppUserId == userId && g.RoleId == r.Id && g.IsActive &&
                        db.Branchs.IgnoreQueryFilters().Any(b => b.Id == g.BranchId && b.CompanyId == company.Id && b.IsActive))))
                .ToListAsync(ct);            var roleIds = roles.Select(x => x.Id).ToArray();
            var permissions = await db.RolePermissions.AsNoTracking()
                .Where(x => roleIds.Contains(x.RoleId) && x.IsActive)
                .Join(db.Permissions.Where(x => x.IsActive), x => x.PermissionId, x => x.Id,
                    (link, permission) => permission.Code).Distinct().ToListAsync(ct);
            result.Add(new CompanyAccess(company.Id, company.BusinessGroupId, company.GroupName,
                company.TradeName, company.Cnpj, employeeId,
                roles.Select(x => x.Name).Distinct().ToArray(), permissions));
        }
        return result;
    }

    public async Task<CompanyAccess?> ResolveAsync(long userId, long companyId, CancellationToken ct)
        => (await LoadAllowedAsync(userId, companyId, ct)).SingleOrDefault();
}
