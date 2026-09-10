using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetAllByOrderId
{
    public sealed record GetAllKeetaOrderEventLogsByOrderIdQuery(
        string OrderId) : IQuery<IReadOnlyList<KeetaIntegrationOrderEventLogResponse>>;
}
