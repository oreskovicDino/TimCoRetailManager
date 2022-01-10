CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int
AS
begin
  set nocount on;

  select prod.Id,prod.ProductName, prod.[Description], prod.RetailPrice, prod.QuantityInStock, prod.IsTaxable
	from dbo.Product as prod
	where Id = @Id
end
