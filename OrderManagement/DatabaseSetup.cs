using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace OrderManagement
{
    public static class DatabaseSetup
    {
        public static void EnsureTablesExist()
        {
            try
            {
                // Check if OrderItems table exists
                if (!TableExists("OrderItems"))
                {
                    CreateOrderItemsTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting up database tables: " + ex.Message,
                    "Database Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool TableExists(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(MainClass.conString))
            {
                connection.Open();
                string query = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = @TableName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private static void CreateOrderItemsTable()
        {
            string sql = @"
                CREATE TABLE [dbo].[OrderItems] (
                    [OrderItemId]  INT             IDENTITY (1, 1) NOT NULL,
                    [OrderId]      INT             NOT NULL,
                    [ItemId]       INT             NOT NULL,
                    [ItemName]     VARCHAR (100)   NOT NULL,
                    [ItemPrice]    DECIMAL (18, 2) NOT NULL,
                    [Quantity]     INT             NOT NULL DEFAULT 1,
                    [DateAdded]    DATETIME        NOT NULL DEFAULT GETDATE(),
                    PRIMARY KEY CLUSTERED ([OrderItemId] ASC),
                    CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([OrderId]),
                    CONSTRAINT [FK_OrderItems_FoodItems] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[foodItems] ([Id])
                );";

            using (SqlConnection connection = new SqlConnection(MainClass.conString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
