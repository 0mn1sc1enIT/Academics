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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250427085157_AddNullableValues'
)
BEGIN
    CREATE TABLE [Positions] (
        [Id] int NOT NULL IDENTITY,
        [CreatedAt] datetime2 NULL,
        [CreatedBy] nvarchar(max) NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_Positions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250427085157_AddNullableValues'
)
BEGIN
    CREATE TABLE [Teachers] (
        [Id] int NOT NULL IDENTITY,
        [CreatedAt] datetime2 NULL,
        [CreatedBy] nvarchar(max) NULL,
        [FullName] nvarchar(max) NOT NULL,
        [Photo] varbinary(max) NULL,
        [Description] nvarchar(max) NULL,
        [PositionId] int NOT NULL,
        CONSTRAINT [PK_Teachers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Teachers_Positions_PositionId] FOREIGN KEY ([PositionId]) REFERENCES [Positions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250427085157_AddNullableValues'
)
BEGIN
    CREATE TABLE [Courses] (
        [Id] int NOT NULL IDENTITY,
        [TeacherId] int NOT NULL,
        [Hours] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [CourseTitle] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Stars] int NOT NULL,
        CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Courses_Teachers_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [Teachers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250427085157_AddNullableValues'
)
BEGIN
    CREATE INDEX [IX_Courses_TeacherId] ON [Courses] ([TeacherId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250427085157_AddNullableValues'
)
BEGIN
    CREATE INDEX [IX_Teachers_PositionId] ON [Teachers] ([PositionId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250427085157_AddNullableValues'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250427085157_AddNullableValues', N'8.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250428173545_addedImages'
)
BEGIN
    ALTER TABLE [Courses] ADD [Category] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250428173545_addedImages'
)
BEGIN
    ALTER TABLE [Courses] ADD [Image] varbinary(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250428173545_addedImages'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250428173545_addedImages', N'8.0.0');
END;
GO

COMMIT;
GO

