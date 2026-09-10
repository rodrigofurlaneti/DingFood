using FluentAssertions;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetByInternalMerchantId;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Keeta.MerchantMapping.GetByInternalMerchantId;

public sealed class GetKeetaMerchantMappingByInternalMerchantIdQueryValidatorTests
{
    private readonly GetKeetaMerchantMappingByInternalMerchantIdQueryValidator _validator = new();

    [Fact]
    public void Validate_NonEmptyInternalMerchantId_ShouldBeValid()
        => _validator.Validate(new GetKeetaMerchantMappingByInternalMerchantIdQuery("internal-1")).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyInternalMerchantId_ShouldBeInvalid()
        => _validator.Validate(new GetKeetaMerchantMappingByInternalMerchantIdQuery(string.Empty)).IsValid.Should().BeFalse();
}
