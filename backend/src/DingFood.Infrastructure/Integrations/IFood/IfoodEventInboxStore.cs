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
        db.Add(entry);
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateException)
        {
            db.Entry(entry).State = EntityState.Detached;
            if (!await db.Set<IfoodEventInbox>().AnyAsync(row => row.CompanyId == companyId && row.EventId == eventId, ct)) throw;
        }
    }
}
