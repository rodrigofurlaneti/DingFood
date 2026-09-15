-- Tabela de snapshots da consulta pública de CNPJ (CNPJá / Receita Federal).
-- Global por natureza: dados públicos não pertencem a nenhum tenant, portanto sem CompanyId
-- e sem FK para company. Script idempotente, no mesmo padrão de IfoodWebhook.sql.

CREATE TABLE IF NOT EXISTS `cnpjquery` (
  `Id`                bigint        NOT NULL AUTO_INCREMENT,

  -- Identificação
  `TaxId`             char(14)      NOT NULL,
  `LegalName`         varchar(250)  NOT NULL,
  `TradeName`         varchar(250)  NULL,
  `FoundedOn`         date          NULL,
  `IsHeadOffice`      bit(1)        NOT NULL DEFAULT b'0',

  -- Situação cadastral
  `StatusId`          int           NULL,
  `StatusText`        varchar(60)   NULL,
  `StatusDate`        date          NULL,
  `ReasonText`        varchar(200)  NULL,

  -- Natureza jurídica e porte
  `NatureId`          int           NULL,
  `NatureText`        varchar(150)  NULL,
  `SizeAcronym`       varchar(10)   NULL,
  `SizeText`          varchar(60)   NULL,
  `Equity`            decimal(18,2) NULL,

  -- Simples Nacional / MEI
  `SimplesOptant`     bit(1)        NULL,
  `SimplesSince`      date          NULL,
  `SimeiOptant`       bit(1)        NULL,
  `SimeiSince`        date          NULL,

  -- Atividade principal (CNAE)
  `MainActivityCode`  int           NULL,
  `MainActivityText`  varchar(300)  NULL,

  -- Endereço
  `AddressStreet`     varchar(200)  NULL,
  `AddressNumber`     varchar(30)   NULL,
  `AddressDetails`    varchar(150)  NULL,
  `AddressDistrict`   varchar(150)  NULL,
  `AddressCity`       varchar(150)  NULL,
  `AddressState`      char(2)       NULL,
  `AddressZip`        char(8)       NULL,
  `MunicipalityCode`  int           NULL,

  -- Contato principal
  `PrimaryPhone`      varchar(20)   NULL,
  `PrimaryEmail`      varchar(150)  NULL,

  -- Payload e controle
  `SourceUpdatedAt`   datetime(6)   NULL,
  `RawJson`           longtext      NOT NULL,
  `QueriedAt`         datetime(6)   NOT NULL,
  `CreatedAt`         datetime(6)   NOT NULL,
  `UpdatedAt`         datetime(6)   NULL,
  `IsActive`          bit(1)        NOT NULL DEFAULT b'1',

  PRIMARY KEY (`Id`),
  UNIQUE KEY `UX_CnpjQuery_TaxId` (`TaxId`),
  KEY `IX_CnpjQuery_QueriedAt` (`QueriedAt`),
  KEY `IX_CnpjQuery_StatusId` (`StatusId`),
  KEY `IX_CnpjQuery_AddressState` (`AddressState`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
