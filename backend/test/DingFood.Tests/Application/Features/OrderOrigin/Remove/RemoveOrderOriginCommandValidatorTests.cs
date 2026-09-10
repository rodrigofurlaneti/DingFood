using FluentAssertions;
using DingFood.Application.Features.OrderOrigin.Remove;
using Xunit;

namespace DingFood.Tests.Application.Features.OrderOrigin.Remove;

public sealed class RemoveOrderOriginCommandValidatorTests
{
    private readonly RemoveOrderOriginCommandValidator _validator = new();

    [Fact]
    public void Validate_PositiveId_ShouldBeValid()
        => _validator.Validate(new RemoveOrderOriginCommand(1)).IsValid.Should().BeTrue();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_NonPositiveId_ShouldBeInvalid(long id)
        => _validator.Validate(new RemoveOrderOriginCommand(id)).IsValid.Should().BeFalse();
}
