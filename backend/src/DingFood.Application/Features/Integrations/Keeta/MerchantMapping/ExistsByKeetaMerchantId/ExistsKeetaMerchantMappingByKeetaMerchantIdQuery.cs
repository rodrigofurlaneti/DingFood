using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.ExistsByKeetaMerchantId
{
    public sealed record ExistsKeetaMerchantMappingByKeetaMerchantIdQuery(
        long KeetaMerchantId) : IQuery<bool>;
}
