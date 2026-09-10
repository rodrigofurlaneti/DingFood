using DingFood.Application.Features.Orders.AddItem;
namespace DingFood.Application.Features.Storefront.AddOrder
{
    public sealed record WebStorefrontItemDto(
        long ProductId,
        decimal Quantity,
        string? Notes,
        IReadOnlyCollection<OrderItemComplementSelection>? Complements,
        IReadOnlyCollection<long>? OptionalExtraIds = null,
        IReadOnlyCollection<long>? BoostIds = null);
}
