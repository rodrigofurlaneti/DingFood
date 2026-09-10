using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.GetById
{
    public sealed record GetKeetaOrderByIdQuery(
        long Id) : IQuery<KeetaIntegrationOrderResponse>;
}
