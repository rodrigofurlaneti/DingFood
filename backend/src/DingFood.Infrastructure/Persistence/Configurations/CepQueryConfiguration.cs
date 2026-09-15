using DingFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DingFood.Infrastructure.Persistence.Configurations;

internal sealed class CepQueryConfiguration : IEntityTypeConfiguration<CepQuery>
{
    public void Configure(EntityTypeBuilder<CepQuery> builder)
    {
        builder.ToTable("cepquery");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Cep).HasColumnType("char(8)").IsRequired();

        builder.Property(x => x.Street).HasColumnType("varchar(200)");
        builder.Property(x => x.Complement).HasColumnType("varchar(150)");
        builder.Property(x => x.Unit).HasColumnType("varchar(150)");
        builder.Property(x => x.District).HasColumnType("varchar(150)");
        builder.Property(x => x.City).HasColumnType("varchar(150)");
        builder.Property(x => x.StateAbbreviation).HasColumnType("char(2)");
        builder.Property(x => x.StateName).HasColumnType("varchar(100)");
        builder.Property(x => x.Region).HasColumnType("varchar(30)");

        builder.Property(x => x.GiaCode).HasColumnType("varchar(10)");
        builder.Property(x => x.AreaCode).HasColumnType("varchar(3)");
        builder.Property(x => x.SiafiCode).HasColumnType("varchar(10)");

        builder.Property(x => x.RawJson).HasColumnType("longtext").IsRequired();

        builder.Property(x => x.QueriedAt).HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnType("datetime(6)");
        builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired();

        // Um registro por CEP: a consulta é upsert, não histórico.
        builder.HasIndex(x => x.Cep).IsUnique().HasDatabaseName("UX_CepQuery_Cep");
        builder.HasIndex(x => x.QueriedAt).HasDatabaseName("IX_CepQuery_QueriedAt");
        builder.HasIndex(x => x.IbgeCode).HasDatabaseName("IX_CepQuery_IbgeCode");
        // Cobre a busca por cidade dentro de um estado, que é como um formulário costuma filtrar.
        builder.HasIndex(x => new { x.StateAbbreviation, x.City }).HasDatabaseName("IX_CepQuery_StateCity");

        // Sem FK para Company e sem query filter de tenant: um CEP é o mesmo para todas as
        // empresas da instalação. Declarada em AppDbContext.OperationalScopes.SystemTables.
    }
}
