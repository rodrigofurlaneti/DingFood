using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface ICashRegisterRepository
{
    Task<IReadOnlyCollection<CashRegister>> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default);
    Task AddAsync(CashRegister entity, CancellationToken cancellationToken = default);
}
