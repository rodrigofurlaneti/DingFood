using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.SendTrackingUpdate
{
    public sealed record SendKeetaOrderTrackingUpdateCommand(
        long OrderId,
        string TrackingEventType,
        string? TrackingEventMessage = null) : ICommand;
}
