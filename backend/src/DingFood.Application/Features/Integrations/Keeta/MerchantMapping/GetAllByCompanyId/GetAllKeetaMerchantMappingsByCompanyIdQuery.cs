using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetAllByCompanyId
{
    public sealed record GetAllKeetaMerchantMappingsByCompanyIdQuery(
        long CompanyId) : IQuery<IReadOnlyList<KeetaIntegrationMerchantMappingResponse>>;
}
