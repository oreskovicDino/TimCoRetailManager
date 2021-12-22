CREATE TABLE [dbo].[Sale]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[CashierId] NVARCHAR(128) NOT NULL,
	[SaleDate] datetime2(7) NOT NULL,
	[SubTotal] money NOT NULL,
	[Tax] money NOT NULL,
	[Total] money NOT NULL
)
