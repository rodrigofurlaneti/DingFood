using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class UserRoleRepository(AppDbContext context) : IUserRoleRepository
{
    // Tracked — atribuicao de perfis desativa/reativa vinculos.
    public async Task<IReadOnlyCollection<UserRole>> GetByUserForUpdateAsync(long appUserId, CancellationToken cancellationToken = default)
        => await context.UserRoles
            .Where(x => x.AppUserId == appUserId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<UserRole>> GetByUsersAsync(IReadOnlyCollection<long> appUserIds, CancellationToken cancellationToken = default)
        => await context.UserRoles.AsNoTracking()
            .Where(x => appUserIds.Contains(x.AppUserId) && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(UserRole entity, CancellationToken cancellationToken = default)
    {
        await context.UserRoles.AddAsync(entity, cancellationToken);
        var user = await context.AppUsers.SingleOrDefaultAsync(u => u.Id == entity.AppUserId, cancellationToken);
        if (user?.EmployeeId is { } employeeId)
        {
            var employee = await context.Employees.SingleOrDefaultAsync(e => e.Id == employeeId, cancellationToken);
            if (employee is not null)
            {
                var grant = await context.Set<AppUserBranch>().SingleOrDefaultAsync(g => g.AppUserId == user.Id && g.BranchId == employee.BranchId, cancellationToken);
                grant?.Update(employeeId, true, entity.RoleId);
            }
        }
    }
}
