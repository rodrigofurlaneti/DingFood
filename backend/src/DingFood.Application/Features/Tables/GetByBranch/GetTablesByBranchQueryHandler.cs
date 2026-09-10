using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Tables.GetByBranch;

internal sealed class GetTablesByBranchQueryHandler(
    IDiningTableRepository diningTableRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetTablesByBranchQuery, IReadOnlyCollection<TableResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<TableResponse>>> Handle(
        GetTablesByBranchQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetTablesByBranchQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var tables = await diningTableRepository.GetByBranchAsync(request.BranchId, cancellationToken);
                IReadOnlyCollection<TableResponse> response = tables
                    .OrderBy(t => t.Number)
                    .Select(t => new TableResponse(
                        t.Id, t.BranchId, t.TableStatusId, t.Number, t.Capacity,
                        t.IsCameraInputEnabled, t.IsBarcodeEnabled, t.IsQrCodeEnabled))
                    .ToList();
                return Result.Success(response);
            });
    }
}