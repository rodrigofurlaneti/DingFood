using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IComplementGroupRepository
{
    Task<ComplementGroup?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<ComplementGroup?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ComplementGroup>> GetByCompanyAsync(long companyId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ComplementGroup>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken = default);
    Task AddAsync(ComplementGroup entity, CancellationToken cancellationToken = default);
}
