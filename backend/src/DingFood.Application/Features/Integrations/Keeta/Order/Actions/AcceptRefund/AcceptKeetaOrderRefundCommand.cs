using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.AcceptRefund
{
    public sealed record AcceptKeetaOrderRefundCommand(long OrderId) : ICommand;
}
