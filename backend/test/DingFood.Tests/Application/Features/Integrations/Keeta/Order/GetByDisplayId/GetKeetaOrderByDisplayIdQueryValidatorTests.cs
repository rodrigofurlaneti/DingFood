using FluentAssertions;
using DingFood.Application.Features.Integrations.Keeta.Order.GetByDisplayId;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Keeta.Order.GetByDisplayId;

public sealed class GetKeetaOrderByDisplayIdQueryValidatorTests
{
    private readonly GetKeetaOrderByDisplayIdQueryValidator _validator = new();

    [Fact]
    public void Validate_NonEmptyDisplayId_ShouldBeValid()
        => _validator.Validate(new GetKeetaOrderByDisplayIdQuery("#001")).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyDisplayId_ShouldBeInvalid()
        => _validator.Validate(new GetKeetaOrderByDisplayIdQuery(string.Empty)).IsValid.Should().BeFalse();
}
