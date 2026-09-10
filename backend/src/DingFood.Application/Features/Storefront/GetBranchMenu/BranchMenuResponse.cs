using DingFood.Application.Features.Catalog;
namespace DingFood.Application.Features.Storefront.GetBranchMenu
{
    public sealed record BranchMenuResponse(
        string BranchName,
        List<MenuItemResponse> Items,
        long? CompanyId = null);
}
