using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Features.Integrations.Ifood.Orders;
using DingFood.Domain.Constants;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;
using DingFood.Infrastructure.Persistence;
using DingFood.Infrastructure.Persistence.Repositories;
using DingFood.Tests.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Xunit;

namespace DingFood.Tests.Infrastructure.Integrations.IFood;

public sealed class DarkKitchenOrderIntegrationTests
{
    [Fact]
    public async Task SimultaneousMerchantEventsPersistOnlyInTheirOwnCompany()
    {
        var path = Path.Combine(Path.GetTempPath(), $"dingfood-darkkitchen-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={path};Foreign Keys=False;Pooling=False")
            .ReplaceService<IModelCustomizer, SqliteCompatibleModelCustomizer>().Options;
        try
        {
            long companyA, companyB;
            await using (var seed = new AppDbContext(options))
            {
                await seed.Database.EnsureCreatedAsync();
                var a = Company.Create("Burger", "Burger", "11111111000111", null, null).Value;
                seed.Companies.Add(a); await seed.SaveChangesAsync();
                var b = Company.Create("Pizza", "Pizza", "22222222000122", null, null).Value;
                b.AssignToGroup(a.BusinessGroup);
                seed.Companies.Add(b); await seed.SaveChangesAsync();
                companyA = a.Id; companyB = b.Id;
                var origin = OrderOrigin.Create(null, null, "IFOOD", DateTime.UtcNow).Value;
                typeof(DingFood.Domain.Primitives.Entity).GetProperty("Id")!.SetValue(origin, OrderOriginIds.IFood);
                seed.Set<OrderOrigin>().Add(origin);
                foreach (var company in new[] { a, b })
                {
                    var branch = Branch.Create(company.Id, company.TradeName, null, null, null, null, null, null, null, null).Value;
                    seed.Branchs.Add(branch); await seed.SaveChangesAsync();
                    var employee = Employee.Create(branch.Id, 1, company.TradeName, $"0000000000{company.Id}", null, null, DateTime.Now, null, null).Value;
                    seed.Employees.Add(employee); await seed.SaveChangesAsync();
                    branch.SetSelfServiceEmployee(employee.Id);
                    var setting = IfoodIntegrationSetting.Create(company.Id).Value;
                    setting.SaveCredentials($"client-{company.Id}", $"secret-{company.Id}", true, null);
                    var mapping = IfoodMerchantMapping.Create(branch.Id).Value;
                    mapping.SetMerchant($"merchant-{company.Id}", $"merchant-{company.Id}");
                    seed.IfoodIntegrationSettings.Add(setting); seed.IfoodMerchantMappings.Add(mapping);
                    await seed.SaveChangesAsync();
                }
            }
            var reached = 0;
            var bothRequests = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            using var cache = new MemoryCache(new MemoryCacheOptions());
            async Task Process(long companyId)
            {
                await using var db = new AppDbContext(options, new FakeCurrentTenantService { CompanyId = companyId });
                var tokens = Substitute.For<IIfoodTokenProvider>();
                tokens.GetAccessTokenAsync(companyId, Arg.Any<CancellationToken>()).Returns($"token-{companyId}");
                var client = Substitute.For<IIfoodOrderClient>();
                var orderId = $"order-{companyId}";
                var merchantId = $"merchant-{companyId}";
                client.GetOrderDetailsAsync($"token-{companyId}", orderId, Arg.Any<CancellationToken>()).Returns(async _ =>
                {
                    if (Interlocked.Increment(ref reached) == 2) bothRequests.SetResult();
                    await bothRequests.Task.WaitAsync(TimeSpan.FromSeconds(10));
                    return (IfoodOrderDetailsDto?)new IfoodOrderDetailsDto(orderId, "001", "DELIVERY", "IMMEDIATE", "FOOD", DateTime.Now,
                        null, merchantId, "Cliente", null, "Rua de teste", "Ifood", null, 0, []);
                });
                client.ConfirmOrderAsync($"token-{companyId}", orderId, Arg.Any<CancellationToken>()).Returns(new IfoodOrderActionResult(true, null));
                var handler = new SyncIfoodOrdersCommandHandler(new IfoodIntegrationSettingRepository(db), tokens, client,
                    new IfoodMerchantMappingRepository(db), new IfoodOrderRepository(db), new CustomerOrderRepository(db),
                    Substitute.For<IProductRepository>(), Substitute.For<ICategoryRepository>(), Substitute.For<IUnitOfMeasureRepository>(),
                    new BranchRepository(db), Substitute.For<IComplementGroupRepository>(), Substitute.For<IIfoodComplementMappingRepository>(),
                    TimeProvider.System, cache, Substitute.For<ILogTrackerRepository>(), db);
                var result = await handler.Handle(new SyncIfoodOrdersCommand(companyId,
                    [new IfoodPollingEvent("same-event-id", "PLC", "PLACED", orderId, DateTime.Now, merchantId)]), default);
                result.IsSuccess.Should().BeTrue();
                var order = await db.CustomerOrders.SingleAsync();
                var branch = await db.Branchs.SingleAsync();
                order.BranchId.Should().Be(branch.Id);
                order.OrderOriginId.Should().Be(OrderOriginIds.IFood);
                (await db.IfoodOrders.SingleAsync()).MerchantId.Should().Be(merchantId);
                await tokens.DidNotReceive().GetAccessTokenAsync(Arg.Is<long>(id => id != companyId), Arg.Any<CancellationToken>());
            }
            await Task.WhenAll(Task.Run(() => Process(companyA)), Task.Run(() => Process(companyB)));
            await using var verify = new AppDbContext(options);
            (await verify.CustomerOrders.CountAsync()).Should().Be(2);
        }
        finally { File.Delete(path); }
    }
}
