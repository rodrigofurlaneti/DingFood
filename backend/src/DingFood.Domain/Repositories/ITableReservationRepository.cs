using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface ITableReservationRepository
{
    Task<TableReservation?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TableReservation>> GetByBranchAndDateAsync(long branchId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task AddAsync(TableReservation entity, CancellationToken cancellationToken = default);
}
