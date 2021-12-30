﻿CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
begin
	set nocount on;

	select prod.Id,prod.ProductName, prod.[Description], prod.RetailPrice, prod.QuantityInStock
	from dbo.Product as prod
	order by ProductName;
end