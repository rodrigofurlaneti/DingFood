using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.SavedCard.Delete
{
    public sealed record DeleteAsaasIntegrationSavedCardCommand(
        long Id,
        long CustomerId,
        long CompanyId) : ICommand;
}
