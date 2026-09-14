using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

public sealed class JobTitle : AggregateRoot
{
    public long CompanyId { get; private set; }
    public long? BrandId { get; private set; }
    public string Name { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private JobTitle() : base(0) { }

    private JobTitle(long companyId, long? brandId, string name) : base(0)
    {
        CompanyId = companyId;
        BrandId = brandId;
        Name = name;
        IsActive = true;
        CreatedAt = DateTime.Now;
    }

    public static Result<JobTitle> Create(long companyId, long? brandId, string name)
    {
        if (companyId == 0)
            return Result.Failure<JobTitle>(new Error("JobTitle.EmptyCompany", "Company id is zero."));
        if (brandId == 0)
            return Result.Failure<JobTitle>(new Error("JobTitle.EmptyBrand", "Brand id is zero."));
        if (companyId < 0)
            return Result.Failure<JobTitle>(new Error("JobTitle.EmptyCompany", "Company id is negative."));
        if (brandId < 0)
            return Result.Failure<JobTitle>(new Error("JobTitle.EmptyBrand", "Brand id is negative."));
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<JobTitle>(new Error("JobTitle.EmptyName", "Name is required."));
        return Result.Success(new JobTitle(companyId, brandId, name));
    }

    public void Touch() => UpdatedAt = DateTime.Now;

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.Now;
    }
}
