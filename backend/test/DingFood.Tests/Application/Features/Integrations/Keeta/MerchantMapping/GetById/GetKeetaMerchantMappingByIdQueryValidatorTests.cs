using FluentAssertions;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetById;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Keeta.MerchantMapping.GetById;

public sealed class GetKeetaMerchantMappingByIdQueryValidatorTests
{
    private readonly GetKeetaMerchantMappingByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_PositiveId_ShouldBeValid()
        => _validator.Validate(new GetKeetaMerchantMappingByIdQuery(1)).IsValid.Should().BeTrue();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_NonPositiveId_ShouldBeInvalid(long id)
        => _validator.Validate(new GetKeetaMerchantMappingByIdQuery(id)).IsValid.Should().BeFalse();
}
