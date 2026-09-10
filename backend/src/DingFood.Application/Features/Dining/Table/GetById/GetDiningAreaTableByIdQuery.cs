using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Table.GetById
{
    public sealed record GetDiningAreaTableByIdQuery(long Id) : IQuery<DiningAreaTableResponse>;
}
