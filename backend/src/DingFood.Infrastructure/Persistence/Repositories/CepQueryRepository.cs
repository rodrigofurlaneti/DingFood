using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class CepQueryRepository(AppDbContext context) : ICepQueryRepository
{
    public async Task<CepQuery?> GetByCepAsync(string cep, CancellationToken cancellationToken = default)
        => await context.CepQueries.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Cep == cep && x.IsActive, cancellationToken);

    public async Task<CepQuery?> GetByCepForUpdateAsync(string cep, CancellationToken cancellationToken = default)
        => await context.CepQueries
            .FirstOrDefaultAsync(x => x.Cep == cep, cancellationToken);

    public async Task<IReadOnlyCollection<CepQuery>> GetRecentAsync(int take, CancellationToken cancellationToken = default)
        => await context.CepQueries.AsNoTracking()
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.QueriedAt)
            .Take(take <= 0 ? 20 : take)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(CepQuery entity, CancellationToken cancellationToken = default)
        => await context.CepQueries.AddAsync(entity, cancellationToken);
}
