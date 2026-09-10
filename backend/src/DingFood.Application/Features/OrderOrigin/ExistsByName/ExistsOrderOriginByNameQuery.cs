using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.OrderOrigin.ExistsByName
{
    public sealed record ExistsOrderOriginByNameQuery(
        long? CompanyId,
        long? BranchId,
        string Name,
        long? ExcludeId = null) : IQuery<bool>;
}
