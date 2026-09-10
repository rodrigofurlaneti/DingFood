using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Setting.GetAllActive;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.GetByCompanyId
{
    public sealed record GetAsaasSettingByCompanyIdQuery(
        long CompanyId) : IQuery<AsaasIntegrationSettingResponse>;
}
