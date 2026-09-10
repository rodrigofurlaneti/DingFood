using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Suppliers.GetByCompany;

public sealed record GetSuppliersByCompanyQuery(long CompanyId) : IQuery<IReadOnlyCollection<SupplierResponse>>;
