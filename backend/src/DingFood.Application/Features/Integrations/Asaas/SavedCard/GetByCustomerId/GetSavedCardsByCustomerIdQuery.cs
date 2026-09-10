using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.SavedCard.GetByCustomerId
{
    public sealed record GetSavedCardsByCustomerIdQuery(
        long CustomerId) : IQuery<IReadOnlyList<AsaasIntegrationSavedCardResponse>>;
}
