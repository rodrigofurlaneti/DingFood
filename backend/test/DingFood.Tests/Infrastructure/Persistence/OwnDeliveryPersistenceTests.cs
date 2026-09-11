using DingFood.API.Controllers;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Constants;
using DingFood.Domain.Entities;
using DingFood.Domain.Exceptions;
using DingFood.Infrastructure.Delivery;
using DingFood.Infrastructure.Persistence;
using DingFood.Tests.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace DingFood.Tests.Infrastructure.Persistence;

public sealed class OwnDeliveryPersistenceTests : RepositoryTestBase
{
    private DbContextOptions<AppDbContext> Options => new DbContextOptionsBuilder<AppDbContext>().UseSqlite(Connection).ReplaceService<IModelCustomizer, SqliteCompatibleModelCustomizer>().Options;
    private sealed record Scope(long? CompanyId, long? BranchId) : ICurrentTenantService;
    private sealed class Geo(decimal distance) : IDeliveryGeocoder
    { public Task<decimal> DistanceKmAsync(string origin, string destination, CancellationToken ct) => Task.FromResult(distance); }
    private async Task<Branch> Seed(string model = "PerKm")
    {
        var branch = Branch.Create(1, "Centro", null, null, "Rua Teste", "1", null, "São Paulo", "SP", "01001000").Value;
        Context.Add(branch); await Context.SaveChangesAsync();
        var config = DeliveryFeeConfig.Create(branch.Id); config.Configure(model, 120, 10, 6.99m, "UTC"); Context.Add(config); await Context.SaveChangesAsync();
        return branch;
    }
    private static CustomerOrder Order(long branch, long origin = OrderOriginIds.Local) => CustomerOrder.Create(branch, null, null, 1, null, null, DateTime.UtcNow, orderTypeId: OrderTypeIds.Delivery, orderOriginId: origin, customerName: "Synthetic", deliveryAddress: "Rua Destino 2").Value;

    [Fact] public async Task CreationPricesAndReloadKeepsHistoricalAmount()
    {
        var branch = await Seed();
        using var db = new AppDbContext(Options, null, new Geo(2));
        var order = Order(branch.Id); db.Add(order); await db.SaveChangesAsync();
        order.TotalAmount.Should().Be(13.98m);
        var config = await db.Set<DeliveryFeeConfig>().SingleAsync(); config.Configure("PerKm", 0, 1, 100, "UTC"); await db.SaveChangesAsync();
        db.ChangeTracker.Clear(); var saved = await db.CustomerOrders.SingleAsync(); saved.DeliveryFeeAmount.Should().Be(13.98m);
        saved.AddItem(1, 10, 1, null, 1, DateTime.UtcNow); await db.SaveChangesAsync(); saved.TotalAmount.Should().Be(23.98m);
        db.Entry(saved).Property(x => x.DeliveryFeeAmount).CurrentValue = 999;
        await FluentActions.Awaiting(() => db.SaveChangesAsync()).Should().ThrowAsync<InvalidOperationException>();
    }
    [Fact] public async Task RejectedOrderCannotBeSavedByAuditRetry()
    {
        var branch = await Seed(); using var db = new AppDbContext(Options, null, new Geo(11));
        db.Add(Order(branch.Id)); await FluentActions.Awaiting(() => db.SaveChangesAsync()).Should().ThrowAsync<DeliveryPricingException>();
        await db.SaveChangesAsync(); (await db.CustomerOrders.CountAsync()).Should().Be(0);
    }
    [Fact] public async Task IfoodDoesNotUseOwnDeliveryPricing()
    {
        var branch = await Seed(); using var db = new AppDbContext(Options, null, new Geo(999));
        var order = Order(branch.Id, OrderOriginIds.IFood); db.Add(order); await db.SaveChangesAsync(); order.DeliveryFeeCalculatedAt.Should().BeNull();
    }
    [Fact] public async Task DailyAssignmentIsIdempotentAcrossOrdersAndConfigChanges()
    {
        var branch = await Seed("Daily"); using var db = new AppDbContext(Options);
        var driver = DeliveryDriver.Create(branch.Id, "Synthetic", "123", "ABC1234", "Freelancer"); db.Add(driver); await db.SaveChangesAsync();
        var first = Order(branch.Id); var second = Order(branch.Id); Context.AddRange(first, second); await Context.SaveChangesAsync();
        var controller = new DeliveryController(db, new Scope(1, branch.Id));
        (await controller.Assign(first.Id, new(driver.Id), default)).Should().BeOfType<NoContentResult>();
        (await controller.Assign(first.Id, new(driver.Id), default)).Should().BeOfType<NoContentResult>();
        (await controller.Assign(second.Id, new(driver.Id), default)).Should().BeOfType<NoContentResult>();
        var config = await db.Set<DeliveryFeeConfig>().SingleAsync(); config.Configure("Daily", 300, 0, 0, "UTC"); await db.SaveChangesAsync();
        var payment = await db.Set<DeliveryDriverDailyPayment>().SingleAsync(); payment.Amount.Should().Be(120);
        first.TotalAmount.Should().Be(0); second.TotalAmount.Should().Be(0);
    }
    [Fact] public async Task DailyCanBeRegisteredWithoutOrdersAndUniqueConstraintPreventsDuplicate()
    {
        var branch = await Seed("Daily"); using var db = new AppDbContext(Options, new Scope(1, branch.Id));
        var driver = DeliveryDriver.Create(branch.Id, "Synthetic", "123", "ABC1234", "Fixo"); db.Add(driver); await db.SaveChangesAsync();
        var controller = new DeliveryController(db, new Scope(1, branch.Id)); var date = new DateOnly(2026, 9, 11);
        (await controller.RegisterDaily(new(driver.Id, date), default)).Should().BeOfType<OkObjectResult>();
        (await controller.RegisterDaily(new(driver.Id, date), default)).Should().BeOfType<OkObjectResult>();
        (await db.Set<DeliveryDriverDailyPayment>().CountAsync()).Should().Be(1);
        db.Add(DeliveryDriverDailyPayment.Create(branch.Id, driver.Id, date, 999));
        await FluentActions.Awaiting(() => db.SaveChangesAsync()).Should().ThrowAsync<DbUpdateException>();
    }
    [Fact] public async Task BranchScopeProtectsDriversConfigsAndConditions()
    {
        var branch = await Seed(); var other = Branch.Create(1, "Other", null, null, null, null, null, null, null, null).Value;
        Context.Add(other); await Context.SaveChangesAsync();
        Context.Add(DeliveryDriver.Create(other.Id, "Other", "123", "ABC1234", "Fixo"));
        var config = DeliveryFeeConfig.Create(other.Id); config.Configure("Daily", 99, 0, 0, "UTC"); config.Conditions.Add(DeliveryFeeCondition.Create(127, 1, 2, 10, 1)); Context.Add(config); await Context.SaveChangesAsync();
        using var scoped = new AppDbContext(Options, new Scope(1, branch.Id));
        (await scoped.Set<DeliveryDriver>().CountAsync()).Should().Be(0);
        (await scoped.Set<DeliveryFeeConfig>().CountAsync()).Should().Be(1);
        (await scoped.Set<DeliveryFeeCondition>().CountAsync()).Should().Be(0);
        scoped.Add(DeliveryDriver.Create(other.Id, "Injected", "123", "ABC1234", "Fixo"));
        await FluentActions.Awaiting(() => scoped.SaveChangesAsync()).Should().ThrowAsync<TenantAccessException>();
    }
}
