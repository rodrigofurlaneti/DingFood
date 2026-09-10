using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class PromotionRepository(AppDbContext context) : IPromotionRepository
{
    public async Task<Promotion?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default)
        => await context.Promotions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Promotion>> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default)
        => await context.Promotions.AsNoTracking()
            .Where(x => x.BranchId == branchId && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Promotion entity, CancellationToken cancellationToken = default)
        => await context.Promotions.AddAsync(entity, cancellationToken);
}
