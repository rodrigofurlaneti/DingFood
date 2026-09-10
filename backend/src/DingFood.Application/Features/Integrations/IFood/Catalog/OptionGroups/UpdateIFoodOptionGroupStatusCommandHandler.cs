using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Catalog.OptionGroups;

internal sealed class UpdateIfoodOptionGroupStatusCommandHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodCatalogClient catalogClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<UpdateIfoodOptionGroupStatusCommand>(logRepository, unitOfWork)
{
    public override async Task<Result> Handle(UpdateIfoodOptionGroupStatusCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(UpdateIfoodOptionGroupStatusCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await catalogClient.UpdateOptionGroupStatusAsync(token, merchantId, request.OptionGroupId, request.Available, cancellationToken);
                if (!result.Success)
                    return Result.Failure(new Error("IfoodCatalog.UpdateOptionGroupStatusFailed", result.ErrorMessage ?? "Falha ao atualizar o status do grupo de opções no Ifood."));

                return Result.Success();
            });
    }
}
