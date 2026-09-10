using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Authorization.RequestAuthorizationUrl
{
    public sealed record RequestKeetaAuthorizationUrlCommand(
        long CompanyId,
        long BranchId,
        string RedirectUri) : ICommand<RequestKeetaAuthorizationUrlResponse>;
}
