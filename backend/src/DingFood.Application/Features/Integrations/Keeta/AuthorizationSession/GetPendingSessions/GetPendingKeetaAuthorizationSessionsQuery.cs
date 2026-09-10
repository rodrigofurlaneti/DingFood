using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetById;
namespace DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetPendingSessions
{
    public sealed record GetPendingKeetaAuthorizationSessionsQuery
        : IQuery<IReadOnlyList<KeetaIntegrationAuthorizationSessionResponse>>;
}
