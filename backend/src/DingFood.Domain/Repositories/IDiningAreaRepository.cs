using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories
{
    public interface IDiningAreaRepository
    {
        Task<DiningArea?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<IEnumerable<DiningArea>> GetByBranchIdAsync(long branchId, CancellationToken cancellationToken = default);
        Task AddAsync(DiningArea entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(DiningArea entity, CancellationToken cancellationToken = default);
    }
}

