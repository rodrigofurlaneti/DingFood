using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.Create
{
    public sealed record CreateKeetaIntegrationAuthorizationSessionCommand(
        long CompanyId,
        long BranchId,
        string AuthId,
        int OperationType) : ICommand<CreateKeetaIntegrationAuthorizationSessionResponse>;
}
