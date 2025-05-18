using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OrderManagement.View
{
    public partial class frmOrderHistory : Form
    {
        private DataTable orderHistoryTable;
        private bool isFormatting = false;
        private Bitmap bitmap;
        private bool isInitialLoad = true; // Track initial load

        public frmOrderHistory()
        {
            InitializeComponent();
            SetInitialDateRange(); // Set today's date
            LoadOrderHistory(); // Load data using that date

            // Initialize cmbOrderType
            cmbOrderType.Items.Clear();
            cmbOrderType.Items.Add("ALL");
            cmbOrderType.Items.AddRange(new string[] { "WAITING", "COLLECTION", "DELIVERY", "ONLINE", "RESTAURANT", "CANCELLED" });
            cmbOrderType.SelectedIndex = 0;

            // Initialize cmbPaymentType with all possible values
            cmbPaymentType.Items.Clear();
            cmbPaymentType.Items.Add("ALL");
            cmbPaymentType.Items.AddRange(new string[] { "CASH", "CARD", "NOT PAID", "CANCELLED", "PENDING" });
            cmbPaymentType.SelectedIndex = 0;

            dgvOrderHistoryView.CellFormatting += dgvOrderHistoryView_CellFormatting;
            dgvOrderHistoryView.CellPainting += dgvOrderHistoryView_CellPainting;
        }



        private void dgvOrderHistoryView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string paymentStatus = dgvOrderHistoryView.Rows[e.RowIndex].Cells["dgvPayment"].Value?.ToString();

                if (paymentStatus != null && paymentStatus.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase))
                {
                    if (e.ColumnIndex != dgvOrderHistoryView.Columns["dgvCancel"].Index)
                    {
                        e.PaintBackground(e.ClipBounds, true);
                        e.PaintContent(e.ClipBounds);

                        using (Pen p = new Pen(Color.Red, 2))
                        {
                            Point pt1 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height / 2);
                            Point pt2 = new Point(e.CellBounds.Right - 1, e.CellBounds.Top + e.CellBounds.Height / 2);
                            e.Graphics.DrawLine(p, pt1, pt2);
                        }

                        e.Handled = true;
                    }
                }
            }
        }


        public void LoadOrderHistory()
        {
            string query = @"
        SELECT 
            o.OrderId AS dgvOrderId,
            c.forename + ' ' + c.surname AS dgvName,
            o.Address AS dgvAddress,
            o.OrderType AS dgvOrderType,
            o.OrderDate AS dgvOrderDate,
            o.TotalPrice AS dgvTotalPrice,
            o.PaymentType AS dgvPayment
        FROM Orders o
        JOIN customers c ON o.CustomerId = c.Id
        ORDER BY o.OrderDate DESC";

            try
            {
                DataTable dataTable = DatabaseManager.ExecuteQuery(query);

                if (dataTable != null)
                {
                    // Store the data table
                    orderHistoryTable = dataTable;

                    // Ensure columns exist and are properly formatted before setting the data source
                    EnsureColumnsExist();

                    // Set the data source
                    dgvOrderHistoryView.DataSource = dataTable;

                    // Populate payment types from actual data if this is the initial load
                    if (isInitialLoad)
                    {
                        isInitialLoad = false;

                        // Get unique payment types from the data
                        var paymentTypes = new HashSet<string>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string paymentType = row["dgvPayment"].ToString();
                            if (!string.IsNullOrEmpty(paymentType))
                            {
                                paymentTypes.Add(paymentType);
                            }
                        }

                        // Update the payment type dropdown
                        if (paymentTypes.Count > 0)
                        {
                            cmbPaymentType.Items.Clear();
                            cmbPaymentType.Items.Add("ALL");
                            foreach (string paymentType in paymentTypes)
                            {
                                cmbPaymentType.Items.Add(paymentType);
                            }
                            cmbPaymentType.SelectedIndex = 0;
                        }
                    }

                    // Apply current filters
                    FilterByDateRange();
                    columnOrder();


                    // Refresh the view
                    dgvOrderHistoryView.Refresh();
                    UpdateTotalValue();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void EnsureColumnsExist()
        {
            // Define column mappings: DataPropertyName -> HeaderText
            Dictionary<string, string> columnMappings = new Dictionary<string, string>
    {
        { "dgvOrderId", "Order ID" },
        { "dgvName", "Customer Name" },
        { "dgvAddress", "Address" },
        { "dgvOrderType", "Order Type" },
        { "dgvOrderDate", "Order Date" },
        { "dgvTotalPrice", "Total Price" },
        { "dgvPayment", "Payment" },
        { "dgvView", "View" },
        { "dgvCancel", "Cancel" },
        { "dgvDelete", "Delete" }
    };

            foreach (var mapping in columnMappings)
            {
                string columnName = mapping.Key;
                string headerText = mapping.Value;

                if (!dgvOrderHistoryView.Columns.Contains(columnName))
                {
                    DataGridViewColumn column;

                    if (columnName == "dgvDelete")
                    {
                        column = new DataGridViewButtonColumn
                        {
                            Name = columnName,
                            HeaderText = headerText,
                            Text = "DELETE",
                            UseColumnTextForButtonValue = true
                        };
                    }
                    else if (columnName == "dgvCancel")
                    {
                        column = new DataGridViewButtonColumn
                        {
                            Name = columnName,
                            HeaderText = headerText,
                            UseColumnTextForButtonValue = false
                        };
                    }
                    else if (columnName == "dgvView")
                    {
                        column = new DataGridViewButtonColumn
                        {
                            Name = columnName,
                            HeaderText = headerText,
                            Text = "VIEW",
                            UseColumnTextForButtonValue = true
                        };
                    }
                    else
                    {
                        column = new DataGridViewTextBoxColumn
                        {
                            Name = columnName,
                            HeaderText = headerText,
                            DataPropertyName = columnName
                        };
                    }

                    dgvOrderHistoryView.Columns.Add(column);
                }
                else
                {
                    // Update the header text for existing columns
                    dgvOrderHistoryView.Columns[columnName].HeaderText = headerText;
                }
            }

            // Format specific columns
            if (dgvOrderHistoryView.Columns.Contains("dgvOrderDate"))
            {
                dgvOrderHistoryView.Columns["dgvOrderDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }

            if (dgvOrderHistoryView.Columns.Contains("dgvTotalPrice"))
            {
                dgvOrderHistoryView.Columns["dgvTotalPrice"].DefaultCellStyle.Format = "£0.00";
            }
        }

        // Static field to track the current instance
        private static frmOrderHistory currentInstance;

        public static void ShowOrderHistory()
        {
            // Check if the current instance exists and is still valid
            if (currentInstance != null && !currentInstance.IsDisposed)
            {
                // If it's minimized, restore it
                if (currentInstance.WindowState == FormWindowState.Minimized)
                    currentInstance.WindowState = FormWindowState.Normal;

                // Bring it to the front
                currentInstance.BringToFront();
                currentInstance.Focus();
            }
            else
            {
                // Create a new instance
                currentInstance = new frmOrderHistory();

                // Subscribe to the FormClosed event to clear the reference
                currentInstance.FormClosed += (s, e) => currentInstance = null;

                // Show the form
                currentInstance.Show();
            }
        }

        // Add a method to close the current instance
        public static void CloseOrderHistory()
        {
            if (currentInstance != null && !currentInstance.IsDisposed)
            {
                currentInstance.Close();
                currentInstance = null;
            }
        }



        private void ResetFilters()
        {
            cmbOrderType.SelectedIndex = 0; // Default to the first option
            cmbPaymentType.SelectedIndex = 0; // Default to the first option
            txtSearchBox.Text = "";

            dtpFromDate.Value = DateTime.Today;
            dtpToDate.Value = DateTime.Today;

            LoadOrderHistory();
            dgvOrderHistoryView.Refresh();
        }

        private bool isNoOrdersMessageShown = false;

        private void columnOrder()
        {
            if (dgvOrderHistoryView.DataSource == null || dgvOrderHistoryView.Rows.Count == 0)
            {
                isNoOrdersMessageShown = false;
                return;
            }

            isNoOrdersMessageShown = false;

            try
            {
                string[] columnOrder = new string[]
                {
            "dgvOrderId", "dgvName", "dgvAddress", "dgvOrderType",
            "dgvOrderDate", "dgvTotalPrice", "dgvPayment",
            "dgvView", "dgvCancel", "dgvDelete"
                };

                foreach (string colName in columnOrder)
                {
                    if (dgvOrderHistoryView.Columns.Contains(colName))
                    {
                        dgvOrderHistoryView.Columns[colName].DisplayIndex = Array.IndexOf(columnOrder, colName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error ordering columns: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateOrderType(int orderId, string newOrderType)
        {
            string query = "UPDATE Orders SET OrderType = @OrderType WHERE OrderId = @OrderId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@OrderType", newOrderType },
        { "@OrderId", orderId }
    };

            try
            {
                MainClass.ExecuteNonQuery(query, parameters);
                LoadOrderHistory(); // Reload the grid after update
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order type: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvOrderHistoryView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Handle "Delete" button click
            if (e.ColumnIndex == dgvOrderHistoryView.Columns["dgvDelete"].Index)
            {
                int orderId;
                if (int.TryParse(dgvOrderHistoryView.Rows[e.RowIndex].Cells["dgvOrderId"].Value?.ToString(), out orderId))
                {
                    // Show confirmation prompt
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this order?", "Delete Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.Yes)
                    {
                        // Prompt for PIN code
                        string inputPin = Microsoft.VisualBasic.Interaction.InputBox("Enter PIN code to confirm deletion:", "PIN Required", "");

                        if (inputPin == "1111")
                        {
                            try
                            {
                                // First delete the order items, then delete the order
                                string deleteItemsQuery = "DELETE FROM OrderItems WHERE OrderId = @OrderId";
                                Dictionary<string, object> itemsParameters = new Dictionary<string, object>
                        {
                            { "@OrderId", orderId }
                        };

                                // Delete the order items first
                                DatabaseManager.ExecuteNonQuery(deleteItemsQuery, itemsParameters);

                                // Then delete the order
                                string deleteOrderQuery = "DELETE FROM Orders WHERE OrderId = @OrderId";
                                Dictionary<string, object> orderParameters = new Dictionary<string, object>
                        {
                            { "@OrderId", orderId }
                        };

                                int rowsAffected = DatabaseManager.ExecuteNonQuery(deleteOrderQuery, orderParameters);
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Order deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadOrderHistory(); // Reload the order history
                                }
                                else
                                {
                                    MessageBox.Show("No order was deleted. Please check the Order ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error deleting order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid PIN code. Deletion canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Order ID. Cannot delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Handle "Cancel" button click
            else if (e.ColumnIndex == dgvOrderHistoryView.Columns["dgvCancel"].Index)
            {
                string buttonText = dgvOrderHistoryView.Rows[e.RowIndex].Cells["dgvCancel"].FormattedValue.ToString();

                string dialogMessage;
                string updatePaymentType;

                if (buttonText == "CANCEL")
                {
                    dialogMessage = "Are you sure you want to cancel this order?";
                    updatePaymentType = "CANCELLED";
                }
                else
                {
                    dialogMessage = "Are you sure you want to uncancel this order?";
                    updatePaymentType = "PENDING";
                }

                DialogResult dialogResult = MessageBox.Show(dialogMessage, "Cancel Order", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    int orderId = Convert.ToInt32(dgvOrderHistoryView.Rows[e.RowIndex].Cells["dgvOrderId"].Value);

                    string query = "UPDATE Orders SET PaymentType = @paymentType WHERE OrderId = @orderId";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@paymentType", updatePaymentType },
                { "@orderId", orderId }
            };

                    DatabaseManager.ExecuteNonQuery(query, parameters);
                    LoadOrderHistory();
                }
            }

            // Handle "View" button click - UPDATED CODE HERE
            else if (e.ColumnIndex == dgvOrderHistoryView.Columns["dgvView"].Index)
            {
                int orderId = Convert.ToInt32(dgvOrderHistoryView.Rows[e.RowIndex].Cells["dgvOrderId"].Value);

                using (OrderViewForm orderViewForm = new OrderViewForm(orderId))
                {
                    if (orderViewForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadOrderHistory();
                    }
                }
            }

            columnOrder();
        }

        private void dgvOrderHistoryView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string cancelColumnName = "dgvCancel";
            string paymentColumnName = "dgvPayment";

            if (!isFormatting && e.RowIndex >= 0 && e.ColumnIndex == dgvOrderHistoryView.Columns[cancelColumnName].Index)
            {
                isFormatting = true;

                object paymentStatusObj = dgvOrderHistoryView.Rows[e.RowIndex].Cells[paymentColumnName].Value;
                if (paymentStatusObj != null)
                {
                    string paymentStatus = paymentStatusObj.ToString();
                    e.Value = paymentStatus.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase) ? "UNCANCEL" : "CANCEL";
                }

                isFormatting = false;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Force a checkpoint to ensure data is written to disk
            DatabaseManager.ForceCheckpoint();
        }

        private bool CustomerExists(int customerId)
        {
            string checkCustomerQuery = "SELECT COUNT(*) FROM customers WHERE Id = @CustomerId";
            var result = MainClass.ExecuteScalar(checkCustomerQuery,
                new Dictionary<string, object> { { "@CustomerId", customerId } });

            return Convert.ToInt32(result) > 0;
        }

        private void UpdateOrderTypeInDatabase(int orderId, string newOrderType)
        {
            string query = "UPDATE Orders SET OrderType = @OrderType WHERE OrderId = @OrderId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@OrderType", newOrderType },
        { "@OrderId", orderId }
    };

            try
            {
                DatabaseManager.ExecuteNonQuery(query, parameters);
                LoadOrderHistory(); // Reload the grid after update
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order type: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyAllFilters();
        }

        private void cmbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyAllFilters();
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            ApplyAllFilters();
        }


        private void FilterOrderHistory()
        {
            string selectedOrderType = cmbOrderType.Text.ToUpper();
            string selectedPaymentType = cmbPaymentType.Text.ToUpper();
            DateTime from = dtpFromDate.Value.Date;
            DateTime to = dtpToDate.Value.Date;

            // Ensure the DataTable is not null and contains the required columns
            if (orderHistoryTable == null || !orderHistoryTable.Columns.Contains("dgvOrderType") || !orderHistoryTable.Columns.Contains("dgvPayment"))
            {
                MessageBox.Show("The data source is missing required columns.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Filter rows based on the selected criteria
            var filteredRows = orderHistoryTable.AsEnumerable()
                .Where(r =>
                    r.Field<DateTime>("dgvOrderDate").Date >= from &&
                    r.Field<DateTime>("dgvOrderDate").Date <= to &&
                    (selectedOrderType == "ALL" || r.Field<string>("dgvOrderType").Equals(selectedOrderType, StringComparison.OrdinalIgnoreCase)) &&
                    (selectedPaymentType == "ALL" || r.Field<string>("dgvPayment").Equals(selectedPaymentType, StringComparison.OrdinalIgnoreCase)));

            // Update the DataGridView with the filtered data
            if (filteredRows.Any())
            {
                dgvOrderHistoryView.DataSource = filteredRows.CopyToDataTable();
            }
            else
            {
                dgvOrderHistoryView.DataSource = null; // Clear the DataGridView if no results match
            }

            columnOrder(); // Reorder columns
            UpdateTotalValue();
        }




        private void ApplyAllFilters()
        {
            string selectedOrderType = cmbOrderType.Text.ToUpper();
            string selectedPaymentType = cmbPaymentType.Text.ToUpper();
            string searchTerm = txtSearchBox.Text;
            DateTime from = dtpFromDate.Value.Date;
            DateTime to = dtpToDate.Value.Date;

            if (orderHistoryTable == null) return;

            var filteredRows = orderHistoryTable.AsEnumerable()
                .Where(r =>
                    r.Field<DateTime>("dgvOrderDate").Date >= from &&
                    r.Field<DateTime>("dgvOrderDate").Date <= to &&
                    (selectedOrderType == "ALL" || r.Field<string>("dgvOrderType").Equals(selectedOrderType, StringComparison.OrdinalIgnoreCase)) &&
                    (selectedPaymentType == "ALL" || r.Field<string>("dgvPayment").Equals(selectedPaymentType, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(searchTerm) ||
                        (r["dgvName"] != DBNull.Value && r.Field<string>("dgvName").IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (r["dgvAddress"] != DBNull.Value && r.Field<string>("dgvAddress").IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0))
                );

            dgvOrderHistoryView.DataSource = filteredRows.Any() ? filteredRows.CopyToDataTable() : null;
            columnOrder();
            UpdateTotalValue();
        }



        private void FilterByDateRange()
        {
            DateTime from = dtpFromDate.Value.Date;
            DateTime to = dtpToDate.Value.Date;

            var filteredRows = orderHistoryTable.AsEnumerable()
                .Where(r => r.Field<DateTime>("dgvOrderDate").Date >= from &&
                            r.Field<DateTime>("dgvOrderDate").Date <= to);

            if (filteredRows.Any())
            {
                dgvOrderHistoryView.DataSource = filteredRows.CopyToDataTable();
            }
            else
            {
                // If no orders are found, clear the DataGridView without showing an error message
                dgvOrderHistoryView.DataSource = null;
            }

            columnOrder();
            UpdateTotalValue();
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            ResetFilterControls();
            ApplyAllFilters();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            ResetFilterControls();
            ApplyAllFilters();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Today;
            dtpToDate.Value = DateTime.Today;
            ResetFilterControls();
            ApplyAllFilters();
        }

        // Helper method to reset filter controls
        private void ResetFilterControls()
        {
            cmbOrderType.SelectedIndex = 0; // "ALL"
            cmbPaymentType.SelectedIndex = 0; // "ALL"
            txtSearchBox.Text = "";
        }

        private void SetInitialDateRange()
        {
            SetDateValuesWithoutEvents(DateTime.Today, DateTime.Today);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFromDate.Value;
            DateTime toDate = dtpToDate.Value;
            string fileName = $"{fromDate:ddMMyy}_to_{toDate:ddMMyy}.csv";

            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = fileName;
                    saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                    saveFileDialog.Title = "Save CSV File";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                        {
                            for (int i = 0; i < dgvOrderHistoryView.Columns.Count; i++)
                            {
                                if (dgvOrderHistoryView.Columns[i].Name != "dgvEdit" &&
                                    dgvOrderHistoryView.Columns[i].Name != "dgvView" &&
                                    dgvOrderHistoryView.Columns[i].Name != "dgvCancel")
                                {
                                    streamWriter.Write($"\"{dgvOrderHistoryView.Columns[i].HeaderText}\"");
                                    if (i < dgvOrderHistoryView.Columns.Count - 1)
                                        streamWriter.Write(",");
                                }
                            }
                            streamWriter.WriteLine();

                            foreach (DataGridViewRow dataGridViewRow in dgvOrderHistoryView.Rows)
                            {
                                for (int j = 0; j < dgvOrderHistoryView.Columns.Count; j++)
                                {
                                    if (dgvOrderHistoryView.Columns[j].Name != "dgvEdit" &&
                                        dgvOrderHistoryView.Columns[j].Name != "dgvView" &&
                                        dgvOrderHistoryView.Columns[j].Name != "dgvCancel")
                                    {
                                        streamWriter.Write($"\"{dataGridViewRow.Cells[j].Value?.ToString() ?? ""}\"");
                                        if (j < dgvOrderHistoryView.Columns.Count - 1)
                                            streamWriter.Write(",");
                                    }
                                }
                                streamWriter.WriteLine();
                            }
                        }

                        MessageBox.Show($"CSV file '{Path.GetFileName(saveFileDialog.FileName)}' created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvOrderHistoryView.Rows.Count == 0)
            {
                MessageBox.Show("No data to print.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (Bitmap printBitmap = new Bitmap(dgvOrderHistoryView.Width, dgvOrderHistoryView.Height))
            {
                dgvOrderHistoryView.DrawToBitmap(printBitmap, new Rectangle(0, 0, dgvOrderHistoryView.Width, dgvOrderHistoryView.Height));

                using (PrintDocument printDocument = new PrintDocument())
                {
                    printDocument.PrintPage += (s, args) => args.Graphics.DrawImage(printBitmap, 0, 0);

                    PrintDialog printDialog = new PrintDialog();
                    printDialog.Document = printDocument;

                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        this.Focus();
                        printDocument.Print();
                    }
                }
            }
        }


        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetFilters();
        }

        private DateTime GetEarliestOrderDate()
        {
            string sql = "SELECT MIN(OrderDate) AS EarliestDate FROM Orders";
            DataTable dt = MainClass.ExecuteQuery(sql, new Dictionary<string, object>());

            if (dt.Rows.Count > 0)
            {
                return Convert.ToDateTime(dt.Rows[0]["EarliestDate"]);
            }

            return DateTime.Today;
        }

        private void SetDateValuesWithoutEvents(DateTime from, DateTime to)
        {
            dtpFromDate.ValueChanged -= dtpFromDate_ValueChanged;
            dtpToDate.ValueChanged -= dtpToDate_ValueChanged;

            dtpFromDate.Value = from;
            dtpToDate.Value = to;

            dtpFromDate.ValueChanged += dtpFromDate_ValueChanged;
            dtpToDate.ValueChanged += dtpToDate_ValueChanged;
        }


        private void UpdateTotalValue()
        {
            try
            {
                // Check if we have data to calculate from
                if (dgvOrderHistoryView.DataSource == null || dgvOrderHistoryView.Rows.Count == 0)
                {
                    txtTotalValue.Text = "£0.00";
                    return;
                }

                decimal totalValue = 0;

                // Calculate total from the currently filtered/displayed rows
                foreach (DataGridViewRow row in dgvOrderHistoryView.Rows)
                {
                    // Skip rows that are cancelled or refunded
                    string paymentStatus = row.Cells["dgvPayment"].Value?.ToString();
                    if (paymentStatus != null &&
                        (paymentStatus.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase) ||
                         paymentStatus.Equals("REFUNDED", StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    // Get the total price value
                    if (row.Cells["dgvTotalPrice"].Value != null &&
                        decimal.TryParse(row.Cells["dgvTotalPrice"].Value.ToString().Replace("£", ""), out decimal price))
                    {
                        totalValue += price;
                    }
                }

                // Format and display the total value
                txtTotalValue.Text = $"£{totalValue:F2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating total value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTotalValue.Text = "£0.00";
            }
        }

        private void btnCalculations_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current date range
                DateTime fromDate = dtpFromDate.Value.Date;
                DateTime toDate = dtpToDate.Value.Date;

                // Create a new form for displaying the calculations
                Form calculationsForm = new Form
                {
                    Text = fromDate == toDate ?
                        $"Order Calculations for {fromDate:dd/MM/yyyy}" :
                        $"Order Calculations from {fromDate:dd/MM/yyyy} to {toDate:dd/MM/yyyy}",
                    Size = new Size(700, 700), // Increased size for more content
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.Sizable, // Allow resizing
                    MaximizeBox = true, // Allow maximizing
                    MinimizeBox = true
                };

                // Create a TableLayoutPanel for organizing the content
                TableLayoutPanel tableLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 3,
                    Padding = new Padding(20),
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None
                };

                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));  // Header
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));  // Footer

                // Create header label
                Label headerLabel = new Label
                {
                    Text = fromDate == toDate ?
                        $"Order Summary for {fromDate:dd MMMM yyyy}" :
                        $"Order Summary from {fromDate:dd MMM yyyy} to {toDate:dd MMM yyyy}",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };

                // Create a panel for the main content
                Panel contentPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true
                };

                // Create a FlowLayoutPanel for the footer buttons
                FlowLayoutPanel footerPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft,
                    Padding = new Padding(0, 10, 0, 0)
                };

                // Add a print button
                System.Windows.Forms.Button printButton = new System.Windows.Forms.Button
                {
                    Text = "Print",
                    Size = new Size(100, 30),
                    Margin = new Padding(5)
                };
                printButton.Click += (s, args) =>
                {
                    // Implement printing functionality here
                    MessageBox.Show("Print functionality would be implemented here.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                // Add a close button
                System.Windows.Forms.Button closeButton = new System.Windows.Forms.Button
                {
                    Text = "Close",
                    Size = new Size(100, 30),
                    Margin = new Padding(5)
                };
                closeButton.Click += (s, args) => calculationsForm.Close();


                // Add buttons to footer
                footerPanel.Controls.Add(closeButton);
                footerPanel.Controls.Add(printButton);

                // Add all panels to the table layout
                tableLayout.Controls.Add(headerLabel, 0, 0);
                tableLayout.Controls.Add(contentPanel, 0, 1);
                tableLayout.Controls.Add(footerPanel, 0, 2);

                // Add the table layout to the form
                calculationsForm.Controls.Add(tableLayout);

                // Position the form to use more of the screen
                calculationsForm.StartPosition = FormStartPosition.CenterScreen;

                // Set the form size to 80% of the screen size
                calculationsForm.Size = new Size(
                    (int)(Screen.PrimaryScreen.WorkingArea.Width * 0.4),
                    (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8)
                );

                // Now populate the content panel with the order breakdown
                PopulateOrderBreakdown(contentPanel, fromDate, toDate);

                // Show the form as a dialog
                calculationsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating calculations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void PopulateOrderBreakdown(Panel contentPanel, DateTime fromDate, DateTime toDate)
        {
            // Create a table layout for the content
            TableLayoutPanel contentTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                Padding = new Padding(10)
            };

            contentTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            contentTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            // Query to get order counts and amounts by type (excluding refunded and cancelled)
            string orderTypeQuery = @"
        SELECT 
            OrderType, 
            COUNT(*) AS OrderCount,
            SUM(TotalPrice) AS TotalAmount
        FROM 
            Orders 
        WHERE 
            OrderDate >= @FromDate AND OrderDate <= @ToDate 
            AND PaymentType NOT IN ('CANCELLED', 'REFUNDED')
        GROUP BY 
            OrderType
        ORDER BY
            OrderType";

            Dictionary<string, object> orderTypeParams = new Dictionary<string, object>
    {
        { "@FromDate", fromDate },
        { "@ToDate", toDate.AddDays(1).AddSeconds(-1) } // End of the day
    };

            DataTable orderTypeData = DatabaseManager.ExecuteQuery(orderTypeQuery, orderTypeParams);

            // Query to get refunded orders by type
            string refundedQuery = @"
        SELECT 
            OrderType, 
            COUNT(*) AS OrderCount,
            SUM(TotalPrice) AS TotalAmount
        FROM 
            Orders 
        WHERE 
            OrderDate >= @FromDate AND OrderDate <= @ToDate 
            AND PaymentType = 'REFUNDED'
        GROUP BY 
            OrderType
        ORDER BY
            OrderType";

            Dictionary<string, object> refundedParams = new Dictionary<string, object>
    {
        { "@FromDate", fromDate },
        { "@ToDate", toDate.AddDays(1).AddSeconds(-1) } // End of the day
    };

            DataTable refundedData = DatabaseManager.ExecuteQuery(refundedQuery, refundedParams);

            // Query to get payment breakdown by order type
            string paymentByTypeQuery = @"
        SELECT 
            OrderType,
            PaymentType, 
            COUNT(*) AS OrderCount,
            SUM(TotalPrice) AS TotalAmount
        FROM 
            Orders 
        WHERE 
            OrderDate >= @FromDate AND OrderDate <= @ToDate 
            AND PaymentType NOT IN ('CANCELLED', 'REFUNDED')
        GROUP BY 
            OrderType, PaymentType
        ORDER BY
            OrderType, PaymentType";

            Dictionary<string, object> paymentByTypeParams = new Dictionary<string, object>
    {
        { "@FromDate", fromDate },
        { "@ToDate", toDate.AddDays(1).AddSeconds(-1) } // End of the day
    };

            DataTable paymentByTypeData = DatabaseManager.ExecuteQuery(paymentByTypeQuery, paymentByTypeParams);

            // Calculate overall totals
            decimal totalSales = 0;
            int totalOrders = 0;
            decimal cashTotal = 0;
            decimal cardTotal = 0;
            decimal pendingTotal = 0;
            int totalRefundedOrders = 0;
            decimal totalRefundedAmount = 0;

            // Create dictionaries to store payment totals by order type
            Dictionary<string, decimal> cashByOrderType = new Dictionary<string, decimal>();
            Dictionary<string, decimal> cardByOrderType = new Dictionary<string, decimal>();
            Dictionary<string, decimal> pendingByOrderType = new Dictionary<string, decimal>();
            Dictionary<string, int> refundedCountByOrderType = new Dictionary<string, int>();
            Dictionary<string, decimal> refundedAmountByOrderType = new Dictionary<string, decimal>();

            // Process payment by type data
            if (paymentByTypeData != null && paymentByTypeData.Rows.Count > 0)
            {
                foreach (DataRow paymentRow in paymentByTypeData.Rows)
                {
                    string orderType = paymentRow["OrderType"].ToString();
                    string paymentType = paymentRow["PaymentType"].ToString();
                    decimal amount = Convert.ToDecimal(paymentRow["TotalAmount"]);

                    // Track totals by payment type
                    if (paymentType.Equals("CASH", StringComparison.OrdinalIgnoreCase))
                    {
                        cashTotal += amount;
                        if (!cashByOrderType.ContainsKey(orderType))
                            cashByOrderType[orderType] = 0;
                        cashByOrderType[orderType] += amount;
                    }
                    else if (paymentType.Equals("CARD", StringComparison.OrdinalIgnoreCase))
                    {
                        cardTotal += amount;
                        if (!cardByOrderType.ContainsKey(orderType))
                            cardByOrderType[orderType] = 0;
                        cardByOrderType[orderType] += amount;
                    }
                    else if (paymentType.Equals("PENDING", StringComparison.OrdinalIgnoreCase))
                    {
                        pendingTotal += amount;
                        if (!pendingByOrderType.ContainsKey(orderType))
                            pendingByOrderType[orderType] = 0;
                        pendingByOrderType[orderType] += amount;
                    }
                }
            }

            // Process refunded data
            if (refundedData != null && refundedData.Rows.Count > 0)
            {
                foreach (DataRow refundRow in refundedData.Rows)
                {
                    string orderType = refundRow["OrderType"].ToString();
                    int count = Convert.ToInt32(refundRow["OrderCount"]);
                    decimal amount = Convert.ToDecimal(refundRow["TotalAmount"]);

                    totalRefundedOrders += count;
                    totalRefundedAmount += amount;

                    refundedCountByOrderType[orderType] = count;
                    refundedAmountByOrderType[orderType] = amount;
                }
            }

            // Add section headers
            AddSectionHeader(contentTable, "Order Details", 0);
            AddSectionHeader(contentTable, "Amount", 1);

            int currentRow = 1;

            // Add order type breakdown with payment details
            if (orderTypeData != null && orderTypeData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in orderTypeData.Rows)
                {
                    string orderType = dataRow["OrderType"].ToString();
                    int count = Convert.ToInt32(dataRow["OrderCount"]);
                    decimal amount = Convert.ToDecimal(dataRow["TotalAmount"]);

                    totalOrders += count;
                    totalSales += amount;

                    // Get cash, card, and pending amounts for this order type
                    decimal cashAmount = cashByOrderType.ContainsKey(orderType) ? cashByOrderType[orderType] : 0;
                    decimal cardAmount = cardByOrderType.ContainsKey(orderType) ? cardByOrderType[orderType] : 0;
                    decimal pendingAmount = pendingByOrderType.ContainsKey(orderType) ? pendingByOrderType[orderType] : 0;

                    // Add a header for this order type
                    AddOrderTypeHeader(contentTable, orderType, currentRow++);

                    // Add order count on a separate row
                    AddDetailRow(contentTable, "Orders:", $"{count}", 0, currentRow++);

                    // Add cash, card, and pending breakdown for this order type
                    if (cashAmount > 0)
                        AddDetailRow(contentTable, "Cash:", $"£{cashAmount:F2}", 0, currentRow++);
                    if (cardAmount > 0)
                        AddDetailRow(contentTable, "Card:", $"£{cardAmount:F2}", 0, currentRow++);
                    if (pendingAmount > 0)
                        AddDetailRow(contentTable, "Pending:", $"£{pendingAmount:F2}", 0, currentRow++);

                    // Add refunded information if applicable
                    if (refundedCountByOrderType.ContainsKey(orderType) && refundedCountByOrderType[orderType] > 0)
                    {
                        int refundedCount = refundedCountByOrderType[orderType];
                        decimal refundedAmount = refundedAmountByOrderType[orderType];

                        AddDetailRow(contentTable, "Refunded Orders:", $"{refundedCount}", 0, currentRow++);
                        AddDetailRow(contentTable, "Refunded Amount:", $"£{refundedAmount:F2}", 0, currentRow++);
                    }

                    // Add a total for this order type
                    AddSectionTotal(contentTable, "Total:", $"£{amount:F2}", currentRow++);

                    // Add a small gap after each order type
                    AddEmptyRow(contentTable, currentRow++);
                }
            }
            else
            {
                AddDetailRow(contentTable, "No orders found", "", 0, currentRow++);
            }

            // Add a separator row
            AddSeparatorRow(contentTable, currentRow++, 2);

            // Add total rows for all orders
            AddSummaryRow(contentTable, "Total Orders:", $"{totalOrders}", currentRow++);
            AddSummaryRow(contentTable, "Total Sales:", $"£{totalSales:F2}", currentRow++);

            // Add cash total (including pending amounts as cash)
            decimal adjustedCashTotal = cashTotal + pendingTotal;
            AddSummaryRow(contentTable, "Cash Total:", $"£{adjustedCashTotal:F2}", currentRow++);

            // Show pending amount separately if it exists
            if (pendingTotal > 0)
                AddDetailRow(contentTable, "   (Includes Pending):", $"£{pendingTotal:F2}", 0, currentRow++);

            AddSummaryRow(contentTable, "Card Total:", $"£{cardTotal:F2}", currentRow++);

            // Add refunded information to the summary if applicable
            if (totalRefundedOrders > 0)
            {
                AddEmptyRow(contentTable, currentRow++);
                AddSummaryRow(contentTable, "Refunded Orders:", $"{totalRefundedOrders}", currentRow++);
                AddSummaryRow(contentTable, "Refunded Amount:", $"£{totalRefundedAmount:F2}", currentRow++);
                AddDetailRow(contentTable, "(Not included in totals)", "", 0, currentRow++);
            }

            // Add the content table to the panel
            contentPanel.Controls.Add(contentTable);
        }



        private void AddOrderTypeHeader(TableLayoutPanel table, string text, int row)
        {
            Label headerLabel = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            // Make sure we have enough rows
            EnsureRowExists(table, row);

            table.Controls.Add(headerLabel, 0, row);
            table.SetColumnSpan(headerLabel, 2);
        }

        // Add a new method for section totals
        private void AddSectionTotal(TableLayoutPanel table, string label, string value, int row)
        {
            Label labelControl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            Label valueControl = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            // Make sure we have enough rows
            EnsureRowExists(table, row);

            table.Controls.Add(labelControl, 0, row);
            table.Controls.Add(valueControl, 1, row);
        }

        private void AddSectionHeader(TableLayoutPanel table, string text, int column)
        {
            Label headerLabel = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            // Make sure we have enough rows
            EnsureRowExists(table, 0);

            table.Controls.Add(headerLabel, column, 0);
        }

        private void AddDetailRow(TableLayoutPanel table, string label, string value, int column, int row)
        {
            Label labelControl = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            // Make sure we have enough rows
            EnsureRowExists(table, row);

            table.Controls.Add(labelControl, column, row);

            if (!string.IsNullOrEmpty(value))
            {
                Label valueControl = new Label
                {
                    Text = value,
                    Font = new Font("Segoe UI", 10),
                    TextAlign = ContentAlignment.MiddleRight,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(5)
                };

                table.Controls.Add(valueControl, column + 1, row);
            }
            else
            {
                table.SetColumnSpan(labelControl, 2);
            }
        }


        private void AddEmptyRow(TableLayoutPanel table, int row)
        {
            Label emptyLabel = new Label
            {
                Text = "",
                Height = 5,
                Dock = DockStyle.Fill
            };

            // Make sure we have enough rows
            EnsureRowExists(table, row);

            table.Controls.Add(emptyLabel, 0, row);
            table.SetColumnSpan(emptyLabel, 2);
        }


        private void AddSeparatorRow(TableLayoutPanel table, int row, int columnSpan)
        {
            Panel separatorPanel = new Panel
            {
                Height = 2,
                BackColor = Color.Black,
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 10, 5, 10)
            };

            // Make sure we have enough rows
            EnsureRowExists(table, row);

            table.Controls.Add(separatorPanel, 0, row);
            table.SetColumnSpan(separatorPanel, columnSpan);
        }

        private void AddSummaryRow(TableLayoutPanel table, string label, string value, int row)
        {
            var summaryFont = new Font("Segoe UI", 13, FontStyle.Bold);

            Label labelControl = new Label
            {
                Text = label,
                Font = summaryFont,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            Label valueControl = new Label
            {
                Text = value,
                Font = summaryFont,
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };

            // Make sure we have enough rows
            EnsureRowExists(table, row);

            table.Controls.Add(labelControl, 0, row);
            table.Controls.Add(valueControl, 1, row);
        }




        private void EnsureRowExists(TableLayoutPanel table, int rowIndex)
        {
            while (table.RowCount <= rowIndex)
            {
                table.RowCount++;
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
        }

    }
}
