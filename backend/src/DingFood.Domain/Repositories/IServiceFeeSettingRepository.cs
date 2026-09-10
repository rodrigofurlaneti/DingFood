using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IServiceFeeSettingRepository
{
    Task<ServiceFeeSetting?> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default);
    Task<ServiceFeeSetting?> GetByBranchForUpdateAsync(long branchId, CancellationToken cancellationToken = default);
    Task AddAsync(ServiceFeeSetting entity, CancellationToken cancellationToken = default);
}
