using DingFood.Application.Abstractions.Messaging;

namespace DingFood.Application.Features.Customers.GetByCompany;

public sealed record GetCustomersByCompanyQuery(long CompanyId, string? Search) : IQuery<IReadOnlyCollection<CustomerResponse>>;
