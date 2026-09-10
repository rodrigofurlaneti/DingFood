using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.Delete
{
    public sealed record DeleteKeetaIntegrationSettingCommand(
        long Id,
        long CompanyId) : ICommand;
}
