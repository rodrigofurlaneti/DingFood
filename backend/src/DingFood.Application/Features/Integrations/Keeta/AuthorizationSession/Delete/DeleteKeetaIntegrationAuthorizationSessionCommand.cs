using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.Delete
{
    public sealed record DeleteKeetaIntegrationAuthorizationSessionCommand(
        long Id,
        long CompanyId) : ICommand;
}
