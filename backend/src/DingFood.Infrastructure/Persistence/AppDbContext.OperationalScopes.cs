using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DingFood.Domain.Entities;
using DingFood.Domain.Exceptions;

namespace DingFood.Infrastructure.Persistence;

public sealed partial class AppDbContext
{
    private long? ActiveBrandId => _currentTenant?.BrandId;
    private long? ActiveBranchId => _currentTenant?.BranchId;
    internal long? OperationalBranchId => ActiveBranchId;

    // Explicit classification: a new table cannot silently become a global operational table.
    internal static readonly HashSet<string> SystemTables = new(StringComparer.Ordinal)
    {
        "AppFeature", "Permission", "UnitOfMeasure", "TableStatus", "ComandaStatus", "OrderStatus",
        "OrderItemStatus", "CashSessionStatus", "CashMovementType", "StockMovementType", "PaymentMethod",
        "CostType", "ShiftClosingStatus", "PromotionType", "ReservationStatus"
    };
    internal static readonly HashSet<string> IdentityTables = new(StringComparer.Ordinal)
    { "AppUser", "AppUserCompany", "AppUserBranch", "RefreshToken", "AccessLog", "LogTracker", "BusinessGroup", "Company", "Brand", "CompanyBrand", "UserRole" };

    private static readonly Dictionary<string, string> AggregateParents = new()
    {
        ["CashMovement"] = "CashSession", ["CashSession"] = "CashRegister", ["CashSessionPaymentReconciliation"] = "CashSession",
        ["ComandaItemTransfer"] = "CustomerOrder", ["Complement"] = "ComplementGroup", ["CustomerRefreshToken"] = "CustomerAppUser",
        ["DiningAreaAssignment"] = "DiningArea", ["DiningAreaTable"] = "DiningArea", ["IfoodPizzaElementMapping"] = "IfoodPizzaMapping",
        ["JobTitleFeature"] = "JobTitle", ["OrderItem"] = "CustomerOrder", ["OrderItemBoost"] = "OrderItem", ["OrderItemComplement"] = "OrderItem",
        ["OrderItemOptionalExtra"] = "OrderItem", ["OrderItemPizzaFlavor"] = "OrderItem", ["OrderPartialPayment"] = "CustomerOrder",
        ["PizzaConfiguration"] = "Product", ["PizzaCrust"] = "PizzaConfiguration", ["PizzaEdge"] = "PizzaConfiguration",
        ["PizzaFlavorPrice"] = "PizzaConfiguration", ["PizzaSize"] = "PizzaConfiguration", ["ProductBoost"] = "Product",
        ["ProductComplementGroup"] = "Product", ["ProductOptionalExtra"] = "Product", ["ProductStock"] = "Product",
        ["PurchaseItem"] = "Purchase", ["RolePermission"] = "Role", ["SalePayment"] = "Sale", ["ShiftClosingSession"] = "ShiftClosing",
        ["StockMovement"] = "StockItem", ["TableItemTransfer"] = "CustomerOrder"
    };

    private void ConfigureOperationalScopes(ModelBuilder builder)
    {
        builder.Entity<ProductStock>().HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<DiningAreaAssignment>().HasOne<DiningArea>().WithMany().HasForeignKey(x => x.DiningAreaId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<DiningAreaAssignment>().HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<DiningAreaTable>().HasOne<DiningArea>().WithMany().HasForeignKey(x => x.DiningAreaId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<DiningAreaTable>().HasOne<DiningTable>().WithMany().HasForeignKey(x => x.DiningTableId).OnDelete(DeleteBehavior.Restrict);
        var types = builder.Model.GetEntityTypes().ToArray();
        var classified = new HashSet<IMutableEntityType>();
        foreach (var entity in types)
        {
            var name = entity.ClrType.Name;
            if (SystemTables.Contains(name) || IdentityTables.Contains(name))
            {
                entity.SetAnnotation("DingFood:Scope", SystemTables.Contains(name) ? "SystemReference" : "IdentityOrOrganization");
                classified.Add(entity);
                continue;
            }
            if (entity.FindProperty("CompanyId") is null && entity.FindProperty("BranchId") is null) continue;
            var row = Expression.Parameter(entity.ClrType, "row");
            Expression? ownership = null;
            if (entity.FindProperty("CompanyId") is not null)
                ownership = EqualScope(row, "CompanyId", nameof(ActiveCompanyId));
            if (entity.FindProperty("BranchId") is { } branch)
            {
                var parent = ParentExists(row, branch.Name, typeof(Branch));
                // Company-wide payment settings can be inherited inside the selected brand.
                if (branch.IsNullable && entity.ClrType != typeof(AppUserFeature)) parent = Expression.OrElse(IsNull(row, branch.Name), parent);
                ownership = ownership is null ? parent : Expression.AndAlso(ownership, parent);
            }
            if (entity.FindProperty("BrandId") is not null)
            {
                builder.Entity(entity.ClrType).HasOne(typeof(Brand)).WithMany().HasForeignKey("BrandId").OnDelete(DeleteBehavior.Restrict);
                builder.Entity(entity.ClrType).HasIndex("CompanyId", "BrandId");
                ownership = Expression.AndAlso(ownership!, OptionalScope(row, "BrandId", nameof(ActiveBrandId)));
            }
            if (entity.ClrType == typeof(Branch))
                ownership = Expression.AndAlso(ownership!, OptionalScope(row, "Id", nameof(ActiveBranchId)));
            // Global origin values are system references, operational origins still follow ownership.
            if (entity.ClrType == typeof(OrderOrigin))
                ownership = Expression.OrElse(Expression.AndAlso(IsNull(row, "CompanyId"), IsNull(row, "BranchId")), ownership!);
            entity.SetQueryFilter(Expression.Lambda(UnscopedOr(ownership!), row));
            entity.SetAnnotation("DingFood:Scope", entity.FindProperty("BranchId") is not null || entity.ClrType == typeof(Branch) ? "CompanyBrandBranch" : entity.FindProperty("BrandId") is not null ? "CompanyBrand" : "Company");
            classified.Add(entity);
        }

        // Every dependent inherits its aggregate's scope, including tables without a DbSet property.
        while (true)
        {
            var progress = false;
            foreach (var entity in types.Where(e => !classified.Contains(e)))
            {
                var owners = entity.GetForeignKeys().Where(fk => fk.Properties.Count == 1 && fk.PrincipalKey.Properties.Count == 1 &&
                    classified.Contains(fk.PrincipalEntityType) && !SystemTables.Contains(fk.PrincipalEntityType.ClrType.Name) &&
                    fk.PrincipalEntityType.ClrType != typeof(Brand) && fk.PrincipalEntityType.ClrType != typeof(BusinessGroup)).ToArray();
                // Prefer a required operational parent; do not inherit scope from an optional audit actor.
                var owner = AggregateParents.TryGetValue(entity.ClrType.Name, out var parentName) ? owners.FirstOrDefault(fk => fk.PrincipalEntityType.ClrType.Name == parentName) : owners.Length == 1 ? owners[0] : null;
                if (owner is null) continue;
                var row = Expression.Parameter(entity.ClrType, "row");
                entity.SetQueryFilter(Expression.Lambda(UnscopedOr(ParentExists(row, owner.Properties[0].Name, owner.PrincipalEntityType.ClrType)), row));
                entity.SetAnnotation("DingFood:Scope", "Inherited:" + owner.PrincipalEntityType.ClrType.Name);
                classified.Add(entity); progress = true;
            }
            if (!progress) break;
        }
        builder.Entity<Brand>().HasQueryFilter(b => !ActiveCompanyId.HasValue || Set<CompanyBrand>().Any(link => link.BrandId == b.Id && link.CompanyId == ActiveCompanyId && link.IsActive));
        builder.Entity<CompanyBrand>().HasQueryFilter(b => !ActiveCompanyId.HasValue || b.CompanyId == ActiveCompanyId);
        builder.Entity<AppUserBranch>().HasQueryFilter(g => !ActiveCompanyId.HasValue || Branchs.Any(b => b.Id == g.BranchId));
        var missing = types.Where(e => !classified.Contains(e)).Select(e => e.ClrType.Name).ToArray();
        if (missing.Length != 0) throw new InvalidOperationException("Entities without ownership: " + string.Join(", ", missing));
    }

    private Expression Scope(string name) => Expression.Property(Expression.Constant(this), name);
    private Expression UnscopedOr(Expression predicate) => Expression.OrElse(Expression.Equal(Scope(nameof(ActiveCompanyId)), Expression.Constant(null, typeof(long?))), predicate);
    private static Expression LongValue(ParameterExpression row, string name) => Expression.Convert(Expression.Property(row, name), typeof(long?));
    private static Expression IsNull(ParameterExpression row, string name) => Expression.Equal(LongValue(row, name), Expression.Constant(null, typeof(long?)));
    private Expression EqualScope(ParameterExpression row, string property, string scope) => Expression.Equal(LongValue(row, property), Scope(scope));
    private Expression OptionalScope(ParameterExpression row, string property, string scope) => Expression.OrElse(Expression.Equal(Scope(scope), Expression.Constant(null, typeof(long?))), EqualScope(row, property, scope));
    private Expression ParentExists(ParameterExpression row, string foreignKey, Type parentType)
    {
        var parent = Expression.Parameter(parentType, "owner");
        var set = typeof(DbContext).GetMethods().Single(m => m.Name == nameof(Set) && m.IsGenericMethod && m.GetParameters().Length == 0).MakeGenericMethod(parentType);
        return Expression.Call(typeof(Queryable), nameof(Queryable.Any), [parentType], Expression.Call(Expression.Constant(this), set),
            Expression.Lambda(Expression.Equal(LongValue(parent, "Id"), LongValue(row, foreignKey)), parent));
    }

    private async Task AssignBrandOwnershipAsync(CancellationToken ct)
    {
        foreach (var feature in ChangeTracker.Entries<AppUserFeature>().Where(e => e.State == EntityState.Added && e.Entity.BranchId == null))
            if (ActiveBranchId.HasValue) feature.Property(nameof(AppUserFeature.BranchId)).CurrentValue = ActiveBranchId;
        foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Metadata.FindProperty("BrandId") is not null && e.Metadata.FindProperty("CompanyId") is not null).ToArray())
        {
            if (entry.Property("BrandId").CurrentValue is not null) continue;
            if (entry.Property("CompanyId").CurrentValue is not long companyId) continue;
            if (ActiveBrandId.HasValue) { entry.Property("BrandId").CurrentValue = ActiveBrandId; continue; }
            var brands = await Set<CompanyBrand>().IgnoreQueryFilters().Where(b => b.CompanyId == companyId && b.IsActive).Select(b => b.BrandId).Take(2).ToListAsync(ct);
            if (brands.Count > 1) throw new TenantAccessException();
            if (brands.Count == 1) entry.Property("BrandId").CurrentValue = brands[0];
        }
    }
}
