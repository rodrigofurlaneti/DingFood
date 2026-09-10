using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

public sealed class Company : AggregateRoot
{
    public long BusinessGroupId { get; private set; }
    public BusinessGroup BusinessGroup { get; private set; } = null!;
    public string LegalName { get; private set; } = null!;
    public string TradeName { get; private set; } = null!;
    public string Cnpj { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private Company() : base(0) { }

    private Company(string legalName, string tradeName, string cnpj, string? email, string? phone) : base(0)
    {
        LegalName = legalName;
        TradeName = tradeName;
        Cnpj = cnpj;
        Email = email;
        Phone = phone;
        IsActive = true;
        CreatedAt = DateTime.Now;
        BusinessGroup = BusinessGroup.Create(tradeName).Value;
    }

    public static Result<Company> Create(string legalName, string tradeName, string cnpj, string? email, string? phone)
    {
        if (string.IsNullOrWhiteSpace(legalName))
            return Result.Failure<Company>(new Error("Company.EmptyLegalName", "LegalName is required."));
        if (string.IsNullOrWhiteSpace(tradeName))
            return Result.Failure<Company>(new Error("Company.EmptyTradeName", "TradeName is required."));
        if (string.IsNullOrWhiteSpace(cnpj))
            return Result.Failure<Company>(new Error("Company.EmptyCnpj", "Cnpj is required."));
        return Result.Success(new Company(legalName, tradeName, cnpj, email, phone));
    }

    public void Touch() => UpdatedAt = DateTime.Now;

    public Result AssignToGroup(BusinessGroup group)
    {
        if (!group.IsActive || (BusinessGroupId != 0 && BusinessGroupId != group.Id))
            return Result.Failure(new Error("Company.InvalidGroup", "A empresa não pode ser transferida para outro grupo."));
        BusinessGroup = group;
        BusinessGroupId = group.Id;
        return Result.Success();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.Now;
    }
}
