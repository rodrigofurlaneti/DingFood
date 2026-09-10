using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Orders.GetTableReadingValidationSetting;

internal sealed class GetTableReadingValidationSettingQueryHandler(
    IDiningTableRepository diningTableRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetTableReadingValidationSettingQuery, TableReadingValidationSettingResponse>(logRepository, unitOfWork)
{
    public override async Task<Result<TableReadingValidationSettingResponse>> Handle(
        GetTableReadingValidationSettingQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetTableReadingValidationSettingQueryHandler),
            nameof(Handle),
            null,
            async (userIdBox) =>
            {
                var tables = await diningTableRepository.GetByBranchAsync(request.BranchId, cancellationToken);
                // Todas as mesas da filial compartilham o mesmo valor (espelha GetQrViewSettingQueryHandler):
                // basta ler a primeira. Sem mesas cadastradas, os três cenários nascem desligados.
                var first = tables.FirstOrDefault();
                var response = first is null
                    ? new TableReadingValidationSettingResponse(false, false, false)
                    : new TableReadingValidationSettingResponse(
                        first.IsCameraInputEnabled, first.IsBarcodeEnabled, first.IsQrCodeEnabled);
                return Result.Success(response);
            });
    }
}
