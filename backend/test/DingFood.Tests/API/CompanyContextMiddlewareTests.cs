using System.Security.Claims;
using DingFood.API.Middleware;
using DingFood.Application.Abstractions.Tenancy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace DingFood.Tests.API;

public sealed class CompanyContextMiddlewareTests
{
    private static DefaultHttpContext Context(string companyId)
    {
        var context = new DefaultHttpContext();
        context.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "1"), new Claim("companyId", "10"),
            new Claim(ClaimTypes.Role, "Administrador"), new Claim("employeeId", "100"),
            new Claim("permission", "old-permission")], "test"));
        context.Request.Headers["X-Company-Id"] = companyId;
        return context;
    }

    [Theory]
    [InlineData("20")]
    [InlineData("30")]
    [InlineData("invalid")]
    public async Task UnauthorizedCompanyNeverReachesHandler(string selected)
    {
        var context = Context(selected);
        var called = false;
        var access = Substitute.For<ICompanyAccessService>();
        await new CompanyContextMiddleware(_ => { called = true; return Task.CompletedTask; }).InvokeAsync(context, access);
        called.Should().BeFalse();
        context.Response.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task SwitchReplacesPermissionsAndDoesNotCarryEmployeeOrAdminRole()
    {
        var context = Context("20");
        var access = Substitute.For<ICompanyAccessService>();
        access.ResolveAsync(1, 20, Arg.Any<CancellationToken>()).Returns(new CompanyAccess(20, 5, "Group", "Pizza", "22222222000122", null, ["Atendente"], ["new-permission"]));
        await new CompanyContextMiddleware(_ => Task.CompletedTask).InvokeAsync(context, access);
        context.User.FindFirstValue("companyId").Should().Be("20");
        context.User.IsInRole("Administrador").Should().BeFalse();
        context.User.FindFirst("employeeId").Should().BeNull();
        context.User.FindAll("permission").Select(x => x.Value).Should().Equal("new-permission");
    }

    [Fact]
    public async Task CustomerCannotUseAppUserMembership()
    {
        var context = Context("20");
        ((ClaimsIdentity)context.User.Identity!).AddClaim(new Claim(ClaimTypes.Role, "Customer"));
        var access = Substitute.For<ICompanyAccessService>();
        var called = false;
        await new CompanyContextMiddleware(_ => { called = true; return Task.CompletedTask; }).InvokeAsync(context, access);
        called.Should().BeFalse();
        context.Response.StatusCode.Should().Be(403);
        await access.DidNotReceiveWithAnyArgs().ResolveAsync(default, default, default);
    }
}
