CREATE TABLE [dbo].[Orders] (
    [OrderId]       INT           IDENTITY (1, 1) NOT NULL,
    [CustomerId]    INT           NOT NULL,
    [OrderType]     VARCHAR (20)  NOT NULL,
    [OrderDate]     DATETIME      NOT NULL DEFAULT GETDATE(),
    [TotalPrice]    DECIMAL (18, 2) NOT NULL,
    [DesiredCompletionTime] TIME,
    -- Add other order-related fields as needed
    PRIMARY KEY CLUSTERED ([OrderId] ASC),
    CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id])
);