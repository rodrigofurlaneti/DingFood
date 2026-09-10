using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.MarkDelivered
{
    public sealed record MarkKeetaOrderDeliveredCommand(long OrderId) : ICommand;
}
