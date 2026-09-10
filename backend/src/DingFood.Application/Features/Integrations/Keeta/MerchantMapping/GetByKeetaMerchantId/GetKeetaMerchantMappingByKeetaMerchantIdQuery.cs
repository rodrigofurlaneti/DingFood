using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetByKeetaMerchantId
{
    public sealed record GetKeetaMerchantMappingByKeetaMerchantIdQuery(
        long KeetaMerchantId) : IQuery<KeetaIntegrationMerchantMappingResponse>;
}
