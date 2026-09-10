using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetById;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetByInternalMerchantId
{
    internal sealed class GetKeetaMerchantMappingByInternalMerchantIdQueryHandler
        : BaseQueryHandler<GetKeetaMerchantMappingByInternalMerchantIdQuery, KeetaIntegrationMerchantMappingResponse>
    {
        private readonly IKeetaIntegrationMerchantMappingRepository _mappingRepository;

        public GetKeetaMerchantMappingByInternalMerchantIdQueryHandler(
            IKeetaIntegrationMerchantMappingRepository mappingRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _mappingRepository = mappingRepository;
        }

        public override async Task<Result<KeetaIntegrationMerchantMappingResponse>> Handle(
            GetKeetaMerchantMappingByInternalMerchantIdQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(GetKeetaMerchantMappingByInternalMerchantIdQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var mapping = await _mappingRepository.GetByInternalMerchantIdAsync(
                        request.InternalMerchantId, cancellationToken);

                    if (mapping is null)
                    {
                        return Result.Failure<KeetaIntegrationMerchantMappingResponse>(
                            Error.NotFound(
                                "KeetaMerchantMapping.NotFound",
                                $"Vínculo de merchant Keeta com InternalMerchantId {request.InternalMerchantId} não foi encontrado."));
                    }

                    var response = new KeetaIntegrationMerchantMappingResponse(
                        mapping.Id,
                        mapping.CompanyId,
                        mapping.BranchId,
                        mapping.InternalMerchantId,
                        mapping.KeetaMerchantId,
                        mapping.StoreName,
                        mapping.TimeZone,
                        mapping.IsAuthorized,
                        mapping.IsOnboarded,
                        mapping.LastMenuSyncAtUtc,
                        mapping.MenuBaseUrl,
                        mapping.WebhookUrl,
                        mapping.CreatedAtUtc);

                    return Result.Success(response);
                });
        }
    }
}
