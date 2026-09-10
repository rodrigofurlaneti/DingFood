using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Orders.ServiceFeeSetting;

internal sealed class GetServiceFeeSettingQueryHandler(
    IServiceFeeSettingRepository settingRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetServiceFeeSettingQuery, ServiceFeeSettingResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<ServiceFeeSettingResponse>> Handle(
        GetServiceFeeSettingQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetServiceFeeSettingQueryHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                // Se o seu request possuir o Id do usuário/funcionário consultando a configuração, preencha:

                var setting = await settingRepository.GetByBranchAsync(request.BranchId, cancellationToken);
                // Sem configuracao: taxa LIGADA por padrao (cobra os 10%).
                return Result.Success(new ServiceFeeSettingResponse(setting?.Enabled ?? true));
            });
    }
}