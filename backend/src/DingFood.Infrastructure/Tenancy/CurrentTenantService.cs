using Microsoft.AspNetCore.Http;
using DingFood.Application.Abstractions.Tenancy;

namespace DingFood.Infrastructure.Tenancy;

internal sealed class CurrentTenantService(IHttpContextAccessor httpContextAccessor) : ICurrentTenantService
{
    private long? _backgroundCompanyId;

    internal void SetBackgroundCompany(long companyId)
    {
        if (httpContextAccessor.HttpContext is not null || companyId <= 0 ||
            (_backgroundCompanyId.HasValue && _backgroundCompanyId != companyId))
            throw new InvalidOperationException("A background scope belongs to exactly one company.");
        _backgroundCompanyId = companyId;
    }
    public long? CompanyId
    {
        get
        {
            var claim = httpContextAccessor.HttpContext?.User?.FindFirst("companyId")?.Value;
            return long.TryParse(claim, out var companyId) ? companyId : _backgroundCompanyId;
        }
    }
}
