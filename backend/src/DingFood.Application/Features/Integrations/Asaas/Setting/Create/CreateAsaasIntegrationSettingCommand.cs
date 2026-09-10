using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.Create
{
    public sealed record CreateAsaasIntegrationSettingCommand(
        long CompanyId,
        long? BranchId,
        string ApiKey,
        string? WebhookToken,
        string? Environment,
        bool IsActive = true) : ICommand<CreateAsaasIntegrationSettingResponse>;
}
