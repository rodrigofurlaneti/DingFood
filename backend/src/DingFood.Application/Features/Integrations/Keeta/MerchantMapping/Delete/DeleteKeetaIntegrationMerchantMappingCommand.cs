using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.Delete
{
    public sealed record DeleteKeetaIntegrationMerchantMappingCommand(
        long Id,
        long CompanyId) : ICommand;
}
