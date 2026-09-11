using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

/// <summary>Commercial identity. Sharing a group never shares operational data or access.</summary>
public sealed class Brand : AggregateRoot
{
    public long BusinessGroupId { get; private set; }
    public BusinessGroup? BusinessGroup { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    private Brand() : base(0) { }
    public static Brand Create(long groupId, string name)
    {
        if (groupId <= 0 || string.IsNullOrWhiteSpace(name) || name.Trim().Length > 150)
            throw new ArgumentException("Informe o grupo e o nome da marca.");
        return new Brand { BusinessGroupId = groupId, Name = name.Trim() };
    }
    public static Brand Create(BusinessGroup group, string name)
    {
        if (!group.IsActive || string.IsNullOrWhiteSpace(name) || name.Trim().Length > 150) throw new ArgumentException("Marca inválida.");
        return new Brand { BusinessGroup = group, BusinessGroupId = group.Id, Name = name.Trim() };
    }
}

/// <summary>Explicit legal entity/brand association; the brand can operate under multiple CNPJs.</summary>
public sealed class CompanyBrand : AggregateRoot
{
    public long CompanyId { get; private set; }
    public long BrandId { get; private set; }
    public Company? Company { get; private set; }
    public Brand? Brand { get; private set; }
    public bool IsActive { get; private set; } = true;
    private CompanyBrand() : base(0) { }
    public static CompanyBrand Create(long companyId, long brandId)
        => new() { CompanyId = companyId, BrandId = brandId };
    public static CompanyBrand Create(Company company, Brand brand)
        => new() { Company = company, CompanyId = company.Id, Brand = brand, BrandId = brand.Id };
}

/// <summary>An explicit assignment to one operational branch, never to every kitchen in a group.</summary>
public sealed class AppUserBranch : AggregateRoot
{
    public long AppUserId { get; private set; }
    public AppUser? AppUser { get; private set; }
    public long BranchId { get; private set; }
    public Branch? Branch { get; private set; }
    public long? EmployeeId { get; private set; }
    public long? RoleId { get; private set; }
    public bool UsesLegacyRoles { get; private set; }
    public bool IsActive { get; private set; } = true;
    private AppUserBranch() : base(0) { }
    public static AppUserBranch Create(long userId, long branchId, long? employeeId = null, long? roleId = null)
        => new() { AppUserId = userId, BranchId = branchId, EmployeeId = employeeId, RoleId = roleId };
    public static AppUserBranch Create(long userId, Branch branch, long? roleId) => new() { AppUserId = userId, BranchId = branch.Id, Branch = branch, RoleId = roleId };
    public static AppUserBranch Create(AppUser user, long branchId)
        => new() { AppUser = user, AppUserId = user.Id, BranchId = branchId, EmployeeId = user.EmployeeId };
    public void Update(long? employeeId, bool enabled, long? roleId = null) { EmployeeId = employeeId; IsActive = enabled; RoleId = roleId; UsesLegacyRoles = false; }
}
