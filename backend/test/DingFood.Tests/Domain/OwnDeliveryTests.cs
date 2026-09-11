using DingFood.Domain.Constants;
using DingFood.Domain.Entities;
using DingFood.Infrastructure.Delivery;
using FluentAssertions;
using Xunit;

namespace DingFood.Tests.Domain;

public sealed class OwnDeliveryTests
{
    private static DeliveryFeeConfig Config()
    {
        var config = DeliveryFeeConfig.Create(1); config.Configure("PerKm", 100, 10, 6.99m, "America/Sao_Paulo"); return config;
    }
    [Theory]
    [InlineData("2026-09-11T20:59:00Z", 13.98)]
    [InlineData("2026-09-11T21:00:00Z", 17)]
    [InlineData("2026-09-12T01:59:00Z", 17)]
    [InlineData("2026-09-12T02:00:00Z", 13.98)]
    [InlineData("2026-09-14T21:00:00Z", 13.98)]
    public void DynamicRatesRespectDaysAndLocalTime(string instant, decimal expected)
    {
        var config = Config(); config.Conditions.Add(DeliveryFeeCondition.Create(97, 1080, 1380, 8.5m, 1));
        config.Calculate(2, DateTimeOffset.Parse(instant)).Amount.Should().Be(expected);
    }
    [Theory]
    [InlineData("2026-09-12T01:00:00Z", true)]
    [InlineData("2026-09-12T04:59:00Z", true)]
    [InlineData("2026-09-12T05:00:00Z", false)]
    [InlineData("2026-09-13T04:00:00Z", false)]
    public void OvernightRuleBelongsToStartingDay(string instant, bool match)
    {
        var config = Config(); config.Conditions.Add(DeliveryFeeCondition.Create(32, 1320, 120, 9, 1));
        config.Calculate(1, DateTimeOffset.Parse(instant)).PricePerKm.Should().Be(match ? 9 : 6.99m);
    }
    [Fact] public void PriorityAndRoundingAreDeterministic()
    {
        var config = Config(); config.Conditions.Add(DeliveryFeeCondition.Create(127, 0, 1439, 8, 1)); config.Conditions.Add(DeliveryFeeCondition.Create(127, 0, 1439, 9, 2));
        config.Calculate(1.005m, DateTimeOffset.Parse("2026-09-11T15:00Z")).Amount.Should().Be(9.05m);
    }
    [Fact] public void RadiusIncludesBoundaryButRejectsBeyondIt()
    {
        var config = Config(); config.Calculate(10, DateTimeOffset.UtcNow).Amount.Should().Be(69.90m);
        FluentActions.Invoking(() => config.Calculate(10.000001m, DateTimeOffset.UtcNow)).Should().Throw<ArgumentException>();
    }
    [Fact] public void DailyDoesNotChargePerOrderAndSnapshotSurvivesConfigChanges()
    {
        var config = Config(); config.Configure("Daily", 120, 0, 0, "UTC");
        var quote = config.Calculate(999, DateTimeOffset.UtcNow); quote.Amount.Should().Be(0); quote.DailyAmount.Should().Be(120);
        var order = CustomerOrder.Create(1, null, null, 1, null, null, DateTime.UtcNow, orderTypeId: OrderTypeIds.Delivery, customerName: "Test", deliveryAddress: "Test").Value;
        order.FreezeDeliveryFee(quote, DateTime.UtcNow, "UTC");
        config.Configure("Daily", 300, 0, 0, "UTC"); order.DeliveryDailyAmount.Should().Be(120);
        FluentActions.Invoking(() => order.FreezeDeliveryFee(quote, DateTime.UtcNow, "UTC")).Should().Throw<InvalidOperationException>();
    }
    [Fact] public void FeeRemainsInTotalAfterItemsChange()
    {
        var order = CustomerOrder.Create(1, null, null, 1, null, null, DateTime.UtcNow, orderTypeId: OrderTypeIds.Delivery, customerName: "Test", deliveryAddress: "Test").Value;
        order.FreezeDeliveryFee(Config().Calculate(2, DateTimeOffset.UtcNow), DateTime.UtcNow, "UTC");
        order.AddItem(1, 20, 2, null, 1, DateTime.UtcNow); order.TotalAmount.Should().Be(53.98m);
        FluentActions.Invoking(() => order.AssignDeliveryDriver(DeliveryDriver.Create(2, "Test", "123", "ABC1234", "Fixo"))).Should().Throw<ArgumentException>();
    }
    [Fact] public void HaversineUsesKilometres()
    {
        GoogleDeliveryGeocoder.Distance(0, 0, 0, 0).Should().Be(0);
        GoogleDeliveryGeocoder.Distance(0, 0, 0, 1).Should().BeApproximately(111.195m, .001m);
    }
}
