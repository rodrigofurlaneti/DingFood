using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.Authorization.HandleOAuthCallback
{
    public sealed record HandleKeetaOAuthCallbackCommand(
        long CompanyId,
        long BranchId,
        string AuthId,
        string? State,
        long? KeetaMerchantId,
        string? Code) : ICommand<HandleKeetaOAuthCallbackResponse>;
}
