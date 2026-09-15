using CheckPay.Domain.Primitives;
using System;

namespace CheckPay.Domain.Entities;

public sealed class Category : AggregateRoot
{
    public long AppUserId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Type { get; private set; } = null!; // INCOME or EXPENSE
    public string? ColorHex { get; private set; }
    public string? Icon { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private Category() : base(0) { }

    private Category(long appUserId, string name, string type, string? colorHex, string? icon) : base(0)
    {
        AppUserId = appUserId;
        Name = name;
        Type = type;
        ColorHex = colorHex;
        Icon = icon;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<Category> Create(long appUserId, string name, string type, string? colorHex, string? icon)
    {
        if (appUserId <= 0)
            return Result.Failure<Category>(new Error("Category.InvalidUserId", "A valid AppUserId is required."));
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Category>(new Error("Category.EmptyName", "Name is required."));
        if (type != "INCOME" && type != "EXPENSE")
            return Result.Failure<Category>(new Error("Category.InvalidType", "Type must be INCOME or EXPENSE."));

        return Result.Success(new Category(appUserId, name, type, colorHex, icon));
    }

    public Result Update(string name, string? colorHex, string? icon)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(new Error("Category.EmptyName", "Name is required."));

        Name = name;
        ColorHex = colorHex;
        Icon = icon;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
