using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DingFood.Infrastructure.Persistence.Migrations;

// Frozen compatibility bridge for existing SQL-managed installations.
[DbContext(typeof(AppDbContext))]
[Migration("202609100003_CompleteLegacyOperationalSchema")]
public sealed class CompleteLegacyOperationalSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS `shiftclosingstatus` (
              `Id` bigint NOT NULL AUTO_INCREMENT,
              `Name` varchar(50) NOT NULL,
              `CreatedAt` datetime(6) NOT NULL,
              `UpdatedAt` datetime(6) NULL,
              `IsActive` tinyint(1) NOT NULL,
              PRIMARY KEY (`Id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
            
            CREATE TABLE IF NOT EXISTS `shiftclosing` (
              `Id` bigint NOT NULL AUTO_INCREMENT,
              `BranchId` bigint NOT NULL,
              `ShiftClosingStatusId` bigint NOT NULL,
              `OpenedByEmployeeId` bigint NOT NULL,
              `ClosedByEmployeeId` bigint NULL,
              `PeriodStart` datetime(6) NOT NULL,
              `PeriodEnd` datetime(6) NULL,
              `CashSessionsCount` int NOT NULL DEFAULT 0,
              `TotalOpeningAmount` decimal(18,2) NOT NULL DEFAULT 0,
              `TotalExpectedAmount` decimal(18,2) NOT NULL DEFAULT 0,
              `TotalRealizedAmount` decimal(18,2) NOT NULL DEFAULT 0,
              `TotalDifferenceAmount` decimal(18,2) NOT NULL DEFAULT 0,
              `Notes` varchar(500) NULL,
              `CreatedAt` datetime(6) NOT NULL,
              `UpdatedAt` datetime(6) NULL,
              `IsActive` tinyint(1) NOT NULL,
              PRIMARY KEY (`Id`),
              KEY `IX_ShiftClosing_BranchId` (`BranchId`),
              KEY `IX_ShiftClosing_ShiftClosingStatusId` (`ShiftClosingStatusId`),
              KEY `IX_ShiftClosing_OpenedByEmployeeId` (`OpenedByEmployeeId`),
              KEY `IX_ShiftClosing_ClosedByEmployeeId` (`ClosedByEmployeeId`),
              KEY `IX_ShiftClosing_PeriodStart` (`PeriodStart`),
              CONSTRAINT `FK_ShiftClosing_Branch` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
              CONSTRAINT `FK_ShiftClosing_ShiftClosingStatus` FOREIGN KEY (`ShiftClosingStatusId`) REFERENCES `shiftclosingstatus` (`Id`),
              CONSTRAINT `FK_ShiftClosing_OpenedByEmployee` FOREIGN KEY (`OpenedByEmployeeId`) REFERENCES `employee` (`Id`),
              CONSTRAINT `FK_ShiftClosing_ClosedByEmployee` FOREIGN KEY (`ClosedByEmployeeId`) REFERENCES `employee` (`Id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
            
            CREATE TABLE IF NOT EXISTS `shiftclosingsession` (
              `Id` bigint NOT NULL AUTO_INCREMENT,
              `ShiftClosingId` bigint NOT NULL,
              `CashSessionId` bigint NOT NULL,
              `CreatedAt` datetime(6) NOT NULL,
              `UpdatedAt` datetime(6) NULL,
              `IsActive` tinyint(1) NOT NULL,
              PRIMARY KEY (`Id`),
              UNIQUE KEY `UQ_ShiftClosingSession_ShiftClosingId_CashSessionId` (`ShiftClosingId`, `CashSessionId`),
              KEY `IX_ShiftClosingSession_CashSessionId` (`CashSessionId`),
              CONSTRAINT `FK_ShiftClosingSession_ShiftClosing` FOREIGN KEY (`ShiftClosingId`) REFERENCES `shiftclosing` (`Id`),
              CONSTRAINT `FK_ShiftClosingSession_CashSession` FOREIGN KEY (`CashSessionId`) REFERENCES `cashsession` (`Id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
            
            CREATE TABLE IF NOT EXISTS `cashsessionpaymentreconciliation` (
              `Id` bigint NOT NULL AUTO_INCREMENT,
              `CashSessionId` bigint NOT NULL,
              `PaymentMethodId` bigint NOT NULL,
              `ExpectedAmount` decimal(18,2) NOT NULL DEFAULT 0,
              `CountedAmount` decimal(18,2) NOT NULL DEFAULT 0,
              `DifferenceAmount` decimal(18,2) NOT NULL DEFAULT 0,
              `CreatedAt` datetime(6) NOT NULL,
              `UpdatedAt` datetime(6) NULL,
              `IsActive` tinyint(1) NOT NULL,
              PRIMARY KEY (`Id`),
              UNIQUE KEY `UQ_CashReconciliation_Session_Method` (`CashSessionId`, `PaymentMethodId`),
              KEY `IX_CashSessionPaymentReconciliation_PaymentMethodId` (`PaymentMethodId`),
              CONSTRAINT `FK_CashSessionPaymentReconciliation_CashSession` FOREIGN KEY (`CashSessionId`) REFERENCES `cashsession` (`Id`),
              CONSTRAINT `FK_CashSessionPaymentReconciliation_PaymentMethod` FOREIGN KEY (`PaymentMethodId`) REFERENCES `paymentmethod` (`Id`)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
            INSERT IGNORE INTO shiftclosingstatus (Id, Name, CreatedAt, IsActive) VALUES (1, 'Aberto', NOW(), 1), (2, 'Fechado', NOW(), 1);
            """, suppressTransaction: true);
        foreach (var (column, definition, legacy) in new[] {
            ("CardBrand", "varchar(50) NOT NULL", "CreditCardBrand"),
            ("Last4Digits", "varchar(10) NOT NULL", "CreditCardNumber"),
            ("HolderName", "longtext NULL", ""), ("ExpiryMonth", "longtext NULL", ""),
            ("ExpiryYear", "longtext NULL", ""), ("IsDefault", "tinyint(1) NOT NULL DEFAULT 0", "") })
        {
            var statement = legacy.Length == 0
                ? $"ALTER TABLE `asaasintegrationsavedcard` ADD COLUMN `{column}` {definition}"
                : $"ALTER TABLE `asaasintegrationsavedcard` CHANGE COLUMN `{legacy}` `{column}` {definition}";
            migrationBuilder.Sql($"""
                SET @compat_ddl = IF(EXISTS(SELECT 1 FROM information_schema.columns WHERE table_schema = DATABASE() AND LOWER(table_name) = 'asaasintegrationsavedcard' AND column_name = '{column}'), 'SELECT 1', '{statement}');
                PREPARE compat_stmt FROM @compat_ddl; EXECUTE compat_stmt; DEALLOCATE PREPARE compat_stmt;
                """, suppressTransaction: true);
        }
    }
    protected override void Down(MigrationBuilder migrationBuilder)
        => throw new NotSupportedException("Restore a verified backup to reverse operational schema changes.");
}
