using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OrderManagement;

public class OrderDetailsDto
{
    public int OrderId;
    public int CustomerId;
    public string Forename;
    public string Surname;
    public string TelephoneNo;
    public string Email;
    public string HouseNameNumber;
    public string AddressLine1;
    public string AddressLine2;
    public string AddressLine3;
    public string AddressLine4;
    public string Postcode;
    public string OrderType;
    public string PaymentType;
    public string Address;
    public DateTime OrderDate;
    public decimal TotalPrice;
    // Add more fields as needed
}

public class OrderItemDto
{
    public string ItemName;
    public decimal ItemPrice;
    public int Quantity;
}

public class OrderManager
{
    public OrderDetailsDto GetOrderDetails(int orderId)
    {
        string query = @"
            SELECT 
                o.OrderId, o.CustomerId, o.OrderType, o.TotalPrice, o.OrderDate, o.PaymentType, o.Address,
                c.forename, c.surname, c.telephoneNo, c.Email, c.houseNameNumber,
                c.AddressLine1, c.AddressLine2, c.AddressLine3, c.AddressLine4, c.Postcode
            FROM Orders o
            JOIN customers c ON o.CustomerId = c.Id
            WHERE o.OrderId = @OrderId";

        var parameters = new Dictionary<string, object> { { "@OrderId", orderId } };
        DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);

        if (dt != null && dt.Rows.Count > 0)
        {
            var row = dt.Rows[0];
            return new OrderDetailsDto
            {
                OrderId = Convert.ToInt32(row["OrderId"]),
                CustomerId = Convert.ToInt32(row["CustomerId"]),
                Forename = row["forename"].ToString(),
                Surname = row["surname"].ToString(),
                TelephoneNo = row["telephoneNo"].ToString(),
                Email = row["Email"].ToString(),
                HouseNameNumber = row["houseNameNumber"].ToString(),
                AddressLine1 = row["AddressLine1"].ToString(),
                AddressLine2 = row["AddressLine2"].ToString(),
                AddressLine3 = row["AddressLine3"].ToString(),
                AddressLine4 = row["AddressLine4"].ToString(),
                Postcode = row["Postcode"].ToString(),
                OrderType = row["OrderType"].ToString(),
                PaymentType = row["PaymentType"].ToString(),
                Address = row["Address"].ToString(),
                OrderDate = Convert.ToDateTime(row["OrderDate"]),
                TotalPrice = Convert.ToDecimal(row["TotalPrice"])
            };
        }
        return null;
    }

    public List<OrderItemDto> GetOrderItems(int orderId)
    {
        string query = @"
            SELECT ItemName, ItemPrice, Quantity
            FROM OrderItems
            WHERE OrderId = @OrderId";
        var parameters = new Dictionary<string, object> { { "@OrderId", orderId } };
        DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);

        var items = new List<OrderItemDto>();
        if (dt != null)
        {
            foreach (DataRow row in dt.Rows)
            {
                items.Add(new OrderItemDto
                {
                    ItemName = row["ItemName"].ToString(),
                    ItemPrice = Convert.ToDecimal(row["ItemPrice"]),
                    Quantity = Convert.ToInt32(row["Quantity"])
                });
            }
        }
        return items;
    }
}