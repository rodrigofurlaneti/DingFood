using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class CashMovementRepository(AppDbContext context) : ICashMovementRepository
{
    public async Task<IReadOnlyCollection<CashMovement>> GetBySessionAsync(long cashSessionId, CancellationToken cancellationToken = default)
        => await context.CashMovements.AsNoTracking()
            .Where(x => x.CashSessionId == cashSessionId && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(CashMovement entity, CancellationToken cancellationToken = default)
        => await context.CashMovements.AddAsync(entity, cancellationToken);
}
