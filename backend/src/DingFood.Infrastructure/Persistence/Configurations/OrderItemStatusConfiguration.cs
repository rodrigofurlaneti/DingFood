using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DingFood.Domain.Entities;

namespace DingFood.Infrastructure.Persistence.Configurations;

internal sealed class OrderItemStatusConfiguration : IEntityTypeConfiguration<OrderItemStatus>
{
    public void Configure(EntityTypeBuilder<OrderItemStatus> builder)
    {
        builder.ToTable("orderitemstatus");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Name).HasColumnType("nvarchar(50)").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnType("datetime(6)");
        
    }
}
