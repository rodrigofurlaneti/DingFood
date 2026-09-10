using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Storefront.GetBranchMenu
{
    public sealed record GetBranchMenuQuery(long BranchId) : IQuery<BranchMenuResponse>;
}
