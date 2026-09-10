using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Stock.GetByBranch;

internal sealed class GetStockByBranchQueryHandler(
    IStockItemRepository stockItemRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetStockByBranchQuery, IReadOnlyCollection<StockItemResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<StockItemResponse>>> Handle(
        GetStockByBranchQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetStockByBranchQueryHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                // Se o seu request possuir o Id do usuário/gerente que está consultando o estoque, preencha:

                var items = await stockItemRepository.GetByBranchAsync(request.BranchId, cancellationToken);

                IReadOnlyCollection<StockItemResponse> response = items
                    .OrderBy(i => i.ProductId)
                    .Select(i => new StockItemResponse(
                        i.Id, i.BranchId, i.ProductId, i.CurrentQuantity,
                        i.MinimumQuantity, i.MaximumQuantity, i.IsBelowMinimum()))
                    .ToList();

                return Result.Success(response);
            });
    }
}