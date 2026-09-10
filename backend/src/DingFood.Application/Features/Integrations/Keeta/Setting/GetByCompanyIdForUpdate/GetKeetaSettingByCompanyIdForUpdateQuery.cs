using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Setting.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByCompanyIdForUpdate
{
    public sealed record GetKeetaSettingByCompanyIdForUpdateQuery(
        long CompanyId) : IQuery<KeetaIntegrationSettingResponse>;
}
