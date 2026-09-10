using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class OrderPartialPaymentRepository(AppDbContext context) : IOrderPartialPaymentRepository
{
    public async Task<IReadOnlyCollection<OrderPartialPayment>> GetByOrderAsync(long customerOrderId, CancellationToken cancellationToken = default)
        => await context.OrderPartialPayments.AsNoTracking()
            .Where(x => x.CustomerOrderId == customerOrderId && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<OrderPartialPayment>> GetByCashSessionAsync(long cashSessionId, CancellationToken cancellationToken = default)
        => await context.OrderPartialPayments.AsNoTracking()
            .Where(x => x.CashSessionId == cashSessionId && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(OrderPartialPayment entity, CancellationToken cancellationToken = default)
        => await context.OrderPartialPayments.AddAsync(entity, cancellationToken);
}
