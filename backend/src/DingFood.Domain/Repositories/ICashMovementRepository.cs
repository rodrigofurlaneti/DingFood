using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface ICashMovementRepository
{
    Task<IReadOnlyCollection<CashMovement>> GetBySessionAsync(long cashSessionId, CancellationToken cancellationToken = default);
    Task AddAsync(CashMovement entity, CancellationToken cancellationToken = default);
}
