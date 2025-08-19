CREATE TABLE [Etats] (
    [Id] int NOT NULL IDENTITY,
    [Libelle] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Etats] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Techniciens] (
    [Id] int NOT NULL IDENTITY,
    [Nom] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Telephone] nvarchar(max) NOT NULL,
    [Specialite] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Techniciens] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [Username] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    [UserType] nvarchar(21) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Articles] (
    [Id] int NOT NULL IDENTITY,
    [Libelle] nvarchar(max) NOT NULL,
    [EstSousGarantie] bit NOT NULL,
    [Prix] decimal(18,2) NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [DateAchat] datetime2 NULL,
    [ClientId] int NULL,
    CONSTRAINT [PK_Articles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Articles_Users_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Users] ([Id])
);
GO


CREATE TABLE [PiecesRechange] (
    [Id] int NOT NULL IDENTITY,
    [Nom] nvarchar(max) NOT NULL,
    [Prix] decimal(18,2) NOT NULL,
    [ArticleId] int NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    CONSTRAINT [PK_PiecesRechange] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PiecesRechange_Articles_ArticleId] FOREIGN KEY ([ArticleId]) REFERENCES [Articles] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Reclamations] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    [DateReclamation] datetime2 NOT NULL,
    [idArticleReclamation] int NOT NULL,
    [EtatId] int NOT NULL,
    [ClientId] int NOT NULL,
    [InterventionId] int NULL,
    CONSTRAINT [PK_Reclamations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reclamations_Articles_idArticleReclamation] FOREIGN KEY ([idArticleReclamation]) REFERENCES [Articles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reclamations_Etats_EtatId] FOREIGN KEY ([EtatId]) REFERENCES [Etats] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Reclamations_Users_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);
GO


CREATE TABLE [Interventions] (
    [Id] int NOT NULL IDENTITY,
    [DateIntervention] datetime2 NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Prix] decimal(18,2) NOT NULL,
    [MontantFacture] decimal(18,2) NOT NULL,
    [TechnicienId] int NOT NULL,
    [ReclamationId] int NOT NULL,
    CONSTRAINT [PK_Interventions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Interventions_Reclamations_ReclamationId] FOREIGN KEY ([ReclamationId]) REFERENCES [Reclamations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Interventions_Techniciens_TechnicienId] FOREIGN KEY ([TechnicienId]) REFERENCES [Techniciens] ([Id]) ON DELETE NO ACTION
);
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Libelle') AND [object_id] = OBJECT_ID(N'[Etats]'))
    SET IDENTITY_INSERT [Etats] ON;
INSERT INTO [Etats] ([Id], [Libelle])
VALUES (1, N'En Attente'),
(2, N'Traité'),
(3, N'En Cours');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Libelle') AND [object_id] = OBJECT_ID(N'[Etats]'))
    SET IDENTITY_INSERT [Etats] OFF;
GO


CREATE INDEX [IX_Articles_ClientId] ON [Articles] ([ClientId]);
GO


CREATE UNIQUE INDEX [IX_Interventions_ReclamationId] ON [Interventions] ([ReclamationId]);
GO


CREATE INDEX [IX_Interventions_TechnicienId] ON [Interventions] ([TechnicienId]);
GO


CREATE INDEX [IX_PiecesRechange_ArticleId] ON [PiecesRechange] ([ArticleId]);
GO


CREATE INDEX [IX_Reclamations_ClientId] ON [Reclamations] ([ClientId]);
GO


CREATE INDEX [IX_Reclamations_EtatId] ON [Reclamations] ([EtatId]);
GO


CREATE INDEX [IX_Reclamations_idArticleReclamation] ON [Reclamations] ([idArticleReclamation]);
GO


