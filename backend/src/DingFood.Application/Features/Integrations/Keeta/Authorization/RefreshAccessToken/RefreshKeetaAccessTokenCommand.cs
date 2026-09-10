using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Authorization.RefreshAccessToken
{
    public sealed record RefreshKeetaAccessTokenCommand(long CompanyId, long BranchId) : ICommand<RefreshKeetaAccessTokenResponse>;
}
