using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.Update
{
    public sealed record UpdateKeetaIntegrationSettingCommand(
        long Id,
        long CompanyId,
        string? ClientId = null,
        string? ClientSecret = null,
        string? AppId = null,
        string? BaseUrl = null) : ICommand;
}
