using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.OrderOrigin.GetAll;
namespace DingFood.Application.Features.OrderOrigin.GetByCompanyAndBranch
{
    public sealed record GetOrderOriginsByCompanyAndBranchQuery(
        long? CompanyId,
        long? BranchId) : IQuery<IReadOnlyCollection<OrderOriginResponse>>;
}
