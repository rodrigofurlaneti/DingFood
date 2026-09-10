using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Catalog.Complements.CreateComplementGroup;

internal sealed class CreateComplementGroupCommandHandler : BaseCommandHandler<CreateComplementGroupCommand, long>
{
    private readonly IComplementGroupRepository _complementGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateComplementGroupCommandHandler(
        IComplementGroupRepository complementGroupRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _complementGroupRepository = complementGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public override Task<Result<long>> Handle(CreateComplementGroupCommand request, CancellationToken cancellationToken) =>
        ExecuteWithLogAsync(
            nameof(CreateComplementGroupCommandHandler),
            nameof(Handle),
            null, // Substitua por request.IpAddress se aplicável
            async (userIdBox) =>
            {
                var complementGroup = ComplementGroup.Create(
                    request.CompanyId, request.Name, request.ComplementGroupTypeId,
                    request.MinSelection, request.MaxSelection);
                if (complementGroup.IsFailure)
                    return Result.Failure<long>(complementGroup.Error);

                await _complementGroupRepository.AddAsync(complementGroup.Value, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                // Sem TriggerCompanySync aqui: grupo recém-criado ainda não tem Complements nem
                // está vinculado a nenhum Product — não afeta o catálogo do Ifood ainda.
                return Result.Success(complementGroup.Value.Id);
            });
}
