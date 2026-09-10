using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Assignment.GetActiveByDiningAreaId
{
    public sealed record GetActiveAssignmentsByDiningAreaIdQuery(long DiningAreaId) : IQuery<IReadOnlyCollection<DiningAreaAssignmentListResponse>>;
}
