

CREATE TABLE [dbo].[Fiskepladser]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(MAX) NOT NULL,
	[Description] NVARCHAR(MAX) NULL
)

INSERT INTO [dbo].[Fiskepladser] (Id, Name, Description) VALUES (1, 'Vrinners', 'Dejligt ude ved Vrinners.'),
INSERT INTO [dbo].[Fiskepladser] (Id, Name, Description) VALUES (2, 'Knebel', 'Flot natur. Mulighed for at fiske fra kysten uden vaders. Ligger dejlig isoleret.')
INSERT INTO [dbo].[Fiskepladser] (Id, Name, Description) VALUES (3, 'Vosnæs', 'Optimalt hvis man bor i Aarhus og ikke ville køre så langt for en fin natur.')