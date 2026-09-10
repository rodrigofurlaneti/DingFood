using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Setting.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByScope
{
    public sealed record GetKeetaSettingByScopeQuery(
        long CompanyId,
        long? BranchId) : IQuery<KeetaIntegrationSettingResponse>;
}
