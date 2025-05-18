SET IDENTITY_INSERT [dbo].[customers] ON
INSERT INTO [dbo].[customers] ([Id], [forename], [surname], [telephoneNo], [houseNameNumber], [AddressLine1], [AddressLine2], [AddressLine3], [AddressLine4], [Postcode], [previousOrders]) VALUES (2, N'John', N'Doe', 1912730073, N'123       ', N'Alphabet Street     ', N'Fenham              ', N'Newcastle Upon Tyne ', N'          ', N'NE3 3DE   ', 2)
SET IDENTITY_INSERT [dbo].[customers] OFF
