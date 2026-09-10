using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Order.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Order.GetByDisplayId
{
    public sealed record GetKeetaOrderByDisplayIdQuery(
        string DisplayId) : IQuery<KeetaIntegrationOrderResponse>;
}
