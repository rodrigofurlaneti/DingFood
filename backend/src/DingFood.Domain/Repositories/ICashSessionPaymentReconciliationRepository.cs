using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface ICashSessionPaymentReconciliationRepository
{
    Task<IReadOnlyCollection<CashSessionPaymentReconciliation>> GetByCashSessionAsync(
        long cashSessionId, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<CashSessionPaymentReconciliation> entities, CancellationToken cancellationToken = default);
}
