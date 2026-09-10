using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using FluentAssertions;
using Reqnroll;
using Moq;
using DingFood.Domain.Repositories;
using DingFood.Application.Abstractions.Authentication;
using DingFood.Application.Abstractions.Security;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Features.Companies;
using DingFood.Application.Features.Integrations.Ifood.Orders;
using Microsoft.Extensions.Caching.Memory;

namespace DingFood.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Dark Kitchen com empresas independentes")]
public sealed class DarkKitchenSteps
{
    private Company _burger = null!, _pizza = null!, _other = null!;
    private AppUser _user = null!;
    private Result<AppUserCompany> _grant = null!;
    private IfoodIntegrationSetting _settingA = null!, _settingB = null!;
    private StockItem _stockA = null!, _stockB = null!;
    private Result<SwitchCompanyResponse> _session = null!;
    private CustomerOrder? _order;

    private static void Id(Entity entity, long id) => typeof(Entity).GetProperty(nameof(Entity.Id))!.SetValue(entity, id);

    [Given("um grupo Dark Kitchen com Burger e Pizza")]
    public void Group()
    {
        var group = BusinessGroup.Create("Dark Kitchen Centro").Value;
        Id(group, 1);
        _burger = Company.Create("Burger", "Burger", "11111111000111", null, null).Value;
        _pizza = Company.Create("Pizza", "Pizza", "22222222000122", null, null).Value;
        _burger.AssignToGroup(group).IsSuccess.Should().BeTrue();
        _pizza.AssignToGroup(group).IsSuccess.Should().BeTrue();
        Id(_burger, 10); Id(_pizza, 20);
    }
    [Then("as duas marcas pertencem ao mesmo grupo")]
    public void SameGroup() => _burger.BusinessGroupId.Should().Be(_pizza.BusinessGroupId);
    [Then("cada marca preserva seu CNPJ")]
    public void DifferentCnpj() => _burger.Cnpj.Should().NotBe(_pizza.Cnpj);
    [Given("um usuário da Burger e uma empresa de outro grupo")]
    public void OtherGroup()
    {
        _user = AppUser.Create(_burger.Id, null, "admin", "admin@example.test", "hash").Value;
        Id(_user, 1);
        _other = Company.Create("Outra", "Outra", "33333333000133", null, null).Value;
        var group = BusinessGroup.Create("Outro grupo").Value;
        Id(group, 2); _other.AssignToGroup(group); Id(_other, 30);
    }
    [When("tento conceder acesso à empresa de outro grupo")]
    public void Grant() => _grant = AppUserCompany.Create(_user, _burger, _other);
    [Then("a concessão é rejeitada")]
    public void Rejected() => _grant.IsFailure.Should().BeTrue();
    [When("cada empresa configura suas credenciais iFood")]
    public void Credentials()
    {
        _settingA = IfoodIntegrationSetting.Create(_burger.Id).Value;
        _settingB = IfoodIntegrationSetting.Create(_pizza.Id).Value;
        _settingA.SaveCredentials("client-a", "encrypted-a", true, null);
        _settingB.SaveCredentials("client-b", "encrypted-b", true, null);
    }
    [Then("os identificadores e segredos permanecem separados")]
    public void IndependentCredentials()
    {
        _settingA.CompanyId.Should().NotBe(_settingB.CompanyId);
        _settingA.ClientId.Should().NotBe(_settingB.ClientId);
        _settingA.ClientSecretEncrypted.Should().NotBe(_settingB.ClientSecretEncrypted);
    }
    [When("uma saída de estoque ocorre na Burger")]
    public void StockMovement()
    {
        _stockA = StockItem.Create(10, 100, 0, null).Value;
        _stockB = StockItem.Create(20, 200, 0, null).Value;
        _stockA.Increase(10); _stockB.Increase(40); _stockA.Decrease(1);
    }
    [Then("o estoque da Pizza permanece intacto")]
    public void IndependentStock()
    {
        _stockA.CurrentQuantity.Should().Be(9);
        _stockB.CurrentQuantity.Should().Be(40);
    }

    [When("o administrador autorizado alterna para Pizza")]
    public async Task SwitchCompany()
    {
        var user = AppUser.Create(_burger.Id, null, "admin", "admin@example.test", "hash").Value;
        Id(user, 1);
        var identity = new Mock<ICurrentUserService>(); identity.SetupGet(x => x.UserId).Returns(1);
        var access = new Mock<ICompanyAccessService>();
        var target = new CompanyAccess(_pizza.Id, 1, "Dark Kitchen Centro", "Pizza", _pizza.Cnpj, null, ["Administrador"], []);
        access.Setup(x => x.ResolveAsync(1, _pizza.Id, It.IsAny<CancellationToken>())).ReturnsAsync(target);
        var groups = new Mock<IBusinessGroupRepository>();
        groups.Setup(x => x.GetUserHomeAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((user, _burger));
        var tokens = new Mock<IJwtTokenProvider>();
        tokens.Setup(x => x.GenerateCompanyToken(user, target)).Returns(new AccessToken("pizza-token", DateTime.UtcNow.AddHours(1)));
        _session = await new SwitchCompanyCommandHandler(access.Object, identity.Object, groups.Object, tokens.Object)
            .Handle(new SwitchCompanyCommand(_pizza.Id), default);
    }

    [Then("a sessão representa a empresa Pizza")]
    public void SessionCompany()
    {
        _session.IsSuccess.Should().BeTrue();
        _session.Value.CompanyId.Should().Be(_pizza.Id);
        _session.Value.BusinessGroupId.Should().Be(_pizza.BusinessGroupId);
    }

    [When("chega um pedido do merchant da Pizza")]
    public async Task ReceivePizzaOrder()
    {
        var settings = new Mock<IIfoodIntegrationSettingRepository>();
        var setting = IfoodIntegrationSetting.Create(_pizza.Id).Value;
        setting.SaveCredentials("pizza-client", "pizza-secret", true, null);
        settings.Setup(x => x.GetByCompanyAsync(_pizza.Id, It.IsAny<CancellationToken>())).ReturnsAsync(setting);
        var tokens = new Mock<IIfoodTokenProvider>();
        tokens.Setup(x => x.GetAccessTokenAsync(_pizza.Id, It.IsAny<CancellationToken>())).ReturnsAsync("pizza-token");
        var mapping = IfoodMerchantMapping.Create(200).Value;
        mapping.SetMerchant("pizza-merchant", "pizza-merchant");
        var mappings = new Mock<IIfoodMerchantMappingRepository>();
        mappings.Setup(x => x.GetByCompanyAsync(_pizza.Id, It.IsAny<CancellationToken>())).ReturnsAsync(new Dictionary<long, IfoodMerchantMapping> { [200] = mapping });
        var branch = Branch.Create(_pizza.Id, "Pizza", null, null, null, null, null, null, null, null).Value;
        Id(branch, 200); branch.SetSelfServiceEmployee(2);
        var branches = new Mock<IBranchRepository>();
        branches.Setup(x => x.GetByIdAsync(200, It.IsAny<CancellationToken>())).ReturnsAsync(branch);
        var client = new Mock<IIfoodOrderClient>();
        client.Setup(x => x.GetOrderDetailsAsync("pizza-token", "order-pizza", It.IsAny<CancellationToken>())).ReturnsAsync(
            new IfoodOrderDetailsDto("order-pizza", "001", "DELIVERY", "IMMEDIATE", "FOOD", DateTime.Now, null,
                "pizza-merchant", "Cliente", null, "Rua de teste", "Ifood", null, 0, []));
        client.Setup(x => x.ConfirmOrderAsync("pizza-token", "order-pizza", It.IsAny<CancellationToken>())).ReturnsAsync(new IfoodOrderActionResult(true, null));
        var orders = new Mock<ICustomerOrderRepository>();
        orders.Setup(x => x.AddAsync(It.IsAny<CustomerOrder>(), It.IsAny<CancellationToken>())).Callback<CustomerOrder, CancellationToken>((order, _) => _order = order).Returns(Task.CompletedTask);
        using var cache = new MemoryCache(new MemoryCacheOptions());
        var handler = new SyncIfoodOrdersCommandHandler(settings.Object, tokens.Object, client.Object, mappings.Object,
            Mock.Of<IIfoodOrderRepository>(), orders.Object, Mock.Of<IProductRepository>(), Mock.Of<ICategoryRepository>(),
            Mock.Of<IUnitOfMeasureRepository>(), branches.Object, Mock.Of<IComplementGroupRepository>(), Mock.Of<IIfoodComplementMappingRepository>(),
            TimeProvider.System, cache, Mock.Of<ILogTrackerRepository>(), Mock.Of<IUnitOfWork>());
        var result = await handler.Handle(new SyncIfoodOrdersCommand(_pizza.Id,
            [new IfoodPollingEvent("event-pizza", "PLC", "PLACED", "order-pizza", DateTime.Now, "pizza-merchant")]), default);
        result.IsSuccess.Should().BeTrue();
    }

    [Then("o pedido pertence somente à filial da Pizza")]
    public void RoutedOrder()
    {
        _order.Should().NotBeNull();
        _order!.BranchId.Should().Be(200);
    }
}
