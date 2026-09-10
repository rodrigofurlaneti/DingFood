using System.Security.Claims;
using DingFood.Application.Abstractions.Tenancy;
using Microsoft.AspNetCore.Authorization;

namespace DingFood.API.Middleware;

public sealed class CompanyContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ICompanyAccessService access)
    {
        if (context.User.Identity?.IsAuthenticated != true ||
            context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            await next(context);
            return;
        }

        var selected = context.Request.Headers["X-Company-Id"].FirstOrDefault()
            ?? context.User.FindFirstValue("companyId");
        if (!long.TryParse(selected, out var companyId) || companyId <= 0)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        // Customer tokens cannot be used to impersonate an AppUser with the same numeric Id.
        if (context.User.HasClaim(x => x.Type == "customerId") || context.User.IsInRole("Customer"))
        {
            if (selected != context.User.FindFirstValue("companyId"))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
            await next(context);
            return;
        }

        if (!long.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
        var company = await access.ResolveAsync(userId, companyId, context.RequestAborted);
        if (company is null)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
        var identity = new ClaimsIdentity(context.User.Identity as ClaimsIdentity);
        foreach (var claim in identity.Claims.Where(x => x.Type is "companyId" or "businessGroupId" or "employeeId" or "permission" || x.Type == ClaimTypes.Role).ToArray())
            identity.RemoveClaim(claim);
        identity.AddClaim(new Claim("companyId", company.CompanyId.ToString()));
        identity.AddClaim(new Claim("businessGroupId", company.BusinessGroupId.ToString()));
        if (company.EmployeeId is { } employeeId)
            identity.AddClaim(new Claim("employeeId", employeeId.ToString()));
        identity.AddClaims(company.Roles.Select(x => new Claim(ClaimTypes.Role, x)));
        identity.AddClaims(company.Permissions.Select(x => new Claim("permission", x)));
        context.User = new ClaimsPrincipal(identity);
        await next(context);
    }
}
