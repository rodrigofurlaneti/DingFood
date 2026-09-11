using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DingFood.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("202609110001_AddOwnDelivery")]
public sealed class AddOwnDelivery : Migration
{
    protected override void Up(MigrationBuilder b)
    {
        b.Sql("""
            CREATE TABLE `deliverydriver` (
              `Id` bigint NOT NULL AUTO_INCREMENT PRIMARY KEY, `BranchId` bigint NOT NULL,
              `Name` varchar(150) NOT NULL, `Phone` varchar(20) NOT NULL, `VehiclePlate` varchar(10) NOT NULL,
              `EmploymentType` varchar(20) NOT NULL, `IsActive` tinyint(1) NOT NULL,
              FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`)
            ) ENGINE=InnoDB;
            CREATE TABLE `deliveryfeeconfig` (
              `Id` bigint NOT NULL AUTO_INCREMENT PRIMARY KEY, `BranchId` bigint NOT NULL,
              `Model` varchar(20) NOT NULL, `DailyAmount` decimal(18,2) NOT NULL, `MaxRadiusKm` decimal(18,6) NOT NULL,
              `PricePerKm` decimal(18,2) NOT NULL, `TimeZoneId` varchar(100) NOT NULL,
              UNIQUE KEY `IX_DeliveryFeeConfig_BranchId` (`BranchId`), FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`)
            ) ENGINE=InnoDB;
            CREATE TABLE `deliveryfeecondition` (
              `Id` bigint NOT NULL AUTO_INCREMENT PRIMARY KEY, `DeliveryFeeConfigId` bigint NOT NULL,
              `DaysOfWeek` int NOT NULL, `StartMinute` int NOT NULL, `EndMinute` int NOT NULL,
              `PricePerKm` decimal(18,2) NOT NULL, `Priority` int NOT NULL,
              FOREIGN KEY (`DeliveryFeeConfigId`) REFERENCES `deliveryfeeconfig` (`Id`) ON DELETE CASCADE
            ) ENGINE=InnoDB;
            CREATE TABLE `deliverydriverdailypayment` (
              `Id` bigint NOT NULL AUTO_INCREMENT PRIMARY KEY, `BranchId` bigint NOT NULL, `DeliveryDriverId` bigint NOT NULL,
              `WorkDate` date NOT NULL, `Amount` decimal(18,2) NOT NULL,
              UNIQUE KEY `IX_DailyPayment_Driver_Date` (`BranchId`, `DeliveryDriverId`, `WorkDate`),
              FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`), FOREIGN KEY (`DeliveryDriverId`) REFERENCES `deliverydriver` (`Id`)
            ) ENGINE=InnoDB;
            ALTER TABLE `customerorder`
              ADD COLUMN `DeliveryDriverId` bigint NULL,
              ADD COLUMN `DeliveryFeeAmount` decimal(18,2) NOT NULL DEFAULT 0,
              ADD COLUMN `DeliveryDistanceKm` decimal(18,6) NULL,
              ADD COLUMN `DeliveryPricePerKm` decimal(18,2) NULL,
              ADD COLUMN `DeliveryDailyAmount` decimal(18,2) NULL,
              ADD COLUMN `DeliveryPaymentModel` varchar(20) NULL,
              ADD COLUMN `DeliveryTimeZoneId` varchar(100) NULL,
              ADD COLUMN `DeliveryFeeCalculatedAt` datetime(6) NULL,
              ADD CONSTRAINT `FK_CustomerOrder_DeliveryDriver` FOREIGN KEY (`DeliveryDriverId`) REFERENCES `deliverydriver` (`Id`);
            """);
    }
    protected override void Down(MigrationBuilder b)
    {
        b.Sql("""
            ALTER TABLE `customerorder` DROP FOREIGN KEY `FK_CustomerOrder_DeliveryDriver`,
              DROP COLUMN `DeliveryDriverId`, DROP COLUMN `DeliveryFeeAmount`, DROP COLUMN `DeliveryDistanceKm`,
              DROP COLUMN `DeliveryPricePerKm`, DROP COLUMN `DeliveryDailyAmount`, DROP COLUMN `DeliveryPaymentModel`,
              DROP COLUMN `DeliveryTimeZoneId`, DROP COLUMN `DeliveryFeeCalculatedAt`;
            DROP TABLE `deliverydriverdailypayment`;
            DROP TABLE `deliveryfeecondition`;
            DROP TABLE `deliveryfeeconfig`;
            DROP TABLE `deliverydriver`;
            """);
    }
}
