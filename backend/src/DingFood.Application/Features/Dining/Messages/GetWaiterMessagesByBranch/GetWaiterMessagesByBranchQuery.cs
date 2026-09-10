using MediatR;
using DingFood.Domain.Primitives;
namespace DingFood.Application.Features.Dining.Messages.GetWaiterMessagesByBranch
{
    public sealed record GetWaiterMessagesByBranchQuery(long BranchId, long? DiningAreaId) : IRequest<Result<IEnumerable<WaiterMessageResponse>>>;
}
