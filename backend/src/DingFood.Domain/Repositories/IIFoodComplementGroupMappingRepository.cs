using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IIfoodComplementGroupMappingRepository
{
    Task<IfoodComplementGroupMapping?> GetByComplementGroupAndBranchAsync(long complementGroupId, long branchId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<IfoodComplementGroupMapping>> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default);
    Task AddAsync(IfoodComplementGroupMapping entity, CancellationToken cancellationToken = default);
}
