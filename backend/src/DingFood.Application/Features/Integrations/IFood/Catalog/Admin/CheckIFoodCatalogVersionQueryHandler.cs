using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.Admin;

internal sealed class CheckIfoodCatalogVersionQueryHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<CheckIfoodCatalogVersionQuery, IfoodCatalogVersionResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<IfoodCatalogVersionResponse>> Handle(
        CheckIfoodCatalogVersionQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(CheckIfoodCatalogVersionQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IfoodCatalogVersionResponse>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await catalogClient.CheckVersionAsync(token, merchantId, cancellationToken);
                if (!result.Success)
                    return Result.Failure<IfoodCatalogVersionResponse>(new Error("IfoodCatalog.VersionCheckFailed", result.ErrorMessage ?? "Falha ao consultar a versão do catálogo no Ifood."));

                return Result.Success(new IfoodCatalogVersionResponse(result.Version));
            });
    }
}
