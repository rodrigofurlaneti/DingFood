using DingFood.Domain.Entities;
using DingFood.Domain.Exceptions;
using DingFood.Infrastructure.Tenancy;
using DingFood.Tests.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DingFood.Tests.Infrastructure.Persistence;

public sealed class BusinessGroupIsolationTests : RepositoryTestBase
{
    private async Task<(Company A, Company B, Company Other, AppUser User)> SeedAsync()
    {
        var a = Company.Create("Burger Ltda", "Burger", "11111111000111", null, null).Value;
        Context.Companies.Add(a);
        await Context.SaveChangesAsync();
        var b = Company.Create("Pizza Ltda", "Pizza", "22222222000122", null, null).Value;
        b.AssignToGroup(a.BusinessGroup).IsSuccess.Should().BeTrue();
        var other = Company.Create("Outra Ltda", "Outra", "33333333000133", null, null).Value;
        Context.Companies.AddRange(b, other);
        await Context.SaveChangesAsync();
        var user = AppUser.Create(a.Id, null, "admin", "admin@example.test", "test-hash").Value;
        Context.AppUsers.Add(user);
        Context.AppUserCompanies.Add(AppUserCompany.CreateHomeAccess(user).Value);
        await Context.SaveChangesAsync();
        return (a, b, other, user);
    }

    [Fact]
    public async Task CreationRunsInsideRetryStrategyAndPersistsAdministratorAndBranch()
    {
        var (a, _, _, user) = await SeedAsync();
        var options = new DbContextOptionsBuilder<DingFood.Infrastructure.Persistence.AppDbContext>()
            .UseSqlite(Connection, sqlite => sqlite.ExecutionStrategy(dependencies => new TestRetryStrategy(dependencies)))
            .ReplaceService<Microsoft.EntityFrameworkCore.Infrastructure.IModelCustomizer, SqliteCompatibleModelCustomizer>()
            .Options;
        await using var retryContext = new DingFood.Infrastructure.Persistence.AppDbContext(options);
        var repository = new DingFood.Infrastructure.Persistence.Repositories.BusinessGroupRepository(retryContext, options);
        var company = Company.Create("Nova Ltda", "Nova", "44444444000144", null, null).Value;
        company.AssignToGroup(a.BusinessGroup);
        var id = await repository.AddCompanyWithAdministratorAsync(company, user.Id, "Matriz", default);
        (await Context.Companies.SingleAsync(c => c.Id == id)).BusinessGroupId.Should().Be(a.BusinessGroupId);
        (await Context.Branchs.SingleAsync(b => b.CompanyId == id)).Name.Should().Be("Matriz");
        (await Context.AppUserCompanies.SingleAsync(g => g.CompanyId == id)).AppUserId.Should().Be(user.Id);
        (await Context.UserRoles.SingleAsync(r => r.CompanyId == id)).AppUserId.Should().Be(user.Id);
    }

    private sealed class TestRetryStrategy(Microsoft.EntityFrameworkCore.Storage.ExecutionStrategyDependencies dependencies)
        : Microsoft.EntityFrameworkCore.Storage.ExecutionStrategy(dependencies, 1, TimeSpan.Zero)
    {
        protected override bool ShouldRetryOn(Exception exception) => false;
    }
    [Fact]
    public async Task GroupDoesNotGrantAccessToSiblingOrForeignCompany()
    {
        var (a, b, other, user) = await SeedAsync();
        var access = new CompanyAccessService(Context);
        (await access.GetAllowedAsync(user.Id, default)).Select(x => x.CompanyId).Should().Equal(a.Id);
        (await access.ResolveAsync(user.Id, b.Id, default)).Should().BeNull();
        AppUserCompany.Create(user, a, other).IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task ExplicitGrantAllowsSiblingAndRevocationTakesEffectImmediately()
    {
        var (a, b, _, user) = await SeedAsync();
        var grant = AppUserCompany.Create(user, a, b).Value;
        Context.AppUserCompanies.Add(grant);
        await Context.SaveChangesAsync();
        TenantService.CompanyId = a.Id;
        var access = new CompanyAccessService(Context);
        (await access.ResolveAsync(user.Id, b.Id, default)).Should().NotBeNull();
        grant.Revoke();
        await Context.SaveChangesAsync();
        (await access.ResolveAsync(user.Id, b.Id, default)).Should().BeNull();
    }

    [Fact]
    public async Task SwitchingCompanyKeepsCatalogSettingsAndBranchesIsolated()
    {
        var (a, b, _, _) = await SeedAsync();
        Context.Categories.AddRange(Category.Create(a.Id, "Burger", 0).Value, Category.Create(b.Id, "Pizza", 0).Value);
        Context.Branchs.AddRange(Branch.Create(a.Id, "A", null, null, null, null, null, null, null, null).Value,
            Branch.Create(b.Id, "B", null, null, null, null, null, null, null, null).Value);
        var settingA = IfoodIntegrationSetting.Create(a.Id).Value;
        settingA.SaveCredentials("client-a", "secret-a", true, null);
        var settingB = IfoodIntegrationSetting.Create(b.Id).Value;
        settingB.SaveCredentials("client-b", "secret-b", true, null);
        Context.IfoodIntegrationSettings.AddRange(settingA, settingB);
        await Context.SaveChangesAsync();
        TenantService.CompanyId = a.Id;
        (await Context.Categories.SingleAsync()).Name.Should().Be("Burger");
        (await Context.IfoodIntegrationSettings.SingleAsync()).ClientId.Should().Be("client-a");
        (await Context.Branchs.SingleAsync()).CompanyId.Should().Be(a.Id);
        TenantService.CompanyId = b.Id;
        (await Context.Categories.SingleAsync()).Name.Should().Be("Pizza");
        (await Context.IfoodIntegrationSettings.SingleAsync()).ClientId.Should().Be("client-b");
        (await Context.Branchs.SingleAsync()).CompanyId.Should().Be(b.Id);
    }

    [Fact]
    public async Task WriteWithForeignCompanyIsRejected()
    {
        var (a, b, _, _) = await SeedAsync();
        TenantService.CompanyId = a.Id;
        Context.Categories.Add(Category.Create(b.Id, "Injected", 0).Value);
        await FluentActions.Awaiting(() => Context.SaveChangesAsync()).Should().ThrowAsync<TenantAccessException>();
    }

    [Fact]
    public async Task MerchantCannotReferenceForeignBranch()
    {
        var (a, b, _, _) = await SeedAsync();
        var branch = Branch.Create(b.Id, "Pizza", null, null, null, null, null, null, null, null).Value;
        Context.Branchs.Add(branch);
        await Context.SaveChangesAsync();
        TenantService.CompanyId = a.Id;
        Context.IfoodMerchantMappings.Add(IfoodMerchantMapping.Create(branch.Id).Value);
        await FluentActions.Awaiting(() => Context.SaveChangesAsync()).Should().ThrowAsync<TenantAccessException>();
    }

    [Fact]
    public async Task CompanyCannotMoveBetweenGroups()
    {
        var (a, _, other, _) = await SeedAsync();
        a.AssignToGroup(other.BusinessGroup).IsFailure.Should().BeTrue();
    }
}
