using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public partial class frmOrderProgress : Form
    {
        // Helper method to safely convert database values to int
        // Helper method to safely convert database values to int
        private int SafeConvertToInt(object value)
        {
            if (value == null || value == DBNull.Value)
                return 0;

            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        public frmOrderProgress()
        {
            InitializeComponent();
            flpDynamicProgressView.AutoScroll = true;
            flpDynamicProgressView.FlowDirection = FlowDirection.LeftToRight;
            flpDynamicProgressView.WrapContents = true;
            LoadOrders();
            btnDoneView.Text = "VIEW DONE ORDERS";
        }

        private void LoadOrders()
        {
            string ordersQuery = @"
    SELECT 
        O.OrderId, 
        O.CustomerId, 
        O.OrderType,
        O.TotalPrice, 
        O.OrderDate,
        O.DesiredCompletionTime,
        O.PaymentType,
        C.forename, 
        C.surname, 
        DATEDIFF(MINUTE, O.OrderDate, GETDATE()) AS TimeSinceOrder  -- This calculates minutes since order was placed
    FROM 
        Orders O
    INNER JOIN 
        customers C ON O.CustomerId = C.Id
    WHERE 
        O.completion = 'no' AND CAST(O.OrderDate AS DATE) = CAST(GETDATE() AS DATE)";

            // Get the orders from the database
            DataTable ordersTable = DatabaseManager.ExecuteQuery(ordersQuery);

            // Convert DataTable to List<Dictionary<string, object>>
            var orders = new List<Dictionary<string, object>>();
            foreach (DataRow row in ordersTable.Rows)
            {
                var order = new Dictionary<string, object>();
                foreach (DataColumn col in ordersTable.Columns)
                {
                    order[col.ColumnName] = row[col];
                }
                orders.Add(order);
            }

            // Sort the orders - now we're using TimeSinceOrder directly (higher values = older orders)
            orders = orders.OrderByDescending(o => o["OrderType"].ToString() == "Collection" && SafeConvertToInt(o["TimeSinceOrder"]) > 20)
                   .ThenByDescending(o => o["OrderType"].ToString() == "Delivery" && SafeConvertToInt(o["TimeSinceOrder"]) > 60)
                   .ThenByDescending(o => o["OrderType"].ToString() == "Collection" && SafeConvertToInt(o["TimeSinceOrder"]) > 10)
                   .ThenByDescending(o => o["OrderType"].ToString() == "Delivery" && SafeConvertToInt(o["TimeSinceOrder"]) > 45)
                   .ToList();

            // Clear the panel first
            flpDynamicProgressView.Controls.Clear();

            // Add the orders to the panel
            foreach (var order in orders)
            {
                // Create a new panel
                Panel panel = new Panel
                {
                    Width = flpDynamicProgressView.Width - flpDynamicProgressView.Padding.Horizontal - 30,
                    Height = 50,
                    Margin = new Padding(5),
                    BorderStyle = BorderStyle.FixedSingle,

                    // Set the BackColor based on the TimeSinceOrder - using safe conversion
                    BackColor = order["OrderType"].ToString() == "Collection" && SafeConvertToInt(order["TimeSinceOrder"]) > 20 ||
                               order["OrderType"].ToString() == "Delivery" && SafeConvertToInt(order["TimeSinceOrder"]) > 60 ? Color.Red :
                               order["OrderType"].ToString() == "Collection" && SafeConvertToInt(order["TimeSinceOrder"]) > 10 ||
                               order["OrderType"].ToString() == "Delivery" && SafeConvertToInt(order["TimeSinceOrder"]) > 45 ? Color.Orange : DefaultBackColor
                };

                // Calculate hours and minutes
                int totalMinutes = SafeConvertToInt(order["TimeSinceOrder"]);
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;

                // Create a new label - now showing "Time Since Order" instead of "Time Remaining"
                Label lbl = new Label
                {
                    Text = $"{order["OrderType"].ToString().ToUpper()}: {order["forename"]} {order["surname"]}, Total Price: £{order["TotalPrice"]}, [{order["PaymentType"]}], Time Since Order: {hours} hours {minutes} minutes",
                    AutoSize = true,
                    Location = new Point(5, 15)
                };

                // Create a new button
                Button btn = new Button
                {
                    Text = "DONE",
                    Dock = DockStyle.Right,
                    Location = new Point(700, 10),
                    Size = new Size(50, 30)
                };

                // Attach a click event to the button
                btn.Click += (s, e) =>
                {
                    // Update the order's completion status to 'yes'
                    string updateQuery = "UPDATE Orders SET completion = @completion WHERE OrderId = @orderId";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@completion", "yes" },
                { "@orderId", SafeConvertToInt(order["OrderId"]) }
            };

                    DatabaseManager.ExecuteNonQuery(updateQuery, parameters);

                    // Reload the orders
                    LoadOrders();
                };

                // Add the label and button to the panel
                panel.Controls.Add(lbl);
                panel.Controls.Add(btn);

                // Add the panel to the FlowLayoutPanel
                flpDynamicProgressView.Controls.Add(panel);
            }
        }


        private float CalculateFontSize(Panel panel)
        {
            float fontSize = panel.Width / 80;

            fontSize = Math.Max(8.25f, fontSize); // Minimum size
            fontSize = Math.Min(72f, fontSize); // Maximum size

            return fontSize;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDoneView_Click(object sender, EventArgs e)
        {
            if (btnDoneView.Text == "VIEW DONE ORDERS")
            {
                LoadCompletedOrders();
                btnDoneView.Text = "VIEW CURRENT ORDERS";
            }
            else
            {
                LoadOrders();
                btnDoneView.Text = "VIEW DONE ORDERS";
            }
        }

        private void LoadCompletedOrders()
        {
            // Set the query for the database
            string completedOrdersQuery = @"
    SELECT 
        O.OrderId, 
        O.CustomerId, 
        O.TotalPrice, 
        O.OrderDate,
        O.DesiredCompletionTime,
        O.PaymentType,
        C.forename, 
        C.surname, 
        DATEDIFF(MINUTE, O.OrderDate, GETDATE()) AS TimeSinceOrder  -- This calculates minutes since order was placed
    FROM 
        Orders O
    INNER JOIN 
        customers C ON O.CustomerId = C.Id
    WHERE 
        O.completion = 'yes' AND CAST(O.OrderDate AS DATE) = CAST(GETDATE() AS DATE)";

            // Get the orders from the database
            DataTable ordersTable = DatabaseManager.ExecuteQuery(completedOrdersQuery);

            // Convert DataTable to List<Dictionary<string, object>>
            var orders = new List<Dictionary<string, object>>();
            foreach (DataRow row in ordersTable.Rows)
            {
                var order = new Dictionary<string, object>();
                foreach (DataColumn col in ordersTable.Columns)
                {
                    order[col.ColumnName] = row[col];
                }
                orders.Add(order);
            }

            // Clear the panel first
            flpDynamicProgressView.Controls.Clear();

            // Add the orders to the panel
            foreach (var order in orders)
            {
                // Create a new panel
                Panel panel = new Panel
                {
                    Width = flpDynamicProgressView.Width - flpDynamicProgressView.Padding.Horizontal - 30,
                    Height = 50,
                    Margin = new Padding(5),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Calculate hours and minutes
                int totalMinutes = SafeConvertToInt(order["TimeSinceOrder"]);
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;

                // Create a new label - now showing "Time Since Order" instead of "Time Remaining"
                Label lbl = new Label
                {
                    Text = $"{order["forename"]} {order["surname"]}, Total Price: £{order["TotalPrice"]}, [{order["PaymentType"]}], Time Since Order: {hours} hours {minutes} minutes",
                    AutoSize = true,
                    Location = new Point(5, 15)
                };

                // Create a new button
                Button btn = new Button
                {
                    Text = "UNDO",
                    Dock = DockStyle.Right,
                    Location = new Point(700, 10),
                    Size = new Size(50, 30)
                };

                // Attach a click event to the button
                btn.Click += (s, e) =>
                {
                    // Update the order's completion status to 'no'
                    string updateQuery = "UPDATE Orders SET completion = @completion WHERE OrderId = @orderId";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@completion", "no" },
                { "@orderId", SafeConvertToInt(order["OrderId"]) }
            };

                    DatabaseManager.ExecuteNonQuery(updateQuery, parameters);

                    // Reload the orders
                    LoadCompletedOrders();
                };

                // Add the label and button to the panel
                panel.Controls.Add(lbl);
                panel.Controls.Add(btn);

                // Add the panel to the FlowLayoutPanel
                flpDynamicProgressView.Controls.Add(panel);
            }
        }

    }
}