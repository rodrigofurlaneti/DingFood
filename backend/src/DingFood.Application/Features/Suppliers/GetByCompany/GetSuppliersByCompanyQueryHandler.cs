using DingFood.Application.Abstractions.Messaging;
using DingFood.Domain.Primitives;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Features.Suppliers.GetByCompany;

internal sealed class GetSuppliersByCompanyQueryHandler(
    ISupplierRepository supplierRepository,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork)
    : BaseQueryHandler<GetSuppliersByCompanyQuery, IReadOnlyCollection<SupplierResponse>>(logRepository, unitOfWork)
{
    public override async Task<Result<IReadOnlyCollection<SupplierResponse>>> Handle(
        GetSuppliersByCompanyQuery request, CancellationToken cancellationToken)
    {
        return await ExecuteWithLogAsync(
            nameof(GetSuppliersByCompanyQueryHandler),
            nameof(Handle),
            null, // Substitua pelo IP presente no request, caso aplicável
            async (userIdBox) =>
            {
                // Se o seu request possuir o Id do usuário/gestor consultando a lista, preencha:

                var suppliers = await supplierRepository.GetByCompanyAsync(request.CompanyId, cancellationToken);

                IReadOnlyCollection<SupplierResponse> response = suppliers
                    .Select(s => new SupplierResponse(s.Id, s.LegalName, s.TradeName, s.Cnpj, s.Email, s.Phone, s.IsActive))
                    .ToList();

                return Result.Success(response);
            });
    }
}