using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DingFood.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("202609100001_AddBusinessGroups")]
public sealed class AddBusinessGroups : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        using var stream = typeof(AddBusinessGroups).Assembly.GetManifestResourceStream("DingFood.BusinessGroups.sql")
            ?? throw new InvalidOperationException("Missing business group migration resource.");
        using var reader = new StreamReader(stream);
        migrationBuilder.Sql(reader.ReadToEnd(), suppressTransaction: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
        => throw new NotSupportedException("Business groups contain access grants. Restore a verified backup to reverse this migration.");
}
