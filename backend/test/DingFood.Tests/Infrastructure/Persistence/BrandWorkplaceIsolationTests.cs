using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Entities;
using DingFood.Domain.Exceptions;
using DingFood.Infrastructure.Persistence;
using DingFood.Infrastructure.Tenancy;
using DingFood.Tests.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using Xunit;

namespace DingFood.Tests.Infrastructure.Persistence;

public sealed class BrandWorkplaceIsolationTests : RepositoryTestBase
{
    private DbContextOptions<AppDbContext> Options => new DbContextOptionsBuilder<AppDbContext>().UseSqlite(Connection)
        .ReplaceService<IModelCustomizer, SqliteCompatibleModelCustomizer>().Options;
    private async Task<(Company Company, Branch A, Branch B, AppUser Admin)> SeedAsync()
    {
        var company = Company.Create("Kitchen Ltda", "Burger", "88888888000188", null, null).Value;
        Context.Add(company); await Context.SaveChangesAsync();
        var a = Branch.Create(company.Id, "Centro", null, null, null, null, null, null, null, null).Value;
        Context.Add(a); await Context.SaveChangesAsync();
        var admin = AppUser.Create(company.Id, null, "admin", "admin@test", "hash").Value;
        Context.Add(admin); await Context.SaveChangesAsync();
        var role = Role.Create(company.Id, "Administrador", null).Value;
        Context.Add(role); await Context.SaveChangesAsync();
        Context.Add(AppUserCompany.CreateHomeAccess(admin).Value);
        Context.Add(UserRole.Create(company.Id, admin.Id, role.Id).Value);
        Context.Add(AppUserBranch.Create(admin.Id, a.Id, roleId: role.Id));
        await Context.SaveChangesAsync();
        var service = new WorkplaceAccessService(Context, Options, new CompanyAccessService(Context));
        var created = await service.CreateAsync(admin.Id, company.Id, new CreateWorkplace("Pizza", "Centro Pizza"), default);
        created.IsSuccess.Should().BeTrue();
        var b = await Context.Branchs.SingleAsync(x => x.Id == created.Value);
        return (company, a, b, admin);
    }

    [Fact]
    public async Task ProductsAndTheirChildrenCannotCrossBrandsWithinSameCnpj()
    {
        var (company, a, b, _) = await SeedAsync();
        async Task<Product> ProductFor(Branch branch, string name)
        {
            using var scoped = new AppDbContext(Options, new Scope(company.Id, branch.BrandId, branch.Id));
            var category = Category.Create(company.Id, name, 0).Value;
            scoped.Add(category); await scoped.SaveChangesAsync();
            // Unit is a system reference, common to the two catalogs.
            if (!await scoped.UnitOfMeasures.AnyAsync())
            {
                var unit = UnitOfMeasure.Create("Unidade", "UN").Value;
                scoped.Add(unit); await scoped.SaveChangesAsync();
            }
            var unitId = await scoped.UnitOfMeasures.Select(x => x.Id).FirstAsync();
            var product = Product.Create(company.Id, category.Id, unitId, name, null, null, 10, null, false, null).Value;
            scoped.Add(product); await scoped.SaveChangesAsync();
            return product;
        }
        var burger = await ProductFor(a, "Burger");
        var pizza = await ProductFor(b, "Pizza");
        using var current = new AppDbContext(Options, new Scope(company.Id, a.BrandId, a.Id));
        (await current.Products.Select(x => x.Name).ToArrayAsync()).Should().Equal("Burger");
        (await current.Products.SingleOrDefaultAsync(x => x.Id == pizza.Id)).Should().BeNull();
        burger.BrandId.Should().Be(a.BrandId);
        var foreign = Product.Create(company.Id, pizza.CategoryId, pizza.UnitOfMeasureId, "Injected", null, null, 1, null, false, null).Value;
        current.Add(foreign);
        await FluentActions.Awaiting(() => current.SaveChangesAsync()).Should().ThrowAsync<TenantAccessException>();
    }

    [Fact]
    public async Task EmployeeAssignmentDoesNotGrantAnotherKitchenOrBrand()
    {
        var (company, a, b, admin) = await SeedAsync();
        var employeeUser = AppUser.Create(company.Id, null, "worker", "worker@test", "hash").Value;
        Context.Add(employeeUser); await Context.SaveChangesAsync();
        Context.Add(AppUserCompany.CreateHomeAccess(employeeUser).Value);
        Context.Add(AppUserBranch.Create(employeeUser.Id, a.Id));
        await Context.SaveChangesAsync();
        var service = new WorkplaceAccessService(Context, Options, new CompanyAccessService(Context));
        (await service.GetAllowedAsync(employeeUser.Id, company.Id, default)).Select(x => x.Id).Should().Equal(a.Id);
        (await service.SetAccessAsync(employeeUser.Id, company.Id, new UpdateWorkplaceAccess(admin.Id, b.Id, null, true), default)).IsFailure.Should().BeTrue();
        (await service.SetAccessAsync(admin.Id, company.Id, new UpdateWorkplaceAccess(employeeUser.Id, b.Id, null, true), default)).IsSuccess.Should().BeTrue();
        (await service.GetAllowedAsync(employeeUser.Id, company.Id, default)).Should().HaveCount(2);
        (await service.SetAccessAsync(admin.Id, company.Id, new UpdateWorkplaceAccess(employeeUser.Id, b.Id, null, false), default)).IsSuccess.Should().BeTrue();
        (await service.GetAllowedAsync(employeeUser.Id, company.Id, default)).Select(x => x.Id).Should().Equal(a.Id);
    }

    [Fact]
    public async Task SameBrandDifferentBranchesRemainIsolated()
    {
        var (company, a, _, admin) = await SeedAsync();
        var service = new WorkplaceAccessService(Context, Options, new CompanyAccessService(Context));
        var other = await service.CreateAsync(admin.Id, company.Id, new CreateWorkplace("", "Outra cozinha", a.BrandId), default);
        other.IsSuccess.Should().BeTrue();
        using var current = new AppDbContext(Options, new Scope(company.Id, a.BrandId, a.Id));
        (await current.Branchs.Select(x => x.Id).ToArrayAsync()).Should().Equal(a.Id);
        var table = DiningTable.Create(other.Value, 1, 1, 4).Value;
        current.Add(table);
        await FluentActions.Awaiting(() => current.SaveChangesAsync()).Should().ThrowAsync<TenantAccessException>();
    }

    [Fact]
    public async Task ForeignGroupBrandCannotBeLinked()
    {
        var (company, _, _, admin) = await SeedAsync();
        var foreign = Company.Create("Foreign", "Foreign", "99999999000199", null, null).Value;
        Context.Add(foreign); await Context.SaveChangesAsync();
        var brandId = await Context.Set<CompanyBrand>().Where(b => b.CompanyId == foreign.Id).Select(b => b.BrandId).SingleAsync();
        var service = new WorkplaceAccessService(Context, Options, new CompanyAccessService(Context));
        (await service.CreateAsync(admin.Id, company.Id, new CreateWorkplace("", "Injected", brandId), default)).IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task StockDeductionUsesTheSelectedBranchAndTheRealStockItemId()
    {
        var (company, a, _, admin) = await SeedAsync();
        var service = new WorkplaceAccessService(Context, Options, new CompanyAccessService(Context));
        var other = await service.CreateAsync(admin.Id, company.Id, new CreateWorkplace("", "Norte", a.BrandId), default);
        var category = Category.Create(company.Id, "Burger", 0).Value;
        Context.Entry(category).Property(nameof(Category.BrandId)).CurrentValue = a.BrandId;
        Context.Add(category);
        var unit = UnitOfMeasure.Create("Unidade", "UN").Value;
        Context.Add(unit); await Context.SaveChangesAsync();
        var product = Product.Create(company.Id, category.Id, unit.Id, "Burger", null, null, 10, null, true, null).Value;
        Context.Entry(product).Property(nameof(Product.BrandId)).CurrentValue = a.BrandId;
        Context.Add(product); await Context.SaveChangesAsync();
        var stockA = StockItem.Create(a.Id, product.Id, 0, null).Value;
        var stockB = StockItem.Create(other.Value, product.Id, 0, null).Value;
        stockA.Increase(10); stockB.Increase(50);
        Context.AddRange(stockA, stockB); await Context.SaveChangesAsync();
        using var current = new AppDbContext(Options, new Scope(company.Id, a.BrandId, a.Id));
        var snapshot = await new DingFood.Infrastructure.Persistence.Repositories.ProductStockRepository(current).GetByProductIdAsync(product.Id);
        snapshot.Should().NotBeNull(); snapshot!.StockItemId.Should().Be(stockA.Id);
        snapshot.Deduct(3).IsSuccess.Should().BeTrue();
        await current.SaveChangesAsync();
        Context.ChangeTracker.Clear();
        (await Context.StockItems.SingleAsync(s => s.Id == stockA.Id)).CurrentQuantity.Should().Be(7);
        (await Context.StockItems.SingleAsync(s => s.Id == stockB.Id)).CurrentQuantity.Should().Be(50);
    }

    [Fact]
    public void EveryMappedEntityHasExplicitOwnershipAndOperationalTablesHaveFilters()
    {
        var rows = new List<string> { "# Isolamento das entidades", "", "Inventário gerado a partir do modelo EF Core, sem dados de clientes.", "", "| Entidade | Escopo |", "| --- | --- |" };
        foreach (var entity in Context.Model.GetEntityTypes().OrderBy(x => x.ClrType.Name))
        {
            var scope = entity.FindAnnotation("DingFood:Scope")?.Value as string;
            scope.Should().NotBeNullOrWhiteSpace(entity.ClrType.Name);
            if (scope is not ("SystemReference" or "IdentityOrOrganization")) entity.GetQueryFilter().Should().NotBeNull(entity.ClrType.Name);
            rows.Add($"| {entity.ClrType.Name} | {scope} |");
        }
        File.WriteAllLines(Path.Combine(AppContext.BaseDirectory, "tenant-entity-inventory.md"), rows);
        File.WriteAllLines(Path.Combine(AppContext.BaseDirectory, "tenant-model-columns.txt"), Context.Model.GetEntityTypes().SelectMany(e =>
            e.GetProperties().Select(p => e.GetTableName()!.ToLowerInvariant() + "." + p.GetColumnName(Microsoft.EntityFrameworkCore.Metadata.StoreObjectIdentifier.Table(e.GetTableName()!, e.GetSchema())))));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public async Task AuthorizationQueryCountDoesNotGrowWithBranchCount(int extraBranches)
    {
        var (company, a, _, admin) = await SeedAsync();
        for (var i = 0; i < extraBranches; i++)
        {
            var branch = Branch.Create(company.Id, $"Extra {i}", null, null, null, null, null, null, null, null).Value;
            Context.Entry(branch).Property(nameof(Branch.BrandId)).CurrentValue = a.BrandId;
            Context.Add(branch); await Context.SaveChangesAsync();
            Context.Add(AppUserBranch.Create(admin.Id, branch.Id)); await Context.SaveChangesAsync();
        }
        var counter = new QueryCounter();
        var options = new DbContextOptionsBuilder<AppDbContext>(Options).AddInterceptors(counter).Options;
        using var db = new AppDbContext(options);
        var service = new WorkplaceAccessService(db, options, new CompanyAccessService(db));
        var selected = await service.ResolveAsync(admin.Id, company.Id, a.Id, default);
        selected.Should().NotBeNull(); selected!.Id.Should().Be(a.Id);
        counter.Count.Should().Be(3, "selected-branch authorization must have a fixed query budget");
        counter.Count = 0;
        (await service.GetAllowedAsync(admin.Id, company.Id, default)).Should().HaveCount(2 + extraBranches);
        counter.Count.Should().Be(3, "the branch selector must batch roles and permissions");
    }

    [Fact]
    public async Task SelectedAuthorizationKeepsBranchRolesSeparateAndHonorsRevocation()
    {
        var (company, a, b, admin) = await SeedAsync();
        var waiter = Role.Create(company.Id, "Garçom", null).Value;
        var permission = Permission.Create("Orders.Read", "Read", "Orders").Value;
        Context.AddRange(waiter, permission); await Context.SaveChangesAsync();
        Context.Add(RolePermission.Create(waiter.Id, permission.Id).Value);
        var grant = await Context.Set<AppUserBranch>().SingleAsync(g => g.AppUserId == admin.Id && g.BranchId == b.Id);
        grant.Update(null, true, waiter.Id); await Context.SaveChangesAsync();
        var service = new WorkplaceAccessService(Context, Options, new CompanyAccessService(Context));
        (await service.ResolveAsync(admin.Id, company.Id, a.Id, default))!.Roles.Should().Contain("Administrador");
        var selected = await service.ResolveAsync(admin.Id, company.Id, b.Id, default);
        selected!.Roles.Should().Equal("Garçom"); selected.Permissions.Should().Equal("Orders.Read");
        (await service.ResolveAsync(admin.Id, company.Id, long.MaxValue, default)).Should().BeNull();
        permission.Deactivate(); await Context.SaveChangesAsync();
        (await service.ResolveAsync(admin.Id, company.Id, b.Id, default))!.Permissions.Should().BeEmpty();
        grant.Update(null, false, waiter.Id); await Context.SaveChangesAsync();
        (await service.ResolveAsync(admin.Id, company.Id, b.Id, default)).Should().BeNull();
        var membership = await Context.AppUserCompanies.SingleAsync(x => x.AppUserId == admin.Id && x.CompanyId == company.Id);
        membership.Revoke(); await Context.SaveChangesAsync();
        (await service.ResolveAsync(admin.Id, company.Id, a.Id, default)).Should().BeNull();
    }

    private sealed class QueryCounter : DbCommandInterceptor
    {
        public int Count { get; set; }
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData,
            InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        { Count++; return ValueTask.FromResult(result); }
    }
    private sealed record Scope(long? CompanyId, long? BrandId, long? BranchId) : ICurrentTenantService;
}
