using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.OrderOrigin.GetAll
{
    public sealed record GetAllOrderOriginsQuery() : IQuery<IReadOnlyCollection<OrderOriginResponse>>;
}
