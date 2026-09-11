using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class BranchRepository(AppDbContext context, DingFood.Application.Abstractions.Tenancy.ICurrentTenantService? tenant = null) : IBranchRepository
{
    public async Task<Branch?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.Branchs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Branch?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default)
        => await context.Branchs.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Branch>> GetByCompanyAsync(long companyId, CancellationToken cancellationToken = default)
        => await context.Branchs.AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Branch entity, CancellationToken cancellationToken = default)
    {
        await context.Branchs.AddAsync(entity, cancellationToken);
        if (tenant?.UserId is { } userId && tenant.BranchId is { } activeBranch)
        {
            var roleId = await context.Set<AppUserBranch>().Where(g => g.AppUserId == userId && g.BranchId == activeBranch && g.IsActive).Select(g => g.RoleId).SingleOrDefaultAsync(cancellationToken);
            context.Add(AppUserBranch.Create(userId, entity, roleId));
        }
    }
}
