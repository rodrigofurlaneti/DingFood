using FluentAssertions;
using DingFood.Domain.Entities;
using Xunit;

namespace DingFood.Tests.Domain.Entities;

public sealed class StockItemBalanceTests
{
    [Fact]
    public void Decrease_BelowZero_ShouldFail()
    {
        var stock = StockItem.Create(1, 1, 10, null).Value;
        stock.Increase(5);

        var result = stock.Decrease(6);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("StockItem.InsufficientStock");
        stock.CurrentQuantity.Should().Be(5);
    }

    [Fact]
    public void IncreaseAndDecrease_ShouldTrackBalance()
    {
        var stock = StockItem.Create(1, 1, 10, null).Value;

        stock.Increase(20);
        stock.Decrease(8);

        stock.CurrentQuantity.Should().Be(12);
        stock.IsBelowMinimum().Should().BeFalse();
    }
}
