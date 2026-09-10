using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.MarkReadyForPickup
{
    public sealed record MarkKeetaOrderReadyForPickupCommand(long OrderId) : ICommand;
}
