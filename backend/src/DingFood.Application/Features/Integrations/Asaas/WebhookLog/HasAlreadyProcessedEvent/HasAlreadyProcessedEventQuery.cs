using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.WebhookLog.HasAlreadyProcessedEvent
{
    public sealed record HasAlreadyProcessedEventQuery(
        string AsaasEventId) : IQuery<bool>;
}
