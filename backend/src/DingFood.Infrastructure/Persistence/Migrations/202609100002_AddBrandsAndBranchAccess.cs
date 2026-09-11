using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DingFood.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("202609100002_AddBrandsAndBranchAccess")]
public sealed class AddBrandsAndBranchAccess : Migration
{
    // Frozen migration inventory: do not derive historical schema from the current domain model.
    internal static readonly string[] BrandTables = ["asaasintegrationcustomer", "asaasintegrationsavedcard", "asaasintegrationsetting",
        "asaasintegrationwebhooklog", "branch", "branchpaymentmethodsetting", "category", "complementgroup", "complementitem",
        "customer", "customeraddress", "customerappuser", "ifoodeventinbox", "ifoodintegrationsetting", "jobtitle", "orderorigin", "pizzaflavor", "product", "supplier"];
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Older SQL-managed installations stored ownership of saved cards only through Customer.
        migrationBuilder.Sql("""
            SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.columns WHERE table_schema = DATABASE() AND LOWER(table_name) = 'asaasintegrationsavedcard' AND column_name = 'CompanyId'),
              'SELECT 1', 'ALTER TABLE `asaasintegrationsavedcard` ADD COLUMN `CompanyId` bigint NULL');
            PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
            UPDATE `asaasintegrationsavedcard` card JOIN `customer` c ON c.`Id` = card.`CustomerId` SET card.`CompanyId` = c.`CompanyId` WHERE card.`CompanyId` IS NULL;
            ALTER TABLE `asaasintegrationsavedcard` MODIFY COLUMN `CompanyId` bigint NOT NULL;
            """, suppressTransaction: true);
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS `brand` (
              `Id` bigint NOT NULL AUTO_INCREMENT PRIMARY KEY, `BusinessGroupId` bigint NOT NULL,
              `Name` varchar(150) NOT NULL, `IsActive` tinyint(1) NOT NULL,
              CONSTRAINT `FK_Brand_Group` FOREIGN KEY (`BusinessGroupId`) REFERENCES `businessgroup` (`Id`)
            ) ENGINE=InnoDB;
            CREATE TABLE IF NOT EXISTS `companybrand` (
              `Id` bigint NOT NULL AUTO_INCREMENT PRIMARY KEY, `CompanyId` bigint NOT NULL, `BrandId` bigint NOT NULL, `IsActive` tinyint(1) NOT NULL,
              UNIQUE KEY `UX_CompanyBrand` (`CompanyId`, `BrandId`),
              CONSTRAINT `FK_CompanyBrand_Company` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
              CONSTRAINT `FK_CompanyBrand_Brand` FOREIGN KEY (`BrandId`) REFERENCES `brand` (`Id`)
            ) ENGINE=InnoDB;
            INSERT INTO `brand` (`Id`, `BusinessGroupId`, `Name`, `IsActive`)
              SELECT c.`Id`, c.`BusinessGroupId`, c.`TradeName`, c.`IsActive` FROM `company` c
              WHERE NOT EXISTS (SELECT 1 FROM `brand` b WHERE b.`Id` = c.`Id`);
            INSERT INTO `companybrand` (`CompanyId`, `BrandId`, `IsActive`)
              SELECT c.`Id`, c.`Id`, c.`IsActive` FROM `company` c
              WHERE NOT EXISTS (SELECT 1 FROM `companybrand` b WHERE b.`CompanyId` = c.`Id` AND b.`BrandId` = c.`Id`);
            """, suppressTransaction: true);
        foreach (var table in BrandTables)
        {
            migrationBuilder.Sql($"""
                SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.columns WHERE table_schema = DATABASE() AND LOWER(table_name) = '{table}' AND column_name = 'BrandId'),
                  'SELECT 1', 'ALTER TABLE `{table}` ADD COLUMN `BrandId` bigint NULL');
                PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
                UPDATE `{table}` SET `BrandId` = `CompanyId` WHERE `BrandId` IS NULL AND `CompanyId` IS NOT NULL;
                SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.statistics WHERE table_schema = DATABASE() AND LOWER(table_name) = '{table}' AND index_name = 'IX_{table}_Company_Brand'),
                  'SELECT 1', 'CREATE INDEX `IX_{table}_Company_Brand` ON `{table}` (`CompanyId`, `BrandId`)');
                PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
                SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.table_constraints WHERE constraint_schema = DATABASE() AND constraint_name = 'FK_{table}_Brand'),
                  'SELECT 1', 'ALTER TABLE `{table}` ADD CONSTRAINT `FK_{table}_Brand` FOREIGN KEY (`BrandId`) REFERENCES `brand` (`Id`)');
                PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
                """, suppressTransaction: true);
        }
        migrationBuilder.Sql("""
            CREATE TABLE IF NOT EXISTS `appuserbranch` (
              `Id` bigint NOT NULL AUTO_INCREMENT PRIMARY KEY, `AppUserId` bigint NOT NULL, `BranchId` bigint NOT NULL,
              `EmployeeId` bigint NULL, `RoleId` bigint NULL, `UsesLegacyRoles` tinyint(1) NOT NULL DEFAULT 0, `IsActive` tinyint(1) NOT NULL,
              UNIQUE KEY `UX_AppUserBranch` (`AppUserId`, `BranchId`),
              CONSTRAINT `FK_AppUserBranch_User` FOREIGN KEY (`AppUserId`) REFERENCES `appuser` (`Id`),
              CONSTRAINT `FK_AppUserBranch_Branch` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
              CONSTRAINT `FK_AppUserBranch_Employee` FOREIGN KEY (`EmployeeId`) REFERENCES `employee` (`Id`),
              CONSTRAINT `FK_AppUserBranch_Role` FOREIGN KEY (`RoleId`) REFERENCES `role` (`Id`)
            ) ENGINE=InnoDB;
            -- Preserve existing administrators' explicit company memberships. Other employees get only their own branch.
            INSERT INTO `appuserbranch` (`AppUserId`, `BranchId`, `EmployeeId`, `RoleId`, `UsesLegacyRoles`, `IsActive`)
            SELECT g.`AppUserId`, b.`Id`, CASE WHEN e.`BranchId` = b.`Id` THEN e.`Id` ELSE NULL END,
              (SELECT r.`Id` FROM `userrole` ur JOIN `role` r ON r.`Id` = ur.`RoleId`
               WHERE ur.`AppUserId` = g.`AppUserId` AND ur.`CompanyId` = b.`CompanyId` AND ur.`IsActive` = 1 AND r.`IsActive` = 1
               ORDER BY (r.`Name` = 'Administrador') DESC, r.`Id` LIMIT 1), 1, 1
            FROM `appusercompany` g JOIN `appuser` u ON u.`Id` = g.`AppUserId`
            JOIN `branch` b ON b.`CompanyId` = g.`CompanyId` AND b.`IsActive` = 1
            LEFT JOIN `employee` e ON e.`Id` = COALESCE(g.`EmployeeId`, CASE WHEN u.`CompanyId` = g.`CompanyId` THEN u.`EmployeeId` END) AND e.`IsActive` = 1
            WHERE g.`IsActive` = 1 AND u.`IsActive` = 1 AND (e.`BranchId` = b.`Id` OR EXISTS (
              SELECT 1 FROM `userrole` ur JOIN `role` r ON r.`Id` = ur.`RoleId`
              WHERE ur.`AppUserId` = g.`AppUserId` AND ur.`CompanyId` = b.`CompanyId` AND ur.`IsActive` = 1 AND r.`IsActive` = 1 AND r.`Name` = 'Administrador'))
            AND NOT EXISTS (SELECT 1 FROM `appuserbranch` old WHERE old.`AppUserId` = g.`AppUserId` AND old.`BranchId` = b.`Id`);
            SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.statistics WHERE table_schema = DATABASE() AND LOWER(table_name) = 'employee' AND index_name = 'UQ_Employee_Cpf'),
              'ALTER TABLE `employee` DROP INDEX `UQ_Employee_Cpf`', 'SELECT 1');
            PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
            CREATE UNIQUE INDEX `UQ_Employee_Cpf` ON `employee` (`BranchId`, `Cpf`);
            """, suppressTransaction: true);
        migrationBuilder.Sql("""
            SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.columns WHERE table_schema = DATABASE() AND LOWER(table_name) = 'appuserfeature' AND column_name = 'BranchId'),
              'SELECT 1', 'ALTER TABLE `appuserfeature` ADD COLUMN `BranchId` bigint NULL');
            PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
            UPDATE `appuserfeature` f JOIN `appuser` u ON u.`Id` = f.`AppUserId` JOIN `employee` e ON e.`Id` = u.`EmployeeId`
              SET f.`BranchId` = e.`BranchId` WHERE f.`BranchId` IS NULL;
            SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.statistics WHERE table_schema = DATABASE() AND LOWER(table_name) = 'appuserfeature' AND index_name = 'IX_AppUserFeature_User'),
              'SELECT 1', 'CREATE INDEX `IX_AppUserFeature_User` ON `appuserfeature` (`AppUserId`)');
            PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
            SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.statistics WHERE table_schema = DATABASE() AND LOWER(table_name) = 'appuserfeature' AND index_name = 'UQ_AppUserFeature_AppUserId_AppFeatureId'),
              'ALTER TABLE `appuserfeature` DROP INDEX `UQ_AppUserFeature_AppUserId_AppFeatureId`', 'SELECT 1');
            PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
            CREATE UNIQUE INDEX `UQ_AppUserFeature_AppUserId_AppFeatureId` ON `appuserfeature` (`AppUserId`, `AppFeatureId`, `BranchId`);
            """, suppressTransaction: true);
        foreach (var (table, column, parent) in new[] {
            ("appuserfeature", "BranchId", "branch"), ("productstock", "ProductId", "product"),
            ("diningareaassignment", "DiningAreaId", "diningarea"), ("diningareaassignment", "EmployeeId", "employee"),
            ("diningareatable", "DiningAreaId", "diningarea"), ("diningareatable", "DiningTableId", "diningtable") })
        {
            migrationBuilder.Sql($"""
                SET @brand_ddl = IF(EXISTS(SELECT 1 FROM information_schema.key_column_usage WHERE constraint_schema = DATABASE() AND LOWER(table_name) = '{table}' AND column_name = '{column}' AND referenced_table_name IS NOT NULL),
                  'SELECT 1', 'ALTER TABLE `{table}` ADD CONSTRAINT `FK_{table}_{column}_Owner` FOREIGN KEY (`{column}`) REFERENCES `{parent}` (`Id`)');
                PREPARE brand_stmt FROM @brand_ddl; EXECUTE brand_stmt; DEALLOCATE PREPARE brand_stmt;
                """, suppressTransaction: true);
        }
    }
    protected override void Down(MigrationBuilder migrationBuilder)
        => throw new NotSupportedException("Brand and branch assignments contain ownership information. Restore a verified backup to reverse this migration.");
}
