using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Order.ExistsByKeetaOrderId
{
    public sealed record ExistsKeetaOrderByKeetaOrderIdQuery(
        string KeetaOrderId) : IQuery<bool>;
}
