using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class AppUserFeatureRepository(AppDbContext context) : IAppUserFeatureRepository
{
    public async Task<IReadOnlyCollection<AppUserFeature>> GetByUserAsync(long appUserId, CancellationToken cancellationToken = default)
        => await context.AppUserFeatures.AsNoTracking()
            .Where(x => x.AppUserId == appUserId && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<AppUserFeature>> GetByUserForUpdateAsync(long appUserId, CancellationToken cancellationToken = default)
        => await context.AppUserFeatures
            .Where(x => x.AppUserId == appUserId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AppUserFeature entity, CancellationToken cancellationToken = default)
        => await context.AppUserFeatures.AddAsync(entity, cancellationToken);
}
