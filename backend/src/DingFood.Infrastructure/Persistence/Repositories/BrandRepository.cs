using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class BrandRepository(AppDbContext context, DingFood.Application.Abstractions.Tenancy.ICurrentTenantService? tenant = null) : IBrandRepository
{
    public async Task<Brand?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.Brands.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Brand?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default)
        => await context.Brands.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Brand>> GetByBusinessGroupIdAsync(long businessGroupId, CancellationToken cancellationToken = default)
        => await context.Brands.AsNoTracking()
            .Where(x => x.BusinessGroupId == businessGroupId && x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Brand entity, CancellationToken cancellationToken = default)
    {
        await context.Brands.AddAsync(entity, cancellationToken);
    }
}
