using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

public sealed class DeliveryFeeCondition : AggregateRoot
{
    private DeliveryFeeCondition() : base(0) { }
    public long DeliveryFeeConfigId { get; private set; }
    public int DaysOfWeek { get; private set; }
    public int StartMinute { get; private set; }
    public int EndMinute { get; private set; }
    public decimal PricePerKm { get; private set; }
    public int Priority { get; private set; }
    public static DeliveryFeeCondition Create(int daysOfWeek, int startMinute, int endMinute, decimal pricePerKm, int priority)
    {
        if (daysOfWeek is < 1 or > 127 || startMinute is < 0 or > 1439 || endMinute is < 0 or > 1439 || startMinute == endMinute || pricePerKm < 0 || pricePerKm > 100000)
            throw new ArgumentException("Dias, horário ou tarifa dinâmica inválidos.");
        return new() { DaysOfWeek = daysOfWeek, StartMinute = startMinute, EndMinute = endMinute, PricePerKm = pricePerKm, Priority = priority };
    }
    public bool Matches(DateTime local)
    {
        var minute = local.Hour * 60 + local.Minute;
        var day = (int)local.DayOfWeek;
        if (StartMinute < EndMinute) return (DaysOfWeek & (1 << day)) != 0 && minute >= StartMinute && minute < EndMinute;
        if (minute < EndMinute) day = (day + 6) % 7;
        return (DaysOfWeek & (1 << day)) != 0 && (minute >= StartMinute || minute < EndMinute);
    }
}
