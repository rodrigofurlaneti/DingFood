using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DingFood.Application.Abstractions.Authentication;
using DingFood.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DingFood.Infrastructure.Authentication;

internal sealed class JwtTokenProvider(IOptions<JwtOptions> options) : IJwtTokenProvider
{
    private readonly JwtOptions _options = options.Value;

    public AccessToken GenerateCustomerToken(CustomerAppUser customer, IReadOnlyCollection<string> roles, IReadOnlyCollection<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, customer.UserName),
            new(JwtRegisteredClaimNames.Email, customer.Email),
            new("companyId", customer.CompanyId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (customer.BrandId is { } brandId) claims.Add(new Claim("brandId", brandId.ToString()));
        if (customer.CustomerId is { } customerId)
        {
            claims.Add(new Claim("customerId", customerId.ToString()));
        }

        if (customer.BranchId is { } branchId)
        {
            claims.Add(new Claim("branchId", branchId.ToString()));
        }

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var expiresAt = DateTime.Now.AddMinutes(_options.ExpiresInMinutes);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    public AccessToken GenerateToken(AppUser user, IReadOnlyCollection<string> roles, IReadOnlyCollection<string> permissions)
        => GenerateUserToken(user, roles, permissions, user.CompanyId, null, user.EmployeeId);

    public AccessToken GenerateCompanyToken(AppUser user, DingFood.Application.Abstractions.Tenancy.CompanyAccess company)
        => GenerateUserToken(user, company.Roles, company.Permissions, company.CompanyId, company.BusinessGroupId, company.EmployeeId);

    private AccessToken GenerateUserToken(AppUser user, IReadOnlyCollection<string> roles, IReadOnlyCollection<string> permissions,
        long companyId, long? businessGroupId, long? employeeId)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("companyId", companyId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (businessGroupId.HasValue) claims.Add(new Claim("businessGroupId", businessGroupId.Value.ToString()));
        claims.Add(new Claim("accountType", "AppUser"));
        if (employeeId.HasValue)
            claims.Add(new Claim("employeeId", employeeId.Value.ToString()));

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var expiresAt = DateTime.Now.AddMinutes(_options.ExpiresInMinutes);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }

    public string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
