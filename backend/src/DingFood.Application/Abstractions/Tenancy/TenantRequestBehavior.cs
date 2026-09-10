using MediatR;
using DingFood.Domain.Exceptions;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Abstractions.Tenancy;

public sealed class TenantRequestBehavior<TRequest, TResponse>(ICurrentTenantService tenant, IBranchRepository branches)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (tenant.CompanyId is { } companyId)
        {
            foreach (var property in typeof(TRequest).GetProperties())
            {
                if (property.GetIndexParameters().Length != 0) continue;
                if (property.Name == "CompanyId" && property.GetValue(request) is long requested && requested != companyId)
                    throw new TenantAccessException();
                if (property.Name.EndsWith("BranchId", StringComparison.Ordinal) && property.GetValue(request) is long branchId && branchId > 0)
                {
                    var branch = await branches.GetByIdAsync(branchId, ct);
                    if (branch is null || branch.CompanyId != companyId || !branch.IsActive) throw new TenantAccessException();
                }
            }
        }
        return await next();
    }
}
