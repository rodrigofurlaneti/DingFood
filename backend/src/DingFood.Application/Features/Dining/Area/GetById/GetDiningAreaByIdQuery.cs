using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Area.GetById
{
    public sealed record GetDiningAreaByIdQuery(long Id) : IQuery<DiningAreaResponse>;
}
