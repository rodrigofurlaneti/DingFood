using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetByAuthId
{
    public sealed record GetKeetaAuthorizationSessionByAuthIdQuery(
        string AuthId) : IQuery<KeetaIntegrationAuthorizationSessionResponse>;
}
