using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Exceptions;
using DingFood.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DingFood.Infrastructure.Tenancy;

internal sealed class PublicWorkplaceScope(AppDbContext db, CurrentTenantService tenant) : IPublicWorkplaceScope
{
    public async Task BindAsync(long? branchId, Guid? tableToken, long? orderId, CancellationToken ct)
    {
        if (tableToken.HasValue)
            branchId = await db.DiningTables.IgnoreQueryFilters().Where(t => t.QrToken == tableToken && t.IsActive).Select(t => (long?)t.BranchId).SingleOrDefaultAsync(ct);
        if (orderId.HasValue)
            branchId = await db.CustomerOrders.IgnoreQueryFilters().Where(o => o.Id == orderId && o.IsActive).Select(o => (long?)o.BranchId).SingleOrDefaultAsync(ct);
        if (!branchId.HasValue) throw new TenantAccessException();
        var branch = await db.Branchs.IgnoreQueryFilters().SingleOrDefaultAsync(b => b.Id == branchId && b.IsActive, ct);
        if (branch?.BrandId is null) throw new TenantAccessException();
        if (tenant.BranchId.HasValue && tenant.BranchId != branch.Id || tenant.CompanyId.HasValue && tenant.CompanyId != branch.CompanyId)
            throw new TenantAccessException();
        if (!await db.Set<DingFood.Domain.Entities.Brand>().IgnoreQueryFilters().AnyAsync(b => b.Id == branch.BrandId && b.IsActive, ct) ||
            !await db.Companies.IgnoreQueryFilters().AnyAsync(c => c.Id == branch.CompanyId && c.IsActive, ct) ||
            !await db.Set<DingFood.Domain.Entities.CompanyBrand>().IgnoreQueryFilters().AnyAsync(c => c.CompanyId == branch.CompanyId && c.BrandId == branch.BrandId && c.IsActive, ct))
            throw new TenantAccessException();
        tenant.SetResolvedWorkplace(branch.CompanyId, branch.BrandId.Value, branch.Id);
    }
}
