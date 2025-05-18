CREATE TABLE [dbo].[users]
(
	[userID] INT NOT NULL PRIMARY KEY, 
    [username] VARCHAR(50) NOT NULL, 
    [upass] VARCHAR(10) NOT NULL, 
    [uName] VARCHAR(50) NOT NULL, 
    [uphone] VARCHAR(20) NULL
)
