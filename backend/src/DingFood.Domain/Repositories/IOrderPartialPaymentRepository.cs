using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IOrderPartialPaymentRepository
{
    Task<IReadOnlyCollection<OrderPartialPayment>> GetByOrderAsync(long customerOrderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<OrderPartialPayment>> GetByCashSessionAsync(long cashSessionId, CancellationToken cancellationToken = default);
    Task AddAsync(OrderPartialPayment entity, CancellationToken cancellationToken = default);
}
