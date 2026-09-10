using DingFood.Application.Abstractions.Messaging;
using DingFood.Application.Abstractions.Security;
using DingFood.Application.Abstractions.Tenancy;
using DingFood.Domain.Primitives;

namespace DingFood.Application.Features.Companies;

public sealed record GetAllowedCompaniesQuery : IQuery<IReadOnlyCollection<CompanyAccess>>;

internal sealed class GetAllowedCompaniesQueryHandler(ICompanyAccessService access, ICurrentUserService user)
    : IQueryHandler<GetAllowedCompaniesQuery, IReadOnlyCollection<CompanyAccess>>
{
    public async Task<Result<IReadOnlyCollection<CompanyAccess>>> Handle(GetAllowedCompaniesQuery request, CancellationToken ct)
        => Result.Success(await access.GetAllowedAsync(user.UserId, ct));
}
