using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Comandas.Settings;

internal sealed class GetComandaSettingQueryHandler(
    IComandaSettingRepository settingRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetComandaSettingQuery, ComandaSettingResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<ComandaSettingResponse>> Handle(
        GetComandaSettingQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetComandaSettingQueryHandler),
            nameof(Handle),
            null, // Substitua por request.IpAddress se você tiver essa propriedade mapeada
            async (userIdBox) =>
            {
                // Se o seu request possuir o Id do usuário, associe-o aqui:

                var setting = await settingRepository.GetByBranchAsync(request.BranchId, cancellationToken);

                // Sem configuracao: comandas sem limite (0 = ilimitado na exibicao).
                return Result.Success(new ComandaSettingResponse(setting?.DefaultLimitAmount ?? 0));
            });
    }
}