using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;
using DomainIfoodMerchantMapping = DingFood.Domain.Entities.IfoodMerchantMapping;

namespace DingFood.Application.Features.Integrations.Ifood;

internal sealed class SetIfoodMerchantMappingCommandHandler : BaseCommandHandler<SetIfoodMerchantMappingCommand>
{
    private readonly IIfoodMerchantMappingRepository _mappingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetIfoodMerchantMappingCommandHandler(
        IIfoodMerchantMappingRepository mappingRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _mappingRepository = mappingRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(SetIfoodMerchantMappingCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(SetIfoodMerchantMappingCommandHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                // Upsert por filial — mesmo padrão do ServiceFeeSetting.
                var mapping = await _mappingRepository.GetByBranchForUpdateAsync(request.BranchId, cancellationToken);
                if (mapping is null)
                {
                    var created = DomainIfoodMerchantMapping.Create(request.BranchId);
                    if (created.IsFailure)
                        return Result.Failure(created.Error);

                    var set = created.Value.SetMerchant(request.MerchantId, request.MerchantUuid);
                    if (set.IsFailure)
                        return set;

                    await _mappingRepository.AddAsync(created.Value, cancellationToken);
                }
                else
                {
                    var set = mapping.SetMerchant(request.MerchantId, request.MerchantUuid);
                    if (set.IsFailure)
                        return set;
                }

                await _unitOfWork.CommitAsync(cancellationToken);
                return Result.Success();
            });
    }
}
