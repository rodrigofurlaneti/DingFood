using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.ConfirmOrder
{
    public sealed record ConfirmKeetaOrderCommand(
        long OrderId,
        string? Reason = null,
        int? PreparationTimeMinutes = null) : ICommand;
}
