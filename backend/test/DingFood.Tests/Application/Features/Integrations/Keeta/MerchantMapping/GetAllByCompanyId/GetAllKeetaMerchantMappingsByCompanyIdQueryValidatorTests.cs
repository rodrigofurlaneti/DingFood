using FluentAssertions;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetAllByCompanyId;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Keeta.MerchantMapping.GetAllByCompanyId;

public sealed class GetAllKeetaMerchantMappingsByCompanyIdQueryValidatorTests
{
    private readonly GetAllKeetaMerchantMappingsByCompanyIdQueryValidator _validator = new();

    [Fact]
    public void Validate_PositiveCompanyId_ShouldBeValid()
        => _validator.Validate(new GetAllKeetaMerchantMappingsByCompanyIdQuery(1)).IsValid.Should().BeTrue();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_NonPositiveCompanyId_ShouldBeInvalid(long companyId)
        => _validator.Validate(new GetAllKeetaMerchantMappingsByCompanyIdQuery(companyId)).IsValid.Should().BeFalse();
}
