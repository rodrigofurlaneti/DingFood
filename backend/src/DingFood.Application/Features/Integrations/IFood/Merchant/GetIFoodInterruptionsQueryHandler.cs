using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Integrations.Ifood.Merchant;

internal sealed class GetIfoodInterruptionsQueryHandler(
    IBranchRepository branchRepository,
    IIfoodTokenProvider tokenProvider,
    IIfoodIntegrationSettingRepository settingRepository,
    IIfoodMerchantMappingRepository mappingRepository,
    IIfoodMerchantClient merchantClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetIfoodInterruptionsQuery, IReadOnlyCollection<IfoodInterruptionResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<IfoodInterruptionResponse>>> Handle(
        GetIfoodInterruptionsQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetIfoodInterruptionsQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var resolved = await IfoodMerchantResolution.ResolveAsync(
                    request.BranchId, branchRepository, tokenProvider, settingRepository, mappingRepository, cancellationToken);
                if (resolved.IsFailure)
                    return Result.Failure<IReadOnlyCollection<IfoodInterruptionResponse>>(resolved.Error);

                var (_, merchantId, token, _) = resolved.Value;
                var result = await merchantClient.GetInterruptionsAsync(token, merchantId, cancellationToken);
                if (!result.Success)
                    return Result.Failure<IReadOnlyCollection<IfoodInterruptionResponse>>(new Error("IfoodMerchant.InterruptionsFailed", result.ErrorMessage ?? "Failed to fetch interruptions from Ifood."));

                IReadOnlyCollection<IfoodInterruptionResponse> response = result.Interruptions
                    .Select(i => new IfoodInterruptionResponse(i.Id, i.Description, i.Start, i.End))
                    .ToList();

                return Result.Success(response);
            });
    }
}
