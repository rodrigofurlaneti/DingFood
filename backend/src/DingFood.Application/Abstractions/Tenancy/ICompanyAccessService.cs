namespace DingFood.Application.Abstractions.Tenancy;

public sealed record CompanyAccess(long CompanyId, long BusinessGroupId, string GroupName, string TradeName,
    string Cnpj, long? EmployeeId, IReadOnlyCollection<string> Roles, IReadOnlyCollection<string> Permissions);

public interface ICompanyAccessService
{
    Task<IReadOnlyCollection<CompanyAccess>> GetAllowedAsync(long userId, CancellationToken ct);
    Task<CompanyAccess?> ResolveAsync(long userId, long companyId, CancellationToken ct);
}
