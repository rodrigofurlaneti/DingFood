using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetAllByBranchId
{
    public sealed record GetAllKeetaMerchantMappingsByBranchIdQuery(
        long BranchId) : IQuery<IReadOnlyList<KeetaIntegrationMerchantMappingResponse>>;
}
