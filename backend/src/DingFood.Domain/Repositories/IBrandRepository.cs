using DingFood.Domain.Entities;
namespace DingFood.Domain.Repositories
{
    public interface IBrandRepository
    {
        Task<Brand?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Brand?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Brand>> GetByBusinessGroupIdAsync(long businessGroupId, CancellationToken cancellationToken = default);
        Task AddAsync(Brand entity, CancellationToken cancellationToken = default);
    }
}
