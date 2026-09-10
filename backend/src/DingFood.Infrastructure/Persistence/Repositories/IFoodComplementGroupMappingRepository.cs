using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class IfoodComplementGroupMappingRepository(AppDbContext context) : IIfoodComplementGroupMappingRepository
{
    public async Task<IfoodComplementGroupMapping?> GetByComplementGroupAndBranchAsync(long complementGroupId, long branchId, CancellationToken cancellationToken = default)
        => await context.IfoodComplementGroupMappings.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ComplementGroupId == complementGroupId && x.BranchId == branchId && x.IsActive, cancellationToken);

    public async Task<IReadOnlyCollection<IfoodComplementGroupMapping>> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default)
        => await context.IfoodComplementGroupMappings.AsNoTracking()
            .Where(x => x.BranchId == branchId && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(IfoodComplementGroupMapping entity, CancellationToken cancellationToken = default)
        => await context.IfoodComplementGroupMappings.AddAsync(entity, cancellationToken);
}
