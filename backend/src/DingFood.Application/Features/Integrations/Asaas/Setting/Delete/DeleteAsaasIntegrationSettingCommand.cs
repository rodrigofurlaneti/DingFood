using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.Delete
{
    public sealed record DeleteAsaasIntegrationSettingCommand(
        long Id,
        long CompanyId) : ICommand;
}
