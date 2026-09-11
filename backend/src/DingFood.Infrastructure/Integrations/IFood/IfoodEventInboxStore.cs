using Microsoft.EntityFrameworkCore;
using DingFood.Application.Abstractions.Integrations.Ifood;
using DingFood.Domain.Entities;
using DingFood.Infrastructure.Persistence;

namespace DingFood.Infrastructure.Integrations.Ifood;

internal sealed class IfoodEventInboxStore(AppDbContext db, TimeProvider time) : IIfoodEventInbox
{
    public async Task EnqueueAsync(long companyId, string eventId, string payload, CancellationToken ct)
    {
        if (await db.Set<IfoodEventInbox>().AnyAsync(row => row.CompanyId == companyId && row.EventId == eventId, ct)) return;
        var entry = IfoodEventInbox.Receive(companyId, eventId, payload, time.GetUtcNow().UtcDateTime);
        using var document = System.Text.Json.JsonDocument.Parse(payload);
        if (document.RootElement.TryGetProperty("merchantId", out var merchant))
        {
            var merchantId = merchant.GetString();
            var brandId = await (from mapping in db.IfoodMerchantMappings.IgnoreQueryFilters()
                join branch in db.Branchs.IgnoreQueryFilters() on mapping.BranchId equals branch.Id
                where mapping.MerchantUuid == merchantId && mapping.IsActive && branch.CompanyId == companyId
                select branch.BrandId).SingleOrDefaultAsync(ct);
            db.Entry(entry).Property(nameof(IfoodEventInbox.BrandId)).CurrentValue = brandId;
        }
        db.Add(entry);
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateException)
        {
            db.Entry(entry).State = EntityState.Detached;
            if (!await db.Set<IfoodEventInbox>().AnyAsync(row => row.CompanyId == companyId && row.EventId == eventId, ct)) throw;
        }
    }
}
