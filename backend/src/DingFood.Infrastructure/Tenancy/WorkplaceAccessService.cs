using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DingFood.Infrastructure.Tenancy;

internal sealed class WorkplaceAccessService(AppDbContext db, DbContextOptions<AppDbContext> options, ICompanyAccessService companies) : IWorkplaceAccessService
{
    public Task<IReadOnlyCollection<WorkplaceAccess>> GetAllowedAsync(long userId, long companyId, CancellationToken ct)
        => LoadAsync(userId, companyId, null, false, ct);

    public async Task<WorkplaceAccess?> ResolveAsync(long userId, long companyId, long? branchId, CancellationToken ct)
        => (await LoadAsync(userId, companyId, branchId, true, ct)).SingleOrDefault();

    private async Task<IReadOnlyCollection<WorkplaceAccess>> LoadAsync(long userId, long companyId, long? branchId, bool single, CancellationToken ct)
    {
        // Validate membership in SQL without loading company-wide roles that the branch replaces.
        // No authorization cache: revoked grants and inactive organizations take effect immediately.
        var query = from grant in db.Set<AppUserBranch>().IgnoreQueryFilters().AsNoTracking()
            join branch in db.Branchs.IgnoreQueryFilters() on grant.BranchId equals branch.Id
            join brand in db.Set<Brand>().IgnoreQueryFilters() on branch.BrandId equals brand.Id
            join company in db.Companies.IgnoreQueryFilters() on branch.CompanyId equals company.Id
            join membership in db.AppUserCompanies.IgnoreQueryFilters() on company.Id equals membership.CompanyId
            join user in db.AppUsers.IgnoreQueryFilters() on membership.AppUserId equals user.Id
            join home in db.Companies.IgnoreQueryFilters() on user.CompanyId equals home.Id
            where grant.AppUserId == userId && user.Id == userId && user.IsActive && home.IsActive && membership.IsActive &&
                company.IsActive && company.BusinessGroup.IsActive && company.BusinessGroupId == home.BusinessGroupId &&
                grant.IsActive && branch.CompanyId == companyId && branch.IsActive && brand.IsActive &&
                (!branchId.HasValue || branch.Id == branchId) &&
                db.Set<CompanyBrand>().IgnoreQueryFilters().Any(link => link.CompanyId == companyId && link.BrandId == brand.Id && link.IsActive)
            orderby brand.Name, branch.Name, branch.Id
            select new {
                branch.Id, branch.Name, branch.IsActive, BrandId = brand.Id, BrandName = brand.Name,
                grant.RoleId, grant.UsesLegacyRoles,
                EmployeeId = db.Employees.IgnoreQueryFilters().Where(e => e.Id == grant.EmployeeId && e.BranchId == branch.Id && e.IsActive).Select(e => (long?)e.Id).FirstOrDefault()
            };
        var allowed = await (single ? query.Take(1) : query).ToListAsync(ct);
        if (allowed.Count == 0) return [];
        var explicitRoles = allowed.Where(g => !g.UsesLegacyRoles && g.RoleId.HasValue).Select(g => g.RoleId!.Value).Distinct().ToArray();
        var needsLegacy = allowed.Any(g => g.UsesLegacyRoles);
        var roles = await db.Roles.IgnoreQueryFilters().AsNoTracking().Where(r => r.CompanyId == companyId && r.IsActive)
            .Select(r => new { r.Id, r.Name, Legacy = needsLegacy && db.UserRoles.IgnoreQueryFilters().Any(ur => ur.AppUserId == userId && ur.CompanyId == companyId && ur.RoleId == r.Id && ur.IsActive) })
            .Where(r => explicitRoles.Contains(r.Id) || r.Legacy).ToListAsync(ct);
        var roleIds = roles.Select(r => r.Id).ToArray();
        var permissions = await db.RolePermissions.IgnoreQueryFilters().AsNoTracking().Where(r => roleIds.Contains(r.RoleId) && r.IsActive)
            .Join(db.Permissions.Where(p => p.IsActive), r => r.PermissionId, p => p.Id, (r, p) => new { r.RoleId, p.Code }).ToListAsync(ct);
        return allowed.Select(g => {
            var assignedRoles = roles.Where(r => g.UsesLegacyRoles ? r.Legacy : r.Id == g.RoleId).ToArray();
            var assignedIds = assignedRoles.Select(r => r.Id).ToHashSet();
            return new WorkplaceAccess(g.Id, g.Name, g.IsActive, g.BrandId, g.BrandName, g.EmployeeId,
                assignedRoles.Select(r => r.Name).Distinct().ToArray(), permissions.Where(p => assignedIds.Contains(p.RoleId)).Select(p => p.Code).Distinct().ToArray());
        }).ToArray();
    }

    public async Task<IReadOnlyCollection<BrandOption>> GetBrandsAsync(long userId, long companyId, CancellationToken ct)
    {
        var access = await companies.ResolveAsync(userId, companyId, ct);
        if (access is null || !access.Roles.Contains("Administrador")) return [];
        return await db.Set<Brand>().IgnoreQueryFilters().Where(b => b.BusinessGroupId == access.BusinessGroupId && b.IsActive)
            .OrderBy(b => b.Name).Select(b => new BrandOption(b.Id, b.Name)).ToListAsync(ct);
    }

    public async Task<Result<long>> CreateAsync(long userId, long companyId, CreateWorkplace request, CancellationToken ct)
    {
        var companyAccess = await companies.ResolveAsync(userId, companyId, ct);
        if (companyAccess is null || !companyAccess.Roles.Contains("Administrador"))
            return Result.Failure<long>(new Error("Workplace.Forbidden", "Somente administradores podem criar operações."));
        if (string.IsNullOrWhiteSpace(request.BranchName) || request.BranchName.Trim().Length > 150 ||
            (!request.ExistingBrandId.HasValue && (string.IsNullOrWhiteSpace(request.BrandName) || request.BrandName.Trim().Length > 150)))
            return Result.Failure<long>(new Error("Workplace.Invalid", "Informe marca e unidade válidas."));
        return await db.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            await using var admin = new AppDbContext(options);
            await using var transaction = await admin.Database.BeginTransactionAsync(ct);
            var brand = request.ExistingBrandId.HasValue
                ? await admin.Set<Brand>().SingleOrDefaultAsync(b => b.Id == request.ExistingBrandId && b.BusinessGroupId == companyAccess.BusinessGroupId && b.IsActive, ct)
                : Brand.Create(companyAccess.BusinessGroupId, request.BrandName);
            if (brand is null) return Result.Failure<long>(new Error("Workplace.InvalidBrand", "Marca indisponível neste grupo."));
            if (!request.ExistingBrandId.HasValue) { admin.Add(brand); await admin.SaveChangesAsync(ct); }
            if (!await admin.Set<CompanyBrand>().AnyAsync(b => b.CompanyId == companyId && b.BrandId == brand.Id && b.IsActive, ct))
            { admin.Add(CompanyBrand.Create(companyId, brand.Id)); await admin.SaveChangesAsync(ct); }
            var branch = Branch.Create(companyId, request.BranchName.Trim(), null, null, null, null, null, null, null, null).Value;
            admin.Entry(branch).Property(nameof(Branch.BrandId)).CurrentValue = brand.Id;
            admin.Add(branch); await admin.SaveChangesAsync(ct);
            var adminRole = await admin.Roles.FirstOrDefaultAsync(r => r.CompanyId == companyId && r.Name == "Administrador" && r.IsActive, ct);
            admin.Add(AppUserBranch.Create(userId, branch.Id, roleId: adminRole?.Id));
            await admin.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return Result.Success(branch.Id);
        });
    }

    public async Task<Result> SetAccessAsync(long userId, long companyId, UpdateWorkplaceAccess request, CancellationToken ct)
    {
        var companyAccess = await companies.ResolveAsync(userId, companyId, ct);
        if (companyAccess is null || !companyAccess.Roles.Contains("Administrador") || userId == request.AppUserId ||
            !(await GetAllowedAsync(userId, companyId, ct)).Any(b => b.Id == request.BranchId))
            return Result.Failure(new Error("Workplace.Forbidden", "Usuário ou unidade sem autorização nesta empresa."));
        await using var admin = new AppDbContext(options);
        var target = await admin.AppUsers.SingleOrDefaultAsync(u => u.Id == request.AppUserId && u.IsActive, ct);
        var home = target is null ? null : await admin.Companies.SingleOrDefaultAsync(c => c.Id == target.CompanyId && c.IsActive, ct);
        var company = await admin.Companies.SingleAsync(c => c.Id == companyId, ct);
        if (target is null || home is null || home.BusinessGroupId != company.BusinessGroupId)
            return Result.Failure(new Error("Workplace.Forbidden", "Usuário indisponível no grupo."));
        if (request.Enabled)
        {
            var membership = await admin.AppUserCompanies.SingleOrDefaultAsync(g => g.AppUserId == target.Id && g.CompanyId == companyId, ct);
            if (membership is null) admin.Add(AppUserCompany.Create(target, home, company).Value);
            else membership.Restore();
        }
        if (request.EmployeeId.HasValue && !await admin.Employees.AnyAsync(e => e.Id == request.EmployeeId && e.BranchId == request.BranchId && e.IsActive, ct))
            return Result.Failure(new Error("Workplace.InvalidEmployee", "Funcionário não pertence à unidade autorizada."));
        var grant = await admin.Set<AppUserBranch>().SingleOrDefaultAsync(g => g.AppUserId == request.AppUserId && g.BranchId == request.BranchId, ct);
        if (grant is null) { grant = AppUserBranch.Create(request.AppUserId, request.BranchId); admin.Add(grant); }
        if (request.RoleId.HasValue && !await admin.Roles.AnyAsync(r => r.Id == request.RoleId && r.CompanyId == companyId && r.IsActive, ct))
            return Result.Failure(new Error("Workplace.InvalidRole", "Perfil não pertence à empresa."));
        grant.Update(request.EmployeeId, request.Enabled, request.RoleId);
        await admin.SaveChangesAsync(ct);
        return Result.Success();
    }
}
