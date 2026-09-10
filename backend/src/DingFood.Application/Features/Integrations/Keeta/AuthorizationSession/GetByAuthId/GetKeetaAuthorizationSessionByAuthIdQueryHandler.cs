using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetById;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Integrations.Keeta.AuthorizationSession.GetByAuthId
{
    internal sealed class GetKeetaAuthorizationSessionByAuthIdQueryHandler
        : BaseQueryHandler<GetKeetaAuthorizationSessionByAuthIdQuery, KeetaIntegrationAuthorizationSessionResponse>
    {
        private readonly IKeetaIntegrationAuthorizationSessionRepository _sessionRepository;

        public GetKeetaAuthorizationSessionByAuthIdQueryHandler(
            IKeetaIntegrationAuthorizationSessionRepository sessionRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _sessionRepository = sessionRepository;
        }

        public override async Task<Result<KeetaIntegrationAuthorizationSessionResponse>> Handle(
            GetKeetaAuthorizationSessionByAuthIdQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(GetKeetaAuthorizationSessionByAuthIdQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var session = await _sessionRepository.GetByAuthIdAsync(request.AuthId, cancellationToken);

                    if (session is null)
                    {
                        return Result.Failure<KeetaIntegrationAuthorizationSessionResponse>(
                            Error.NotFound(
                                "KeetaAuthorizationSession.NotFound",
                                $"Sessão de autorização Keeta com AuthId {request.AuthId} não foi encontrada."));
                    }

                    var response = new KeetaIntegrationAuthorizationSessionResponse(
                        session.Id,
                        session.CompanyId,
                        session.BranchId,
                        session.AuthId,
                        session.State,
                        session.KeetaMerchantId,
                        session.AuthorizationCode,
                        session.OperationType,
                        session.IsProcessed,
                        session.CreatedAtUtc);

                    return Result.Success(response);
                });
        }
    }
}
