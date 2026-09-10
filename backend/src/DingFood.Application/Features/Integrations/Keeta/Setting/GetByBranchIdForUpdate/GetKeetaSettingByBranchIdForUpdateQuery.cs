using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Setting.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByBranchIdForUpdate
{
    public sealed record GetKeetaSettingByBranchIdForUpdateQuery(
        long BranchId) : IQuery<KeetaIntegrationSettingResponse>;
}
