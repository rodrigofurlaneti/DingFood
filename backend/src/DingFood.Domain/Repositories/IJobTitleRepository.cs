using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IJobTitleRepository
{
    Task<JobTitle?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<JobTitle>> GetByCompanyAsync(long companyId, CancellationToken cancellationToken = default);
    Task AddAsync(JobTitle entity, CancellationToken cancellationToken = default);
}
