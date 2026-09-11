using MediatR;
using DingFood.Domain.Exceptions;
using DingFood.Domain.Repositories;

namespace DingFood.Application.Abstractions.Tenancy;

public sealed class TenantRequestBehavior<TRequest, TResponse>(ICurrentTenantService tenant, IBranchRepository branches, IPublicWorkplaceScope? publicScope = null)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var ns = typeof(TRequest).Namespace ?? "";
        if (publicScope is not null && (ns.StartsWith("DingFood.Application.Features.PublicOrdering.", StringComparison.Ordinal) ||
            ns.StartsWith("DingFood.Application.Features.Storefront.", StringComparison.Ordinal) || ns.StartsWith("DingFood.Application.Features.Checkout.", StringComparison.Ordinal) ||
            ns.StartsWith("DingFood.Application.Features.Auth.CustomerLogin", StringComparison.Ordinal) ||
            (!tenant.BranchId.HasValue && ns.StartsWith("DingFood.Application.Features.CustomerAppUser.Create", StringComparison.Ordinal))))
        {
            var properties = typeof(TRequest).GetProperties();
            var branchValue = properties.FirstOrDefault(p => p.Name == "BranchId")?.GetValue(request);
            long? branchId = branchValue is null ? null : Convert.ToInt64(branchValue);
            var token = properties.FirstOrDefault(p => p.Name == "Token")?.GetValue(request) as Guid?;
            var orderId = properties.FirstOrDefault(p => p.Name == "CustomerOrderId")?.GetValue(request) as long?;
            if (branchId.HasValue || token.HasValue || orderId.HasValue)
                await publicScope.BindAsync(branchId, token, orderId, ct);
        }
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
