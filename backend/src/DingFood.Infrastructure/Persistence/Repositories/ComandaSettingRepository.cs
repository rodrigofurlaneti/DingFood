using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class ComandaSettingRepository(AppDbContext context) : IComandaSettingRepository
{
    public async Task<ComandaSetting?> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default)
        => await context.ComandaSettings.AsNoTracking()
            .FirstOrDefaultAsync(x => x.BranchId == branchId && x.IsActive, cancellationToken);

    public async Task<ComandaSetting?> GetByBranchForUpdateAsync(long branchId, CancellationToken cancellationToken = default)
        => await context.ComandaSettings
            .FirstOrDefaultAsync(x => x.BranchId == branchId && x.IsActive, cancellationToken);

    public async Task AddAsync(ComandaSetting entity, CancellationToken cancellationToken = default)
        => await context.ComandaSettings.AddAsync(entity, cancellationToken);
}
