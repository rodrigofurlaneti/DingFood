using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Setting.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByIdForUpdate
{
    public sealed record GetKeetaSettingByIdForUpdateQuery(
        long Id) : IQuery<KeetaIntegrationSettingResponse>;
}
