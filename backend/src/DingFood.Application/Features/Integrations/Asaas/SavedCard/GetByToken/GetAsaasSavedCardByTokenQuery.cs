using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.SavedCard.GetByCustomerId;
namespace DingFood.Application.Features.Integrations.Asaas.SavedCard.GetByToken
{
    public sealed record GetAsaasSavedCardByTokenQuery(
        string CreditCardToken) : IQuery<AsaasIntegrationSavedCardResponse>;
}
