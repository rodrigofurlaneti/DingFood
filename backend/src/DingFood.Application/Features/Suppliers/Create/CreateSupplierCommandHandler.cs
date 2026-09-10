using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Entities;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Suppliers.Create;

internal sealed class CreateSupplierCommandHandler : BaseCommandHandler<CreateSupplierCommand, long>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSupplierCommandHandler(
        ISupplierRepository supplierRepository,
        ILogTrackerRepository logRepository,
        IUnitOfWork unitOfWork)
        : base(logRepository, unitOfWork)
    {
        _supplierRepository = supplierRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<long>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(CreateSupplierCommandHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                // Se o seu request possuir o Id do usuário/gestor criando o fornecedor, preencha:

                var supplier = Supplier.Create(
                    request.CompanyId, request.LegalName, request.TradeName, request.Cnpj, request.Email, request.Phone);

                if (supplier.IsFailure)
                    return Result.Failure<long>(supplier.Error);

                await _supplierRepository.AddAsync(supplier.Value, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return Result.Success(supplier.Value.Id);
            });
    }
}