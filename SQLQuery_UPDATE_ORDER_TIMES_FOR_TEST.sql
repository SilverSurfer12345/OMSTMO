WITH CTE AS (
    SELECT 
        OrderId, 
        OrderType, 
        DesiredCompletionTime, 
        ROW_NUMBER() OVER(PARTITION BY OrderType ORDER BY OrderId) AS RowNum,
        COUNT(*) OVER(PARTITION BY OrderType) AS TotalCount
    FROM 
        Orders
)
UPDATE CTE
SET DesiredCompletionTime = 
    CASE 
        WHEN OrderType = 'Collection' THEN
            CASE 
                WHEN RowNum <= 10 THEN DATEADD(MINUTE, 0, GETDATE())
                WHEN RowNum < TotalCount THEN DATEADD(MINUTE, 16, GETDATE())
                ELSE DATEADD(MINUTE, 21, GETDATE())
            END
        WHEN OrderType = 'Delivery' THEN
            CASE 
                WHEN RowNum <= 3 THEN DATEADD(MINUTE, 46, GETDATE())
                WHEN RowNum < TotalCount THEN DATEADD(MINUTE, 0, GETDATE())
                ELSE DATEADD(MINUTE, 60, GETDATE())
            END
    END