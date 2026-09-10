using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Tables.SetReadingValidation;

internal sealed class SetDiningTableReadingValidationCommandHandler : BaseCommandHandler<SetDiningTableReadingValidationCommand>
{
    private readonly IDiningTableRepository _diningTableRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetDiningTableReadingValidationCommandHandler(
        IDiningTableRepository diningTableRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _diningTableRepository = diningTableRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(SetDiningTableReadingValidationCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(SetDiningTableReadingValidationCommandHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                var table = await _diningTableRepository.GetByIdForUpdateAsync(request.DiningTableId, cancellationToken);
                if (table is null || !table.IsActive)
                    return Result.Failure(new Error("DiningTable.NotFound", "Dining table not found."));

                table.SetReadingValidationSettings(request.IsCameraInputEnabled, request.IsBarcodeEnabled, request.IsQrCodeEnabled);

                await _unitOfWork.CommitAsync(cancellationToken);
                return Result.Success();
            });
    }
}
