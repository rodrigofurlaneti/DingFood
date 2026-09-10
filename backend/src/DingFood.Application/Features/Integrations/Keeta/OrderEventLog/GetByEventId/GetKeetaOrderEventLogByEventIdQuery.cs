using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetByEventId
{
    public sealed record GetKeetaOrderEventLogByEventIdQuery(
        string EventId) : IQuery<KeetaIntegrationOrderEventLogResponse>;
}
