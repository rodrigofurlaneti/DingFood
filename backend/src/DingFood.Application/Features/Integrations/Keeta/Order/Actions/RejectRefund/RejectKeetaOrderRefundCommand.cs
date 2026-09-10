using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.Actions.RejectRefund
{
    public sealed record RejectKeetaOrderRefundCommand(long OrderId, string Reason, string Code) : ICommand;
}
