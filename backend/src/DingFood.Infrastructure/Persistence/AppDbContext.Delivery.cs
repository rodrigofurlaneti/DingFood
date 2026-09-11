using DingFood.Domain.Constants;
using DingFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DingFood.Infrastructure.Persistence;

public sealed partial class AppDbContext
{
    private async Task PriceDeliveriesAsync(CancellationToken ct)
    {
        foreach (var entry in ChangeTracker.Entries<CustomerOrder>().Where(e => e.State == EntityState.Modified))
        {
            foreach (var name in new[] { nameof(CustomerOrder.DeliveryFeeAmount), nameof(CustomerOrder.DeliveryDistanceKm), nameof(CustomerOrder.DeliveryPricePerKm), nameof(CustomerOrder.DeliveryDailyAmount), nameof(CustomerOrder.DeliveryPaymentModel), nameof(CustomerOrder.DeliveryFeeCalculatedAt), nameof(CustomerOrder.DeliveryTimeZoneId) })
                if (entry.Property(name).IsModified) throw new InvalidOperationException("A precificação gravada no pedido é imutável.");
        }
        foreach (var order in ChangeTracker.Entries<CustomerOrder>().Where(e => e.State == EntityState.Added).Select(e => e.Entity).ToArray())
        {
            if (order.OrderOriginId is OrderOriginIds.IFood or OrderOriginIds.Keeta ||
                !(order.OrderTypeId == OrderTypeIds.Delivery || order.OrderTypeId == OrderTypeIds.WebSite && !string.IsNullOrWhiteSpace(order.DeliveryAddress))) continue;
            var config = await Set<DeliveryFeeConfig>().Include(x => x.Conditions).SingleOrDefaultAsync(x => x.BranchId == order.BranchId, ct);
            // Existing installations retain their behavior until the branch opts into own delivery.
            if (config is null) continue;
            if (order.DeliveryFeeCalculatedAt.HasValue) continue;
            var distance = 0m;
            if (config.Model == "PerKm")
            {
                var branch = await Branchs.SingleAsync(x => x.Id == order.BranchId, ct);
                if (string.IsNullOrWhiteSpace(branch.AddressStreet) || string.IsNullOrWhiteSpace(branch.AddressCity)) throw new ArgumentException("Complete o endereço da filial antes de configurar entregas por KM.");
                var origin = string.Join(", ", new[] { branch.AddressStreet, branch.AddressNumber, branch.AddressDistrict, branch.AddressCity, branch.AddressState, branch.AddressZipCode }.Where(x => !string.IsNullOrWhiteSpace(x)));
                if (deliveryGeocoder is null) throw new ArgumentException("Serviço de geocodificação não configurado.");
                distance = await deliveryGeocoder.DistanceKmAsync(origin, order.DeliveryAddress ?? "", ct);
            }
            var now = DateTimeOffset.UtcNow;
            order.FreezeDeliveryFee(config.Calculate(distance, now), now.UtcDateTime, config.TimeZoneId);
        }
    }
}
