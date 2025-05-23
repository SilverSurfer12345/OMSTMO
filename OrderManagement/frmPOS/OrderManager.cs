// OrderManager.cs (Conceptual)
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OrderManagement.Model
{
    public class OrderManager
    {
        // DTOs (Data Transfer Objects)
        public class OrderDto
        {
            public int OrderId { get; set; }
            public int CustomerId { get; set; }
            public string OrderType { get; set; }
            public decimal TotalPrice { get; set; }
            public DateTime OrderDate { get; set; }
            public string PaymentType { get; set; }
            public string Address { get; set; }
            public decimal DeliveryCharge { get; set; }
            public decimal DiscountRate { get; set; }
            public string DiscountLabel { get; set; }
            public string Forename { get; set; } // Joined from Customer
            public string Surname { get; set; }  // Joined from Customer
            public string TelephoneNo { get; set; } // Joined from Customer
            public string Email { get; set; }
            public string HouseNameNumber { get; set; }
            public string AddressLine1 { get; set; }
            public string AddressLine2 { get; set; }
            public string AddressLine3 { get; set; }
            public string AddressLine4 { get; set; }
            public string Postcode { get; set; }
        }

        public class OrderItemDto
        {
            public string ItemName { get; set; }
            public decimal ItemPrice { get; set; }
            public int Quantity { get; set; }
            public decimal ExtraCharge { get; set; }
        }

        public class FoodItemDto
        {
            public string Item { get; set; }
            public decimal Price { get; set; }
            public byte[] Icon { get; set; }
            public string Category { get; set; }
        }

        public class PreviousOrderItemDto
        {
            public string ItemName { get; set; }
            public decimal ItemPrice { get; set; }
            public int Quantity { get; set; }
            public byte[] ItemImage { get; set; } // Assuming you store image for food items
        }

        // --- Methods for Order Management ---

        public OrderDto GetOrderDetails(int orderId)
        {
            string query = @"
                SELECT O.*, C.forename, C.surname, C.telephoneNo, C.Email, C.houseNameNumber,
                       C.AddressLine1, C.AddressLine2, C.AddressLine3, C.AddressLine4, C.Postcode
                FROM Orders O
                JOIN Customers C ON O.CustomerId = C.Id
                WHERE O.OrderId = @OrderId";
            var parameters = new Dictionary<string, object> { { "@OrderId", orderId } };
            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new OrderDto
                {
                    OrderId = SafeOperations.SafeGetInt(row, "OrderId"),
                    CustomerId = SafeOperations.SafeGetInt(row, "CustomerId"),
                    OrderType = SafeOperations.SafeGetString(row, "OrderType"),
                    TotalPrice = SafeOperations.SafeGetDecimal(row, "TotalPrice"),
                    OrderDate = SafeOperations.SafeGetDateTime(row, "OrderDate"),
                    PaymentType = SafeOperations.SafeGetString(row, "PaymentType"),
                    Address = SafeOperations.SafeGetString(row, "Address"),
                    DeliveryCharge = SafeOperations.SafeGetDecimal(row, "DeliveryCharge"),
                    DiscountRate = SafeOperations.SafeGetDecimal(row, "DiscountRate"),
                    DiscountLabel = SafeOperations.SafeGetString(row, "DiscountLabel"),
                    Forename = SafeOperations.SafeGetString(row, "forename"),
                    Surname = SafeOperations.SafeGetString(row, "surname"),
                    TelephoneNo = SafeOperations.SafeGetString(row, "telephoneNo"),
                    Email = SafeOperations.SafeGetString(row, "Email"),
                    HouseNameNumber = SafeOperations.SafeGetString(row, "houseNameNumber"),
                    AddressLine1 = SafeOperations.SafeGetString(row, "AddressLine1"),
                    AddressLine2 = SafeOperations.SafeGetString(row, "AddressLine2"),
                    AddressLine3 = SafeOperations.SafeGetString(row, "AddressLine3"),
                    AddressLine4 = SafeOperations.SafeGetString(row, "AddressLine4"),
                    Postcode = SafeOperations.SafeGetString(row, "Postcode")
                };
            }
            return null;
        }

        public List<OrderItemDto> GetOrderItems(int orderId)
        {
            string query = "SELECT ItemName, ItemPrice, Quantity, ExtraCharge FROM OrderItems WHERE OrderId = @OrderId";
            var parameters = new Dictionary<string, object> { { "@OrderId", orderId } };
            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);
            List<OrderItemDto> items = new List<OrderItemDto>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new OrderItemDto
                    {
                        ItemName = SafeOperations.SafeGetString(row, "ItemName"),
                        ItemPrice = SafeOperations.SafeGetDecimal(row, "ItemPrice"),
                        Quantity = SafeOperations.SafeGetInt(row, "Quantity"),
                        ExtraCharge = SafeOperations.SafeGetDecimal(row, "ExtraCharge")
                    });
                }
            }
            return items;
        }

        public int SaveNewOrder(int customerId, string orderType, decimal totalPrice, DateTime orderDate, string paymentType, string address, decimal deliveryCharge, decimal presetCharges, decimal discountRate, string discountLabel)
        {
            // Original MainClass.SaveOrder logic moved here
            string query = @"
                INSERT INTO Orders (CustomerId, OrderType, TotalPrice, OrderDate, PaymentType, Address, DeliveryCharge, PresetCharges, DiscountRate, DiscountLabel, completion)
                VALUES (@CustomerId, @OrderType, @TotalPrice, @OrderDate, @PaymentType, @Address, @DeliveryCharge, @PresetCharges, @DiscountRate, @DiscountLabel, 'no');
                SELECT SCOPE_IDENTITY();";
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerId", customerId }, { "@OrderType", orderType }, { "@TotalPrice", totalPrice },
                { "@OrderDate", orderDate }, { "@PaymentType", paymentType }, { "@Address", address },
                { "@DeliveryCharge", deliveryCharge }, { "@PresetCharges", presetCharges },
                { "@DiscountRate", discountRate }, { "@DiscountLabel", discountLabel }
            };
            object result = DatabaseManager.ExecuteScalar(query, parameters);
            return (result == null || result == DBNull.Value) ? -1 : Convert.ToInt32(result);
        }

        public void UpdateOrder(int orderId, int customerId, string orderType, decimal totalPrice, string paymentType, string address, decimal deliveryCharge, decimal discountRate, string discountLabel)
        {
            string query = @"
                UPDATE Orders SET
                    CustomerId = @CustomerId, OrderType = @OrderType, TotalPrice = @TotalPrice,
                    PaymentType = @PaymentType, Address = @Address, DeliveryCharge = @DeliveryCharge,
                    DiscountRate = @DiscountRate, DiscountLabel = @DiscountLabel
                WHERE OrderId = @OrderId";
            var parameters = new Dictionary<string, object>
            {
                { "@CustomerId", customerId }, { "@OrderType", orderType }, { "@TotalPrice", totalPrice },
                { "@PaymentType", paymentType }, { "@Address", address }, { "@DeliveryCharge", deliveryCharge },
                { "@DiscountRate", discountRate }, { "@DiscountLabel", discountLabel }, { "@OrderId", orderId }
            };
            DatabaseManager.ExecuteNonQuery(query, parameters);
        }

        public void DeleteOrderItems(int orderId)
        {
            string query = "DELETE FROM OrderItems WHERE OrderId = @OrderId";
            var parameters = new Dictionary<string, object> { { "@OrderId", orderId } };
            DatabaseManager.ExecuteNonQuery(query, parameters);
        }

        public int SaveOrderItem(int orderId, string itemName, decimal itemPrice, int quantity, decimal extraCharge)
        {
            // Original MainClass.SaveOrderItem logic moved here
            string query = @"
                INSERT INTO OrderItems (OrderId, ItemName, ItemPrice, Quantity, ExtraCharge, DateAdded)
                VALUES (@OrderId, @ItemName, @ItemPrice, @Quantity, @ExtraCharge, GETDATE());
                SELECT SCOPE_IDENTITY();";
            var parameters = new Dictionary<string, object>
            {
                { "@OrderId", orderId }, { "@ItemName", itemName }, { "@ItemPrice", itemPrice },
                { "@Quantity", quantity }, { "@ExtraCharge", extraCharge }
            };
            object result = DatabaseManager.ExecuteScalar(query, parameters);
            return (result == null || result == DBNull.Value) ? -1 : Convert.ToInt32(result);
        }

        public decimal GetFoodItemPrice(string itemName)
        {
            string query = "SELECT price FROM foodItems WHERE Item = @ItemName";
            var parameters = new Dictionary<string, object> { { "@ItemName", itemName } };
            object result = DatabaseManager.ExecuteScalar(query, parameters);
            return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0m;
        }

        public List<string> GetAllCategories()
        {
            string query = "SELECT DISTINCT category FROM foodItems ORDER BY category";
            DataTable dt = DatabaseManager.ExecuteQuery(query);
            return dt.AsEnumerable().Select(row => SafeOperations.SafeGetString(row, "category")).ToList();
        }

        public List<FoodItemDto> GetFoodItemsByCategory(string category)
        {
            string query = "SELECT Id, Item, price, icon, category FROM foodItems WHERE category = @category ORDER BY Item";
            var parameters = new Dictionary<string, object> { { "@category", category } };
            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);
            List<FoodItemDto> items = new List<FoodItemDto>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new FoodItemDto
                    {
                        Item = SafeOperations.SafeGetString(row, "Item"),
                        Price = SafeOperations.SafeGetDecimal(row, "price"),
                        Icon = row["icon"] != DBNull.Value ? (byte[])row["icon"] : null,
                        Category = SafeOperations.SafeGetString(row, "category")
                    });
                }
            }
            return items;
        }

        public List<FoodItemDto> SearchFoodItems(string searchText)
        {
            string query = "SELECT Id, Item, price, icon, category FROM foodItems WHERE Item LIKE @searchText ORDER BY Item";
            var parameters = new Dictionary<string, object> { { "@searchText", "%" + searchText + "%" } };
            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);
            List<FoodItemDto> items = new List<FoodItemDto>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new FoodItemDto
                    {
                        Item = SafeOperations.SafeGetString(row, "Item"),
                        Price = SafeOperations.SafeGetDecimal(row, "price"),
                        Icon = row["icon"] != DBNull.Value ? (byte[])row["icon"] : null,
                        Category = SafeOperations.SafeGetString(row, "category")
                    });
                }
            }
            return items;
        }

        public List<PreviousOrderItemDto> GetPreviousOrderedItems(int customerId)
        {
            string query = @"
                SELECT OI.ItemName, MAX(OI.ItemPrice) AS ItemPrice, COUNT(*) AS OrderCount,
                       MAX(O.OrderDate) AS LastOrdered, F.icon AS ItemImage
                FROM OrderItems OI
                JOIN Orders O ON OI.OrderId = O.OrderId
                LEFT JOIN foodItems F ON OI.ItemName = F.Item
                WHERE O.CustomerId = @CustomerId
                GROUP BY OI.ItemName, F.icon
                ORDER BY COUNT(*) DESC, MAX(O.OrderDate) DESC";
            var parameters = new Dictionary<string, object> { { "@CustomerId", customerId } };
            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);
            List<PreviousOrderItemDto> items = new List<PreviousOrderItemDto>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new PreviousOrderItemDto
                    {
                        ItemName = SafeOperations.SafeGetString(row, "ItemName"),
                        ItemPrice = SafeOperations.SafeGetDecimal(row, "ItemPrice"),
                        Quantity = SafeOperations.SafeGetInt(row, "OrderCount"), // Reusing for count
                        ItemImage = row["ItemImage"] != DBNull.Value ? (byte[])row["ItemImage"] : null
                    });
                }
            }
            return items;
        }

        public bool CheckForCriticalOrders()
        {
            string query = @"
            SELECT COUNT(*) FROM Orders O
            WHERE O.completion = 'no'
            AND CAST(O.OrderDate AS DATE) = CAST(GETDATE() AS DATE)
            AND (
                (O.OrderType = 'COLLECTION' AND DATEDIFF(MINUTE, O.OrderDate, GETDATE()) > 20)
                OR
                (O.OrderType = 'DELIVERY' AND DATEDIFF(MINUTE, O.OrderDate, GETDATE()) > 60)
            )";
            object result = DatabaseManager.ExecuteScalar(query);
            return result != null && Convert.ToInt32(result) > 0;
        }

        public List<Dictionary<string, object>> GetAlertableOrders()
        {
            string query = @"
            SELECT
                O.OrderId,
                O.OrderType,
                O.DesiredCompletionTime,
                O.completion,
                CAST(DATEDIFF(MINUTE,
                    DATEADD(SECOND,
                        DATEPART(HOUR, O.DesiredCompletionTime) * 3600 +
                        DATEPART(MINUTE, O.DesiredCompletionTime) * 60 +
                        DATEPART(SECOND, O.DesiredCompletionTime),
                        CAST(O.OrderDate AS DATETIME)
                    ),
                    GETDATE()
                ) AS INT) AS TimeSinceDesiredCompletion
            FROM
                Orders O
            WHERE
                O.completion = 'no'
                AND CAST(O.OrderDate AS DATE) = CAST(GETDATE() AS DATE)
            AND O.OrderType IN ('COLLECTION', 'DELIVERY')";

            DataTable dt = DatabaseManager.ExecuteQuery(query);
            List<Dictionary<string, object>> orders = new List<Dictionary<string, object>>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var orderData = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        orderData[col.ColumnName] = row[col];
                    }
                    orders.Add(orderData);
                }
            }
            return orders;
        }

        public void UpdateOrderPaymentStatus(int orderId, string paymentType)
        {
            string query = "UPDATE Orders SET PaymentType = @PaymentType WHERE OrderId = @OrderId";
            var parameters = new Dictionary<string, object> { { "@PaymentType", paymentType }, { "@OrderId", orderId } };
            DatabaseManager.ExecuteNonQuery(query, parameters);
        }
    }
}