using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.SavedCard.GetByCustomerId;
namespace DingFood.Application.Features.Integrations.Asaas.SavedCard.GetById
{
    public sealed record GetAsaasSavedCardByIdQuery(
        long Id) : IQuery<AsaasIntegrationSavedCardResponse>;
}
