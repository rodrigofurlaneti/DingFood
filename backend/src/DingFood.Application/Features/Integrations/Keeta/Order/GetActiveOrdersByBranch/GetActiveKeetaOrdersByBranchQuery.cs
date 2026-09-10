using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Order.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Order.GetActiveOrdersByBranch
{
    public sealed record GetActiveKeetaOrdersByBranchQuery(
        long BranchId) : IQuery<IReadOnlyList<KeetaIntegrationOrderResponse>>;
}
