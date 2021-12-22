CREATE PROCEDURE [dbo].[spUserLookUp]
	@Id nvarchar(128) = 0
AS
BEGIN
	set nocount on;

	SELECT Id, FirstName, LastName, EmailAddress, CreatedDate
	FROM [dbo].[User]
	where Id = @Id
END
