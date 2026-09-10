using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetById;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetPendingSessions
{
    internal sealed class GetPendingKeetaAuthorizationSessionsQueryHandler
        : BaseQueryHandler<GetPendingKeetaAuthorizationSessionsQuery, IReadOnlyList<KeetaIntegrationAuthorizationSessionResponse>>
    {
        private readonly IKeetaIntegrationAuthorizationSessionRepository _sessionRepository;

        public GetPendingKeetaAuthorizationSessionsQueryHandler(
            IKeetaIntegrationAuthorizationSessionRepository sessionRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _sessionRepository = sessionRepository;
        }

        public override async Task<Result<IReadOnlyList<KeetaIntegrationAuthorizationSessionResponse>>> Handle(
            GetPendingKeetaAuthorizationSessionsQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(GetPendingKeetaAuthorizationSessionsQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var sessions = await _sessionRepository.GetPendingSessionsAsync(cancellationToken);

                    var response = sessions
                        .Select(s => new KeetaIntegrationAuthorizationSessionResponse(
                            s.Id,
                            s.CompanyId,
                            s.BranchId,
                            s.AuthId,
                            s.State,
                            s.KeetaMerchantId,
                            s.AuthorizationCode,
                            s.OperationType,
                            s.IsProcessed,
                            s.CreatedAtUtc))
                        .ToList();

                    return Result.Success<IReadOnlyList<KeetaIntegrationAuthorizationSessionResponse>>(response);
                });
        }
    }
}
