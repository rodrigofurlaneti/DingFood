using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Products;

internal sealed class BatchUpdateIfoodProductStatusesCommandHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<BatchUpdateIfoodProductStatusesCommand>(logRepository, unitOfWork)
{
    public override async Task<Result> Handle(BatchUpdateIfoodProductStatusesCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(BatchUpdateIfoodProductStatusesCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var items = request.Items
                    .Select(i => new IfoodBatchProductStatusItem(i.ProductId, i.ExternalCode, i.Status, i.Resources))
                    .ToList();

                var result = await catalogClient.BatchUpdateProductStatusesAsync(token, merchantId, items, request.CatalogContext, cancellationToken);
                if (!result.Success)
                    return Result.Failure(new Error("IfoodCatalog.BatchUpdateProductStatusesFailed", result.ErrorMessage ?? "Falha ao atualizar o status dos produtos em lote no Ifood."));

                return Result.Success();
            });
    }
}
