using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class IfoodSettlementRepository(AppDbContext context) : IIfoodSettlementRepository
{
    public async Task<IfoodSettlement?> GetByIfoodSettlementIdForUpdateAsync(long branchId, string IfoodSettlementId, CancellationToken cancellationToken = default)
        => await context.IfoodSettlements
            .FirstOrDefaultAsync(x => x.BranchId == branchId && x.IfoodSettlementId == IfoodSettlementId, cancellationToken);

    public async Task<IReadOnlyCollection<IfoodSettlement>> GetByBranchAndPeriodAsync(
        long branchId, DateTime periodStart, DateTime periodEnd, CancellationToken cancellationToken = default)
        => await context.IfoodSettlements.AsNoTracking()
            .Where(x => x.BranchId == branchId && x.IsActive &&
                        ((x.PaymentDate ?? x.CreatedAt) >= periodStart && (x.PaymentDate ?? x.CreatedAt) <= periodEnd))
            .OrderBy(x => x.PaymentDate ?? x.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(IfoodSettlement entity, CancellationToken cancellationToken = default)
        => await context.IfoodSettlements.AddAsync(entity, cancellationToken);
}
