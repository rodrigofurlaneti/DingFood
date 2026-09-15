using DingFood.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DingFood.Infrastructure.Persistence.Configurations;

internal sealed class CnpjQueryConfiguration : IEntityTypeConfiguration<CnpjQuery>
{
    public void Configure(EntityTypeBuilder<CnpjQuery> builder)
    {
        builder.ToTable("cnpjquery");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.TaxId).HasColumnType("char(14)").IsRequired();
        builder.Property(x => x.LegalName).HasColumnType("varchar(250)").IsRequired();
        builder.Property(x => x.TradeName).HasColumnType("varchar(250)");
        builder.Property(x => x.FoundedOn).HasColumnType("date");
        builder.Property(x => x.IsHeadOffice).HasColumnType("bit").IsRequired();

        builder.Property(x => x.StatusText).HasColumnType("varchar(60)");
        builder.Property(x => x.StatusDate).HasColumnType("date");
        builder.Property(x => x.ReasonText).HasColumnType("varchar(200)");

        builder.Property(x => x.NatureText).HasColumnType("varchar(150)");
        builder.Property(x => x.SizeAcronym).HasColumnType("varchar(10)");
        builder.Property(x => x.SizeText).HasColumnType("varchar(60)");
        builder.Property(x => x.Equity).HasColumnType("decimal(18,2)");

        builder.Property(x => x.SimplesOptant).HasColumnType("bit");
        builder.Property(x => x.SimplesSince).HasColumnType("date");
        builder.Property(x => x.SimeiOptant).HasColumnType("bit");
        builder.Property(x => x.SimeiSince).HasColumnType("date");

        builder.Property(x => x.MainActivityText).HasColumnType("varchar(300)");

        builder.Property(x => x.AddressStreet).HasColumnType("varchar(200)");
        builder.Property(x => x.AddressNumber).HasColumnType("varchar(30)");
        builder.Property(x => x.AddressDetails).HasColumnType("varchar(150)");
        builder.Property(x => x.AddressDistrict).HasColumnType("varchar(150)");
        builder.Property(x => x.AddressCity).HasColumnType("varchar(150)");
        builder.Property(x => x.AddressState).HasColumnType("char(2)");
        builder.Property(x => x.AddressZip).HasColumnType("char(8)");

        builder.Property(x => x.PrimaryPhone).HasColumnType("varchar(20)");
        builder.Property(x => x.PrimaryEmail).HasColumnType("varchar(150)");

        builder.Property(x => x.RawJson).HasColumnType("longtext").IsRequired();

        builder.Property(x => x.SourceUpdatedAt).HasColumnType("datetime(6)");
        builder.Property(x => x.QueriedAt).HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnType("datetime(6)").IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnType("datetime(6)");
        builder.Property(x => x.IsActive).HasColumnType("bit").IsRequired();

        // Um registro por CNPJ: a consulta é upsert, não histórico.
        builder.HasIndex(x => x.TaxId).IsUnique().HasDatabaseName("UX_CnpjQuery_TaxId");
        builder.HasIndex(x => x.QueriedAt).HasDatabaseName("IX_CnpjQuery_QueriedAt");
        builder.HasIndex(x => x.StatusId).HasDatabaseName("IX_CnpjQuery_StatusId");
        builder.HasIndex(x => x.AddressState).HasDatabaseName("IX_CnpjQuery_AddressState");

        // Sem FK para Company e sem query filter de tenant: dados públicos da Receita Federal
        // são compartilhados por todas as empresas da instalação. É isso que faz uma consulta
        // aproveitar para todos e protege a cota de 5 requisições/minuto da API pública.
    }
}
