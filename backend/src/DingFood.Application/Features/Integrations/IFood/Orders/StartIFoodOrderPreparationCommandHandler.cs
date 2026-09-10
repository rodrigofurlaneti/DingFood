using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Constants;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Orders;

internal sealed class StartIfoodOrderPreparationCommandHandler : BaseCommandHandler<StartIfoodOrderPreparationCommand>
{
    private readonly IIfoodOrderRepository _IfoodOrderRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IIfoodTokenProvider _tokenProvider;
    private readonly IIfoodOrderClient _orderClient;
    private readonly TimeProvider _timeProviderCustom;
    private readonly IUnitOfWork _unitOfWork;

    public StartIfoodOrderPreparationCommandHandler(
        IIfoodOrderRepository IfoodOrderRepository,
        IBranchRepository branchRepository,
        IIfoodTokenProvider tokenProvider,
        IIfoodOrderClient orderClient,
        TimeProvider timeProviderCustom,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _IfoodOrderRepository = IfoodOrderRepository;
        _branchRepository = branchRepository;
        _tokenProvider = tokenProvider;
        _orderClient = orderClient;
        _timeProviderCustom = timeProviderCustom;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(StartIfoodOrderPreparationCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(StartIfoodOrderPreparationCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var IfoodOrder = await _IfoodOrderRepository.GetByIdForUpdateAsync(request.IfoodOrderId, cancellationToken);
                if (IfoodOrder is null)
                    return Result.Failure(new Error("IfoodOrder.NotFound", "Pedido Ifood não encontrado."));

                var branch = await _branchRepository.GetByIdAsync(IfoodOrder.BranchId, cancellationToken);
                if (branch is null)
                    return Result.Failure(new Error("Branch.NotFound", "Filial não encontrada."));

                var token = await _tokenProvider.GetAccessTokenAsync(branch.CompanyId, cancellationToken);
                if (token is null)
                    return Result.Failure(new Error("Ifood.NotConnected",
                        "Não foi possível autenticar com o Ifood — confira as credenciais em Integrações."));

                var actionResult = await _orderClient.StartPreparationAsync(token, IfoodOrder.IfoodOrderId, cancellationToken);
                if (!actionResult.Success)
                    return Result.Failure(new Error("Ifood.ActionFailed", actionResult.ErrorMessage ?? "Falha ao iniciar preparo no Ifood."));

                var now = _timeProviderCustom.GetLocalNow().DateTime;
                IfoodOrder.SetStatus(IfoodOrderStatuses.PreparationStarted, now);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success();
            });
    }
}
