using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.DispatchOrder
{
    public sealed record DispatchKeetaOrderCommand(
        long OrderId,
        string? TrackingEventType = null,
        string? TrackingEventMessage = null) : ICommand;
}
