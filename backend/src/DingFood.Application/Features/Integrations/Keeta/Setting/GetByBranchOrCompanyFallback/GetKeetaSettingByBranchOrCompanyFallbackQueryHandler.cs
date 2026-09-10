using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Features.Integrations.Keeta.Setting.GetById;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
namespace DingFood.Application.Features.Integrations.Keeta.Setting.GetByBranchOrCompanyFallback
{
    internal sealed class GetKeetaSettingByBranchOrCompanyFallbackQueryHandler
        : BaseQueryHandler<GetKeetaSettingByBranchOrCompanyFallbackQuery, KeetaIntegrationSettingResponse>
    {
        private readonly IKeetaIntegrationSettingRepository _settingRepository;

        public GetKeetaSettingByBranchOrCompanyFallbackQueryHandler(
            IKeetaIntegrationSettingRepository settingRepository,
            ILogTrackerRepository logRepository,
            IUnitOfWork unitOfWork)
            : base(logRepository, unitOfWork)
        {
            _settingRepository = settingRepository;
        }

        public override async Task<Result<KeetaIntegrationSettingResponse>> Handle(
            GetKeetaSettingByBranchOrCompanyFallbackQuery request,
            CancellationToken cancellationToken)
        {
            return await ExecuteWithLogAsync(
                nameof(GetKeetaSettingByBranchOrCompanyFallbackQueryHandler),
                nameof(Handle),
                null,
                async (userIdBox) =>
                {
                    var setting = await _settingRepository.GetByBranchOrCompanyFallbackAsync(
                        request.CompanyId, request.BranchId, cancellationToken);

                    if (setting is null)
                    {
                        return Result.Failure<KeetaIntegrationSettingResponse>(
                            Error.NotFound(
                                "KeetaSetting.NotFound",
                                $"Nenhuma configuração de integração Keeta encontrada para a empresa {request.CompanyId}."));
                    }

                    var response = new KeetaIntegrationSettingResponse(
                        setting.Id,
                        setting.CompanyId,
                        setting.BranchId,
                        setting.ClientId,
                        setting.AppId,
                        setting.BaseUrl,
                        !string.IsNullOrWhiteSpace(setting.CurrentAccessToken),
                        setting.TokenExpiresAtUtc,
                        setting.UpdatedAtUtc);

                    return Result.Success(response);
                });
        }
    }
}
