using System.Linq.Expressions;
using System.Reflection;
using DingFood.Domain.Entities;
using DingFood.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DingFood.Infrastructure.Persistence;

public sealed partial class AppDbContext
{
    private long? ActiveCompanyId => _currentTenant?.CompanyId;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (ActiveCompanyId.HasValue) await ValidateTenantWritesAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateTenantWritesAsync(CancellationToken ct)
    {
        ChangeTracker.DetectChanges();
        var entries = ChangeTracker.Entries().Where(x => x.State is EntityState.Added or EntityState.Modified or EntityState.Deleted).ToArray();
        foreach (var entry in entries)
        {
            // Authentication/audit records are owned by the identity, not by its selected company.
            if (entry.Entity is LogTracker or AccessLog or RefreshToken or BusinessGroup or AppUserCompany) continue;
            if (entry.Metadata.FindProperty("CompanyId") is not null)
            {
                var property = entry.Property("CompanyId");
                if (property.CurrentValue is long company && company != ActiveCompanyId ||
                    entry.State != EntityState.Added && property.OriginalValue is long original && original != ActiveCompanyId)
                    throw new TenantAccessException();
            }
            foreach (var fk in entry.Metadata.GetForeignKeys())
            {
                if (fk.Properties.Count != 1 || fk.PrincipalKey.Properties.Count != 1 ||
                    fk.PrincipalEntityType.GetQueryFilter() is null) continue;
                var value = entry.Property(fk.Properties[0].Name).CurrentValue;
                if (value is not long id) continue;
                // New aggregate children reference tracked parents before generated IDs exist.
                if (entries.Any(parent => parent.State == EntityState.Added &&
                    parent.Metadata == fk.PrincipalEntityType && Equals(parent.Property(fk.PrincipalKey.Properties[0].Name).CurrentValue, value))) continue;
                // UserRole grants intentionally refer to an identity whose home company can differ.
                if (entry.Entity is UserRole && fk.PrincipalEntityType.ClrType == typeof(AppUser)) continue;
                var method = typeof(AppDbContext).GetMethod(nameof(IsVisibleAsync), BindingFlags.Instance | BindingFlags.NonPublic)!
                    .MakeGenericMethod(fk.PrincipalEntityType.ClrType);
                if (!await (Task<bool>)method.Invoke(this, [id, ct])!) throw new TenantAccessException();
            }
        }
    }

    private Task<bool> IsVisibleAsync<T>(long id, CancellationToken ct) where T : class
        => Set<T>().AsNoTracking().AnyAsync(x => EF.Property<long>(x, "Id") == id, ct);

    private void ConfigureAdditionalTenantFilters(ModelBuilder modelBuilder)
    {
        // Cover all direct company/branch owners, including newly added integration entities.
        foreach (var entity in modelBuilder.Model.GetEntityTypes().ToArray())
        {
            var parameter = Expression.Parameter(entity.ClrType, "row");
            var company = Expression.Property(Expression.Constant(this), nameof(ActiveCompanyId));
            Expression? ownership = null;
            if (entity.FindProperty("CompanyId") is { } companyProperty)
                ownership = Expression.Equal(Expression.Convert(Expression.Property(parameter, companyProperty.Name), typeof(long?)), company);
            else if (entity.FindProperty("BranchId") is { } branchProperty)
            {
                var branch = Expression.Parameter(typeof(Branch), "branch");
                var predicate = Expression.Lambda<Func<Branch, bool>>(Expression.AndAlso(
                    Expression.Equal(Expression.Convert(Expression.Property(branch, nameof(Branch.Id)), typeof(long?)),
                        Expression.Convert(Expression.Property(parameter, branchProperty.Name), typeof(long?))),
                    Expression.Equal(Expression.Convert(Expression.Property(branch, nameof(Branch.CompanyId)), typeof(long?)), company)), branch);
                ownership = Expression.Call(typeof(Queryable), nameof(Queryable.Any), [typeof(Branch)],
                    Expression.Property(Expression.Constant(this), nameof(Branchs)), predicate);
            }
            if (ownership is not null)
                entity.SetQueryFilter(Expression.Lambda(Expression.OrElse(
                    Expression.Equal(company, Expression.Constant(null, typeof(long?))), ownership), parameter));
        }
        modelBuilder.Entity<Company>().HasQueryFilter(x => !ActiveCompanyId.HasValue || x.Id == ActiveCompanyId);
        modelBuilder.Entity<OrderOrigin>().HasQueryFilter(x => !ActiveCompanyId.HasValue ||
            (x.CompanyId == null && x.BranchId == null) ||
            (x.CompanyId == ActiveCompanyId && (x.BranchId == null || Branchs.Any(b => b.Id == x.BranchId))));
        modelBuilder.Entity<BusinessGroup>().HasQueryFilter(x => !ActiveCompanyId.HasValue ||
            Companies.Any(c => c.Id == ActiveCompanyId && c.BusinessGroupId == x.Id));
        modelBuilder.Entity<Complement>().HasQueryFilter(x => !ActiveCompanyId.HasValue || ComplementGroups.Any(p => p.Id == x.ComplementGroupId));
        modelBuilder.Entity<ProductComplementGroup>().HasQueryFilter(x => !ActiveCompanyId.HasValue || Products.Any(p => p.Id == x.ProductId));
        modelBuilder.Entity<PizzaConfiguration>().HasQueryFilter(x => !ActiveCompanyId.HasValue || Products.Any(p => p.Id == x.ProductId));
        modelBuilder.Entity<PizzaSize>().HasQueryFilter(x => !ActiveCompanyId.HasValue || PizzaConfigurations.Any(p => p.Id == x.PizzaConfigurationId));
        modelBuilder.Entity<PizzaEdge>().HasQueryFilter(x => !ActiveCompanyId.HasValue || PizzaConfigurations.Any(p => p.Id == x.PizzaConfigurationId));
        modelBuilder.Entity<PizzaCrust>().HasQueryFilter(x => !ActiveCompanyId.HasValue || PizzaConfigurations.Any(p => p.Id == x.PizzaConfigurationId));
        modelBuilder.Entity<PizzaFlavorPrice>().HasQueryFilter(x => !ActiveCompanyId.HasValue || PizzaConfigurations.Any(p => p.Id == x.PizzaConfigurationId));
        modelBuilder.Entity<IfoodPizzaElementMapping>().HasQueryFilter(x => !ActiveCompanyId.HasValue || IfoodPizzaMappings.Any(p => p.Id == x.IfoodPizzaMappingId));
        modelBuilder.Entity<StockMovement>().HasQueryFilter(x => !ActiveCompanyId.HasValue || StockItems.Any(p => p.Id == x.StockItemId));
        modelBuilder.Entity<CashSession>().HasQueryFilter(x => !ActiveCompanyId.HasValue || CashRegisters.Any(p => p.Id == x.CashRegisterId));
        modelBuilder.Entity<CashMovement>().HasQueryFilter(x => !ActiveCompanyId.HasValue || CashSessions.Any(p => p.Id == x.CashSessionId));
        modelBuilder.Entity<SalePayment>().HasQueryFilter(x => !ActiveCompanyId.HasValue || Sales.Any(p => p.Id == x.SaleId));
        modelBuilder.Entity<OrderItem>().HasQueryFilter(x => !ActiveCompanyId.HasValue || CustomerOrders.Any(p => p.Id == x.CustomerOrderId));
        modelBuilder.Entity<OrderItemComplement>().HasQueryFilter(x => !ActiveCompanyId.HasValue || OrderItems.Any(p => p.Id == x.OrderItemId));
        modelBuilder.Entity<OrderItemPizzaFlavor>().HasQueryFilter(x => !ActiveCompanyId.HasValue || OrderItems.Any(p => p.Id == x.OrderItemId));
        modelBuilder.Entity<ProductOptionalExtra>().HasQueryFilter(x => !ActiveCompanyId.HasValue || Products.Any(p => p.Id == x.ProductId));
        modelBuilder.Entity<ProductBoost>().HasQueryFilter(x => !ActiveCompanyId.HasValue || Products.Any(p => p.Id == x.ProductId));
        modelBuilder.Entity<PurchaseItem>().HasQueryFilter(x => !ActiveCompanyId.HasValue || Purchases.Any(p => p.Id == x.PurchaseId));
        modelBuilder.Entity<CashSessionPaymentReconciliation>().HasQueryFilter(x => !ActiveCompanyId.HasValue || CashSessions.Any(p => p.Id == x.CashSessionId));
        modelBuilder.Entity<OrderPartialPayment>().HasQueryFilter(x => !ActiveCompanyId.HasValue || CustomerOrders.Any(p => p.Id == x.CustomerOrderId));
        modelBuilder.Entity<JobTitleFeature>().HasQueryFilter(x => !ActiveCompanyId.HasValue || JobTitles.Any(p => p.Id == x.JobTitleId));
        modelBuilder.Entity<AppUserFeature>().HasQueryFilter(x => !ActiveCompanyId.HasValue || AppUsers.Any(p => p.Id == x.AppUserId));
        modelBuilder.Entity<ShiftClosingSession>().HasQueryFilter(x => !ActiveCompanyId.HasValue || ShiftClosings.Any(p => p.Id == x.ShiftClosingId));
    }
}
