using Microsoft.EntityFrameworkCore;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Persistence.Repositories;

internal sealed class CashRegisterRepository(AppDbContext context) : ICashRegisterRepository
{
    public async Task<IReadOnlyCollection<CashRegister>> GetByBranchAsync(long branchId, CancellationToken cancellationToken = default)
        => await context.CashRegisters.AsNoTracking()
            .Where(x => x.BranchId == branchId && x.IsActive)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(CashRegister entity, CancellationToken cancellationToken = default)
        => await context.CashRegisters.AddAsync(entity, cancellationToken);
}
