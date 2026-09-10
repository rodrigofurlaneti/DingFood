using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Order.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Order.GetAllByBranchId
{
    public sealed record GetAllKeetaOrdersByBranchIdQuery(
        long BranchId) : IQuery<IReadOnlyList<KeetaIntegrationOrderResponse>>;
}
