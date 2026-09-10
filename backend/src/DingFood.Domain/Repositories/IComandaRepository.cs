using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IComandaRepository
{
    Task<Comanda?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Comanda?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
    Task<Comanda?> GetByCodeAsync(long branchId, string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Comanda>> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default);
    Task AddAsync(Comanda entity, CancellationToken cancellationToken = default);
}
