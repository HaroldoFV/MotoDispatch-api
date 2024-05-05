CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory"
(
    "MigrationId"    character varying(150) NOT NULL,
    "ProductVersion" character varying(32)  NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240430172535_CreateMotorcyleTable') THEN
            CREATE TABLE "Motorcycles"
            (
                "Id"           uuid                     NOT NULL,
                "Year"         integer                  NOT NULL,
                "LicensePlate" character varying(10)    NOT NULL,
                "Model"        character varying(255)   NOT NULL,
                "CreatedAt"    timestamp with time zone NOT NULL,
                CONSTRAINT "PK_Motorcycles" PRIMARY KEY ("Id")
            );
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240430172535_CreateMotorcyleTable') THEN
            INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
            VALUES ('20240430172535_CreateMotorcyleTable', '8.0.4');
        END IF;
    END
$EF$;
COMMIT;

START TRANSACTION;


DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240501124144_CreateDeliveryDriverTable') THEN
            CREATE TABLE "DeliveryDrivers"
            (
                "Id"           uuid                     NOT NULL,
                "Name"         character varying(100)   NOT NULL,
                "CNPJ"         character varying(14)    NOT NULL,
                "DateOfBirth"  timestamp with time zone NOT NULL,
                "CNHNumber"    character varying(11)    NOT NULL,
                "CNHType"      integer                  NOT NULL,
                "CNHImagePath" text,
                "CreatedAt"    timestamp with time zone NOT NULL,
                CONSTRAINT "PK_DeliveryDrivers" PRIMARY KEY ("Id")
            );
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240501124144_CreateDeliveryDriverTable') THEN
            CREATE UNIQUE INDEX "IX_DeliveryDrivers_CNHNumber" ON "DeliveryDrivers" ("CNHNumber");
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240501124144_CreateDeliveryDriverTable') THEN
            CREATE UNIQUE INDEX "IX_DeliveryDrivers_CNPJ" ON "DeliveryDrivers" ("CNPJ");
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240501124144_CreateDeliveryDriverTable') THEN
            INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
            VALUES ('20240501124144_CreateDeliveryDriverTable', '8.0.4');
        END IF;
    END
$EF$;
COMMIT;

START TRANSACTION;


DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503011117_CreateRentalTable') THEN
            CREATE TABLE "Rentals"
            (
                "Id"                       uuid                     NOT NULL,
                "MotorcycleId"             uuid                     NOT NULL,
                "DeliveryDriverId"         uuid                     NOT NULL,
                "StartDate"                timestamp with time zone NOT NULL,
                "EndDate"                  timestamp with time zone NOT NULL,
                "ActualEndDate"            timestamp with time zone,
                "Plan_Days"                integer                  NOT NULL,
                "Plan_DailyRate"           numeric                  NOT NULL,
                "Plan_PenaltyRate"         numeric                  NOT NULL,
                "Plan_AdditionalDailyRate" numeric                  NOT NULL,
                "CreatedAt"                timestamp with time zone NOT NULL,
                CONSTRAINT "PK_Rentals" PRIMARY KEY ("Id"),
                CONSTRAINT "FK_Rentals_DeliveryDrivers_DeliveryDriverId" FOREIGN KEY ("DeliveryDriverId") REFERENCES "DeliveryDrivers" ("Id") ON DELETE RESTRICT,
                CONSTRAINT "FK_Rentals_Motorcycles_MotorcycleId" FOREIGN KEY ("MotorcycleId") REFERENCES "Motorcycles" ("Id") ON DELETE RESTRICT
            );
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503011117_CreateRentalTable') THEN
            CREATE INDEX "IX_Rentals_DeliveryDriverId" ON "Rentals" ("DeliveryDriverId");
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503011117_CreateRentalTable') THEN
            CREATE INDEX "IX_Rentals_MotorcycleId" ON "Rentals" ("MotorcycleId");
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503011117_CreateRentalTable') THEN
            INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
            VALUES ('20240503011117_CreateRentalTable', '8.0.4');
        END IF;
    END
$EF$;
COMMIT;

START TRANSACTION;


DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            ALTER TABLE "Rentals"
                DROP COLUMN "Plan_AdditionalDailyRate";
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            ALTER TABLE "Rentals"
                DROP COLUMN "Plan_DailyRate";
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            ALTER TABLE "Rentals"
                DROP COLUMN "Plan_Days";
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            ALTER TABLE "Rentals"
                DROP COLUMN "Plan_PenaltyRate";
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            ALTER TABLE "Rentals"
                ADD "RentalPlanId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            CREATE TABLE "RentalPlans"
            (
                "Id"                  uuid           NOT NULL,
                "Days"                integer        NOT NULL,
                "DailyRate"           numeric(18, 2) NOT NULL,
                "PenaltyRate"         numeric(18, 2) NOT NULL,
                "AdditionalDailyRate" numeric(18, 2) NOT NULL,
                "FixedAdditionalRate" numeric(18, 2) NOT NULL,
                CONSTRAINT "PK_RentalPlans" PRIMARY KEY ("Id")
            );
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            CREATE INDEX "IX_Rentals_RentalPlanId" ON "Rentals" ("RentalPlanId");
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            ALTER TABLE "Rentals"
                ADD CONSTRAINT "FK_Rentals_RentalPlans_RentalPlanId" FOREIGN KEY ("RentalPlanId") REFERENCES "RentalPlans" ("Id") ON DELETE CASCADE;
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503032329_CreateRentalPlan') THEN
            INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
            VALUES ('20240503032329_CreateRentalPlan', '8.0.4');
        END IF;
    END
$EF$;
COMMIT;

START TRANSACTION;


DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503122416_AjustePKRentalPlan') THEN
            INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
            VALUES ('20240503122416_AjustePKRentalPlan', '8.0.4');
        END IF;
    END
$EF$;
COMMIT;

START TRANSACTION;


DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503233918_RetirarAdditionalDailyRate') THEN
            ALTER TABLE "RentalPlans"
                DROP COLUMN "AdditionalDailyRate";
        END IF;
    END
$EF$;

DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1
                      FROM "__EFMigrationsHistory"
                      WHERE "MigrationId" = '20240503233918_RetirarAdditionalDailyRate') THEN
            INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
            VALUES ('20240503233918_RetirarAdditionalDailyRate', '8.0.4');
        END IF;
    END
$EF$;

-- Criação da tabela RentalPlans se ela não existir
DO
$EF$
    BEGIN
        IF NOT EXISTS(SELECT 1 FROM "RentalPlans") THEN
            -- Inserção de dados padrões na tabela RentalPlans
            INSERT INTO "RentalPlans" ("Id", "Days", "DailyRate", "PenaltyRate", "FixedAdditionalRate")
            VALUES ('550e8400-e29b-41d4-a716-446655440000', 7, 30.00, 0.20, 50.00),  -- Plano de 7 dias
                   ('550e8400-e29b-41d4-a716-446655440001', 15, 28.00, 0.40, 50.00), -- Plano de 15 dias
                   ('550e8400-e29b-41d4-a716-446655440002', 30, 22.00, 0.00, 50.00), -- Plano de 30 dias
                   ('550e8400-e29b-41d4-a716-446655440003', 45, 20.00, 0.00, 50.00), -- Plano de 45 dias
                   ('550e8400-e29b-41d4-a716-446655440004', 50, 18.00, 0.00, 50.00); -- Plano de 50 dias
        END IF;
    END
$EF$;
COMMIT;

