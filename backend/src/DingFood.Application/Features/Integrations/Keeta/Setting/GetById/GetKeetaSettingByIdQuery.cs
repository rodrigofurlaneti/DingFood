using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetById
{
    public sealed record GetKeetaSettingByIdQuery(
        long Id) : IQuery<KeetaIntegrationSettingResponse>;
}
