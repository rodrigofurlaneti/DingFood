using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IIfoodCategoryMappingRepository
{
    Task<IfoodCategoryMapping?> GetByCategoryAndBranchAsync(long categoryId, long branchId, CancellationToken cancellationToken = default);
    Task AddAsync(IfoodCategoryMapping entity, CancellationToken cancellationToken = default);
}
