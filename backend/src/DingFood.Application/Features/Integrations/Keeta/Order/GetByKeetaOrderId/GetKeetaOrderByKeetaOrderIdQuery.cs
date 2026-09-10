using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Order.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Order.GetByKeetaOrderId
{
    public sealed record GetKeetaOrderByKeetaOrderIdQuery(
        string KeetaOrderId) : IQuery<KeetaIntegrationOrderResponse>;
}
