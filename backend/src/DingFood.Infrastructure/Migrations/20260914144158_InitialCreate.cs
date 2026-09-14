using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DingFood.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appfeature",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appfeature", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "businessgroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_businessgroup", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cashmovementtype",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IsInflow = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashmovementtype", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cashsessionstatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashsessionstatus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comandastatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comandastatus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "costtype",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_costtype", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "diningarea",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diningarea", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderitemstatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderitemstatus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderstatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderstatus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "paymentmethod",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    AllowsChange = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentmethod", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shiftclosingstatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shiftclosingstatus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stockmovementtype",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    IsInflow = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockmovementtype", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tablestatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tablestatus", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "unitofmeasure",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Abbreviation = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unitofmeasure", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "waitermessage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    SenderEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    RecipientEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    DiningAreaId = table.Column<long>(type: "bigint", nullable: false),
                    Message = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_waitermessage", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "brand",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BusinessGroupId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_brand_businessgroup_BusinessGroupId",
                        column: x => x.BusinessGroupId,
                        principalTable: "businessgroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "company",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BusinessGroupId = table.Column<long>(type: "bigint", nullable: false),
                    LegalName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Cnpj = table.Column<string>(type: "char(14)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_company_businessgroup_BusinessGroupId",
                        column: x => x.BusinessGroupId,
                        principalTable: "businessgroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "asaasintegrationwebhooklog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    Event = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AsaasEventId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Payload = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestHeaders = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IpAddress = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asaasintegrationwebhooklog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asaasintegrationwebhooklog_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_category_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "companybrand",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companybrand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_companybrand_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_companybrand_company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "complementgroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    ComplementGroupTypeId = table.Column<sbyte>(type: "tinyint", nullable: false),
                    MinSelection = table.Column<int>(type: "int", nullable: false),
                    MaxSelection = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_complementgroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplementGroup_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_complementgroup_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cpf = table.Column<string>(type: "char(11)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoyaltyPoints = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customer_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodeventinbox",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    EventId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Payload = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceivedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextAttemptAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProcessedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Attempts = table.Column<int>(type: "int", nullable: false),
                    LastError = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodeventinbox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ifoodeventinbox_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ifoodeventinbox_company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodintegrationsetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    ClientId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientSecretEncrypted = table.Column<string>(type: "varchar(1000)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enabled = table.Column<ulong>(type: "bit", nullable: false),
                    EventDeliveryMode = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, defaultValue: "Polling")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IfoodCustomerId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastConnectionTestAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastConnectionTestSucceeded = table.Column<ulong>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodintegrationsetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodIntegrationSetting_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ifoodintegrationsetting_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "jobtitle",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobtitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitle_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_jobtitle_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pizzaflavor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pizzaflavor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaFlavor_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_pizzaflavor_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    LegalName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    Cnpj = table.Column<string>(type: "char(14)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplier_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_supplier_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HasOptionalExtras = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasBoosts = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    UnitOfMeasureId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Barcode = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsStockControlled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PreparationTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Category",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_UnitOfMeasure",
                        column: x => x.UnitOfMeasureId,
                        principalTable: "unitofmeasure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "asaasintegrationcustomer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    AsaasCustomerId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asaasintegrationcustomer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsaasIntegrationCustomer_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AsaasIntegrationCustomer_Customer",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asaasintegrationcustomer_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "asaasintegrationsavedcard",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    CreditCardToken = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CardBrand = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Last4Digits = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HolderName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryMonth = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiryYear = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asaasintegrationsavedcard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsaasIntegrationSavedCard_Customer",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asaasintegrationsavedcard_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "jobtitlefeature",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobTitleId = table.Column<long>(type: "bigint", nullable: false),
                    AppFeatureId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobtitlefeature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitleFeature_AppFeature",
                        column: x => x.AppFeatureId,
                        principalTable: "appfeature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobTitleFeature_JobTitle",
                        column: x => x.JobTitleId,
                        principalTable: "jobtitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rolepermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolepermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission",
                        column: x => x.PermissionId,
                        principalTable: "permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "complementitem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    LinkedProductId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_complementitem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplementItem_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComplementItem_LinkedProduct",
                        column: x => x.LinkedProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_complementitem_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pizzaconfiguration",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pizzaconfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaConfiguration_Product",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "productboost",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    BoostName = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IncrementalValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productboost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_productboost_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "productcomplementgroup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ComplementGroupId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productcomplementgroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductComplementGroup_ComplementGroup",
                        column: x => x.ComplementGroupId,
                        principalTable: "complementgroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductComplementGroup_Product",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "productoptionalextra",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    OptionalExtraName = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productoptionalextra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_productoptionalextra_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "productstock",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    MinimumQuantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    RowVersion = table.Column<DateTime>(type: "timestamp(6)", rowVersion: true, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productstock", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_productstock_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "complement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComplementGroupId = table.Column<long>(type: "bigint", nullable: false),
                    ComplementItemId = table.Column<long>(type: "bigint", nullable: false),
                    ExtraPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_complement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complement_ComplementGroup",
                        column: x => x.ComplementGroupId,
                        principalTable: "complementgroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complement_ComplementItem",
                        column: x => x.ComplementItemId,
                        principalTable: "complementitem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pizzacrust",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PizzaConfigurationId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    ExtraPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pizzacrust", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaCrust_PizzaConfiguration",
                        column: x => x.PizzaConfigurationId,
                        principalTable: "pizzaconfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pizzaedge",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PizzaConfigurationId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    ExtraPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pizzaedge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaEdge_PizzaConfiguration",
                        column: x => x.PizzaConfigurationId,
                        principalTable: "pizzaconfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pizzasize",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PizzaConfigurationId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Slices = table.Column<int>(type: "int", nullable: true),
                    AcceptedFractions = table.Column<int>(type: "int", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pizzasize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaSize_PizzaConfiguration",
                        column: x => x.PizzaConfigurationId,
                        principalTable: "pizzaconfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pizzaflavorprice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PizzaConfigurationId = table.Column<long>(type: "bigint", nullable: false),
                    PizzaFlavorId = table.Column<long>(type: "bigint", nullable: false),
                    PizzaSizeId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pizzaflavorprice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PizzaFlavorPrice_PizzaConfiguration",
                        column: x => x.PizzaConfigurationId,
                        principalTable: "pizzaconfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PizzaFlavorPrice_PizzaFlavor",
                        column: x => x.PizzaFlavorId,
                        principalTable: "pizzaflavor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PizzaFlavorPrice_PizzaSize",
                        column: x => x.PizzaSizeId,
                        principalTable: "pizzasize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "accesslog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerAppUserId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventType = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IpAddress = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserAgent = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accesslog", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appuser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordSalt = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FailedAccessCount = table.Column<int>(type: "int", nullable: false),
                    LockoutEndAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appuser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUser_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "logtracker",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(type: "bigint", nullable: true),
                    DirectoryName = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClassName = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MethodName = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSuccess = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExecutionTimeMs = table.Column<long>(type: "bigint", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StackTrace = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IpAddress = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logtracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogTracker_AppUser",
                        column: x => x.AppUserId,
                        principalTable: "appuser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "refreshtoken",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshtoken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AppUser",
                        column: x => x.AppUserId,
                        principalTable: "appuser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "userrole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userrole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_AppUser",
                        column: x => x.AppUserId,
                        principalTable: "appuser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Role",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appuserbranch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: true),
                    UsesLegacyRoles = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appuserbranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appuserbranch_appuser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "appuser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appuserbranch_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appusercompany",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appusercompany", x => x.Id);
                    table.ForeignKey(
                        name: "FK_appusercompany_appuser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "appuser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_appusercompany_company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "appuserfeature",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    AppUserId = table.Column<long>(type: "bigint", nullable: false),
                    AppFeatureId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appuserfeature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserFeature_AppFeature",
                        column: x => x.AppFeatureId,
                        principalTable: "appfeature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUserFeature_AppUser",
                        column: x => x.AppUserId,
                        principalTable: "appuser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "asaasintegrationpayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerOrderId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    AsaasPaymentId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BillingType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NetValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PixQrCodeBase64 = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PixPayload = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InvoiceUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankSlipUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InstallmentCount = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreditCardToken = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asaasintegrationpayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsaasIntegrationPayment_Customer",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "asaasintegrationsetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    Environment = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Sandbox")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApiKeyEncrypted = table.Column<string>(type: "varchar(1000)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WebhookSecretEncrypted = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WalletId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asaasintegrationsetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsaasIntegrationSetting_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asaasintegrationsetting_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "branch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Cnpj = table.Column<string>(type: "char(14)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressStreet = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    AddressNumber = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressDistrict = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AddressCity = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AddressState = table.Column<string>(type: "char(2)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressZipCode = table.Column<string>(type: "char(8)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SelfServiceEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branch_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_branch_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "branchpaymentmethodsetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    EnablePix = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EnableBoleto = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EnableCreditCard = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EnableDebitCard = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    EnableCashMachine = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branchpaymentmethodsetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchPaymentMethodSetting_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchPaymentMethodSetting_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_branchpaymentmethodsetting_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cashregister",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashregister", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashRegister_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comanda",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    ComandaStatusId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comanda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comanda_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comanda_ComandaStatus",
                        column: x => x.ComandaStatusId,
                        principalTable: "comandastatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comandasetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    DefaultLimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comandasetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComandaSetting_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customerappuser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FailedAccessCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LockoutEndAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerappuser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAppUser_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAppUser_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAppUser_Customer",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customerappuser_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "deliverydriver",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VehiclePlate = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmploymentType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliverydriver", x => x.Id);
                    table.ForeignKey(
                        name: "FK_deliverydriver_branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "deliveryfeeconfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Model = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DailyAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxRadiusKm = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PricePerKm = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TimeZoneId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveryfeeconfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_deliveryfeeconfig_branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "diningtable",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    TableStatusId = table.Column<long>(type: "bigint", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    QrToken = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsQrViewEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    IsCameraInputEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IsBarcodeEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IsQrCodeEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diningtable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiningTable_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiningTable_TableStatus",
                        column: x => x.TableStatusId,
                        principalTable: "tablestatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    JobTitleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    Cpf = table.Column<string>(type: "char(11)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HiredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DismissedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommissionPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_JobTitle",
                        column: x => x.JobTitleId,
                        principalTable: "jobtitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodanalyticssnapshot",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReferenceDate = table.Column<DateTime>(type: "date", nullable: false),
                    AggregatesJson = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodanalyticssnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ifoodanalyticssnapshot_branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodcategorymapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodCategoryId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodcategorymapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodCategoryMapping_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IfoodCategoryMapping_Category",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodcomplementgroupmapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComplementGroupId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodOptionGroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodcomplementgroupmapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodComplementGroupMapping_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IfoodComplementGroupMapping_ComplementGroup",
                        column: x => x.ComplementGroupId,
                        principalTable: "complementgroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodcomplementmapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComplementId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodOptionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IfoodProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodcomplementmapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodComplementMapping_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IfoodComplementMapping_Complement",
                        column: x => x.ComplementId,
                        principalTable: "complement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodfinancialevent",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodEventId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Trigger = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HasTransferImpact = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompetenceDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SettlementExpectedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReferenceType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReferenceId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RawPayload = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodfinancialevent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodFinancialEvent_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodmerchantmapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    MerchantId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MerchantUuid = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreparationTimeMinutes = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodmerchantmapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodMerchantMapping_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodopeninghours",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    Start = table.Column<TimeSpan>(type: "time(0)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodopeninghours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodOpeningHours_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodpizzamapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PizzaConfigurationId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodPizzaId = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodpizzamapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodPizzaMapping_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IfoodPizzaMapping_PizzaConfiguration",
                        column: x => x.PizzaConfigurationId,
                        principalTable: "pizzaconfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodproductmapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodItemId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IfoodProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodproductmapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodProductMapping_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IfoodProductMapping_Product",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodsettlement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodSettlementId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Product = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    BankCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankAgency = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankAccount = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RawPayload = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodsettlement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodSettlement_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodshippingdelivery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    OrderReference = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerPhoneAreaCode = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerPhoneNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StreetName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StreetNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complement = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Neighborhood = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reference = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<double>(type: "double", nullable: true),
                    Longitude = table.Column<double>(type: "double", nullable: true),
                    MerchantFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuoteId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IfoodDeliveryId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TrackingUrl = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequestedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CancelledAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CancellationReason = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodshippingdelivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodShippingDelivery_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodshippingtracking",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextPollAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Latitude = table.Column<double>(type: "double", nullable: true),
                    Longitude = table.Column<double>(type: "double", nullable: true),
                    ExpectedDelivery = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeliveryEtaEndMinutes = table.Column<double>(type: "double", nullable: true),
                    PickupEtaStartMinutes = table.Column<double>(type: "double", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodshippingtracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ifoodshippingtracking_branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "keetaintegrationauthorizationsession",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", maxLength: 36, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    AuthId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KeetaMerchantId = table.Column<long>(type: "bigint", nullable: true),
                    AuthorizationCode = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    IsProcessed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keetaintegrationauthorizationsession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeetaSession_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaSession_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "keetaintegrationmerchantmapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", maxLength: 36, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    InternalMerchantId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KeetaMerchantId = table.Column<long>(type: "bigint", nullable: false),
                    StoreName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TimeZone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAuthorized = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    IsOnboarded = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    LastMenuSyncAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    MenuBaseUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WebhookUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keetaintegrationmerchantmapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeetaMapping_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaMapping_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "keetaintegrationsetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", maxLength: 36, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientSecret = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AppId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BaseUrl = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "https://open.mykeeta.com/api/open/opendelivery")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CurrentAccessToken = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TokenExpiresAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keetaintegrationsetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeetaSetting_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaSetting_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operatingcost",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CostTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReferenceYear = table.Column<int>(type: "int", nullable: false),
                    ReferenceMonth = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operatingcost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatingCost_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperatingCost_CostType",
                        column: x => x.CostTypeId,
                        principalTable: "costtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderorigin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderorigin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderOrigin_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderOrigin_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orderorigin_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "printer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ConnectionType = table.Column<int>(type: "int", nullable: false),
                    PrinterName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    IpAddress = table.Column<string>(type: "varchar(45)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Port = table.Column<int>(type: "int", nullable: true),
                    PrintsOrders = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PrintsBills = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_printer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Printer_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "printersetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    PrintOrdersEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PrintBillsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_printersetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrinterSetting_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "promotion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartMinuteOfDay = table.Column<int>(type: "int", nullable: false),
                    EndMinuteOfDay = table.Column<int>(type: "int", nullable: false),
                    PromotionTypeId = table.Column<long>(type: "bigint", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotion_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotion_Product",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "purchase",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    SupplierId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentNumber = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PurchasedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Supplier",
                        column: x => x.SupplierId,
                        principalTable: "supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "revenuetarget",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceYear = table.Column<int>(type: "int", nullable: false),
                    ReferenceMonth = table.Column<int>(type: "int", nullable: false),
                    TargetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_revenuetarget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevenueTarget_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "servicefeesetting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<ulong>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servicefeesetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceFeeSetting_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stockitem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentQuantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    MinimumQuantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    MaximumQuantity = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockitem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockItem_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockItem_Product",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customerrefreshtoken",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerAppUserId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerrefreshtoken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerRefreshToken_CustomerAppUser",
                        column: x => x.CustomerAppUserId,
                        principalTable: "customerappuser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "deliverydriverdailypayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    DeliveryDriverId = table.Column<long>(type: "bigint", nullable: false),
                    WorkDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliverydriverdailypayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_deliverydriverdailypayment_branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_deliverydriverdailypayment_deliverydriver_DeliveryDriverId",
                        column: x => x.DeliveryDriverId,
                        principalTable: "deliverydriver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "deliveryfeecondition",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeliveryFeeConfigId = table.Column<long>(type: "bigint", nullable: false),
                    DaysOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartMinute = table.Column<int>(type: "int", nullable: false),
                    EndMinute = table.Column<int>(type: "int", nullable: false),
                    PricePerKm = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveryfeecondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_deliveryfeecondition_deliveryfeeconfig_DeliveryFeeConfigId",
                        column: x => x.DeliveryFeeConfigId,
                        principalTable: "deliveryfeeconfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "diningareatable",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiningAreaId = table.Column<long>(type: "bigint", nullable: false),
                    DiningTableId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diningareatable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_diningareatable_diningarea_DiningAreaId",
                        column: x => x.DiningAreaId,
                        principalTable: "diningarea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_diningareatable_diningtable_DiningTableId",
                        column: x => x.DiningTableId,
                        principalTable: "diningtable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tablereservation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    DiningTableId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PartySize = table.Column<int>(type: "int", nullable: false),
                    ReservedFor = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReservationStatusId = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tablereservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableReservation_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TableReservation_DiningTable",
                        column: x => x.DiningTableId,
                        principalTable: "diningtable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cashsession",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CashRegisterId = table.Column<long>(type: "bigint", nullable: false),
                    CashSessionStatusId = table.Column<long>(type: "bigint", nullable: false),
                    OpenedByEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ClosedByEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    OpeningAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClosingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpectedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OpenedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashsession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashSession_CashRegister",
                        column: x => x.CashRegisterId,
                        principalTable: "cashregister",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashSession_CashSessionStatus",
                        column: x => x.CashSessionStatusId,
                        principalTable: "cashsessionstatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashSession_ClosedByEmployee",
                        column: x => x.ClosedByEmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashSession_OpenedByEmployee",
                        column: x => x.OpenedByEmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "diningareaassignment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiningAreaId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diningareaassignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_diningareaassignment_diningarea_DiningAreaId",
                        column: x => x.DiningAreaId,
                        principalTable: "diningarea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_diningareaassignment_employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shiftclosing",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    ShiftClosingStatusId = table.Column<long>(type: "bigint", nullable: false),
                    OpenedByEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ClosedByEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    PeriodStart = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CashSessionsCount = table.Column<int>(type: "int", nullable: false),
                    TotalOpeningAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalExpectedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRealizedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shiftclosing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftClosing_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftClosing_ClosedByEmployee",
                        column: x => x.ClosedByEmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftClosing_OpenedByEmployee",
                        column: x => x.OpenedByEmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftClosing_ShiftClosingStatus",
                        column: x => x.ShiftClosingStatusId,
                        principalTable: "shiftclosingstatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodpizzaelementmapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IfoodPizzaMappingId = table.Column<long>(type: "bigint", nullable: false),
                    Kind = table.Column<sbyte>(type: "tinyint", nullable: false),
                    LocalId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodElementId = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodpizzaelementmapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodPizzaElementMapping_IfoodPizzaMapping",
                        column: x => x.IfoodPizzaMappingId,
                        principalTable: "ifoodpizzamapping",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customerorder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    DeliveryDriverId = table.Column<long>(type: "bigint", nullable: true),
                    DeliveryFeeAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DeliveryDistanceKm = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    DeliveryPricePerKm = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DeliveryDailyAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DeliveryPaymentModel = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeliveryFeeCalculatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeliveryTimeZoneId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiningTableId = table.Column<long>(type: "bigint", nullable: true),
                    ComandaId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    OrderStatusId = table.Column<long>(type: "bigint", nullable: false),
                    OrderTypeId = table.Column<sbyte>(type: "tinyint", nullable: false),
                    OrderOriginId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerName = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerPhone = table.Column<string>(type: "varchar(20)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeliveryAddress = table.Column<string>(type: "varchar(300)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    GuestCount = table.Column<int>(type: "int", nullable: true),
                    OpenedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SubtotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditLimitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "varchar(500)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerorder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_Comanda",
                        column: x => x.ComandaId,
                        principalTable: "comanda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_Customer",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_DiningTable",
                        column: x => x.DiningTableId,
                        principalTable: "diningtable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_OrderOrigin",
                        column: x => x.OrderOriginId,
                        principalTable: "orderorigin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_OrderStatus",
                        column: x => x.OrderStatusId,
                        principalTable: "orderstatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customerorder_deliverydriver_DeliveryDriverId",
                        column: x => x.DeliveryDriverId,
                        principalTable: "deliverydriver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "purchaseitem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PurchaseId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_purchaseitem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseItem_Product",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseItem_Purchase",
                        column: x => x.PurchaseId,
                        principalTable: "purchase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cashsessionpaymentreconciliation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CashSessionId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    ExpectedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CountedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DifferenceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashsessionpaymentreconciliation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashSessionPaymentReconciliation_CashSession",
                        column: x => x.CashSessionId,
                        principalTable: "cashsession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashSessionPaymentReconciliation_PaymentMethod",
                        column: x => x.PaymentMethodId,
                        principalTable: "paymentmethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shiftclosingsession",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShiftClosingId = table.Column<long>(type: "bigint", nullable: false),
                    CashSessionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shiftclosingsession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftClosingSession_CashSession",
                        column: x => x.CashSessionId,
                        principalTable: "cashsession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftClosingSession_ShiftClosing",
                        column: x => x.ShiftClosingId,
                        principalTable: "shiftclosing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comandaitemtransfer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerOrderId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    SourceComandaId = table.Column<long>(type: "bigint", nullable: false),
                    TargetComandaId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comandaitemtransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comandaitemtransfer_customerorder_CustomerOrderId",
                        column: x => x.CustomerOrderId,
                        principalTable: "customerorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_comandaitemtransfer_employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "customeraddress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true),
                    LastOrderId = table.Column<long>(type: "bigint", nullable: true),
                    Street = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Supplement = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ZipCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastOrderAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customeraddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Customer",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Order",
                        column: x => x.LastOrderId,
                        principalTable: "customerorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_customeraddress_brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodorder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerOrderId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    IfoodOrderId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MerchantId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IfoodOrderType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeliveredBy = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderTiming = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "IMMEDIATE")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PreparationStartDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ConfirmDeadlineAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HasUnmappedItems = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodorder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodOrder_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IfoodOrder_CustomerOrder",
                        column: x => x.CustomerOrderId,
                        principalTable: "customerorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "keetaintegrationorder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", maxLength: 36, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerOrderId = table.Column<long>(type: "bigint", nullable: false),
                    KeetaOrderId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InternalMerchantId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KeetaMerchantId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeliveredBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, defaultValue: "BRL")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RawOrderJson = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderCreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ConfirmedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReadyForPickupAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ConcludedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keetaintegrationorder", x => x.Id);
                    table.UniqueConstraint("AK_keetaintegrationorder_KeetaOrderId", x => x.KeetaOrderId);
                    table.ForeignKey(
                        name: "FK_KeetaOrder_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaOrder_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaOrder_Customer",
                        column: x => x.CustomerId,
                        principalTable: "customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaOrder_CustomerOrder",
                        column: x => x.CustomerOrderId,
                        principalTable: "customerorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderitem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerOrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    OrderItemStatusId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    SentToKitchenAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CancelledByEmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    PizzaSizeId = table.Column<long>(type: "bigint", nullable: true),
                    PizzaCrustId = table.Column<long>(type: "bigint", nullable: true),
                    PizzaEdgeId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderitem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_CustomerOrder",
                        column: x => x.CustomerOrderId,
                        principalTable: "customerorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_OrderItemStatus",
                        column: x => x.OrderItemStatusId,
                        principalTable: "orderitemstatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_PizzaCrust",
                        column: x => x.PizzaCrustId,
                        principalTable: "pizzacrust",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_PizzaEdge",
                        column: x => x.PizzaEdgeId,
                        principalTable: "pizzaedge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_PizzaSize",
                        column: x => x.PizzaSizeId,
                        principalTable: "pizzasize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product",
                        column: x => x.ProductId,
                        principalTable: "product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderpartialpayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerOrderId = table.Column<long>(type: "bigint", nullable: false),
                    CashSessionId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AuthorizationCode = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayerName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderpartialpayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPartialPayment_CashSession",
                        column: x => x.CashSessionId,
                        principalTable: "cashsession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderPartialPayment_CustomerOrder",
                        column: x => x.CustomerOrderId,
                        principalTable: "customerorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderPartialPayment_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderPartialPayment_PaymentMethod",
                        column: x => x.PaymentMethodId,
                        principalTable: "paymentmethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sale",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerOrderId = table.Column<long>(type: "bigint", nullable: false),
                    CashSessionId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    SaleNumber = table.Column<long>(type: "bigint", nullable: false),
                    SubtotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceFeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoldAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sale_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_CashSession",
                        column: x => x.CashSessionId,
                        principalTable: "cashsession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_CustomerOrder",
                        column: x => x.CustomerOrderId,
                        principalTable: "customerorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tableitemtransfer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerOrderId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerOrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    SourceDiningTableId = table.Column<long>(type: "bigint", nullable: false),
                    TargetDiningTableId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tableitemtransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tableitemtransfer_customerorder_CustomerOrderId",
                        column: x => x.CustomerOrderId,
                        principalTable: "customerorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tableitemtransfer_diningtable_SourceDiningTableId",
                        column: x => x.SourceDiningTableId,
                        principalTable: "diningtable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tableitemtransfer_diningtable_TargetDiningTableId",
                        column: x => x.TargetDiningTableId,
                        principalTable: "diningtable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tableitemtransfer_employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ifoodlogisticsdelivery",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IfoodOrderId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    DriverName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DriverPhone = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DriverVehicleType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssignedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GoingToOriginAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ArrivedAtOriginAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DispatchedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ArrivedAtDestinationAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeliveryCodeVerifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ifoodlogisticsdelivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IfoodLogisticsDelivery_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IfoodLogisticsDelivery_IfoodOrder",
                        column: x => x.IfoodOrderId,
                        principalTable: "ifoodorder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "keetaintegrationordereventlog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", maxLength: 36, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    EventId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrderId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RawPayload = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProcessedSuccessfully = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventCreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReceivedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keetaintegrationordereventlog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeetaEventLog_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaEventLog_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaEventLog_Order",
                        column: x => x.OrderId,
                        principalTable: "keetaintegrationorder",
                        principalColumn: "KeetaOrderId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "keetaintegrationrefunddispute",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", maxLength: 36, nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AfterSaleOrderId = table.Column<long>(type: "bigint", nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, defaultValue: "BRL")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApplyReason = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResolutionStatus = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "PENDING")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DenialReasonCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DenialReasonText = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReceivedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResolvedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_keetaintegrationrefunddispute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeetaRefund_Branch",
                        column: x => x.BranchId,
                        principalTable: "branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaRefund_Company",
                        column: x => x.CompanyId,
                        principalTable: "company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeetaRefund_Order",
                        column: x => x.OrderId,
                        principalTable: "keetaintegrationorder",
                        principalColumn: "KeetaOrderId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderitemboost",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    ProductBoostId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnitPriceCharged = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderitemboost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orderitemboost_orderitem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "orderitem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderitemboost_productboost_ProductBoostId",
                        column: x => x.ProductBoostId,
                        principalTable: "productboost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderitemcomplement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    ComplementId = table.Column<long>(type: "bigint", nullable: false),
                    UnitPriceCharged = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderitemcomplement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemComplement_Complement",
                        column: x => x.ComplementId,
                        principalTable: "complement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemComplement_OrderItem",
                        column: x => x.OrderItemId,
                        principalTable: "orderitem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderitemoptionalextra",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    ProductOptionalExtraId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderitemoptionalextra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orderitemoptionalextra_orderitem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "orderitem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orderitemoptionalextra_productoptionalextra_ProductOptionalE~",
                        column: x => x.ProductOptionalExtraId,
                        principalTable: "productoptionalextra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orderitempizzaflavor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: false),
                    PizzaFlavorId = table.Column<long>(type: "bigint", nullable: false),
                    FractionShare = table.Column<decimal>(type: "decimal(9,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderitempizzaflavor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemPizzaFlavor_OrderItem",
                        column: x => x.OrderItemId,
                        principalTable: "orderitem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemPizzaFlavor_PizzaFlavor",
                        column: x => x.PizzaFlavorId,
                        principalTable: "pizzaflavor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stockmovement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StockItemId = table.Column<long>(type: "bigint", nullable: false),
                    StockMovementTypeId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseItemId = table.Column<long>(type: "bigint", nullable: true),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DocumentNumber = table.Column<string>(type: "varchar(50)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockmovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovement_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovement_OrderItem",
                        column: x => x.OrderItemId,
                        principalTable: "orderitem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovement_PurchaseItem",
                        column: x => x.PurchaseItemId,
                        principalTable: "purchaseitem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovement_StockItem",
                        column: x => x.StockItemId,
                        principalTable: "stockitem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovement_StockMovementType",
                        column: x => x.StockMovementTypeId,
                        principalTable: "stockmovementtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cashmovement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CashSessionId = table.Column<long>(type: "bigint", nullable: false),
                    CashMovementTypeId = table.Column<long>(type: "bigint", nullable: false),
                    SaleId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "varchar(300)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashmovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashMovement_CashMovementType",
                        column: x => x.CashMovementTypeId,
                        principalTable: "cashmovementtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashMovement_CashSession",
                        column: x => x.CashSessionId,
                        principalTable: "cashsession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashMovement_Employee",
                        column: x => x.EmployeeId,
                        principalTable: "employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashMovement_Sale",
                        column: x => x.SaleId,
                        principalTable: "sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "salepayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaleId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethodId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChangeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AuthorizationCode = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salepayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalePayment_PaymentMethod",
                        column: x => x.PaymentMethodId,
                        principalTable: "paymentmethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalePayment_Sale",
                        column: x => x.SaleId,
                        principalTable: "sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AccessLog_AppUserId",
                table: "accesslog",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_accesslog_CustomerAppUserId",
                table: "accesslog",
                column: "CustomerAppUserId");

            migrationBuilder.CreateIndex(
                name: "UQ_AppFeature_Code",
                table: "appfeature",
                column: "Code",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_CompanyId",
                table: "appuser",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_EmployeeId",
                table: "appuser",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "UQ_AppUser_Email",
                table: "appuser",
                column: "Email",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "UQ_AppUser_UserName",
                table: "appuser",
                column: "UserName",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_appuserbranch_AppUserId_BranchId",
                table: "appuserbranch",
                columns: new[] { "AppUserId", "BranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appuserbranch_BranchId",
                table: "appuserbranch",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_appuserbranch_EmployeeId",
                table: "appuserbranch",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_appuserbranch_RoleId",
                table: "appuserbranch",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_appusercompany_AppUserId_CompanyId",
                table: "appusercompany",
                columns: new[] { "AppUserId", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_appusercompany_CompanyId",
                table: "appusercompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_appusercompany_EmployeeId",
                table: "appusercompany",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_appuserfeature_AppFeatureId",
                table: "appuserfeature",
                column: "AppFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_appuserfeature_BranchId",
                table: "appuserfeature",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "UQ_AppUserFeature_AppUserId_AppFeatureId",
                table: "appuserfeature",
                columns: new[] { "AppUserId", "AppFeatureId", "BranchId" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationCustomer_AsaasCustomerId",
                table: "asaasintegrationcustomer",
                column: "AsaasCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_asaasintegrationcustomer_BrandId",
                table: "asaasintegrationcustomer",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationCustomer_Company",
                table: "asaasintegrationcustomer",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_asaasintegrationcustomer_CompanyId_BrandId",
                table: "asaasintegrationcustomer",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "UQ_AsaasIntegrationCustomer_Customer_Company",
                table: "asaasintegrationcustomer",
                columns: new[] { "CustomerId", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationPayment_Branch",
                table: "asaasintegrationpayment",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationPayment_Customer",
                table: "asaasintegrationpayment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationPayment_CustomerOrder",
                table: "asaasintegrationpayment",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "UQ_AsaasIntegrationPayment_AsaasPaymentId",
                table: "asaasintegrationpayment",
                column: "AsaasPaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_asaasintegrationsavedcard_BrandId",
                table: "asaasintegrationsavedcard",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_asaasintegrationsavedcard_CompanyId_BrandId",
                table: "asaasintegrationsavedcard",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationSavedCard_Customer",
                table: "asaasintegrationsavedcard",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationSetting_Branch",
                table: "asaasintegrationsetting",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_asaasintegrationsetting_BrandId",
                table: "asaasintegrationsetting",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationSetting_Company",
                table: "asaasintegrationsetting",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_asaasintegrationsetting_CompanyId_BrandId",
                table: "asaasintegrationsetting",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationWebhookLog_AsaasEventId",
                table: "asaasintegrationwebhooklog",
                column: "AsaasEventId");

            migrationBuilder.CreateIndex(
                name: "IX_asaasintegrationwebhooklog_BrandId",
                table: "asaasintegrationwebhooklog",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationWebhookLog_Company_Payment",
                table: "asaasintegrationwebhooklog",
                columns: new[] { "CompanyId", "PaymentId" });

            migrationBuilder.CreateIndex(
                name: "IX_AsaasIntegrationWebhookLog_Company_Status",
                table: "asaasintegrationwebhooklog",
                columns: new[] { "CompanyId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_asaasintegrationwebhooklog_CompanyId_BrandId",
                table: "asaasintegrationwebhooklog",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_branch_BrandId",
                table: "branch",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CompanyId",
                table: "branch",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_branch_CompanyId_BrandId",
                table: "branch",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_branch_SelfServiceEmployeeId",
                table: "branch",
                column: "SelfServiceEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchPaymentMethodSetting_Branch",
                table: "branchpaymentmethodsetting",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_branchpaymentmethodsetting_BrandId",
                table: "branchpaymentmethodsetting",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchPaymentMethodSetting_Company",
                table: "branchpaymentmethodsetting",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_branchpaymentmethodsetting_CompanyId_BrandId",
                table: "branchpaymentmethodsetting",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_brand_BusinessGroupId",
                table: "brand",
                column: "BusinessGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CashMovement_CashMovementTypeId",
                table: "cashmovement",
                column: "CashMovementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CashMovement_CashSessionId",
                table: "cashmovement",
                column: "CashSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CashMovement_CreatedAt",
                table: "cashmovement",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CashMovement_EmployeeId",
                table: "cashmovement",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CashMovement_SaleId",
                table: "cashmovement",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_CashRegister_BranchId",
                table: "cashregister",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CashSession_CashRegisterId",
                table: "cashsession",
                column: "CashRegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_CashSession_CashSessionStatusId",
                table: "cashsession",
                column: "CashSessionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CashSession_ClosedByEmployeeId",
                table: "cashsession",
                column: "ClosedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CashSession_CreatedAt",
                table: "cashsession",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CashSession_OpenedAt",
                table: "cashsession",
                column: "OpenedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CashSession_OpenedByEmployeeId",
                table: "cashsession",
                column: "OpenedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CashSessionPaymentReconciliation_PaymentMethodId",
                table: "cashsessionpaymentreconciliation",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "UQ_CashReconciliation_Session_Method",
                table: "cashsessionpaymentreconciliation",
                columns: new[] { "CashSessionId", "PaymentMethodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_BrandId",
                table: "category",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CompanyId",
                table: "category",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_category_CompanyId_BrandId",
                table: "category",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comanda_ComandaStatusId",
                table: "comanda",
                column: "ComandaStatusId");

            migrationBuilder.CreateIndex(
                name: "UQ_Comanda_BranchId_Code",
                table: "comanda",
                columns: new[] { "BranchId", "Code" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_comandaitemtransfer_CustomerOrderId",
                table: "comandaitemtransfer",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_comandaitemtransfer_EmployeeId",
                table: "comandaitemtransfer",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "UQ_ComandaSetting_BranchId",
                table: "comandasetting",
                column: "BranchId",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_company_BusinessGroupId",
                table: "company",
                column: "BusinessGroupId");

            migrationBuilder.CreateIndex(
                name: "UQ_Company_Cnpj",
                table: "company",
                column: "Cnpj",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_companybrand_BrandId",
                table: "companybrand",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_companybrand_CompanyId_BrandId",
                table: "companybrand",
                columns: new[] { "CompanyId", "BrandId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complement_ComplementGroupId",
                table: "complement",
                column: "ComplementGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Complement_ComplementItemId",
                table: "complement",
                column: "ComplementItemId");

            migrationBuilder.CreateIndex(
                name: "IX_complementgroup_BrandId",
                table: "complementgroup",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplementGroup_CompanyId",
                table: "complementgroup",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_complementgroup_CompanyId_BrandId",
                table: "complementgroup",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_complementitem_BrandId",
                table: "complementitem",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplementItem_CompanyId",
                table: "complementitem",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_complementitem_CompanyId_BrandId",
                table: "complementitem",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_ComplementItem_LinkedProductId",
                table: "complementitem",
                column: "LinkedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_BrandId",
                table: "customer",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CompanyId",
                table: "customer",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_CompanyId_BrandId",
                table: "customer",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "UQ_Customer_Cpf",
                table: "customer",
                columns: new[] { "CompanyId", "Cpf" },
                unique: true,
                filter: "[Cpf] IS NOT NULL AND [IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_customeraddress_BranchId",
                table: "customeraddress",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_customeraddress_BrandId",
                table: "customeraddress",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_customeraddress_CompanyId_BrandId",
                table: "customeraddress",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_customeraddress_CustomerId",
                table: "customeraddress",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_customeraddress_LastOrderId",
                table: "customeraddress",
                column: "LastOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_customerappuser_BranchId",
                table: "customerappuser",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_customerappuser_BrandId",
                table: "customerappuser",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_customerappuser_CompanyId_BrandId",
                table: "customerappuser",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_customerappuser_CustomerId",
                table: "customerappuser",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_BranchId",
                table: "customerorder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_ComandaId",
                table: "customerorder",
                column: "ComandaId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_CreatedAt",
                table: "customerorder",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_CreatedAt_OrderStatusId",
                table: "customerorder",
                columns: new[] { "CreatedAt", "OrderStatusId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_CustomerId",
                table: "customerorder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_customerorder_DeliveryDriverId",
                table: "customerorder",
                column: "DeliveryDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_DiningTableId",
                table: "customerorder",
                column: "DiningTableId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_EmployeeId",
                table: "customerorder",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_OpenedAt",
                table: "customerorder",
                column: "OpenedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_OrderOriginId",
                table: "customerorder",
                column: "OrderOriginId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_OrderStatusId",
                table: "customerorder",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRefreshToken_CustomerAppUserId",
                table: "customerrefreshtoken",
                column: "CustomerAppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_deliverydriver_BranchId",
                table: "deliverydriver",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_deliverydriverdailypayment_BranchId_DeliveryDriverId_WorkDate",
                table: "deliverydriverdailypayment",
                columns: new[] { "BranchId", "DeliveryDriverId", "WorkDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_deliverydriverdailypayment_DeliveryDriverId",
                table: "deliverydriverdailypayment",
                column: "DeliveryDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_deliveryfeecondition_DeliveryFeeConfigId",
                table: "deliveryfeecondition",
                column: "DeliveryFeeConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_deliveryfeeconfig_BranchId",
                table: "deliveryfeeconfig",
                column: "BranchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiningArea_BranchId",
                table: "diningarea",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DiningAreaAssignment_DiningAreaId",
                table: "diningareaassignment",
                column: "DiningAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_DiningAreaAssignment_EmployeeId",
                table: "diningareaassignment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DiningAreaTable_DiningAreaId",
                table: "diningareatable",
                column: "DiningAreaId");

            migrationBuilder.CreateIndex(
                name: "UK_DiningAreaTable_Table",
                table: "diningareatable",
                column: "DiningTableId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiningTable_BranchId_Number",
                table: "diningtable",
                columns: new[] { "BranchId", "Number" });

            migrationBuilder.CreateIndex(
                name: "IX_DiningTable_TableStatusId",
                table: "diningtable",
                column: "TableStatusId");

            migrationBuilder.CreateIndex(
                name: "UQ_DiningTable_QrToken",
                table: "diningtable",
                column: "QrToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_BranchId",
                table: "employee",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_JobTitleId",
                table: "employee",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "UQ_Employee_Cpf",
                table: "employee",
                columns: new[] { "BranchId", "Cpf" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ifoodanalyticssnapshot_BranchId_MerchantId_ReferenceDate",
                table: "ifoodanalyticssnapshot",
                columns: new[] { "BranchId", "MerchantId", "ReferenceDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ifoodcategorymapping_BranchId",
                table: "ifoodcategorymapping",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_IfoodCategoryMapping_CategoryId_BranchId",
                table: "ifoodcategorymapping",
                columns: new[] { "CategoryId", "BranchId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodComplementGroupMapping_BranchId",
                table: "ifoodcomplementgroupmapping",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_IfoodComplementGroupMapping_ComplementGroupId_BranchId",
                table: "ifoodcomplementgroupmapping",
                columns: new[] { "ComplementGroupId", "BranchId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodComplementMapping_BranchId",
                table: "ifoodcomplementmapping",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_IfoodComplementMapping_ComplementId_BranchId",
                table: "ifoodcomplementmapping",
                columns: new[] { "ComplementId", "BranchId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodComplementMapping_IfoodOptionId",
                table: "ifoodcomplementmapping",
                column: "IfoodOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ifoodeventinbox_BrandId",
                table: "ifoodeventinbox",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_ifoodeventinbox_CompanyId_BrandId",
                table: "ifoodeventinbox",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_ifoodeventinbox_CompanyId_EventId",
                table: "ifoodeventinbox",
                columns: new[] { "CompanyId", "EventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ifoodeventinbox_ProcessedAtUtc_NextAttemptAtUtc",
                table: "ifoodeventinbox",
                columns: new[] { "ProcessedAtUtc", "NextAttemptAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodFinancialEvent_BranchId_CompetenceDate",
                table: "ifoodfinancialevent",
                columns: new[] { "BranchId", "CompetenceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodFinancialEvent_BranchId_IfoodEventId",
                table: "ifoodfinancialevent",
                columns: new[] { "BranchId", "IfoodEventId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodFinancialEvent_ReferenceType_ReferenceId",
                table: "ifoodfinancialevent",
                columns: new[] { "ReferenceType", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "IX_ifoodintegrationsetting_BrandId",
                table: "ifoodintegrationsetting",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_IfoodIntegrationSetting_CompanyId",
                table: "ifoodintegrationsetting",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ifoodintegrationsetting_CompanyId_BrandId",
                table: "ifoodintegrationsetting",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodLogisticsDelivery_BranchId",
                table: "ifoodlogisticsdelivery",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "UQ_IfoodLogisticsDelivery_IfoodOrderId",
                table: "ifoodlogisticsdelivery",
                column: "IfoodOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IfoodMerchantMapping_BranchId",
                table: "ifoodmerchantmapping",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "UX_IfoodMerchantMapping_MerchantUuid",
                table: "ifoodmerchantmapping",
                column: "MerchantUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IfoodOpeningHours_BranchId_DayOfWeek",
                table: "ifoodopeninghours",
                columns: new[] { "BranchId", "DayOfWeek" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodOrder_BranchId",
                table: "ifoodorder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_IfoodOrder_CustomerOrderId",
                table: "ifoodorder",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "UQ_IfoodOrder_IfoodOrderId",
                table: "ifoodorder",
                column: "IfoodOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IfoodPizzaElementMapping_MappingId_Kind_LocalId",
                table: "ifoodpizzaelementmapping",
                columns: new[] { "IfoodPizzaMappingId", "Kind", "LocalId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodPizzaMapping_BranchId",
                table: "ifoodpizzamapping",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_IfoodPizzaMapping_PizzaConfigurationId_BranchId",
                table: "ifoodpizzamapping",
                columns: new[] { "PizzaConfigurationId", "BranchId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodProductMapping_BranchId",
                table: "ifoodproductmapping",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_IfoodProductMapping_ProductId_BranchId",
                table: "ifoodproductmapping",
                columns: new[] { "ProductId", "BranchId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodSettlement_BranchId_IfoodSettlementId",
                table: "ifoodsettlement",
                columns: new[] { "BranchId", "IfoodSettlementId" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodSettlement_BranchId_PaymentDate",
                table: "ifoodsettlement",
                columns: new[] { "BranchId", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_IfoodShippingDelivery_BranchId",
                table: "ifoodshippingdelivery",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "UQ_IfoodShippingDelivery_IfoodDeliveryId",
                table: "ifoodshippingdelivery",
                column: "IfoodDeliveryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ifoodshippingtracking_BranchId",
                table: "ifoodshippingtracking",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ifoodshippingtracking_CompanyId_OrderId",
                table: "ifoodshippingtracking",
                columns: new[] { "CompanyId", "OrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ifoodshippingtracking_IsActive_NextPollAtUtc",
                table: "ifoodshippingtracking",
                columns: new[] { "IsActive", "NextPollAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_jobtitle_BrandId",
                table: "jobtitle",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitle_CompanyId",
                table: "jobtitle",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_jobtitle_CompanyId_BrandId",
                table: "jobtitle",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_jobtitlefeature_AppFeatureId",
                table: "jobtitlefeature",
                column: "AppFeatureId");

            migrationBuilder.CreateIndex(
                name: "UQ_JobTitleFeature_JobTitleId_AppFeatureId",
                table: "jobtitlefeature",
                columns: new[] { "JobTitleId", "AppFeatureId" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "idx_auth_id",
                table: "keetaintegrationauthorizationsession",
                column: "AuthId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationauthorizationsession_BranchId",
                table: "keetaintegrationauthorizationsession",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationauthorizationsession_CompanyId",
                table: "keetaintegrationauthorizationsession",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "idx_internal_merchant",
                table: "keetaintegrationmerchantmapping",
                column: "InternalMerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationmerchantmapping_BranchId",
                table: "keetaintegrationmerchantmapping",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "uid_keeta_merchant",
                table: "keetaintegrationmerchantmapping",
                columns: new[] { "CompanyId", "BranchId", "KeetaMerchantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_order_merchant",
                table: "keetaintegrationorder",
                column: "KeetaMerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationorder_BranchId",
                table: "keetaintegrationorder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationorder_CompanyId",
                table: "keetaintegrationorder",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationorder_CustomerId",
                table: "keetaintegrationorder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationorder_CustomerOrderId",
                table: "keetaintegrationorder",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "uid_keeta_order_id",
                table: "keetaintegrationorder",
                column: "KeetaOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_event_order_id",
                table: "keetaintegrationordereventlog",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationordereventlog_BranchId",
                table: "keetaintegrationordereventlog",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationordereventlog_CompanyId",
                table: "keetaintegrationordereventlog",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "uid_event_id",
                table: "keetaintegrationordereventlog",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_refund_order_id",
                table: "keetaintegrationrefunddispute",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationrefunddispute_BranchId",
                table: "keetaintegrationrefunddispute",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_keetaintegrationrefunddispute_CompanyId",
                table: "keetaintegrationrefunddispute",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "uid_after_sale_order",
                table: "keetaintegrationrefunddispute",
                column: "AfterSaleOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeetaSetting_Branch",
                table: "keetaintegrationsetting",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_KeetaSetting_Company",
                table: "keetaintegrationsetting",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LogTracker_AppUserId",
                table: "logtracker",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LogTracker_CreatedAt",
                table: "logtracker",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_OperatingCost_BranchId_ReferenceYear_ReferenceMonth",
                table: "operatingcost",
                columns: new[] { "BranchId", "ReferenceYear", "ReferenceMonth" });

            migrationBuilder.CreateIndex(
                name: "IX_operatingcost_CostTypeId",
                table: "operatingcost",
                column: "CostTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_CustomerOrderId",
                table: "orderitem",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_EmployeeId",
                table: "orderitem",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderItemStatusId",
                table: "orderitem",
                column: "OrderItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_orderitem_PizzaCrustId",
                table: "orderitem",
                column: "PizzaCrustId");

            migrationBuilder.CreateIndex(
                name: "IX_orderitem_PizzaEdgeId",
                table: "orderitem",
                column: "PizzaEdgeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_PizzaSizeId",
                table: "orderitem",
                column: "PizzaSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductId",
                table: "orderitem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_orderitemboost_OrderItemId",
                table: "orderitemboost",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_orderitemboost_ProductBoostId",
                table: "orderitemboost",
                column: "ProductBoostId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemComplement_ComplementId",
                table: "orderitemcomplement",
                column: "ComplementId");

            migrationBuilder.CreateIndex(
                name: "IX_orderitemcomplement_OrderItemId",
                table: "orderitemcomplement",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_orderitemoptionalextra_OrderItemId",
                table: "orderitemoptionalextra",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_orderitemoptionalextra_ProductOptionalExtraId",
                table: "orderitemoptionalextra",
                column: "ProductOptionalExtraId");

            migrationBuilder.CreateIndex(
                name: "IX_orderitempizzaflavor_OrderItemId",
                table: "orderitempizzaflavor",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemPizzaFlavor_PizzaFlavorId",
                table: "orderitempizzaflavor",
                column: "PizzaFlavorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOrigin_BranchId",
                table: "orderorigin",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_orderorigin_BrandId",
                table: "orderorigin",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOrigin_CompanyId",
                table: "orderorigin",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_orderorigin_CompanyId_BrandId",
                table: "orderorigin",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPartialPayment_CashSessionId",
                table: "orderpartialpayment",
                column: "CashSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPartialPayment_CustomerOrderId",
                table: "orderpartialpayment",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_orderpartialpayment_EmployeeId",
                table: "orderpartialpayment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_orderpartialpayment_PaymentMethodId",
                table: "orderpartialpayment",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "UQ_Permission_Code",
                table: "permission",
                column: "Code",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaConfiguration_ProductId",
                table: "pizzaconfiguration",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaCrust_PizzaConfigurationId",
                table: "pizzacrust",
                column: "PizzaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaEdge_PizzaConfigurationId",
                table: "pizzaedge",
                column: "PizzaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_pizzaflavor_BrandId",
                table: "pizzaflavor",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaFlavor_CompanyId",
                table: "pizzaflavor",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_pizzaflavor_CompanyId_BrandId",
                table: "pizzaflavor",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_PizzaFlavorPrice_PizzaConfigurationId",
                table: "pizzaflavorprice",
                column: "PizzaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaFlavorPrice_PizzaFlavorId",
                table: "pizzaflavorprice",
                column: "PizzaFlavorId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaFlavorPrice_PizzaFlavorId_PizzaSizeId",
                table: "pizzaflavorprice",
                columns: new[] { "PizzaFlavorId", "PizzaSizeId" });

            migrationBuilder.CreateIndex(
                name: "IX_pizzaflavorprice_PizzaSizeId",
                table: "pizzaflavorprice",
                column: "PizzaSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_PizzaSize_PizzaConfigurationId",
                table: "pizzasize",
                column: "PizzaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Printer_BranchId",
                table: "printer",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "UQ_PrinterSetting_BranchId",
                table: "printersetting",
                column: "BranchId",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_product_BrandId",
                table: "product",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CompanyId",
                table: "product",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_product_CompanyId_BrandId",
                table: "product",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_Product_UnitOfMeasureId",
                table: "product",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_productboost_ProductId_DisplayOrder",
                table: "productboost",
                columns: new[] { "ProductId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductComplementGroup_ComplementGroupId",
                table: "productcomplementgroup",
                column: "ComplementGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComplementGroup_ProductId",
                table: "productcomplementgroup",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "UQ_ProductComplementGroup_ProductId_ComplementGroupId",
                table: "productcomplementgroup",
                columns: new[] { "ProductId", "ComplementGroupId" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_productoptionalextra_ProductId_DisplayOrder",
                table: "productoptionalextra",
                columns: new[] { "ProductId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_BranchId",
                table: "promotion",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_ProductId",
                table: "promotion",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_BranchId",
                table: "purchase",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_SupplierId",
                table: "purchase",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItem_ProductId",
                table: "purchaseitem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItem_PurchaseId",
                table: "purchaseitem",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_AppUserId",
                table: "refreshtoken",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "UQ_RevenueTarget_BranchId_ReferenceYear_ReferenceMonth",
                table: "revenuetarget",
                columns: new[] { "BranchId", "ReferenceYear", "ReferenceMonth" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Role_CompanyId",
                table: "role",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "rolepermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "rolepermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ_RolePermission_RoleId_PermissionId",
                table: "rolepermission",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_BranchId",
                table: "sale",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CashSessionId",
                table: "sale",
                column: "CashSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_EmployeeId",
                table: "sale",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_SoldAt",
                table: "sale",
                column: "SoldAt");

            migrationBuilder.CreateIndex(
                name: "UQ_Sale_BranchId_SaleNumber",
                table: "sale",
                columns: new[] { "BranchId", "SaleNumber" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "UQ_Sale_CustomerOrderId",
                table: "sale",
                column: "CustomerOrderId",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_SalePayment_PaymentMethodId",
                table: "salepayment",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_SalePayment_SaleId",
                table: "salepayment",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "UQ_ServiceFeeSetting_BranchId",
                table: "servicefeesetting",
                column: "BranchId",
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftClosing_BranchId",
                table: "shiftclosing",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftClosing_ClosedByEmployeeId",
                table: "shiftclosing",
                column: "ClosedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftClosing_OpenedByEmployeeId",
                table: "shiftclosing",
                column: "OpenedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftClosing_PeriodStart",
                table: "shiftclosing",
                column: "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftClosing_ShiftClosingStatusId",
                table: "shiftclosing",
                column: "ShiftClosingStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftClosingSession_CashSessionId",
                table: "shiftclosingsession",
                column: "CashSessionId");

            migrationBuilder.CreateIndex(
                name: "UQ_ShiftClosingSession_ShiftClosingId_CashSessionId",
                table: "shiftclosingsession",
                columns: new[] { "ShiftClosingId", "CashSessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockItem_ProductId",
                table: "stockitem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "UQ_StockItem_BranchId_ProductId",
                table: "stockitem",
                columns: new[] { "BranchId", "ProductId" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_EmployeeId",
                table: "stockmovement",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_MovedAt",
                table: "stockmovement",
                column: "MovedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_OrderItemId",
                table: "stockmovement",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_PurchaseItemId",
                table: "stockmovement",
                column: "PurchaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_StockItemId",
                table: "stockmovement",
                column: "StockItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_StockMovementTypeId",
                table: "stockmovement",
                column: "StockMovementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_BrandId",
                table: "supplier",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_CompanyId",
                table: "supplier",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_supplier_CompanyId_BrandId",
                table: "supplier",
                columns: new[] { "CompanyId", "BrandId" });

            migrationBuilder.CreateIndex(
                name: "IX_tableitemtransfer_CustomerOrderId",
                table: "tableitemtransfer",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tableitemtransfer_EmployeeId",
                table: "tableitemtransfer",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_tableitemtransfer_SourceDiningTableId",
                table: "tableitemtransfer",
                column: "SourceDiningTableId");

            migrationBuilder.CreateIndex(
                name: "IX_tableitemtransfer_TargetDiningTableId",
                table: "tableitemtransfer",
                column: "TargetDiningTableId");

            migrationBuilder.CreateIndex(
                name: "IX_TableReservation_BranchId_ReservedFor",
                table: "tablereservation",
                columns: new[] { "BranchId", "ReservedFor" });

            migrationBuilder.CreateIndex(
                name: "IX_TableReservation_DiningTableId",
                table: "tablereservation",
                column: "DiningTableId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_AppUserId",
                table: "userrole",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "userrole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ_UserRole_AppUserId_RoleId",
                table: "userrole",
                columns: new[] { "AppUserId", "RoleId" },
                unique: true,
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_WaiterMessage_BranchId",
                table: "waitermessage",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLog_AppUser",
                table: "accesslog",
                column: "AppUserId",
                principalTable: "appuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLog_CustomerAppUser",
                table: "accesslog",
                column: "CustomerAppUserId",
                principalTable: "customerappuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_Employee",
                table: "appuser",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appuserbranch_branch_BranchId",
                table: "appuserbranch",
                column: "BranchId",
                principalTable: "branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appuserbranch_employee_EmployeeId",
                table: "appuserbranch",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appusercompany_employee_EmployeeId",
                table: "appusercompany",
                column: "EmployeeId",
                principalTable: "employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_appuserfeature_branch_BranchId",
                table: "appuserfeature",
                column: "BranchId",
                principalTable: "branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AsaasIntegrationPayment_Branch",
                table: "asaasintegrationpayment",
                column: "BranchId",
                principalTable: "branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AsaasIntegrationPayment_CustomerOrder",
                table: "asaasintegrationpayment",
                column: "CustomerOrderId",
                principalTable: "customerorder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AsaasIntegrationSetting_Branch",
                table: "asaasintegrationsetting",
                column: "BranchId",
                principalTable: "branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_SelfServiceEmployee",
                table: "branch",
                column: "SelfServiceEmployeeId",
                principalTable: "employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Company",
                table: "branch");

            migrationBuilder.DropForeignKey(
                name: "FK_JobTitle_Company",
                table: "jobtitle");

            migrationBuilder.DropForeignKey(
                name: "FK_Branch_SelfServiceEmployee",
                table: "branch");

            migrationBuilder.DropTable(
                name: "accesslog");

            migrationBuilder.DropTable(
                name: "appuserbranch");

            migrationBuilder.DropTable(
                name: "appusercompany");

            migrationBuilder.DropTable(
                name: "appuserfeature");

            migrationBuilder.DropTable(
                name: "asaasintegrationcustomer");

            migrationBuilder.DropTable(
                name: "asaasintegrationpayment");

            migrationBuilder.DropTable(
                name: "asaasintegrationsavedcard");

            migrationBuilder.DropTable(
                name: "asaasintegrationsetting");

            migrationBuilder.DropTable(
                name: "asaasintegrationwebhooklog");

            migrationBuilder.DropTable(
                name: "branchpaymentmethodsetting");

            migrationBuilder.DropTable(
                name: "cashmovement");

            migrationBuilder.DropTable(
                name: "cashsessionpaymentreconciliation");

            migrationBuilder.DropTable(
                name: "comandaitemtransfer");

            migrationBuilder.DropTable(
                name: "comandasetting");

            migrationBuilder.DropTable(
                name: "companybrand");

            migrationBuilder.DropTable(
                name: "customeraddress");

            migrationBuilder.DropTable(
                name: "customerrefreshtoken");

            migrationBuilder.DropTable(
                name: "deliverydriverdailypayment");

            migrationBuilder.DropTable(
                name: "deliveryfeecondition");

            migrationBuilder.DropTable(
                name: "diningareaassignment");

            migrationBuilder.DropTable(
                name: "diningareatable");

            migrationBuilder.DropTable(
                name: "ifoodanalyticssnapshot");

            migrationBuilder.DropTable(
                name: "ifoodcategorymapping");

            migrationBuilder.DropTable(
                name: "ifoodcomplementgroupmapping");

            migrationBuilder.DropTable(
                name: "ifoodcomplementmapping");

            migrationBuilder.DropTable(
                name: "ifoodeventinbox");

            migrationBuilder.DropTable(
                name: "ifoodfinancialevent");

            migrationBuilder.DropTable(
                name: "ifoodintegrationsetting");

            migrationBuilder.DropTable(
                name: "ifoodlogisticsdelivery");

            migrationBuilder.DropTable(
                name: "ifoodmerchantmapping");

            migrationBuilder.DropTable(
                name: "ifoodopeninghours");

            migrationBuilder.DropTable(
                name: "ifoodpizzaelementmapping");

            migrationBuilder.DropTable(
                name: "ifoodproductmapping");

            migrationBuilder.DropTable(
                name: "ifoodsettlement");

            migrationBuilder.DropTable(
                name: "ifoodshippingdelivery");

            migrationBuilder.DropTable(
                name: "ifoodshippingtracking");

            migrationBuilder.DropTable(
                name: "jobtitlefeature");

            migrationBuilder.DropTable(
                name: "keetaintegrationauthorizationsession");

            migrationBuilder.DropTable(
                name: "keetaintegrationmerchantmapping");

            migrationBuilder.DropTable(
                name: "keetaintegrationordereventlog");

            migrationBuilder.DropTable(
                name: "keetaintegrationrefunddispute");

            migrationBuilder.DropTable(
                name: "keetaintegrationsetting");

            migrationBuilder.DropTable(
                name: "logtracker");

            migrationBuilder.DropTable(
                name: "operatingcost");

            migrationBuilder.DropTable(
                name: "orderitemboost");

            migrationBuilder.DropTable(
                name: "orderitemcomplement");

            migrationBuilder.DropTable(
                name: "orderitemoptionalextra");

            migrationBuilder.DropTable(
                name: "orderitempizzaflavor");

            migrationBuilder.DropTable(
                name: "orderpartialpayment");

            migrationBuilder.DropTable(
                name: "pizzaflavorprice");

            migrationBuilder.DropTable(
                name: "printer");

            migrationBuilder.DropTable(
                name: "printersetting");

            migrationBuilder.DropTable(
                name: "productcomplementgroup");

            migrationBuilder.DropTable(
                name: "productstock");

            migrationBuilder.DropTable(
                name: "promotion");

            migrationBuilder.DropTable(
                name: "refreshtoken");

            migrationBuilder.DropTable(
                name: "revenuetarget");

            migrationBuilder.DropTable(
                name: "rolepermission");

            migrationBuilder.DropTable(
                name: "salepayment");

            migrationBuilder.DropTable(
                name: "servicefeesetting");

            migrationBuilder.DropTable(
                name: "shiftclosingsession");

            migrationBuilder.DropTable(
                name: "stockmovement");

            migrationBuilder.DropTable(
                name: "tableitemtransfer");

            migrationBuilder.DropTable(
                name: "tablereservation");

            migrationBuilder.DropTable(
                name: "userrole");

            migrationBuilder.DropTable(
                name: "waitermessage");

            migrationBuilder.DropTable(
                name: "cashmovementtype");

            migrationBuilder.DropTable(
                name: "customerappuser");

            migrationBuilder.DropTable(
                name: "deliveryfeeconfig");

            migrationBuilder.DropTable(
                name: "diningarea");

            migrationBuilder.DropTable(
                name: "ifoodorder");

            migrationBuilder.DropTable(
                name: "ifoodpizzamapping");

            migrationBuilder.DropTable(
                name: "appfeature");

            migrationBuilder.DropTable(
                name: "keetaintegrationorder");

            migrationBuilder.DropTable(
                name: "costtype");

            migrationBuilder.DropTable(
                name: "productboost");

            migrationBuilder.DropTable(
                name: "complement");

            migrationBuilder.DropTable(
                name: "productoptionalextra");

            migrationBuilder.DropTable(
                name: "pizzaflavor");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "paymentmethod");

            migrationBuilder.DropTable(
                name: "sale");

            migrationBuilder.DropTable(
                name: "shiftclosing");

            migrationBuilder.DropTable(
                name: "orderitem");

            migrationBuilder.DropTable(
                name: "purchaseitem");

            migrationBuilder.DropTable(
                name: "stockitem");

            migrationBuilder.DropTable(
                name: "stockmovementtype");

            migrationBuilder.DropTable(
                name: "appuser");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "complementgroup");

            migrationBuilder.DropTable(
                name: "complementitem");

            migrationBuilder.DropTable(
                name: "cashsession");

            migrationBuilder.DropTable(
                name: "shiftclosingstatus");

            migrationBuilder.DropTable(
                name: "customerorder");

            migrationBuilder.DropTable(
                name: "orderitemstatus");

            migrationBuilder.DropTable(
                name: "pizzacrust");

            migrationBuilder.DropTable(
                name: "pizzaedge");

            migrationBuilder.DropTable(
                name: "pizzasize");

            migrationBuilder.DropTable(
                name: "purchase");

            migrationBuilder.DropTable(
                name: "cashregister");

            migrationBuilder.DropTable(
                name: "cashsessionstatus");

            migrationBuilder.DropTable(
                name: "comanda");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "diningtable");

            migrationBuilder.DropTable(
                name: "orderorigin");

            migrationBuilder.DropTable(
                name: "orderstatus");

            migrationBuilder.DropTable(
                name: "deliverydriver");

            migrationBuilder.DropTable(
                name: "pizzaconfiguration");

            migrationBuilder.DropTable(
                name: "supplier");

            migrationBuilder.DropTable(
                name: "comandastatus");

            migrationBuilder.DropTable(
                name: "tablestatus");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "unitofmeasure");

            migrationBuilder.DropTable(
                name: "company");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "branch");

            migrationBuilder.DropTable(
                name: "jobtitle");

            migrationBuilder.DropTable(
                name: "brand");

            migrationBuilder.DropTable(
                name: "businessgroup");
        }
    }
}
