using FluentAssertions;
using DingFood.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentIdForUpdate;
using Xunit;

namespace DingFood.Tests.Application.Features.Integrations.Asaas.Payment.GetByAsaasPaymentIdForUpdate;

public sealed class GetByAsaasPaymentIdForUpdateQueryValidatorTests
{
    private readonly GetByAsaasPaymentIdForUpdateQueryValidator _validator = new();

    [Fact]
    public void Validate_ValidAsaasPaymentId_ShouldBeValid()
        => _validator.Validate(new GetByAsaasPaymentIdForUpdateQuery("pay_000001")).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_EmptyAsaasPaymentId_ShouldBeInvalid()
        => _validator.Validate(new GetByAsaasPaymentIdForUpdateQuery(string.Empty)).IsValid.Should().BeFalse();

    [Fact]
    public void Validate_AsaasPaymentIdExceedingMaxLength_ShouldBeInvalid()
        => _validator.Validate(new GetByAsaasPaymentIdForUpdateQuery(new string('a', 51))).IsValid.Should().BeFalse();

    [Fact]
    public void Validate_AsaasPaymentIdAtMaxLength_ShouldBeValid()
        => _validator.Validate(new GetByAsaasPaymentIdForUpdateQuery(new string('a', 50))).IsValid.Should().BeTrue();
}
