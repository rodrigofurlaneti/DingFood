using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IJobTitleFeatureRepository
{
    Task<IReadOnlyCollection<JobTitleFeature>> GetByJobTitleAsync(long jobTitleId, CancellationToken cancellationToken = default);
    // Tracked — concessao desativa/reativa vinculos.
    Task<IReadOnlyCollection<JobTitleFeature>> GetByJobTitleForUpdateAsync(long jobTitleId, CancellationToken cancellationToken = default);
    Task AddAsync(JobTitleFeature entity, CancellationToken cancellationToken = default);
}
