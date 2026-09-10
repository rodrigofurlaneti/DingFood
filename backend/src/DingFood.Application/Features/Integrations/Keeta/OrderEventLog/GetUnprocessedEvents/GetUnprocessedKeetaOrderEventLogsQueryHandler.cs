using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetById;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Integrations.Keeta.OrderEventLog.GetUnprocessedEvents
{
    internal sealed class GetUnprocessedKeetaOrderEventLogsQueryHandler
        : BaseQueryHandler<GetUnprocessedKeetaOrderEventLogsQuery, IReadOnlyList<KeetaIntegrationOrderEventLogResponse>>
    {
        private readonly IKeetaIntegrationOrderEventLogRepository _eventLogRepository;

        public GetUnprocessedKeetaOrderEventLogsQueryHandler(
            IKeetaIntegrationOrderEventLogRepository eventLogRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _eventLogRepository = eventLogRepository;
        }

        public override async Task<Result<IReadOnlyList<KeetaIntegrationOrderEventLogResponse>>> Handle(
            GetUnprocessedKeetaOrderEventLogsQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(GetUnprocessedKeetaOrderEventLogsQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var eventLogs = await _eventLogRepository.GetUnprocessedEventsAsync(cancellationToken);

                    var response = eventLogs
                        .Select(e => new KeetaIntegrationOrderEventLogResponse(
                            e.Id, e.CompanyId, e.BranchId, e.EventId, e.OrderId, e.EventType,
                            e.RawPayload, e.ProcessedSuccessfully, e.ErrorMessage, e.EventCreatedAtUtc, e.ReceivedAtUtc))
                        .ToList();

                    return Result.Success<IReadOnlyList<KeetaIntegrationOrderEventLogResponse>>(response);
                });
        }
    }
}
