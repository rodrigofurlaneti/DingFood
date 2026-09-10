using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Categories;

internal sealed class CreateIfoodCategoryCommandHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<CreateIfoodCategoryCommand, IfoodCategoryCreateResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<IfoodCategoryCreateResponse>> Handle(
        CreateIfoodCategoryCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(CreateIfoodCategoryCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IfoodCategoryCreateResponse>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await catalogClient.CreateCategoryAsync(token, merchantId, request.CatalogId, request.Name, cancellationToken);
                if (!result.Success)
                    return Result.Failure<IfoodCategoryCreateResponse>(new Error("IfoodCatalog.CreateCategoryFailed", result.ErrorMessage ?? "Falha ao criar a categoria no Ifood."));

                return Result.Success(new IfoodCategoryCreateResponse(result.IfoodCategoryId));
            });
    }
}
