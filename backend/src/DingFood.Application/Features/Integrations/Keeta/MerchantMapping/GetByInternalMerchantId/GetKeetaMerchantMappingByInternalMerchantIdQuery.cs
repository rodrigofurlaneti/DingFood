using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetByInternalMerchantId
{
    public sealed record GetKeetaMerchantMappingByInternalMerchantIdQuery(
        string InternalMerchantId) : IQuery<KeetaIntegrationMerchantMappingResponse>;
}
