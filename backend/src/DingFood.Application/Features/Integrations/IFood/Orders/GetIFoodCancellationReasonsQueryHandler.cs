using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

internal sealed class GetIfoodCancellationReasonsQueryHandler(
    IIfoodOrderRepository IfoodOrderRepository,
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodOrderClient orderClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetIfoodCancellationReasonsQuery, IReadOnlyCollection<IfoodCancellationReasonResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<IfoodCancellationReasonResponse>>> Handle(
        GetIfoodCancellationReasonsQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetIfoodCancellationReasonsQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var IfoodOrder = await IfoodOrderRepository.GetByIdForUpdateAsync(request.IfoodOrderId, cancellationToken);
                if (IfoodOrder is null)
                    return Result.Failure<IReadOnlyCollection<IfoodCancellationReasonResponse>>(
                        new Error("IfoodOrder.NotFound", "Pedido Ifood não encontrado."));

                var branch = await branchRepository.GetByIdAsync(IfoodOrder.BranchId, cancellationToken);
                if (branch is null)
                    return Result.Failure<IReadOnlyCollection<IfoodCancellationReasonResponse>>(
                        new Error("Branch.NotFound", "Filial não encontrada."));

                var token = await tokenProvider.GetAccessTokenAsync(branch.CompanyId, cancellationToken);
                if (token is null)
                    return Result.Success<IReadOnlyCollection<IfoodCancellationReasonResponse>>([]);

                var reasons = await orderClient.GetCancellationReasonsAsync(token, IfoodOrder.IfoodOrderId, cancellationToken);
                IReadOnlyCollection<IfoodCancellationReasonResponse> response = reasons
                    .Select(r => new IfoodCancellationReasonResponse(r.Code, r.Description))
                    .ToList();

                return Result.Success(response);
            });
    }
}
