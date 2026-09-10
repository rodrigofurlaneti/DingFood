using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Items;

internal sealed class DeleteIfoodItemCommandHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<DeleteIfoodItemCommand>(logRepository, unitOfWork)
{
    public override async Task<Result> Handle(DeleteIfoodItemCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(DeleteIfoodItemCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await catalogClient.DeleteItemAsync(token, merchantId, request.CategoryId, request.ProductId, request.CatalogContext, cancellationToken);
                if (!result.Success)
                    return Result.Failure(new Error("IfoodCatalog.DeleteItemFailed", result.ErrorMessage ?? "Falha ao excluir o item no Ifood."));

                return Result.Success();
            });
    }
}
