using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Asaas.Setting.GetAllActive;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.GetById
{
    public sealed record GetAsaasSettingByIdQuery(
        long Id) : IQuery<AsaasIntegrationSettingResponse>;
}
