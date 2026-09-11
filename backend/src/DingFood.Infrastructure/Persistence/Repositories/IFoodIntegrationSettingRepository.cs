using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class IfoodIntegrationSettingRepository(AppDbContext context) : IIfoodIntegrationSettingRepository
{
    public async Task<IfoodIntegrationSetting?> GetByCompanyAsync(long companyId, CancellationToken cancellationToken = default)
        => await context.IfoodIntegrationSettings.AsNoTracking()
            .SingleOrDefaultAsync(x => x.CompanyId == companyId && x.IsActive, cancellationToken);

    public async Task<IfoodIntegrationSetting?> GetByCompanyForUpdateAsync(long companyId, CancellationToken cancellationToken = default)
        => await context.IfoodIntegrationSettings
            .SingleOrDefaultAsync(x => x.CompanyId == companyId && x.IsActive, cancellationToken);

    public async Task<IReadOnlyCollection<long>> GetEnabledCompanyIdsAsync(CancellationToken cancellationToken = default)
        => await context.IfoodIntegrationSettings.AsNoTracking().IgnoreQueryFilters()
            .Where(x => x.IsActive && x.Enabled)
            .Select(x => x.CompanyId)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<IfoodCompanyScope>> GetEnabledScopesAsync(CancellationToken ct = default)
        => await context.IfoodIntegrationSettings.IgnoreQueryFilters().AsNoTracking().Where(s => s.IsActive && s.Enabled)
            .Select(s => new IfoodCompanyScope(s.CompanyId, s.BrandId)).Distinct().ToListAsync(ct);
    public async Task<IReadOnlyCollection<IfoodIntegrationSetting>> GetEnabledSettingsAsync(CancellationToken ct = default)
        => await context.IfoodIntegrationSettings.IgnoreQueryFilters().AsNoTracking().Where(s => s.IsActive && s.Enabled).ToListAsync(ct);
    public async Task AddAsync(IfoodIntegrationSetting entity, CancellationToken cancellationToken = default)
        => await context.IfoodIntegrationSettings.AddAsync(entity, cancellationToken);
}
