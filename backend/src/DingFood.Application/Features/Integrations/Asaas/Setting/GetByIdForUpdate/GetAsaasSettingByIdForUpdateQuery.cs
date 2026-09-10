using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Setting.GetAllActive;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.GetByIdForUpdate
{
    public sealed record GetAsaasSettingByIdForUpdateQuery(
        long Id) : IQuery<AsaasIntegrationSettingResponse>;
}
