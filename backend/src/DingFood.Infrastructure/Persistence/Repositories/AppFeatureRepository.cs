using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class AppFeatureRepository(AppDbContext context) : IAppFeatureRepository
{
    public async Task<IReadOnlyCollection<AppFeature>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.AppFeatures.AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);
}
