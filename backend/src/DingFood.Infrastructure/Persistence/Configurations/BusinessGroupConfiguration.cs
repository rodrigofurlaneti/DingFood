using DingFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DingFood.Infrastructure.Persistence.Configurations;

internal sealed class BusinessGroupConfiguration : IEntityTypeConfiguration<BusinessGroup>
{
    public void Configure(EntityTypeBuilder<BusinessGroup> builder)
    {
        builder.ToTable("businessgroup");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.HasMany<Company>().WithOne(x => x.BusinessGroup).HasForeignKey(x => x.BusinessGroupId).OnDelete(DeleteBehavior.Restrict);
    }
}

internal sealed class AppUserCompanyConfiguration : IEntityTypeConfiguration<AppUserCompany>
{
    public void Configure(EntityTypeBuilder<AppUserCompany> builder)
    {
        builder.ToTable("appusercompany");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasIndex(x => new { x.AppUserId, x.CompanyId }).IsUnique();
        builder.HasOne(x => x.AppUser).WithMany().HasForeignKey(x => x.AppUserId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
