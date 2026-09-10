using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class ShiftClosingSessionRepository(AppDbContext context) : IShiftClosingSessionRepository
{
    public async Task<IReadOnlyCollection<ShiftClosingSession>> GetByShiftClosingAsync(long shiftClosingId, CancellationToken cancellationToken = default)
        => await context.ShiftClosingSessions.AsNoTracking()
            .Where(x => x.ShiftClosingId == shiftClosingId && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task AddRangeAsync(IEnumerable<ShiftClosingSession> entities, CancellationToken cancellationToken = default)
        => await context.ShiftClosingSessions.AddRangeAsync(entities, cancellationToken);
}
