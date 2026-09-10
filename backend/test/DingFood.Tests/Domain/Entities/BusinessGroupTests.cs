using DingFood.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace DingFood.Tests.Domain.Entities;

public sealed class BusinessGroupTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void EmptyGroupNameIsRejected(string? name) => BusinessGroup.Create(name!).IsFailure.Should().BeTrue();

    [Fact]
    public void NewCompanyHasItsOwnActiveGroup()
    {
        var a = Company.Create("A", "Burger", "11111111000111", null, null).Value;
        var b = Company.Create("B", "Pizza", "22222222000122", null, null).Value;
        a.BusinessGroup.IsActive.Should().BeTrue();
        a.BusinessGroup.Should().NotBeSameAs(b.BusinessGroup);
        a.Cnpj.Should().NotBe(b.Cnpj);
    }
}
