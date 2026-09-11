using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public sealed record IfoodCompanyScope(long CompanyId, long? BrandId);
public interface IIfoodIntegrationSettingRepository
{
    Task<IfoodIntegrationSetting?> GetByCompanyAsync(long companyId, CancellationToken cancellationToken = default);
    Task<IfoodIntegrationSetting?> GetByCompanyForUpdateAsync(long companyId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<long>> GetEnabledCompanyIdsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<IfoodCompanyScope>> GetEnabledScopesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<IfoodIntegrationSetting>> GetEnabledSettingsAsync(CancellationToken ct = default);
    Task AddAsync(IfoodIntegrationSetting entity, CancellationToken cancellationToken = default);
}
