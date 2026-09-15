using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class CnpjQueryRepository(AppDbContext context) : ICnpjQueryRepository
{
    public async Task<CnpjQuery?> GetByTaxIdAsync(string taxId, CancellationToken cancellationToken = default)
        => await context.CnpjQueries.AsNoTracking()
            .FirstOrDefaultAsync(x => x.TaxId == taxId && x.IsActive, cancellationToken);

    public async Task<CnpjQuery?> GetByTaxIdForUpdateAsync(string taxId, CancellationToken cancellationToken = default)
        => await context.CnpjQueries
            .FirstOrDefaultAsync(x => x.TaxId == taxId, cancellationToken);

    public async Task<IReadOnlyCollection<CnpjQuery>> GetRecentAsync(int take, CancellationToken cancellationToken = default)
        => await context.CnpjQueries.AsNoTracking()
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.QueriedAt)
            .Take(take <= 0 ? 20 : take)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(CnpjQuery entity, CancellationToken cancellationToken = default)
        => await context.CnpjQueries.AddAsync(entity, cancellationToken);
}
