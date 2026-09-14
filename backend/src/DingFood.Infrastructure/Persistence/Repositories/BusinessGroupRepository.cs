using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class BusinessGroupRepository(AppDbContext db, DbContextOptions<AppDbContext> options) : IBusinessGroupRepository
{
    public async Task<IReadOnlyCollection<AppUser>> GetUsersAsync(long groupId, CancellationToken ct)
        => await db.AppUsers.IgnoreQueryFilters().AsNoTracking().Where(u => u.IsActive &&
            db.Companies.IgnoreQueryFilters().Any(c => c.Id == u.CompanyId && c.BusinessGroupId == groupId && c.IsActive)).ToListAsync(ct);

    public Task<BusinessGroup?> GetByIdAsync(long id, CancellationToken ct)
        => db.BusinessGroups.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);

    public async Task<(AppUser User, Company Home)?> GetUserHomeAsync(long userId, CancellationToken ct)
    {
        var user = await db.AppUsers.IgnoreQueryFilters().AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId && x.IsActive, ct);
        if (user is null) return null;
        var home = await db.Companies.IgnoreQueryFilters().AsNoTracking().SingleOrDefaultAsync(x => x.Id == user.CompanyId && x.IsActive, ct);
        return home is null ? null : (user, home);
    }

    public async Task<long> AddCompanyWithAdministratorAsync(Company company, long userId, string branchName, CancellationToken ct)
    {
        // Dedicated administrative transaction: authorization and group association are checked
        // by the Application handler. Never change the operational request's tenant context.
        var strategy = db.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            // A retry must start with fresh tracking and generated keys after rollback.
            await using var admin = new AppDbContext(options);
            await using var transaction = await admin.Database.BeginTransactionAsync(ct);
            var created = Company.Create(company.LegalName, company.TradeName, company.Cnpj, company.Email, company.Phone).Value;
            created.AssignToGroup(company.BusinessGroup);
            admin.Attach(created.BusinessGroup);
            admin.Companies.Add(created);
            await admin.SaveChangesAsync(ct);
            var role = Role.Create(created.Id, "Administrador", "Administrador da empresa criada no grupo.").Value;
            admin.Roles.Add(role);
            var branch = Branch.Create(created.Id, branchName, null, created.Phone, null, null, null, null, null, null).Value;
            admin.Branchs.Add(branch);
            await admin.SaveChangesAsync(ct);
            var user = await admin.AppUsers.SingleAsync(x => x.Id == userId, ct);
            var home = await admin.Companies.SingleAsync(x => x.Id == user.CompanyId, ct);
            admin.AppUserCompanies.Add(AppUserCompany.Create(user, home, created).Value);
            admin.UserRoles.Add(UserRole.Create(created.Id, userId, role.Id).Value);
            admin.Add(AppUserBranch.Create(userId, branch.Id, roleId: role.Id));
            await admin.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return created.Id;
        });
    }
    public async Task SetAccessAsync(AppUserCompany grant, long? roleId, CancellationToken ct)
    {
        var existing = await db.AppUserCompanies.SingleOrDefaultAsync(x => x.AppUserId == grant.AppUserId && x.CompanyId == grant.CompanyId, ct);
        if (existing is null) db.AppUserCompanies.Add(grant);
        else
        {
            existing.Restore();
            var employee = grant.EmployeeId.HasValue ? await db.Employees.SingleAsync(x => x.Id == grant.EmployeeId, ct) : null;
            var branch = employee is not null ? await db.Branchs.SingleAsync(x => x.Id == employee.BranchId, ct) : null;
            existing.AssignEmployee(employee, branch);
        }
        var roles = await db.UserRoles.Where(x => x.AppUserId == grant.AppUserId && x.CompanyId == grant.CompanyId).ToListAsync(ct);
        foreach (var role in roles) role.Deactivate();
        if (roleId.HasValue)
        {
            var selected = roles.FirstOrDefault(x => x.RoleId == roleId.Value);
            if (selected is not null) selected.Reactivate();
            else db.UserRoles.Add(UserRole.Create(grant.CompanyId, grant.AppUserId, roleId.Value).Value);
        }
        await db.SaveChangesAsync(ct);
    }

    public async Task RevokeAccessAsync(long userId, long companyId, CancellationToken ct)
    {
        var grant = await db.AppUserCompanies.SingleOrDefaultAsync(x => x.AppUserId == userId && x.CompanyId == companyId, ct);
        grant?.Revoke();
        foreach (var role in await db.UserRoles.Where(x => x.AppUserId == userId && x.CompanyId == companyId).ToListAsync(ct)) role.Deactivate();
        await db.SaveChangesAsync(ct);
    }

    public async Task AddAsync(BusinessGroup entity, CancellationToken cancellationToken = default)
    {
        await db.BusinessGroups.AddAsync(entity, cancellationToken);
    }
}
