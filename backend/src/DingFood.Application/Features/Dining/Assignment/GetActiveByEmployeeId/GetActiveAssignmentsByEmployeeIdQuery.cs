using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Dining.Assignment.GetActiveByEmployeeId
{
    public sealed record GetActiveAssignmentsByEmployeeIdQuery(long EmployeeId) : IQuery<IReadOnlyCollection<DiningAreaAssignmentListResponse>>;
}
