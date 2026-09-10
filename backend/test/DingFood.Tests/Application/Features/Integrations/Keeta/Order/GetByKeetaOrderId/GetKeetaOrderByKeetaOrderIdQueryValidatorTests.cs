using FluentAssertions;
using DingFood.Application.Features.Integrations.Keeta.Order.GetByKeetaOrderId;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Keeta.Order.GetByKeetaOrderId;

public sealed class GetKeetaOrderByKeetaOrderIdQueryValidatorTests
{
    private readonly GetKeetaOrderByKeetaOrderIdQueryValidator _validator = new();

    [Fact]
    public void Validate_NonEmptyKeetaOrderId_ShouldBeValid()
        => _validator.Validate(new GetKeetaOrderByKeetaOrderIdQuery("keeta-order-1")).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyKeetaOrderId_ShouldBeInvalid()
        => _validator.Validate(new GetKeetaOrderByKeetaOrderIdQuery(string.Empty)).IsValid.Should().BeFalse();
}
