using DingFood.Domain.Entities;
using DingFood.Infrastructure.Persistence;
using DingFood.Infrastructure.Tenancy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DingFood.Tests.Infrastructure.Persistence;

public sealed class WorkplacePerformanceMySqlTests
{
    [OwnDeliveryMySqlTests.IsolatedDeliveryFact]
    public async Task BatchedAuthorizationTranslatesOnMySqlAndPreservesLegacyRoles()
    {
        const string connection = "Server=127.0.0.1;Port=33198;User ID=root;Database=codex_delivery_migration;Allow User Variables=true;SslMode=None";
        var options = new DbContextOptionsBuilder<AppDbContext>().UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 43))).Options;
        await using var db = new AppDbContext(options);
        await using var transaction = await db.Database.BeginTransactionAsync();
        var company = Company.Create("Performance test", "Performance", "98765432000199", null, null).Value;
        db.Add(company); await db.SaveChangesAsync();
        var branch = Branch.Create(company.Id, "Synthetic", null, null, null, null, null, null, null, null).Value;
        db.Add(branch); await db.SaveChangesAsync();
        var user = AppUser.Create(company.Id, null, "perf-test", "perf@example.test", "synthetic-hash").Value;
        var role = Role.Create(company.Id, "Administrador", null).Value;
        db.AddRange(user, role); await db.SaveChangesAsync();
        db.Add(AppUserCompany.CreateHomeAccess(user).Value);
        db.Add(UserRole.Create(company.Id, user.Id, role.Id).Value);
        var grant = AppUserBranch.Create(user.Id, branch.Id);
        db.Add(grant); db.Entry(grant).Property(x => x.UsesLegacyRoles).CurrentValue = true;
        await db.SaveChangesAsync();
        var service = new WorkplaceAccessService(db, options, new CompanyAccessService(db));
        var selected = await service.ResolveAsync(user.Id, company.Id, branch.Id, default);
        selected.Should().NotBeNull(); selected!.Roles.Should().Equal("Administrador");
        (await service.GetAllowedAsync(user.Id, company.Id, default)).Should().ContainSingle();
        (await service.ResolveAsync(user.Id, company.Id, null, default))!.Id.Should().Be(branch.Id);
        (await service.ResolveAsync(user.Id, company.Id, long.MaxValue, default)).Should().BeNull();
        grant.Update(null, false); await db.SaveChangesAsync();
        (await service.ResolveAsync(user.Id, company.Id, branch.Id, default)).Should().BeNull();
        await transaction.RollbackAsync();
    }
}
