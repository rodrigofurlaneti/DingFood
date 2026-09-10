using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetById
{
    public sealed record GetKeetaOrderEventLogByIdQuery(
        long Id) : IQuery<KeetaIntegrationOrderEventLogResponse>;
}
