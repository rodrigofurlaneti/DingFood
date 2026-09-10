using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Items;

internal sealed class GetIfoodItemFlatQueryHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetIfoodItemFlatQuery, IfoodItemFlatResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<IfoodItemFlatResponse>> Handle(
        GetIfoodItemFlatQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetIfoodItemFlatQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IfoodItemFlatResponse>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await catalogClient.GetItemFlatAsync(token, merchantId, request.ItemId, cancellationToken);
                if (!result.Success)
                    return Result.Failure<IfoodItemFlatResponse>(new Error("IfoodCatalog.ItemFlatFetchFailed", result.ErrorMessage ?? "Falha ao buscar o item no Ifood."));

                return Result.Success(new IfoodItemFlatResponse(
                    result.ItemId, result.Status, result.PriceValue, result.ExternalCode, result.CategoryId, result.RawPayload));
            });
    }
}
