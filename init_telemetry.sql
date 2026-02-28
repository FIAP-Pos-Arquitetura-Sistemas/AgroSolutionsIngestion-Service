IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [TelemetriaSensores] (
    [Id] int NOT NULL IDENTITY,
    [SensorId] nvarchar(max) NOT NULL,
    [TalhaoId] int NOT NULL,
    [Umidade] decimal(5,2) NOT NULL,
    [Temperatura] decimal(5,2) NOT NULL,
    [Precipitacao] decimal(5,2) NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    CONSTRAINT [PK_TelemetriaSensores] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260228010449_InitialTelemetry', N'8.0.24');
GO

COMMIT;
GO

