using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Setting.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByBranchId
{
    public sealed record GetKeetaSettingByBranchIdQuery(
        long BranchId) : IQuery<KeetaIntegrationSettingResponse>;
}
