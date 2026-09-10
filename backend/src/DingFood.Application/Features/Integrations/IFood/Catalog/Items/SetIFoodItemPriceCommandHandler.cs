using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Items;

internal sealed class SetIfoodItemPriceCommandHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<SetIfoodItemPriceCommand>(logRepository, unitOfWork)
{
    public override async Task<Result> Handle(SetIfoodItemPriceCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(SetIfoodItemPriceCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var priceByCatalog = request.PriceByCatalog?
                    .Select(p => new IfoodItemPriceByCatalog(p.Value, p.CatalogContext, p.OriginalValue))
                    .ToList();

                var result = await catalogClient.SetItemPriceAsync(token, merchantId, request.ItemId, request.Value, request.OriginalValue, priceByCatalog, cancellationToken);
                if (!result.Success)
                    return Result.Failure(new Error("IfoodCatalog.SetItemPriceFailed", result.ErrorMessage ?? "Falha ao atualizar o preço do item no Ifood."));

                return Result.Success();
            });
    }
}
