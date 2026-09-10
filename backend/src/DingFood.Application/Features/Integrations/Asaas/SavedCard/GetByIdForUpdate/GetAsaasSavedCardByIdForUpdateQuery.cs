using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.SavedCard.GetByCustomerId;
namespace DingFood.Application.Features.Integrations.Asaas.SavedCard.GetByIdForUpdate
{
    public sealed record GetAsaasSavedCardByIdForUpdateQuery(
        long Id) : IQuery<AsaasIntegrationSavedCardResponse>;
}
