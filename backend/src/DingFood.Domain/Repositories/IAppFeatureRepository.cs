using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IAppFeatureRepository
{
    Task<IReadOnlyCollection<AppFeature>> GetAllAsync(CancellationToken cancellationToken = default);
}
