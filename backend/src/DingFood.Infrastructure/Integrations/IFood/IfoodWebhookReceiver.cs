using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Application.Abstractions.Security;
using DingFood.Domain.Repositories;

namespace DingFood.Infrastructure.Integrations.Ifood;

internal sealed class IfoodWebhookReceiver(
    IIfoodIntegrationSettingRepository settings, IIfoodMerchantMappingRepository mappings,
    ISecretProtector protector, IIfoodEventInbox inbox, IBranchRepository? branches = null) : IIfoodWebhookReceiver
{
    internal static bool Verify(byte[] body, string secret, string? signature)
    {
        if (signature is null || signature.Length != 64) return false;
        byte[] supplied;
        try { supplied = Convert.FromHexString(signature); }
        catch (FormatException) { return false; }
        var expected = HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), body);
        return CryptographicOperations.FixedTimeEquals(expected, supplied);
    }

    public Task<IfoodWebhookReceipt> ReceiveAsync(byte[] body, string? signature, CancellationToken ct)
        => ReceiveForScopeAsync(null, body, signature, ct);

    public Task<IfoodWebhookReceipt> ReceiveAsync(long companyId, byte[] body, string? signature, CancellationToken ct)
        => ReceiveForScopeAsync(companyId, body, signature, ct);

    private async Task<IfoodWebhookReceipt> ReceiveForScopeAsync(long? selectedCompany, byte[] body, string? signature, CancellationToken ct)
    {
        if (signature is null || signature.Length != 64) return new(401);
        var configurations = await settings.GetEnabledSettingsAsync(ct);
        var authenticated = new List<DingFood.Domain.Entities.IfoodIntegrationSetting>();
        var usable = false;
        foreach (var setting in configurations.Where(s => !selectedCompany.HasValue || s.CompanyId == selectedCompany))
        {
            if (!setting.IsActive || !setting.Enabled || setting.EventDeliveryMode != "Webhook" || string.IsNullOrWhiteSpace(setting.ClientSecretEncrypted)) continue;
            try
            {
                var secret = protector.Unprotect("DingFood.Integrations.Ifood.ClientSecret.v1", setting.ClientSecretEncrypted);
                usable = true;
                if (Verify(body, secret, signature)) authenticated.Add(setting);
            }
            catch (CryptographicException) { }
        }
        if (authenticated.Count == 0) return new(usable ? 401 : 503);
        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object) return new(400);
            var code = String(root, "fullCode") ?? String(root, "code");
            if (string.IsNullOrWhiteSpace(code)) return new(400);
            var merchants = new Dictionary<string, HashSet<long>>(StringComparer.OrdinalIgnoreCase);
            foreach (var setting in authenticated)
            {
                foreach (var mapping in (await mappings.GetByCompanyAsync(setting.CompanyId, ct)).Values.Where(m => m.IsActive && !string.IsNullOrWhiteSpace(m.MerchantUuid)))
                {
                    if (setting.BrandId.HasValue && (branches is null || (await branches.GetByIdAsync(mapping.BranchId, ct))?.BrandId != setting.BrandId)) continue;
                    if (!merchants.TryGetValue(mapping.MerchantUuid!, out var owners)) merchants[mapping.MerchantUuid!] = owners = [];
                    owners.Add(setting.CompanyId);
                }
            }
            if (code == "KEEPALIVE")
            {
                if (!root.TryGetProperty("merchantIds", out var requested)) return new(merchants.Count > 0 ? 202 : 503);
                if (requested.ValueKind != JsonValueKind.Array || requested.GetArrayLength() > 1000 || requested.EnumerateArray().Any(x => x.ValueKind != JsonValueKind.String)) return new(400);
                return new(202, requested.EnumerateArray().Select(x => x.GetString()!).Where(merchants.ContainsKey).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
            }
            var id = String(root, "id"); var orderId = String(root, "orderId"); var merchantId = String(root, "merchantId");
            if (string.IsNullOrWhiteSpace(id) || id.Length > 100 || string.IsNullOrWhiteSpace(orderId) ||
                !root.TryGetProperty("createdAt", out var createdAt) || createdAt.ValueKind != JsonValueKind.String || !createdAt.TryGetDateTimeOffset(out _)) return new(400);
            if (merchantId is null || !merchants.TryGetValue(merchantId, out var candidates)) return new(403);
            if (candidates.Count != 1) return new(503);
            await inbox.EnqueueAsync(candidates.Single(), id, Encoding.UTF8.GetString(body), ct);
            return new(202);
        }
        catch (JsonException) { return new(400); }
    }
    private static string? String(JsonElement element, string name) =>
        element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String ? value.GetString() : null;
}
