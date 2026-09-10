using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetById
{
    public sealed record GetKeetaMerchantMappingByIdQuery(
        long Id) : IQuery<KeetaIntegrationMerchantMappingResponse>;
}
