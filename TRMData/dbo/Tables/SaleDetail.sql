CREATE TABLE [dbo].[SaleDetail]
(
	[SaleId] INT NOT NULL PRIMARY KEY,
	[ProductID] int not null, 
    [Quantity] INT NOT NULL DEFAULT 1, 
    [PurchasePrice] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL DEFAULT 0
)
