using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Billing.GetSalesBySession;

internal sealed class GetSalesBySessionQueryHandler(
    ISaleRepository saleRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetSalesBySessionQuery, IReadOnlyCollection<SessionSaleResponse>>(logRepository, unitOfWork)
{
    public override Task<Result<IReadOnlyCollection<SessionSaleResponse>>> Handle(
        GetSalesBySessionQuery request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(nameof(GetSalesBySessionQueryHandler), nameof(Handle), null, async (userIdBox) =>
        {
            var sales = await saleRepository.GetByCashSessionAsync(request.CashSessionId, cancellationToken);

            IReadOnlyCollection<SessionSaleResponse> response = sales
                .OrderByDescending(s => s.SoldAt)
                .Select(s => new SessionSaleResponse(
                    s.Id, s.SaleNumber, s.CustomerOrderId, s.TotalAmount, s.SoldAt,
                    s.Payments.Where(p => p.IsActive)
                        .Select(p => $"{p.PaymentMethodId}:{p.Amount:0.00}")
                        .ToList()))
                .ToList();

            return Result.Success(response);
        });
}