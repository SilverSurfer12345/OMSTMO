CREATE TABLE [dbo].[Customer]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [forename] VARCHAR(50) NULL, 
    [surname] VARBINARY(50) NULL, 
    [telephoneNo] INT NULL, 
    [houseNameNumber] NCHAR(10) NULL, 
    [AddressLine1] NCHAR(20) NULL, 
    [AddressLine2] NCHAR(20) NULL, 
    [AddressLine3] NCHAR(20) NULL, 
    [AddressLine4] NCHAR(10) NULL, 
    [Postcode] NCHAR(10) NULL, 
    [previousOrders] INT NULL
)