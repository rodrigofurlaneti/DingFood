using DingFood.Application.Abstractions.Messaging;
namespace DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetById
{
    public sealed record GetKeetaAuthorizationSessionByIdQuery(
        long Id) : IQuery<KeetaIntegrationAuthorizationSessionResponse>;
}
