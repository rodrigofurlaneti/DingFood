using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IIfoodIntegrationSettingRepository
{
    Task<IfoodIntegrationSetting?> GetByCompanyAsync(long companyId, CancellationToken cancellationToken = default);
    Task<IfoodIntegrationSetting?> GetByCompanyForUpdateAsync(long companyId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<long>> GetEnabledCompanyIdsAsync(CancellationToken cancellationToken = default);
    Task AddAsync(IfoodIntegrationSetting entity, CancellationToken cancellationToken = default);
}
