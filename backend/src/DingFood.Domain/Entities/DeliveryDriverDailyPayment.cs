using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

public sealed class DeliveryDriverDailyPayment : AggregateRoot
{
    private DeliveryDriverDailyPayment() : base(0) { }
    public long BranchId { get; private set; }
    public long DeliveryDriverId { get; private set; }
    public DateOnly WorkDate { get; private set; }
    public decimal Amount { get; private set; }
    public static DeliveryDriverDailyPayment Create(long branchId, long driverId, DateOnly date, decimal amount)
    {
        if (amount < 0) throw new ArgumentException("Diária inválida.");
        return new() { BranchId = branchId, DeliveryDriverId = driverId, WorkDate = date, Amount = amount };
    }
}
