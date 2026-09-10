using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.ExistsForBranch
{
    public sealed record ExistsKeetaSettingForBranchQuery(
        long BranchId) : IQuery<bool>;
}
