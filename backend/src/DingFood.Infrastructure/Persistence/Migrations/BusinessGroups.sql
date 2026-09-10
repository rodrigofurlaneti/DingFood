-- MySQL 8. Apply with application writes paused. Existing company/branch IDs are preserved.
CREATE TABLE IF NOT EXISTS `businessgroup` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

SET @group_ddl = IF(EXISTS(SELECT 1 FROM information_schema.columns
  WHERE table_schema = DATABASE() AND LOWER(table_name) = 'company' AND column_name = 'BusinessGroupId'),
  'SELECT 1', 'ALTER TABLE `company` ADD COLUMN `BusinessGroupId` bigint NULL');
PREPARE group_stmt FROM @group_ddl;
EXECUTE group_stmt;
DEALLOCATE PREPARE group_stmt;

INSERT INTO `businessgroup` (`Id`, `Name`, `IsActive`, `CreatedAt`)
SELECT c.`Id`, c.`TradeName`, c.`IsActive`, c.`CreatedAt` FROM `company` c
LEFT JOIN `businessgroup` g ON g.`Id` = c.`Id`
WHERE c.`BusinessGroupId` IS NULL AND g.`Id` IS NULL;
UPDATE `company` SET `BusinessGroupId` = `Id` WHERE `BusinessGroupId` IS NULL;
ALTER TABLE `company` MODIFY COLUMN `BusinessGroupId` bigint NOT NULL;

SET @group_ddl = IF(EXISTS(SELECT 1 FROM information_schema.table_constraints
  WHERE constraint_schema = DATABASE() AND LOWER(table_name) = 'company' AND constraint_name = 'FK_Company_BusinessGroup'),
  'SELECT 1', 'ALTER TABLE `company` ADD CONSTRAINT `FK_Company_BusinessGroup` FOREIGN KEY (`BusinessGroupId`) REFERENCES `businessgroup` (`Id`) ON DELETE RESTRICT');
PREPARE group_stmt FROM @group_ddl;
EXECUTE group_stmt;
DEALLOCATE PREPARE group_stmt;

CREATE TABLE IF NOT EXISTS `appusercompany` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `AppUserId` bigint NOT NULL,
  `CompanyId` bigint NOT NULL,
  `EmployeeId` bigint NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UX_AppUserCompany_UserCompany` (`AppUserId`, `CompanyId`),
  CONSTRAINT `FK_AppUserCompany_AppUser` FOREIGN KEY (`AppUserId`) REFERENCES `appuser` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AppUserCompany_Employee` FOREIGN KEY (`EmployeeId`) REFERENCES `employee` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AppUserCompany_Company` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO `appusercompany` (`AppUserId`, `CompanyId`, `IsActive`, `CreatedAt`)
SELECT u.`Id`, u.`CompanyId`, u.`IsActive`, u.`CreatedAt` FROM `appuser` u
WHERE NOT EXISTS (SELECT 1 FROM `appusercompany` g WHERE g.`AppUserId` = u.`Id` AND g.`CompanyId` = u.`CompanyId`);

-- Stop on duplicates instead of silently choosing an owner or deleting operational data.
SET @group_ddl = IF(EXISTS(SELECT 1 FROM information_schema.statistics
  WHERE table_schema = DATABASE() AND LOWER(table_name) = 'company' AND index_name = 'UQ_Company_Cnpj'),
  'SELECT 1', 'CREATE UNIQUE INDEX `UQ_Company_Cnpj` ON `company` (`Cnpj`)');
PREPARE group_stmt FROM @group_ddl;
EXECUTE group_stmt;
DEALLOCATE PREPARE group_stmt;

UPDATE `ifoodmerchantmapping` SET `MerchantUuid` = NULL WHERE TRIM(`MerchantUuid`) = '';
SET @group_ddl = IF(EXISTS(SELECT 1 FROM information_schema.statistics
  WHERE table_schema = DATABASE() AND LOWER(table_name) = 'ifoodmerchantmapping' AND index_name = 'UX_IfoodMerchantMapping_MerchantUuid'),
  'SELECT 1', 'CREATE UNIQUE INDEX `UX_IfoodMerchantMapping_MerchantUuid` ON `ifoodmerchantmapping` (`MerchantUuid`)');
PREPARE group_stmt FROM @group_ddl;
EXECUTE group_stmt;
DEALLOCATE PREPARE group_stmt;
