namespace DingFood.Application.Features.Dining.Area
{
    public sealed record DiningAreaResponse(
        long Id,
        string Name,
        bool IsActive);
}
