using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Orders.SetTableReadingValidation;

internal sealed class SetTableReadingValidationCommandHandler : BaseCommandHandler<SetTableReadingValidationCommand>
{
    private readonly IDiningTableRepository _diningTableRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetTableReadingValidationCommandHandler(
        IDiningTableRepository diningTableRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _diningTableRepository = diningTableRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(SetTableReadingValidationCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(SetTableReadingValidationCommandHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                var tables = await _diningTableRepository.GetByBranchAsync(request.BranchId, cancellationToken);
                foreach (var table in tables)
                {
                    table.SetReadingValidationSettings(
                        request.IsCameraInputEnabled, request.IsBarcodeEnabled, request.IsQrCodeEnabled);
                    _diningTableRepository.Update(table);
                }

                await _unitOfWork.CommitAsync(cancellationToken);
                return Result.Success();
            });
    }
}
