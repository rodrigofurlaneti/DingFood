using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.OrderOrigin.GetAll;
namespace DingFood.Application.Features.OrderOrigin.GetFiltered
{
    public sealed record GetFilteredOrderOriginsQuery(
        long? CompanyId,
        long? BranchId,
        string? SearchTerm,
        bool? IsActive) : IQuery<IReadOnlyCollection<OrderOriginResponse>>;
}
