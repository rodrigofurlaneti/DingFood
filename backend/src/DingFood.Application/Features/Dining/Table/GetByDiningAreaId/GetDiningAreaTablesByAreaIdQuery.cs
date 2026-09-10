using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Table.GetByDiningAreaId
{
    public sealed record GetDiningAreaTablesByAreaIdQuery(long DiningAreaId) : IQuery<IReadOnlyCollection<DiningAreaTableListResponse>>;
}
