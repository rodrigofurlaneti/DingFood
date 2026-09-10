using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

public sealed class BusinessGroup : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private BusinessGroup() : base(0) { }

    public static Result<BusinessGroup> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 200)
            return Result.Failure<BusinessGroup>(new Error("BusinessGroup.InvalidName", "Informe um nome de até 200 caracteres."));
        return Result.Success(new BusinessGroup { Name = name.Trim(), IsActive = true, CreatedAt = DateTime.UtcNow });
    }
}
