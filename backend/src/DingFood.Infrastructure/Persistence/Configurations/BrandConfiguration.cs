using DingFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DingFood.Infrastructure.Persistence.Configurations;

internal sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> b)
    {
        b.ToTable("brand"); b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.HasOne(x => x.BusinessGroup).WithMany().HasForeignKey(x => x.BusinessGroupId).OnDelete(DeleteBehavior.Restrict);
    }
}
internal sealed class CompanyBrandConfiguration : IEntityTypeConfiguration<CompanyBrand>
{
    public void Configure(EntityTypeBuilder<CompanyBrand> b)
    {
        b.ToTable("companybrand"); b.HasKey(x => x.Id);
        b.HasIndex(x => new { x.CompanyId, x.BrandId }).IsUnique();
        b.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Brand).WithMany().HasForeignKey(x => x.BrandId).OnDelete(DeleteBehavior.Restrict);
    }
}
internal sealed class AppUserBranchConfiguration : IEntityTypeConfiguration<AppUserBranch>
{
    public void Configure(EntityTypeBuilder<AppUserBranch> b)
    {
        b.ToTable("appuserbranch"); b.HasKey(x => x.Id);
        b.HasIndex(x => new { x.AppUserId, x.BranchId }).IsUnique();
        b.HasOne(x => x.AppUser).WithMany().HasForeignKey(x => x.AppUserId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(x => x.Branch).WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne<Employee>().WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
