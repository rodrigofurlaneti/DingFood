using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Security;
using DingFood.Domain.Entities;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Integrations.Ifood;

internal sealed class IfoodTokenProvider(
    IMemoryCache cache,
    IIfoodIntegrationSettingRepository settingRepository,
    ISecretProtector secretProtector,
    IIfoodAuthClient authClient,
    ILogTrackerRepository logRepository,
    IUnitOfWork unitOfWork,
    DingFood.Application.Abstractions.Tenancy.ICurrentTenantService? tenant = null) : IIfoodTokenProvider
{
    private const string ProtectorPurpose = "DingFood.Integrations.Ifood.ClientSecret.v1";

    private string CacheKey(long companyId) => $"Ifood:token:{companyId}:brand:{tenant?.BrandId}";

    public async Task<string?> GetAccessTokenAsync(long companyId, CancellationToken cancellationToken = default)
    {
        if (tenant?.CompanyId is { } activeCompany && activeCompany != companyId) return null;
        var setting = await settingRepository.GetByCompanyAsync(companyId, cancellationToken);
        if (setting is null || !setting.IsActive || !setting.Enabled ||
            setting.CompanyId != companyId || setting.BrandId != tenant?.BrandId)
        {
            Invalidate(companyId);
            return null;
        }
        if (cache.TryGetValue<string>(CacheKey(companyId), out var cached) && !string.IsNullOrEmpty(cached))
            return cached;
        if (string.IsNullOrWhiteSpace(setting.ClientId) || string.IsNullOrWhiteSpace(setting.ClientSecretEncrypted))
        {
            var log = new LogTracker(0)
            {
                AppUserId = null,
                DirectoryName = "Infrastructure/Integrations/Ifood",
                ClassName = "IfoodTokenProvider",
                MethodName = nameof(GetAccessTokenAsync),
                IsSuccess = false,
                ExecutionTimeMs = 0,
                ErrorMessage = $"iFood habilitado com credenciais incompletas. CompanyId={companyId}; BrandId={tenant?.BrandId}; ClientId ausente={string.IsNullOrWhiteSpace(setting.ClientId)}; ClientSecret ausente={string.IsNullOrWhiteSpace(setting.ClientSecretEncrypted)}.",
                StackTrace = string.Empty,
                IpAddress = null,
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            await logRepository.AddAsync(log, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return null;
        }

        string clientSecret;
        try
        {
            clientSecret = secretProtector.Unprotect(ProtectorPurpose, setting.ClientSecretEncrypted);
        }
        catch (Exception ex)
        {
            var log = new LogTracker(0)
            {
                AppUserId = null,
                DirectoryName = "Infrastructure/Integrations/Ifood",
                ClassName = "IfoodTokenProvider",
                MethodName = nameof(GetAccessTokenAsync),
                IsSuccess = false,
                ExecutionTimeMs = 0,
                ErrorMessage = $"CryptographicException (Chave perdida/alterada): CompanyId={companyId}; BrandId={setting.BrandId}; {ex.Message}",
                StackTrace = ex.StackTrace ?? string.Empty,
                IpAddress = null,
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            await logRepository.AddAsync(log, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return null;
        }

        var auth = await authClient.AuthenticateAsync(setting.ClientId, clientSecret, cancellationToken);
        if (!auth.Success || auth.AccessToken is null)
            return null;

        var ttl = TimeSpan.FromSeconds(Math.Max(30, (auth.ExpiresInSeconds ?? 180) - 60));
        cache.Set(CacheKey(companyId), auth.AccessToken, ttl);
        return auth.AccessToken;
    }

    public async Task<string?> GetAccessTokenAsync(long companyId, Stopwatch stopwatch, CancellationToken cancellationToken = default)
    {
        if (tenant?.CompanyId is { } activeCompany && activeCompany != companyId) return null;
        var setting = await settingRepository.GetByCompanyAsync(companyId, cancellationToken);
        if (setting is null || !setting.IsActive || !setting.Enabled ||
            setting.CompanyId != companyId || setting.BrandId != tenant?.BrandId)
        {
            Invalidate(companyId);
            return null;
        }
        if (cache.TryGetValue<string>(CacheKey(companyId), out var cached) && !string.IsNullOrEmpty(cached))
            return cached;
        if (string.IsNullOrWhiteSpace(setting.ClientId) || string.IsNullOrWhiteSpace(setting.ClientSecretEncrypted))
        {
            var log = new LogTracker(0)
            {
                AppUserId = null,
                DirectoryName = "Infrastructure/Integrations/Ifood",
                ClassName = "IfoodTokenProvider",
                MethodName = nameof(GetAccessTokenAsync),
                IsSuccess = false,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                ErrorMessage = $"iFood habilitado com credenciais incompletas. CompanyId={companyId}; BrandId={tenant?.BrandId}; ClientId ausente={string.IsNullOrWhiteSpace(setting.ClientId)}; ClientSecret ausente={string.IsNullOrWhiteSpace(setting.ClientSecretEncrypted)}.",
                StackTrace = string.Empty,
                IpAddress = null,
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            await logRepository.AddAsync(log, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            return null;
        }

        string clientSecret;
        try
        {
            clientSecret = secretProtector.Unprotect(ProtectorPurpose, setting.ClientSecretEncrypted);
        }
        catch (Exception ex)
        {
            var log = new LogTracker(0)
            {
                AppUserId = null,
                DirectoryName = "Infrastructure/Integrations/Ifood",
                ClassName = "IfoodTokenProvider",
                MethodName = nameof(GetAccessTokenAsync),
                IsSuccess = false,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
                ErrorMessage = $"CryptographicException (Chave perdida/alterada): CompanyId={companyId}; BrandId={setting.BrandId}; {ex.Message}",
                StackTrace = ex.StackTrace ?? string.Empty,
                IpAddress = null,
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            await logRepository.AddAsync(log, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return null;
        }

        var auth = await authClient.AuthenticateAsync(setting.ClientId, clientSecret, cancellationToken);
        if (!auth.Success || auth.AccessToken is null)
            return null;

        var ttl = TimeSpan.FromSeconds(Math.Max(30, (auth.ExpiresInSeconds ?? 180) - 60));
        cache.Set(CacheKey(companyId), auth.AccessToken, ttl);
        return auth.AccessToken;
    }

    public void Invalidate(long companyId) => cache.Remove(CacheKey(companyId));
}
