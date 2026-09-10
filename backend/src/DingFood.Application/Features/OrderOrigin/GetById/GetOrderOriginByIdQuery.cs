using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.OrderOrigin.GetAll;
namespace DingFood.Application.Features.OrderOrigin.GetById
{
    public sealed record GetOrderOriginByIdQuery(long Id) : IQuery<OrderOriginResponse>;
}
