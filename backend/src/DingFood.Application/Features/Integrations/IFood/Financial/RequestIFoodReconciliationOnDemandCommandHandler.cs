using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Ifood.Merchant;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Financial;

internal sealed class RequestIfoodReconciliationOnDemandCommandHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodFinancialClient financialClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseCommandHandler<RequestIfoodReconciliationOnDemandCommand, IfoodReconciliationOnDemandResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<IfoodReconciliationOnDemandResponse>> Handle(
        RequestIfoodReconciliationOnDemandCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(RequestIfoodReconciliationOnDemandCommandHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IfoodReconciliationOnDemandResponse>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await financialClient.RequestReconciliationOnDemandAsync(token, merchantId, request.Competence, cancellationToken);

                return Result.Success(new IfoodReconciliationOnDemandResponse(result.RequestId, result.RawPayload));
            });
    }
}
