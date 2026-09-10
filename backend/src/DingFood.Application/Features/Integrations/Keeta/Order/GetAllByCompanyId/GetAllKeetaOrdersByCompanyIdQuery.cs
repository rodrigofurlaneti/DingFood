using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Order.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Order.GetAllByCompanyId
{
    public sealed record GetAllKeetaOrdersByCompanyIdQuery(
        long CompanyId) : IQuery<IReadOnlyList<KeetaIntegrationOrderResponse>>;
}
