CREATE PROCEDURE [dbo].[spSaleDetail_Insert]
	@SaleId int,
	@ProductId int,
	@Quantity int,
	@PurchasePrice money,
	@Tax money
AS
begin
	set nocount on;

	insert into dbo.SaleDetail(ProductID,PurchasePrice,Quantity,SaleId,Tax)
	values (@ProductID,@PurchasePrice,@Quantity,@SaleId,@Tax);
end
