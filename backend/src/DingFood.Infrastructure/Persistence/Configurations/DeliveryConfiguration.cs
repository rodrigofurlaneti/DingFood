using DingFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DingFood.Infrastructure.Persistence.Configurations;

internal sealed class DeliveryDriverConfiguration : IEntityTypeConfiguration<DeliveryDriver>
{
    public void Configure(EntityTypeBuilder<DeliveryDriver> b)
    {
        b.ToTable("deliverydriver"); b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(150); b.Property(x => x.Phone).HasMaxLength(20);
        b.Property(x => x.VehiclePlate).HasMaxLength(10); b.Property(x => x.EmploymentType).HasMaxLength(20);
        b.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
    }
}
internal sealed class DeliveryFeeConfiguration : IEntityTypeConfiguration<DeliveryFeeConfig>
{
    public void Configure(EntityTypeBuilder<DeliveryFeeConfig> b)
    {
        b.ToTable("deliveryfeeconfig"); b.HasKey(x => x.Id); b.HasIndex(x => x.BranchId).IsUnique();
        b.Property(x => x.Model).HasMaxLength(20); b.Property(x => x.TimeZoneId).HasMaxLength(100);
        b.Property(x => x.DailyAmount).HasPrecision(18,2); b.Property(x => x.PricePerKm).HasPrecision(18,2); b.Property(x => x.MaxRadiusKm).HasPrecision(18,6);
        b.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        b.HasMany(x => x.Conditions).WithOne().HasForeignKey(x => x.DeliveryFeeConfigId).OnDelete(DeleteBehavior.Cascade);
    }
}
internal sealed class DeliveryConditionConfiguration : IEntityTypeConfiguration<DeliveryFeeCondition>
{
    public void Configure(EntityTypeBuilder<DeliveryFeeCondition> b)
    { b.ToTable("deliveryfeecondition"); b.HasKey(x => x.Id); b.Property(x => x.PricePerKm).HasPrecision(18,2); }
}
internal sealed class DeliveryDailyPaymentConfiguration : IEntityTypeConfiguration<DeliveryDriverDailyPayment>
{
    public void Configure(EntityTypeBuilder<DeliveryDriverDailyPayment> b)
    {
        b.ToTable("deliverydriverdailypayment"); b.HasKey(x => x.Id); b.Property(x => x.Amount).HasPrecision(18,2);
        b.HasIndex(x => new { x.BranchId, x.DeliveryDriverId, x.WorkDate }).IsUnique();
        b.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<DeliveryDriver>().WithMany().HasForeignKey(x => x.DeliveryDriverId).OnDelete(DeleteBehavior.Restrict);
    }
}
