using System.Security.Claims;
using DingFood.API.Middleware;
using DingFood.Application.Abstractions.Tenancy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Xunit;

namespace DingFood.Tests.API;

public sealed class WorkplaceContextMiddlewareTests
{
    [Theory]
    [InlineData("10", true)]
    [InlineData(null, true)]
    [InlineData("20", false)]
    [InlineData("invalid", false)]
    [InlineData("0", false)]
    public async Task ResolvesOnlyExplicitBranchAndReplacesCompanyWideClaims(string? header, bool expected)
    {
        var http = new DefaultHttpContext();
        http.Request.Path = "/api/products/company/1/management";
        http.User = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, "7"), new Claim("companyId", "1"),
            new Claim(ClaimTypes.Role, "Administrador"), new Claim("employeeId", "999")], "test"));
        if (header is not null) http.Request.Headers["X-Branch-Id"] = header;
        var access = Substitute.For<IWorkplaceAccessService>();
        access.GetAllowedAsync(7, 1, Arg.Any<CancellationToken>()).Returns(new[] {
            new WorkplaceAccess(10, "Centro", true, 5, "Burger", 100, ["Garçom"], []) });
        var called = false;
        var middleware = new WorkplaceContextMiddleware(_ => { called = true; return Task.CompletedTask; });
        await middleware.InvokeAsync(http, access);
        called.Should().Be(expected);
        if (!expected) { http.Response.StatusCode.Should().Be(403); return; }
        http.User.FindFirstValue("branchId").Should().Be("10");
        http.User.FindFirstValue("brandId").Should().Be("5");
        http.User.FindFirstValue("employeeId").Should().Be("100");
        http.User.IsInRole("Administrador").Should().BeFalse();
        http.User.IsInRole("Garçom").Should().BeTrue();
    }

    [Fact]
    public async Task RevokedGrantRejectsEvenAnExistingCompanyToken()
    {
        var http = new DefaultHttpContext();
        http.Request.Path = "/api/orders/123";
        http.Request.Headers["X-Branch-Id"] = "10";
        http.User = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "7"), new Claim("companyId", "1")], "test"));
        var access = Substitute.For<IWorkplaceAccessService>();
        access.GetAllowedAsync(7, 1, Arg.Any<CancellationToken>()).Returns(Array.Empty<WorkplaceAccess>());
        var called = false;
        await new WorkplaceContextMiddleware(_ => { called = true; return Task.CompletedTask; }).InvokeAsync(http, access);
        called.Should().BeFalse(); http.Response.StatusCode.Should().Be(403);
    }
}
