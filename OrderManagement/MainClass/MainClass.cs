using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OrderManagement
{
    class MainClass
    {
        // Remove the hardcoded conString and use DatabaseManager.ConnectionString everywhere

        public static void CloseAllDatabaseConnections()
        {
            DatabaseManager.CloseConnections();
        }

        public static void ForceCheckpoint()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("CHECKPOINT", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error forcing checkpoint: " + ex.Message);
            }
        }

        public static bool IsValidUser(string user, string pass)
        {
            bool isValid = false;

            string qry = @"SELECT * FROM users WHERE username = @username AND upass = @upass";

            using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@username", user);
                cmd.Parameters.AddWithValue("@upass", pass);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MainClassHelpers.username = dt.Rows[0]["username"].ToString();
                    MainClassHelpers.uName = dt.Rows[0]["uName"].ToString();
                    MainClassHelpers.uPhone = dt.Rows[0]["uphone"].ToString();

                    isValid = true;
                }
            }

            return isValid;
        }

        public static int SQL(string qry, Dictionary<string, object> parameters)
        {
            int res = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.CommandType = CommandType.Text;

                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }

                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("An unexpected error occurred. Please contact support for assistance." + e);
            }

            return res;
        }

        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (var item in lb.Items)
                    {
                        string colName = item.ToString();
                        if (gv.Columns.Contains(colName))
                        {
                            gv.Columns[colName].DataPropertyName = colName;
                        }
                        else
                        {
                            MessageBox.Show($"Column '{colName}' does not exist in the DataGridView.", "Error");
                        }
                    }

                    gv.DataSource = dt;
                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("An unexpected error occurred. Please contact support for assistance." + e);
            }
        }

        public static int CreateCustomer(string forename, string surname, string telephoneNo, string address)
        {
            try
            {
                string query = @"
            INSERT INTO customers (forename, surname, telephoneNo, AddressLine1) 
            VALUES (@forename, @surname, @telephoneNo, @address);
            SELECT SCOPE_IDENTITY();";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@forename", forename },
                    { "@surname", surname },
                    { "@telephoneNo", string.IsNullOrEmpty(telephoneNo) ? DBNull.Value : (object)telephoneNo },
                    { "@address", string.IsNullOrEmpty(address) ? DBNull.Value : (object)address }
                };

                object result = ExecuteScalar(query, parameters);
                return result == null || result == DBNull.Value ? -1 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        public static List<string> getCategoriesFromDatabase(string qry)
        {
            List<string> categories = new List<string>();

            using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        categories.Add(reader["catName"].ToString());
                    }
                    reader.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected Error\nPlease contact support\n" + ex.Message);
                }
            }
            return categories;
        }

        public static List<string> getTableItems(string qry)
        {
            List<string> customers = new List<string>();

            using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string customerInfo = $"{reader["forename"]} {reader["surname"]}, {reader["telephoneNo"]}, {reader["AddressLine1"]}, {reader["AddressLine2"]}, {reader["AddressLine3"]}, {reader["AddressLine4"]}, {reader["Postcode"]}";
                        customers.Add(customerInfo);
                    }

                    reader.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected Error\nPlease contact support\n" + ex.Message);
                }
            }
            return customers;
        }

        public static decimal getItemPriceFromDatabase(string qry, string paramName, string paramValue)
        {
            decimal price = 0;

            using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
            {
                SqlCommand command = new SqlCommand(qry, con);
                try
                {
                    con.Open();
                    command.Parameters.AddWithValue(paramName, paramValue);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        price = Convert.ToDecimal(reader["price"]);
                    }

                    reader.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected Error\nPlease contact support\n" + ex.Message);
                }
            }

            return price;
        }

        public static decimal getItemIdFromDatabase(string qry, string paramName, string paramValue)
        {
            decimal id = 0;

            using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
            {
                SqlCommand command = new SqlCommand(qry, con);
                try
                {
                    con.Open();
                    command.Parameters.AddWithValue(paramName, paramValue);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        id = Convert.ToDecimal(reader["ItemId"]);
                    }

                    reader.Close();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected Error\nPlease contact support\n" + ex.Message);
                }
            }

            return id;
        }

        public static DataTable getDataWithImageFromTable(string qry, string paramName, string paramValue)
        {
            using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
            {
                SqlCommand command = new SqlCommand(qry, con);
                DataTable dataTable = new DataTable();
                try
                {
                    con.Open();
                    command.Parameters.AddWithValue(paramName, paramValue);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected Error\nPlease contact support\n" + ex.Message);
                }
                return dataTable;
            }
        }

        public static dynamic GetJsonResponse(string apiUrl)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string jsonResponse = reader.ReadToEnd();
                            dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);
                            return jsonObject;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetJsonResponse: {ex.Message}");
                return null;
            }
        }

        public static dynamic ParseJsonResponse(string jsonResponse)
        {
            try
            {
                dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                return result;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                return null;
            }
        }

        public static DataTable getDataFromTable(string query, Dictionary<string, object> parameters)
        {
            return DatabaseManager.ExecuteQuery(query, parameters);
        }

        public static void UpdateOrders(int orderId, decimal totalOrderPrice, string orderType, DateTime? orderDateTime, string paymentType)
        {
            if (string.IsNullOrWhiteSpace(orderType))
            {
                orderType = "DefaultOrderType";
            }

            string updateQuery = "UPDATE Orders " +
                "SET TotalPrice = @TotalPrice, OrderType = @OrderType, DesiredCompletionTime = @time, PaymentType = @PaymentType " +
                "WHERE OrderId = @OrderId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@TotalPrice", totalOrderPrice},
                {"@OrderType", orderType},
                {"@time", orderDateTime ?? (object)DBNull.Value},
                {"@PaymentType", paymentType},
                {"@OrderId", orderId}
            };

            ExecuteNonQuery(updateQuery, parameters);
        }

        public static DataTable GetPreviousOrdersAndItems(int customerId)
        {
            string sql = @"SELECT o.OrderId, o.OrderDate, o.TotalPrice, 
                         i.ItemName, i.Quantity
                 FROM Orders o
                 INNER JOIN OrderItems i ON o.OrderId = i.OrderId   
                 WHERE o.CustomerId = @custId
                 ORDER BY o.OrderDate DESC";

            return getDataFromTable(sql, new Dictionary<string, object> {
                {"@custId", customerId}
            });
        }

        public static DataTable GetPreviousOrders(int customerId)
        {
            string sql = @"SELECT O.OrderId, O.OrderDate, O.OrderType, O.PaymentType, O.TotalPrice,
                  I.ItemId, F.Item as ItemName, F.price as ItemPrice, 1 as Quantity, I.DateAdded
           FROM Orders O
           INNER JOIN OrderItems I ON O.OrderId = I.OrderId
           INNER JOIN foodItems F ON I.ItemId = F.Id
           WHERE O.CustomerId = @custId
           ORDER BY I.DateAdded DESC";

            return getDataFromTable(sql, new Dictionary<string, object>
            {
                {"@custId", customerId}
            });
        }

        public static void IncrementPreviousOrders(string customerTelephoneNo)
        {
            string qry = @"UPDATE customers SET previousOrders = previousOrders + 1 WHERE telephoneNo = @telephoneNo";

            using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(qry, con))
            {
                cmd.Parameters.AddWithValue("@telephoneNo", customerTelephoneNo);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void UpdateOrderPaymentType(int orderId, string paymentType)
        {
            try
            {
                string sql = "UPDATE Orders SET PaymentType = @paymentType WHERE OrderId = @orderId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@orderId", orderId},
                    {"@paymentType", paymentType}
                };
                SQL(sql, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating payment type: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool UpdateOrderPayment(int orderId, string paymentType)
        {
            try
            {
                string query = "UPDATE Orders SET PaymentType = @paymentType WHERE OrderId = @orderId";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@paymentType", paymentType },
                    { "@orderId", orderId }
                };

                int rowsAffected = MainClass.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating payment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static DataTable ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            return DatabaseManager.ExecuteQuery(query, parameters);
        }

        public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            return DatabaseManager.ExecuteNonQuery(query, parameters);
        }

        public static bool DeleteOrder(int orderId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
                {
                    con.Open();
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            string deleteItemsQuery = "DELETE FROM OrderItems WHERE OrderId = @OrderId";
                            using (SqlCommand cmd = new SqlCommand(deleteItemsQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", orderId);
                                cmd.ExecuteNonQuery();
                            }

                            string deleteOrderQuery = "DELETE FROM Orders WHERE OrderId = @OrderId";
                            using (SqlCommand cmd = new SqlCommand(deleteOrderQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@OrderId", orderId);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Database error: " + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void AddOrUpdateOrderItem(int orderId, string itemName, decimal itemPrice, int quantity, decimal totalOrderPrice, string orderType, DateTime? orderDateTime, string paymentType)
        {
            string query = @"
                SELECT TOP 1 oi.OrderItemId, oi.Quantity
                FROM OrderItems oi
                INNER JOIN Orders o ON oi.OrderId = o.OrderId
                WHERE o.OrderId = @OrderId
                    AND oi.ItemName = @ItemName
                    AND o.OrderDate >= DATEADD(HOUR, -12, GETDATE())
                ORDER BY o.OrderDate DESC
            ";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@OrderId", orderId },
                { "@ItemName", itemName }
            };

            DataTable dt = ExecuteQuery(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                int orderItemId = Convert.ToInt32(dt.Rows[0]["OrderItemId"]);
                int existingQuantity = Convert.ToInt32(dt.Rows[0]["Quantity"]);
                int updatedQuantity = existingQuantity + quantity;

                UpdateOrderItemQuantity(orderItemId, updatedQuantity);
            }
            else
            {
                SaveOrderItem(orderId, itemName, itemPrice, quantity);
            }

            UpdateOrdersWithCompletion(orderId, totalOrderPrice, orderType, orderDateTime, paymentType);
        }

        public static void UpdateOrderItemQuantity(int orderItemId, int updatedQuantity)
        {
            string updateQuantityQuery = "UPDATE OrderItems SET Quantity = @UpdatedQuantity WHERE OrderItemId = @OrderItemId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@OrderItemId", orderItemId},
                {"@UpdatedQuantity", updatedQuantity}
            };

            SQL(updateQuantityQuery, parameters);
        }

        public static int SaveOrder(
     int customerId, string orderType, decimal totalPrice, DateTime currentTime, string paymentType,
     string address, decimal deliveryCharge, decimal presetCharges, decimal discountRate, string discountLabel)
        {
            SqlConnection con = null;
            SqlTransaction transaction = null;

            try
            {
                string checkQuery = "SELECT COUNT(*) FROM customers WHERE Id = @CustomerId";
                Dictionary<string, object> checkParams = new Dictionary<string, object>
                {
                    { "@CustomerId", customerId }
                };

                int customerCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkQuery, checkParams));
                if (customerCount == 0)
                {
                    MessageBox.Show($"Customer with ID {customerId} does not exist in the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

                TimeSpan desiredCompletionTime = currentTime.AddMinutes(30).TimeOfDay;

                string sql = @"
        INSERT INTO Orders 
            (CustomerId, OrderType, TotalPrice, OrderDate, PaymentType, Address, completion, DesiredCompletionTime, DeliveryCharge, PresetCharges, DiscountRate, DiscountLabel) 
        VALUES 
            (@CustomerId, @OrderType, @TotalPrice, @OrderDate, @PaymentType, @Address, 'no', @DesiredCompletionTime, @DeliveryCharge, @PresetCharges, @DiscountRate, @DiscountLabel);
        SELECT SCOPE_IDENTITY();";

                con = new SqlConnection(DatabaseManager.ConnectionString);
                con.Open();
                transaction = con.BeginTransaction();

                using (SqlCommand cmd = new SqlCommand(sql, con, transaction))
                {
                    cmd.Parameters.Add("@CustomerId", SqlDbType.Int).Value = customerId;
                    cmd.Parameters.Add("@OrderType", SqlDbType.VarChar, 50).Value = orderType;
                    cmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = totalPrice;
                    cmd.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = currentTime;
                    cmd.Parameters.Add("@PaymentType", SqlDbType.VarChar, 50).Value = paymentType;
                    cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(address) ? (object)DBNull.Value : address;
                    cmd.Parameters.Add("@DesiredCompletionTime", SqlDbType.Time).Value = desiredCompletionTime;
                    cmd.Parameters.Add("@DeliveryCharge", SqlDbType.Decimal).Value = deliveryCharge;
                    cmd.Parameters.Add("@PresetCharges", SqlDbType.Decimal).Value = presetCharges;
                    cmd.Parameters.Add("@DiscountRate", SqlDbType.Decimal).Value = discountRate;
                    cmd.Parameters.Add("@DiscountLabel", SqlDbType.NVarChar, 50).Value = discountLabel ?? (object)DBNull.Value;

                    var result = cmd.ExecuteScalar();
                    int orderId = result == null ? -1 : Convert.ToInt32(result);

                    if (orderId > 0)
                    {
                        transaction.Commit();
                        DatabaseManager.ForceCheckpoint();
                        return orderId;
                    }
                    else
                    {
                        transaction.Rollback();
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch { }
                }

                MessageBox.Show("Error saving order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        public static int SaveOrderItem(int orderId, string itemName, decimal itemPrice, int quantity, decimal extraCharge = 0)
        {
            SqlConnection con = null;
            SqlTransaction transaction = null;

            try
            {
                string checkOrderQuery = "SELECT COUNT(*) FROM Orders WHERE OrderId = @OrderId";
                Dictionary<string, object> checkOrderParams = new Dictionary<string, object>
        {
            { "@OrderId", orderId }
        };

                int orderCount = Convert.ToInt32(DatabaseManager.ExecuteScalar(checkOrderQuery, checkOrderParams));
                if (orderCount == 0)
                {
                    MessageBox.Show($"Order with ID {orderId} does not exist in the database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

                con = new SqlConnection(DatabaseManager.ConnectionString);
                con.Open();
                transaction = con.BeginTransaction();

                // Try to get the itemId from foodItems
                string getItemIdQuery = "SELECT Id FROM foodItems WHERE Item = @ItemName";
                object result;
                int? itemId = null;

                using (SqlCommand cmd = new SqlCommand(getItemIdQuery, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@ItemName", itemName);
                    result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        itemId = Convert.ToInt32(result);
                }

                string sql = @"
    INSERT INTO OrderItems 
        (OrderId, ItemId, ItemName, ItemPrice, Quantity, DateAdded, ExtraCharge) 
    VALUES 
        (@OrderId, @ItemId, @ItemName, @ItemPrice, @Quantity, GETDATE(), @ExtraCharge);
    SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(sql, con, transaction))
                {
                    cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderId;
                    cmd.Parameters.Add("@ItemId", SqlDbType.Int).Value = (object)itemId ?? DBNull.Value;
                    cmd.Parameters.Add("@ItemName", SqlDbType.VarChar, 100).Value = itemName;
                    cmd.Parameters.Add("@ItemPrice", SqlDbType.Decimal).Value = itemPrice;
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = quantity;
                    cmd.Parameters.Add("@ExtraCharge", SqlDbType.Decimal).Value = extraCharge;

                    var newId = cmd.ExecuteScalar();
                    int orderItemId = newId == null ? -1 : Convert.ToInt32(newId);

                    if (orderItemId > 0)
                    {
                        transaction.Commit();
                        DatabaseManager.ForceCheckpoint();
                        return orderItemId;
                    }
                    else
                    {
                        transaction.Rollback();
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    try { transaction.Rollback(); } catch { }
                }

                MessageBox.Show("Error saving order item: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }


        public static object ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            return DatabaseManager.ExecuteScalar(query, parameters);
        }

        public static int GetLatestOrderIdForCustomer(int customerId)
        {
            int orderId = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
                {
                    con.Open();

                    string sql = "SELECT TOP 1 OrderId FROM Orders WHERE CustomerId = @custId ORDER BY OrderDate DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@custId", customerId);

                        var scalarResult = cmd.ExecuteScalar();

                        if (scalarResult != null && scalarResult != DBNull.Value)
                        {
                            orderId = Convert.ToInt32(scalarResult);
                        }
                    }

                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("An unexpected error occurred. Please contact support for assistance." + e);
            }

            return orderId;
        }

        public static List<int> GetOrderItemIds(int orderId)
        {
            List<int> orderItemIds = new List<int>();

            string query = "SELECT ItemId FROM OrderItems WHERE OrderId = @OrderId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@OrderId", orderId}
            };

            try
            {
                using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;

                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderItemId = Convert.ToInt32(reader["ItemId"]);
                            orderItemIds.Add(orderItemId);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }

            return orderItemIds;
        }

        public static void RemoveOrderItem(int orderId, int itemId)
        {
            string query = "DELETE FROM OrderItems WHERE OrderId = @OrderId AND ItemId = @ItemId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@OrderId", orderId},
                {"@ItemId", itemId}
            };

            SQL(query, parameters);
        }

        public static string GetItemName(int itemId)
        {
            string query = "SELECT ItemName FROM OrderItems WHERE ItemId = @itemId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                {"@itemId", itemId}
            };
            DataTable dt = getDataFromTable(query, parameters);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ItemName"].ToString();
            }
            return null;
        }

        public static int UpdateTableData(string query, Dictionary<string, object> parameters)
        {
            int rowsAffected;

            using (SqlConnection connection = new SqlConnection(DatabaseManager.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(new SqlParameter(parameter.Key, parameter.Value ?? DBNull.Value));
                    }

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        public static List<Dictionary<string, object>> GetOrdersFromDatabase(string query)
        {
            DataTable dt = ExecuteQuery(query, new Dictionary<string, object>());

            List<Dictionary<string, object>> orders = new List<Dictionary<string, object>>();

            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> order = new Dictionary<string, object>();
                foreach (DataColumn column in dt.Columns)
                {
                    order[column.ColumnName] = row[column];
                }
                orders.Add(order);
            }

            return orders;
        }

        public static void UpdateOrderCompletionStatus(int orderId, string completionStatus)
        {
            string query = "UPDATE Orders SET completion = @completion WHERE OrderId = @orderId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@completion", completionStatus },
                { "@orderId", orderId }
            };

            ExecuteNonQuery(query, parameters);
        }

        public static void UpdateOrdersWithCompletion(int orderId, decimal totalOrderPrice, string orderType, DateTime? orderDateTime, string paymentType)
        {
            if (string.IsNullOrWhiteSpace(orderType))
            {
                throw new ArgumentException("OrderType cannot be null or empty.", nameof(orderType));
            }

            string updateQuery = "UPDATE Orders " +
                "SET TotalPrice = @TotalPrice, OrderType = @OrderType, DesiredCompletionTime = @time, PaymentType = @PaymentType " +
                "WHERE OrderId = @OrderId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@TotalPrice", totalOrderPrice },
                { "@OrderType", orderType },
                { "@time", orderDateTime ?? (object)DBNull.Value },
                { "@PaymentType", paymentType },
                { "@OrderId", orderId }
            };

            ExecuteNonQuery(updateQuery, parameters);
        }
    }
}
