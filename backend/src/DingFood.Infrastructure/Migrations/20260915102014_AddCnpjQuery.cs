using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DingFood.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCnpjQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cnpjquery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TaxId = table.Column<string>(type: "char(14)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LegalName = table.Column<string>(type: "varchar(250)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TradeName = table.Column<string>(type: "varchar(250)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FoundedOn = table.Column<DateOnly>(type: "date", nullable: true),
                    IsHeadOffice = table.Column<ulong>(type: "bit", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    StatusText = table.Column<string>(type: "varchar(60)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StatusDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ReasonText = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NatureId = table.Column<int>(type: "int", nullable: true),
                    NatureText = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SizeAcronym = table.Column<string>(type: "varchar(10)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SizeText = table.Column<string>(type: "varchar(60)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Equity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SimplesOptant = table.Column<ulong>(type: "bit", nullable: true),
                    SimplesSince = table.Column<DateOnly>(type: "date", nullable: true),
                    SimeiOptant = table.Column<ulong>(type: "bit", nullable: true),
                    SimeiSince = table.Column<DateOnly>(type: "date", nullable: true),
                    MainActivityCode = table.Column<int>(type: "int", nullable: true),
                    MainActivityText = table.Column<string>(type: "varchar(300)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressStreet = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressNumber = table.Column<string>(type: "varchar(30)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressDetails = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressDistrict = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressCity = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressState = table.Column<string>(type: "char(2)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressZip = table.Column<string>(type: "char(8)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MunicipalityCode = table.Column<int>(type: "int", nullable: true),
                    PrimaryPhone = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrimaryEmail = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SourceUpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RawJson = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QueriedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<ulong>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cnpjquery", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CnpjQuery_AddressState",
                table: "cnpjquery",
                column: "AddressState");

            migrationBuilder.CreateIndex(
                name: "IX_CnpjQuery_QueriedAt",
                table: "cnpjquery",
                column: "QueriedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CnpjQuery_StatusId",
                table: "cnpjquery",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "UX_CnpjQuery_TaxId",
                table: "cnpjquery",
                column: "TaxId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cnpjquery");
        }
    }
}
