using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.ExistsByEventId
{
    public sealed record ExistsKeetaOrderEventLogByEventIdQuery(
        string EventId) : IQuery<bool>;
}
