using System.Security.Claims;
using DingFood.Application.Abstractions.Tenancy;
using Microsoft.AspNetCore.Authorization;

namespace DingFood.API.Middleware;

public sealed class WorkplaceContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IWorkplaceAccessService workplaces)
    {
        if (context.User.Identity?.IsAuthenticated != true || context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() is not null ||
            context.User.HasClaim(c => c.Type == "customerId") || context.User.IsInRole("Customer"))
        { await next(context); return; }
        if (!long.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId) ||
            !long.TryParse(context.User.FindFirstValue("companyId"), out var companyId))
        { context.Response.StatusCode = 403; return; }
        // Organization bootstrap endpoints resolve explicit grants themselves, without an operational context.
        var path = context.Request.Path.Value?.TrimEnd('/').ToLowerInvariant();
        if (path is "/api/companies/allowed" or "/api/companies/switch" or "/api/workplaces")
        { await next(context); return; }
        var header = context.Request.Headers["X-Branch-Id"].FirstOrDefault();
        long? branchId = null;
        if (header is not null)
        {
            if (!long.TryParse(header, out var parsed) || parsed <= 0) { context.Response.StatusCode = 403; return; }
            branchId = parsed;
        }
        var workplace = await workplaces.ResolveAsync(userId, companyId, branchId, context.RequestAborted);
        if (workplace is null) { context.Response.StatusCode = 403; return; }
        var identity = new ClaimsIdentity(context.User.Identity as ClaimsIdentity);
        foreach (var claim in identity.Claims.Where(c => c.Type is "branchId" or "brandId" or "employeeId" or "permission" || c.Type == ClaimTypes.Role).ToArray()) identity.RemoveClaim(claim);
        identity.AddClaim(new Claim("branchId", workplace.Id.ToString()));
        identity.AddClaim(new Claim("brandId", workplace.BrandId.ToString()));
        if (workplace.EmployeeId.HasValue) identity.AddClaim(new Claim("employeeId", workplace.EmployeeId.Value.ToString()));
        identity.AddClaims((workplace.Roles ?? []).Select(r => new Claim(ClaimTypes.Role, r)));
        identity.AddClaims((workplace.Permissions ?? []).Select(p => new Claim("permission", p)));
        context.User = new ClaimsPrincipal(identity);
        await next(context);
    }
}
