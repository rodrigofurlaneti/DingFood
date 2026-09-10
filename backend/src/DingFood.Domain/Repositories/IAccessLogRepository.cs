using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IAccessLogRepository
{
    Task AddAsync(AccessLog entity, CancellationToken cancellationToken = default);
}
