using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Area.GetByBranchId
{
    public sealed record GetDiningAreasByBranchQuery(long BranchId) : IQuery<IReadOnlyCollection<DiningAreaListResponse>>;
}
