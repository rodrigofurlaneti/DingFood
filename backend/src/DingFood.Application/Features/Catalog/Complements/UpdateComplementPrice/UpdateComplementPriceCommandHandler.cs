using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Catalog.Complements.UpdateComplementPrice;

internal sealed class UpdateComplementPriceCommandHandler : BaseCommandHandler<UpdateComplementPriceCommand>
{
    private readonly IComplementGroupRepository _complementGroupRepository;
    private readonly IIfoodCatalogSyncTrigger _catalogSyncTrigger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateComplementPriceCommandHandler(
        IComplementGroupRepository complementGroupRepository,
        IIfoodCatalogSyncTrigger catalogSyncTrigger,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _complementGroupRepository = complementGroupRepository;
        _catalogSyncTrigger = catalogSyncTrigger;
        _unitOfWork = unitOfWork;
    }

    public override Task<Result> Handle(UpdateComplementPriceCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(
            nameof(UpdateComplementPriceCommandHandler),
            nameof(Handle),
            null, // Substitua por request.IpAddress se aplicável
            async (userIdBox) =>
            {
                var complementGroup = await _complementGroupRepository.GetByIdForUpdateAsync(request.ComplementGroupId, cancellationToken);
                if (complementGroup is null || !complementGroup.IsActive)
                    return Result.Failure(new Error("ComplementGroup.NotFound", "Complement group not found."));

                var result = complementGroup.UpdateComplementPrice(request.ComplementId, request.ExtraPrice);
                if (result.IsFailure)
                    return result;

                await _unitOfWork.CommitAsync(cancellationToken);

                _catalogSyncTrigger.TriggerCompanySync(complementGroup.CompanyId);

                return Result.Success();
            });
}
