using DingFood.Domain.Entities;

namespace DingFood.Domain.Repositories;

public interface IBusinessGroupRepository
{
    Task<BusinessGroup?> GetByIdAsync(long id, CancellationToken ct);
    Task<IReadOnlyCollection<AppUser>> GetUsersAsync(long groupId, CancellationToken ct);
    Task<(AppUser User, Company Home)?> GetUserHomeAsync(long userId, CancellationToken ct);
    Task<long> AddCompanyWithAdministratorAsync(Company company, long userId, string branchName, CancellationToken ct);
    Task SetAccessAsync(AppUserCompany grant, long? roleId, CancellationToken ct);
    Task RevokeAccessAsync(long userId, long companyId, CancellationToken ct);
}
