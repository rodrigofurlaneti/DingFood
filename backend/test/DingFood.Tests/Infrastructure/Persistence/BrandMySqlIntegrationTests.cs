using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Entities;
using DingFood.Infrastructure.Persistence;
using DingFood.Infrastructure.Persistence.Repositories;
using DingFood.Infrastructure.Tenancy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DingFood.Tests.Infrastructure.Persistence;

public sealed class BrandMySqlIntegrationTests
{
    // Opt-in, isolated server only. The ordinary suite never connects to an application database.
    public sealed class LocalMySqlFactAttribute : FactAttribute
    {
        public LocalMySqlFactAttribute()
        {
            if (Environment.GetEnvironmentVariable("DINGFOOD_ISOLATED_MYSQL_TEST") != "1")
                Skip = "Requires the isolated MySQL instance on 127.0.0.1:33197 with dump DDL only.";
        }
    }

    [LocalMySqlFact]
    public async Task DumpSchemaMigrationPreservesOwnershipAndCreatesBrandsWithRetryEnabled()
    {
        const string connection = "Server=127.0.0.1;Port=33197;User ID=root;Database=codex_brand_migration;Allow User Variables=true;SslMode=None";
        var options = new DbContextOptionsBuilder<AppDbContext>().UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 43)),
            mysql => mysql.EnableRetryOnFailure()).Options;
        await using var db = new AppDbContext(options);
        await db.Database.OpenConnectionAsync();
        // The dump already contains these four migrations; DDL-only import omits their history rows.
        await db.Database.ExecuteSqlRawAsync("""
            INSERT INTO `__efmigrationshistory` VALUES
              ('202609090001_AddIfoodWebhook','9.0.2'), ('202609090002_AddProductExtrasAndBoosts','9.0.2'),
              ('202609090003_AddOrderItemCustomizations','9.0.2'), ('202609090004_SeparateCustomerAuthentication','9.0.2');
            """);
        await db.Database.ExecuteSqlRawAsync("""
            INSERT INTO company (Id, LegalName, TradeName, Cnpj) VALUES (101, 'Test kitchen', 'Burger test', '11111111000111');
            INSERT INTO branch (Id, CompanyId, Name) VALUES (111, 101, 'Centro'), (112, 101, 'Norte');
            INSERT INTO jobtitle (Id, CompanyId, Name) VALUES (151, 101, 'Atendente');
            INSERT INTO employee (Id, BranchId, JobTitleId, Name, Cpf, HiredAt) VALUES (121, 111, 151, 'Synthetic worker', '11111111111', CURRENT_TIMESTAMP(6));
            INSERT INTO appuser (Id, CompanyId, EmployeeId, UserName, Email, PasswordHash) VALUES
              (131, 101, NULL, 'synthetic-admin', 'admin@example.test', 'test'), (132, 101, 121, 'synthetic-worker', 'worker@example.test', 'test');
            INSERT INTO role (Id, CompanyId, Name) VALUES (141, 101, 'Administrador'), (142, 101, 'Garçom'), (143, 101, 'Caixa');
            INSERT INTO userrole (CompanyId, AppUserId, RoleId) VALUES (101, 131, 141), (101, 132, 142), (101, 132, 143);
            INSERT INTO category (Id, CompanyId, Name) VALUES (171, 101, 'Legacy category');
            INSERT INTO unitofmeasure (Id, Name, Abbreviation) VALUES (181, 'Unidade', 'UN');
            INSERT INTO product (Id, CompanyId, CategoryId, UnitOfMeasureId, Name, SalePrice) VALUES (161, 101, 171, 181, 'Legacy burger', 10);
            """);
        await db.Database.MigrateAsync();
        var columns = await db.Database.SqlQueryRaw<string>("SELECT CONCAT(LOWER(TABLE_NAME), '.', COLUMN_NAME) AS Value FROM information_schema.columns WHERE table_schema = DATABASE()").ToListAsync();
        var missingColumns = db.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Select(p => e.GetTableName()!.ToLowerInvariant() + "." + p.GetColumnName()))
            .Where(c => !columns.Contains(c, StringComparer.OrdinalIgnoreCase)).ToArray();
        missingColumns.Should().BeEmpty("every mapped entity must be usable after the dump upgrade");
        var service = new WorkplaceAccessService(db, options, new CompanyAccessService(db));
        (await service.GetAllowedAsync(132, 101, default)).Select(b => b.Id).Should().Equal(111);
        (await service.GetAllowedAsync(132, 101, default)).Single().Roles.Should().BeEquivalentTo("Garçom", "Caixa");
        (await service.GetAllowedAsync(131, 101, default)).Should().HaveCount(2);
        (await db.Products.SingleAsync(p => p.Id == 161)).BrandId.Should().Be(101);

        var result = await service.CreateAsync(131, 101, new CreateWorkplace("Pizza test", "Sul"), default);
        result.IsSuccess.Should().BeTrue();
        var branch = await db.Branchs.SingleAsync(b => b.Id == result.Value);
        branch.BrandId.Should().NotBe(101);
        (await service.GetAllowedAsync(132, 101, default)).Select(b => b.Id).Should().Equal(111);
        await using var scoped = new AppDbContext(options, new Scope(101, branch.BrandId, branch.Id));
        (await scoped.Products.AnyAsync()).Should().BeFalse();
        var category = Category.Create(101, "Pizza only", 0).Value;
        scoped.Add(category); await scoped.SaveChangesAsync();
        category.BrandId.Should().Be(branch.BrandId);
        var company = Company.Create("Second test CNPJ", "Second test", "22222222000122", null, null).Value;
        company.AssignToGroup(await db.BusinessGroups.SingleAsync(g => g.Id == 101));
        var companyId = await new BusinessGroupRepository(db, options).AddCompanyWithAdministratorAsync(company, 131, "Matriz", default);
        (await service.GetAllowedAsync(131, companyId, default)).Should().ContainSingle();
    }

    private sealed record Scope(long? CompanyId, long? BrandId, long? BranchId) : ICurrentTenantService;
}
