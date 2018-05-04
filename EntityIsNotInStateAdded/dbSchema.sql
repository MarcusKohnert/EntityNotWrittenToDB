CREATE TABLE [dbo].[Persons] (
    [Id]   INT IDENTITY,
    [Name] VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED ([Id] ASC),
);

CREATE TABLE [dbo].[Attributes]
(
	[PersonId] INT NOT NULL,
	[Attributes] VARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_Attributes] PRIMARY KEY CLUSTERED ([PersonId] ASC),
	CONSTRAINT [FK_Attributes_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Persons] ([Id])
)

INSERT INTO [Persons] (Name) VALUES ('Some 1')
INSERT INTO [Persons] (Name) VALUES ('Some 2')
INSERT INTO [Persons] (Name) VALUES ('Some 3')
INSERT INTO [Persons] (Name) VALUES ('Some 4')
INSERT INTO [Persons] (Name) VALUES ('Some 5')
INSERT INTO [Persons] (Name) VALUES ('Some 6')

INSERT INTO [Attributes] (PersonId, Attributes) VALUES (1, 'some attributes')