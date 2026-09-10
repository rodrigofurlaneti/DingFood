using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetUnprocessedEvents
{
    public sealed record GetUnprocessedKeetaOrderEventLogsQuery
        : IQuery<IReadOnlyList<KeetaIntegrationOrderEventLogResponse>>;
}
