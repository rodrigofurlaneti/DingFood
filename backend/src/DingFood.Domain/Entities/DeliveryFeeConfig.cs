using DingFood.Domain.Primitives;

namespace DingFood.Domain.Entities;

public sealed class DeliveryFeeConfig : AggregateRoot
{
    private DeliveryFeeConfig() : base(0) { }
    public long BranchId { get; private set; }
    public string Model { get; private set; } = "PerKm";
    public decimal DailyAmount { get; private set; }
    public decimal MaxRadiusKm { get; private set; }
    public decimal PricePerKm { get; private set; }
    public string TimeZoneId { get; private set; } = "America/Sao_Paulo";
    public List<DeliveryFeeCondition> Conditions { get; private set; } = [];
    public static DeliveryFeeConfig Create(long branchId) => new() { BranchId = branchId };
    public void Configure(string model, decimal dailyAmount, decimal maxRadiusKm, decimal pricePerKm, string timeZoneId)
    {
        if (model is not ("Daily" or "PerKm") || dailyAmount < 0 || dailyAmount > 100000 || pricePerKm < 0 || pricePerKm > 100000 || maxRadiusKm < 0 || maxRadiusKm > 1000 || (model == "PerKm" && maxRadiusKm == 0))
            throw new ArgumentException("Modelo, valores ou raio de entrega inválidos.");
        try { _ = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId); }
        catch (Exception e) when (e is TimeZoneNotFoundException or InvalidTimeZoneException or ArgumentException) { throw new ArgumentException("Fuso horário inválido."); }
        Model = model; DailyAmount = dailyAmount; MaxRadiusKm = maxRadiusKm; PricePerKm = pricePerKm; TimeZoneId = timeZoneId;
    }
    public DeliveryQuote Calculate(decimal distanceKm, DateTimeOffset instant)
    {
        if (distanceKm < 0) throw new ArgumentException("Distância inválida.");
        if (Model == "Daily") return new(0, 0, 0, DailyAmount, Model, null);
        if (distanceKm > MaxRadiusKm) throw new ArgumentException("Endereço fora do raio máximo de entrega.");
        var local = TimeZoneInfo.ConvertTime(instant, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));
        // Higher priority wins; id breaks ties deterministically.
        var condition = Conditions.Where(c => c.Matches(local.DateTime)).OrderByDescending(c => c.Priority).ThenBy(c => c.Id).FirstOrDefault();
        var rate = condition?.PricePerKm ?? PricePerKm;
        return new(Math.Round(distanceKm * rate, 2, MidpointRounding.AwayFromZero), distanceKm, rate, 0, Model, condition?.Id);
    }
}

public sealed record DeliveryQuote(decimal Amount, decimal DistanceKm, decimal PricePerKm, decimal DailyAmount, string Model, long? ConditionId);
