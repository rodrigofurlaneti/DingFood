using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class RevenueTargetRepository(AppDbContext context) : IRevenueTargetRepository
{
    public async Task<RevenueTarget?> GetByBranchAndMonthAsync(
        long branchId, int referenceYear, int referenceMonth, CancellationToken cancellationToken = default)
        => await context.RevenueTargets.AsNoTracking()
            .FirstOrDefaultAsync(x => x.BranchId == branchId && x.ReferenceYear == referenceYear
                && x.ReferenceMonth == referenceMonth && x.IsActive, cancellationToken);

    public async Task<RevenueTarget?> GetByBranchAndMonthForUpdateAsync(
        long branchId, int referenceYear, int referenceMonth, CancellationToken cancellationToken = default)
        => await context.RevenueTargets
            .FirstOrDefaultAsync(x => x.BranchId == branchId && x.ReferenceYear == referenceYear
                && x.ReferenceMonth == referenceMonth && x.IsActive, cancellationToken);

    public async Task AddAsync(RevenueTarget entity, CancellationToken cancellationToken = default)
        => await context.RevenueTargets.AddAsync(entity, cancellationToken);
}
