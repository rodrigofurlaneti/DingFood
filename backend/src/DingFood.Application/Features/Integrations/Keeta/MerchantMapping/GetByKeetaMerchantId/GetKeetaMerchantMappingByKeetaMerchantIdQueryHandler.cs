using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetById;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Integrations.Keeta.MerchantMapping.GetByKeetaMerchantId
{
    internal sealed class GetKeetaMerchantMappingByKeetaMerchantIdQueryHandler
        : BaseQueryHandler<GetKeetaMerchantMappingByKeetaMerchantIdQuery, KeetaIntegrationMerchantMappingResponse>
    {
        private readonly IKeetaIntegrationMerchantMappingRepository _mappingRepository;

        public GetKeetaMerchantMappingByKeetaMerchantIdQueryHandler(
            IKeetaIntegrationMerchantMappingRepository mappingRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _mappingRepository = mappingRepository;
        }

        public override async Task<Result<KeetaIntegrationMerchantMappingResponse>> Handle(
            GetKeetaMerchantMappingByKeetaMerchantIdQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(GetKeetaMerchantMappingByKeetaMerchantIdQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var mapping = await _mappingRepository.GetByKeetaMerchantIdAsync(request.KeetaMerchantId, cancellationToken);

                    if (mapping is null)
                    {
                        return Result.Failure<KeetaIntegrationMerchantMappingResponse>(
                            Error.NotFound(
                                "KeetaMerchantMapping.NotFound",
                                $"Vínculo de merchant Keeta com KeetaMerchantId {request.KeetaMerchantId} não foi encontrado."));
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
