using DingFood.Domain.Primitives;

namespace DingFood.Application.Abstractions.Tenancy;

public sealed record WorkplaceAccess(long Id, string Name, bool IsActive, long BrandId, string BrandName, long? EmployeeId, IReadOnlyCollection<string>? Roles = null, IReadOnlyCollection<string>? Permissions = null);
public sealed record BrandOption(long Id, string Name);
public sealed record CreateWorkplace(string BrandName, string BranchName, long? ExistingBrandId = null);
public sealed record UpdateWorkplaceAccess(long AppUserId, long BranchId, long? EmployeeId, bool Enabled, long? RoleId = null);

public interface IWorkplaceAccessService
{
    Task<IReadOnlyCollection<BrandOption>> GetBrandsAsync(long userId, long companyId, CancellationToken ct);
    Task<IReadOnlyCollection<WorkplaceAccess>> GetAllowedAsync(long userId, long companyId, CancellationToken ct);
    Task<Result<long>> CreateAsync(long userId, long companyId, CreateWorkplace request, CancellationToken ct);
    Task<Result> SetAccessAsync(long userId, long companyId, UpdateWorkplaceAccess request, CancellationToken ct);
}
