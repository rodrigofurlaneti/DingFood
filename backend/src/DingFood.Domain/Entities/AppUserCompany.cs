using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

/// <summary>Explicit access grant. Membership never grants access to sibling companies.</summary>
public sealed class AppUserCompany : AggregateRoot
{
    public long AppUserId { get; private set; }
    public AppUser? AppUser { get; private set; }
    public long CompanyId { get; private set; }
    public long? EmployeeId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private AppUserCompany() : base(0) { }

    public static Result<AppUserCompany> CreateHomeAccess(AppUser user)
    {
        if (user.CompanyId <= 0 || !user.IsActive)
            return Result.Failure<AppUserCompany>(new Error("AppUserCompany.InvalidUser", "Usuário deve pertencer a uma empresa ativa."));
        return Result.Success(new AppUserCompany
        {
            AppUser = user, AppUserId = user.Id, CompanyId = user.CompanyId, EmployeeId = user.EmployeeId,
            IsActive = true, CreatedAt = DateTime.UtcNow
        });
    }

    public static Result<AppUserCompany> Create(AppUser user, Company homeCompany, Company company)
    {
        if (user.Id <= 0 || company.Id <= 0 || user.CompanyId != homeCompany.Id ||
            homeCompany.BusinessGroupId <= 0 || homeCompany.BusinessGroupId != company.BusinessGroupId ||
            !user.IsActive || !homeCompany.IsActive || !company.IsActive)
            return Result.Failure<AppUserCompany>(new Error("AppUserCompany.InvalidGroup", "O usuário e a empresa devem pertencer ao mesmo grupo ativo."));
        return Result.Success(new AppUserCompany
        {
            AppUserId = user.Id, CompanyId = company.Id, IsActive = true, CreatedAt = DateTime.UtcNow
        });
    }

    public void Revoke() => IsActive = false;
    public void Restore() => IsActive = true;
    public Result AssignEmployee(Employee? employee, Branch? branch)
    {
        if (employee is not null && (branch is null || !employee.IsActive || !branch.IsActive || employee.BranchId != branch.Id || branch.CompanyId != CompanyId))
            return Result.Failure(new Error("AppUserCompany.InvalidEmployee", "Funcionário não pertence à empresa do vínculo."));
        EmployeeId = employee?.Id;
        return Result.Success();
    }
}
