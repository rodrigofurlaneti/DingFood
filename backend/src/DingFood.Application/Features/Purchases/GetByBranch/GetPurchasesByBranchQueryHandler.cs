using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Purchases.GetByBranch;

internal sealed class GetPurchasesByBranchQueryHandler(
    IPurchaseRepository purchaseRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetPurchasesByBranchQuery, IReadOnlyCollection<PurchaseResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<PurchaseResponse>>> Handle(
        GetPurchasesByBranchQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetPurchasesByBranchQueryHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                // Se o seu request possuir o Id do usuário/gerente que está consultando, preencha:

                var purchases = await purchaseRepository.GetByBranchAsync(request.BranchId, cancellationToken);

                IReadOnlyCollection<PurchaseResponse> response = purchases
                    .Select(p => new PurchaseResponse(
                        p.Id, p.SupplierId, p.DocumentNumber, p.PurchasedAt, p.TotalAmount, p.Notes,
                        p.Items.Select(i => new PurchaseItemResponse(i.ProductId, i.Quantity, i.UnitCost, i.TotalCost)).ToList()))
                    .ToList();

                return Result.Success(response);
            });
    }
}