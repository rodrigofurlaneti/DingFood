using Microsoft.AspNetCore.Http;
using DingFood.Application.Abstractions.Tenancy;

namespace DingFood.Infrastructure.Tenancy;

internal sealed class CurrentTenantService(IHttpContextAccessor httpContextAccessor) : ICurrentTenantService
{
    private (long Company, long Brand, long Branch)? _resolvedWorkplace;
    internal void SetResolvedWorkplace(long companyId, long brandId, long branchId)
    {
        if (_resolvedWorkplace.HasValue && _resolvedWorkplace.Value != (companyId, brandId, branchId))
            throw new DingFood.Domain.Exceptions.TenantAccessException();
        _resolvedWorkplace = (companyId, brandId, branchId);
    }
    private long? _backgroundCompanyId;
    private long? _backgroundBrandId;
    public long? BrandId => _resolvedWorkplace?.Brand ?? ReadClaim("brandId") ?? _backgroundBrandId;
    public long? BranchId => _resolvedWorkplace?.Branch ?? ReadClaim("branchId");
    public long? EmployeeId => ReadClaim("employeeId");
    public long? UserId => ReadClaim(System.Security.Claims.ClaimTypes.NameIdentifier);
    private long? ReadClaim(string name) => long.TryParse(httpContextAccessor.HttpContext?.User.FindFirst(name)?.Value, out var id) ? id : null;

    internal void SetBackgroundCompany(long companyId, long? brandId = null)
    {
        if (httpContextAccessor.HttpContext is not null || companyId <= 0 ||
            (_backgroundCompanyId.HasValue && _backgroundCompanyId != companyId))
            throw new InvalidOperationException("A background scope belongs to exactly one company.");
        _backgroundCompanyId = companyId;
        _backgroundBrandId = brandId;
    }
    public long? CompanyId
    {
        get
        {
            if (_resolvedWorkplace.HasValue) return _resolvedWorkplace.Value.Company;
            var claim = httpContextAccessor.HttpContext?.User?.FindFirst("companyId")?.Value;
            return long.TryParse(claim, out var companyId) ? companyId : _backgroundCompanyId;
        }
    }
}
