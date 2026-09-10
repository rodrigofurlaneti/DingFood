using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Dining.Assignment.GetActiveByEmployeeId
{
    internal sealed class GetActiveAssignmentsByEmployeeIdQueryHandler : BaseQueryHandler<GetActiveAssignmentsByEmployeeIdQuery, IReadOnlyCollection<DiningAreaAssignmentListResponse>>
    {
        private readonly IDiningAreaAssignmentRepository _repository;

        public GetActiveAssignmentsByEmployeeIdQueryHandler(
            IDiningAreaAssignmentRepository repository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _repository = repository;
        }
        public override Task<Result<IReadOnlyCollection<DiningAreaAssignmentListResponse>>> Handle(GetActiveAssignmentsByEmployeeIdQuery request, CancellationToken cancellationToken) =>
            ExecuteWithLogAsync(
                nameof(GetActiveAssignmentsByEmployeeIdQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var entities = await _repository.GetActiveByEmployeeIdAsync(request.EmployeeId, cancellationToken);
                    IReadOnlyCollection<DiningAreaAssignmentListResponse> response = entities
                        .Select(e => new DiningAreaAssignmentListResponse(e.Id, e.DiningAreaId, e.EmployeeId, e.StartAt))
                        .ToList();
                    return Result.Success(response);
                });
    }
}
