using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Admin;

internal sealed class GetIfoodBatchResultQueryHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetIfoodBatchResultQuery, IfoodBatchStatusResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<IfoodBatchStatusResponse>> Handle(
        GetIfoodBatchResultQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetIfoodBatchResultQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IfoodBatchStatusResponse>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await catalogClient.GetBatchResultAsync(token, merchantId, request.BatchId, cancellationToken);
                if (!result.Success)
                    return Result.Failure<IfoodBatchStatusResponse>(new Error("IfoodCatalog.BatchResultFetchFailed", result.ErrorMessage ?? "Falha ao consultar o resultado do lote no Ifood."));

                var items = result.Results
                    .Select(r => new IfoodBatchResultItemResponse(r.ResourceId, r.Result, r.FailureReason))
                    .ToList();

                return Result.Success(new IfoodBatchStatusResponse(result.BatchStatus, items));
            });
    }
}
