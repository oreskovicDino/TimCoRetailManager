CREATE TABLE [dbo].[SaleDetail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[SaleId] INT NOT NULL,
	[ProductID] int not null, 
    [Quantity] INT NOT NULL DEFAULT 1, 
    [PurchasePrice] MONEY NOT NULL, 
    [Tax] MONEY NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_SaleDetail_ToSale] FOREIGN KEY (SaleId) REFERENCES Sale(Id), 
    CONSTRAINT [FK_SaleDetail_ToProduct] FOREIGN KEY (ProductID) REFERENCES Product(Id)
)
