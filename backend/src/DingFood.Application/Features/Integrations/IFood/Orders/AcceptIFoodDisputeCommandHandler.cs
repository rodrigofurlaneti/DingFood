using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

internal sealed class AcceptIfoodDisputeCommandHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodOrderClient orderClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<AcceptIfoodDisputeCommand, IfoodDisputeActionResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<IfoodDisputeActionResponse>> Handle(AcceptIfoodDisputeCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(AcceptIfoodDisputeCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var branch = await branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
                if (branch is null)
                    return Result.Failure<IfoodDisputeActionResponse>(new Error("Branch.NotFound", "Filial não encontrada."));

                var token = await tokenProvider.GetAccessTokenAsync(branch.CompanyId, cancellationToken);
                if (token is null)
                    return Result.Failure<IfoodDisputeActionResponse>(new Error("Ifood.NotConnected",
                        "Não foi possível autenticar com o Ifood — confira as credenciais em Integrações."));

                var result = await orderClient.AcceptDisputeAsync(token, request.DisputeId, cancellationToken);
                if (!result.Success)
                    return Result.Failure<IfoodDisputeActionResponse>(new Error("Ifood.ActionFailed", result.ErrorMessage ?? "Falha ao aceitar a disputa no Ifood."));

                return Result.Success(new IfoodDisputeActionResponse(true, result.Status));
            });
    }
}
