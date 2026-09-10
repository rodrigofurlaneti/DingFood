using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Products;

internal sealed class BatchUpdateIfoodProductPricesCommandHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<BatchUpdateIfoodProductPricesCommand, IfoodBatchDispatchResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<IfoodBatchDispatchResponse>> Handle(
        BatchUpdateIfoodProductPricesCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(BatchUpdateIfoodProductPricesCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IfoodBatchDispatchResponse>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var items = request.Items
                    .Select(i => new IfoodBatchProductPriceItem(i.ProductId, i.ExternalCode, i.Value, i.OriginalValue, i.Resources))
                    .ToList();

                var result = await catalogClient.BatchUpdateProductPricesAsync(token, merchantId, items, request.CatalogContext, cancellationToken);
                if (!result.Success)
                    return Result.Failure<IfoodBatchDispatchResponse>(new Error("IfoodCatalog.BatchUpdateProductPricesFailed", result.ErrorMessage ?? "Falha ao atualizar o preço dos produtos em lote no Ifood."));

                return Result.Success(new IfoodBatchDispatchResponse(result.Url, result.BatchId));
            });
    }
}
