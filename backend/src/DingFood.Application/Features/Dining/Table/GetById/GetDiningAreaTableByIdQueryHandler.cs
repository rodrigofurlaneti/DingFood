using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Dining.Table.GetById
{
    internal sealed class GetDiningAreaTableByIdQueryHandler : BaseQueryHandler<GetDiningAreaTableByIdQuery, DiningAreaTableResponse>
    {
        private readonly IDiningAreaTableRepository _repository;
        public GetDiningAreaTableByIdQueryHandler(
            IDiningAreaTableRepository repository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _repository = repository;
        }
        public override Task<Result<DiningAreaTableResponse>> Handle(GetDiningAreaTableByIdQuery request, CancellationToken cancellationToken) =>
            ExecuteWithLogAsync(
                nameof(GetDiningAreaTableByIdQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
                    if (entity is null)
                        return Result.Failure<DiningAreaTableResponse>(new Error("DiningAreaTable.NotFound", "The dining area table assignment was not found."));
                    var response = new DiningAreaTableResponse(
                        entity.Id,
                        entity.DiningAreaId,
                        entity.DiningTableId,
                        entity.IsActive);
                    return Result.Success(response);
                });
    }
}
