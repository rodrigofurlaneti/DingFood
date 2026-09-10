using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Categories;

internal sealed class ListIfoodCategoriesQueryHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<ListIfoodCategoriesQuery, IReadOnlyCollection<IfoodCategoryResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<IfoodCategoryResponse>>> Handle(
        ListIfoodCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(ListIfoodCategoriesQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IReadOnlyCollection<IfoodCategoryResponse>>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await catalogClient.ListCategoriesAsync(token, merchantId, request.CatalogId, request.IncludeItems, cancellationToken);
                if (!result.Success)
                    return Result.Failure<IReadOnlyCollection<IfoodCategoryResponse>>(new Error("IfoodCatalog.CategoriesFetchFailed", result.ErrorMessage ?? "Falha ao listar as categorias do catálogo no Ifood."));

                IReadOnlyCollection<IfoodCategoryResponse> responses = result.Categories
                    .Select(c => new IfoodCategoryResponse(c.Id, c.Index, c.Name, c.ExternalCode, c.Status, c.Template))
                    .ToList();

                return Result.Success(responses);
            });
    }
}
