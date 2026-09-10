using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Storefront.AddOrder
{
    public sealed record AddWebStorefrontOrderCommand(
        long BranchId,
        long? CustomerId,
        string CustomerName,
        string? CustomerPhone,
        string? GeneralNotes,
        IReadOnlyCollection<WebStorefrontItemDto> Items) : ICommand<long>;
}