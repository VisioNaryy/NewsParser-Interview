USE master;
GO
    
DROP DATABASE IF EXISTS NewsParserProductionDb;
GO
    
CREATE DATABASE NewsParserProductionDb
GO

USE [NewsParserProductionDb]

SET ANSI_NULLS ON
GO
    
SET QUOTED_IDENTIFIER ON
GO
    
DROP TABLE IF EXISTS [dbo].[Posts]
GO

CREATE TABLE [dbo].[Posts](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Link] [nvarchar](max) NOT NULL,
    [Title] [nvarchar](max) NOT NULL,
    [Content] [nvarchar](max) NOT NULL,
    [DateGmt] [datetime2](7) NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE [name] = 'PostsFT')
CREATE FULLTEXT CATALOG PostsFT
    
DECLARE @value INT;
SELECT @value = COLUMNPROPERTY(OBJECT_ID('[dbo].[Posts]'), 'Content', 'IsFulltextIndexed')
           IF (@value = 0)
CREATE FULLTEXT INDEX ON [dbo].[Posts] (Link, Title, Content)
        KEY INDEX PK_Posts ON PostsFT
        WITH CHANGE_TRACKING AUTO

GO
IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'idx_date')
CREATE INDEX idx_date ON Posts (DateGmt);
    
DROP TABLE IF EXISTS [dbo].[Users]
GO

CREATE TABLE [dbo].[Users](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] [nvarchar](255) NOT NULL,
    [LastName] [nvarchar](255) NOT NULL,
    [Email] [nvarchar](max) NOT NULL,
    [Password] [nvarchar](255) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Email]
    