using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IPromotionRepository
{
    Task<Promotion?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Promotion>> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default);
    Task AddAsync(Promotion entity, CancellationToken cancellationToken = default);
}
