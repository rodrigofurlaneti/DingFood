using DingFood.Domain.Entities;
using DingFood.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DingFood.Tests.Infrastructure.Persistence;

public sealed class OwnDeliveryMySqlTests
{
    public sealed class IsolatedDeliveryFactAttribute : FactAttribute
    {
        public IsolatedDeliveryFactAttribute() { if (Environment.GetEnvironmentVariable("DINGFOOD_DELIVERY_MYSQL_TEST") != "1") Skip = "Requires DDL-only dump in isolated MySQL on 33198."; }
    }
    [IsolatedDeliveryFact]
    public async Task DumpUpgradeMapsAllColumnsAndKeepsIfoodTable()
    {
        const string connection = "Server=127.0.0.1;Port=33198;User ID=root;Database=codex_delivery_migration;Allow User Variables=true;SslMode=None";
        var options = new DbContextOptionsBuilder<AppDbContext>().UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 43))).Options;
        await using var db = new AppDbContext(options);
        await db.Database.ExecuteSqlRawAsync("""
            INSERT IGNORE INTO `__efmigrationshistory` VALUES
              ('202609090001_AddIfoodWebhook','9.0.2'), ('202609090002_AddProductExtrasAndBoosts','9.0.2'),
              ('202609090003_AddOrderItemCustomizations','9.0.2'), ('202609090004_SeparateCustomerAuthentication','9.0.2'),
              ('202609100001_AddBusinessGroups','9.0.2'), ('202609100002_AddBrandsAndBranchAccess','9.0.2'), ('202609100003_CompleteLegacyOperationalSchema','9.0.2');
            INSERT INTO businessgroup (Id, Name, IsActive, CreatedAt) VALUES (101, 'Synthetic group', 1, CURRENT_TIMESTAMP(6));
            INSERT INTO company (Id, BusinessGroupId, LegalName, TradeName, Cnpj) VALUES (101, 101, 'Test logistics', 'Test', '11111111000111');
            INSERT INTO branch (Id, CompanyId, Name) VALUES (111, 101, 'Test branch');
            """);
        await db.Database.MigrateAsync();
        var columns = await db.Database.SqlQueryRaw<string>("SELECT CONCAT(LOWER(TABLE_NAME), '.', COLUMN_NAME) AS Value FROM information_schema.columns WHERE table_schema = DATABASE()").ToListAsync();
        db.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Select(p => e.GetTableName()!.ToLowerInvariant() + "." + p.GetColumnName())).Where(c => !columns.Contains(c, StringComparer.OrdinalIgnoreCase)).Should().BeEmpty();
        var config = DeliveryFeeConfig.Create(111); config.Configure("Daily", 100, 0, 0, "UTC"); config.Conditions.Add(DeliveryFeeCondition.Create(97, 1080, 1380, 8.5m, 1));
        var driver = DeliveryDriver.Create(111, "Synthetic Driver", "11900000000", "ABC1D23", "Fixo"); db.AddRange(config, driver); await db.SaveChangesAsync();
        var date = new DateOnly(2026, 9, 11); db.Add(DeliveryDriverDailyPayment.Create(111, driver.Id, date, 100)); await db.SaveChangesAsync();
        db.ChangeTracker.Clear(); (await db.Set<DeliveryFeeConfig>().Include(x => x.Conditions).SingleAsync()).Conditions.Should().ContainSingle();
        db.Add(DeliveryDriverDailyPayment.Create(111, driver.Id, date, 200));
        await FluentActions.Awaiting(() => db.SaveChangesAsync()).Should().ThrowAsync<DbUpdateException>();
        columns.Should().Contain(c => c.StartsWith("ifoodlogisticsdelivery."));
    }
}
