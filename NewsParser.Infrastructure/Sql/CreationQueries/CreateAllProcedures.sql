USE [NewsParserProductionDb]
GO
DROP PROCEDURE IF EXISTS [dbo].[get_posts_by_date_range]
GO
CREATE PROCEDURE [dbo].[get_posts_by_date_range]
(
    @start_date [datetime2](7),
    @end_date [datetime2](7)
)
AS
BEGIN
SELECT * FROM [dbo].[Posts]
WHERE [DateGmt] BETWEEN @start_date AND @end_date
END
GO

GO
DROP PROCEDURE IF EXISTS [dbo].[get_posts_with_search_phrase]
GO
CREATE PROCEDURE [dbo].[get_posts_with_search_phrase]
(
    @search_phrase NVARCHAR(1024)
)
AS
BEGIN
DECLARE @queryString as nvarchar(1024);
SELECT
    @queryString = ISNULL(STRING_AGG('"' + value + '*"', ' AND '), '""')
FROM
    STRING_SPLIT(@search_phrase, ' ');
SELECT * FROM [dbo].[Posts]
where CONTAINS([Content], @queryString)
END

GO
DROP PROCEDURE IF EXISTS [dbo].[get_top_words]
GO
CREATE PROCEDURE [dbo].[get_top_words](
    @top_count INT
)
AS
BEGIN
SELECT TOP(@top_count) word, COUNT(*) as word_count
FROM (SELECT LOWER(Content) as string
      FROM [dbo].[Posts]) t
    CROSS APPLY (
  SELECT value as word
  FROM STRING_SPLIT(t.string, ' ')
) s
GROUP BY word
ORDER BY word_count DESC
END

GO
DROP PROCEDURE IF EXISTS [dbo].[get_user_by_email]
    GO
CREATE PROCEDURE get_user_by_email
(
    @email nvarchar(255)
)
AS
BEGIN
SET NOCOUNT ON;

SELECT TOP 1 Id, FirstName, LastName, Email, [Password]
FROM [Users]
WHERE Email = @email
END

GO
DROP PROCEDURE IF EXISTS [dbo].[add_user]
GO
CREATE PROCEDURE add_user
(
    @firstname nvarchar(255),
    @lastname nvarchar(255),
    @email nvarchar(max),
    @password nvarchar(255)
)
AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [Users] (FirstName, LastName, Email, [Password])
VALUES (@firstname, @lastname, @email, @password)
SELECT SCOPE_IDENTITY()
END
