using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Asaas.Setting.ExistsForBranch
{
    public sealed record ExistsAsaasSettingForBranchQuery(
        long CompanyId,
        long BranchId) : IQuery<bool>;
}
