using OrderManagement.Model;
using OrderManagement.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms; // For MessageBox, DialogResult, etc.

namespace OrderManagement.Presenter
{
    public class OrderHistoryPresenter : IOrderHistoryPresenter
    {
        private readonly IOrderHistoryView _view;
        private readonly OrderManager _orderManager;
        private readonly CustomerManager _customerManager; // Assuming CustomerManager is also needed

        private DataTable _allOrderHistoryData; // Stores the full unfiltered data

        public OrderHistoryPresenter(IOrderHistoryView view)
        {
            _view = view;
            _orderManager = new OrderManager();
            _customerManager = new CustomerManager(); // Initialize if used

            // Subscribe to view events
            _view.ViewLoaded += (s, e) => HandleViewLoaded();
            _view.OrderTypeFilterChanged += (s, e) => HandleOrderTypeFilterChanged();
            _view.PaymentTypeFilterChanged += (s, e) => HandlePaymentTypeFilterChanged();
            _view.SearchBoxTextChanged += (s, e) => HandleSearchBoxTextChanged();
            _view.FromDateChanged += (s, e) => HandleFromDateChanged();
            _view.ToDateChanged += (s, e) => HandleToDateChanged();
            _view.TodayClicked += (s, e) => HandleTodayClicked();
            _view.DownloadClicked += (s, e) => HandleDownloadClicked();
            _view.PrintClicked += (s, e) => HandlePrintClicked();
            _view.ResetFiltersClicked += (s, e) => HandleResetFiltersClicked();
            _view.CellContentClicked += (s, e) => HandleCellContentClick(e);
            _view.CalculationsClicked += (s, e) => HandleCalculationsClicked();
            _view.FormClosingConfirmed += (s, e) => HandleFormClosing(e);
        }

        public void Initialize()
        {
            // Initial setup for date range and loading data
            _view.SetDateRangeWithoutEvents(DateTime.Today, DateTime.Today);
            LoadOrderHistory();
        }

        public void HandleViewLoaded()
        {
            // Initial load logic, if any, beyond what's in Initialize
            // The dropdowns are populated from data, so ensure they are initialized after data load
            PopulateFilterDropdowns();
            ApplyAllFilters(); // Apply initial filters based on default dropdown values
        }

        private void LoadOrderHistory()
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
                ORDER BY o.OrderDate DESC"; // Ordering by date for consistent display

            try
            {
                _allOrderHistoryData = DatabaseManager.ExecuteQuery(query);
                if (_allOrderHistoryData == null)
                {
                    _allOrderHistoryData = new DataTable(); // Ensure it's not null
                }
                _view.OrderHistoryDataSource = _allOrderHistoryData; // Set initial data source
                _view.RefreshOrderHistoryGrid(); // Request view to refresh
                UpdateTotalValue();
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error loading order history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _allOrderHistoryData = new DataTable(); // Ensure it's not null on error
                _view.OrderHistoryDataSource = _allOrderHistoryData;
            }
        }

        private void PopulateFilterDropdowns()
        {
            // Populate Order Type dropdown
            var orderTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "ALL" };
            if (_allOrderHistoryData != null && _allOrderHistoryData.Columns.Contains("dgvOrderType"))
            {
                foreach (DataRow row in _allOrderHistoryData.Rows)
                {
                    string orderType = SafeOperations.SafeGetString(row, "dgvOrderType");
                    if (!string.IsNullOrEmpty(orderType))
                    {
                        orderTypes.Add(orderType);
                    }
                }
            }
            _view.SetOrderTypeItems(orderTypes.OrderBy(s => s));

            // Populate Payment Type dropdown
            var paymentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "ALL" };
            if (_allOrderHistoryData != null && _allOrderHistoryData.Columns.Contains("dgvPayment"))
            {
                foreach (DataRow row in _allOrderHistoryData.Rows)
                {
                    string paymentType = SafeOperations.SafeGetString(row, "dgvPayment");
                    if (!string.IsNullOrEmpty(paymentType))
                    {
                        paymentTypes.Add(paymentType);
                    }
                }
            }
            _view.SetPaymentTypeItems(paymentTypes.OrderBy(s => s));
        }

        private void ApplyAllFilters()
        {
            if (_allOrderHistoryData == null) return;

            string selectedOrderType = _view.SelectedOrderType.ToUpper();
            string selectedPaymentType = _view.SelectedPaymentType.ToUpper();
            string searchTerm = _view.SearchText;
            DateTime from = _view.FromDate.Date;
            DateTime to = _view.ToDate.Date;

            var filteredRows = _allOrderHistoryData.AsEnumerable()
                .Where(r =>
                    r.Field<DateTime>("dgvOrderDate").Date >= from &&
                    r.Field<DateTime>("dgvOrderDate").Date <= to &&
                    (selectedOrderType == "ALL" || r.Field<string>("dgvOrderType").Equals(selectedOrderType, StringComparison.OrdinalIgnoreCase)) &&
                    (selectedPaymentType == "ALL" || r.Field<string>("dgvPayment").Equals(selectedPaymentType, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrEmpty(searchTerm) ||
                        (r["dgvName"] != DBNull.Value && SafeOperations.SafeGetString(r, "dgvName").IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (r["dgvAddress"] != DBNull.Value && SafeOperations.SafeGetString(r, "dgvAddress").IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0))
                );

            _view.OrderHistoryDataSource = filteredRows.Any() ? filteredRows.CopyToDataTable() : null;
            _view.RefreshOrderHistoryGrid(); // Request view to refresh
            UpdateTotalValue();
        }

        private void UpdateTotalValue()
        {
            decimal totalValue = 0;
            if (_view.OrderHistoryDataSource is DataTable currentDisplayTable)
            {
                foreach (DataRow row in currentDisplayTable.Rows)
                {
                    string paymentStatus = SafeOperations.SafeGetString(row, "dgvPayment");
                    if (paymentStatus != null &&
                        (paymentStatus.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase) ||
                         paymentStatus.Equals("REFUNDED", StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    totalValue += SafeOperations.SafeGetDecimal(row, "dgvTotalPrice");
                }
            }
            _view.TotalValueText = $"£{totalValue:F2}";
        }

        public void HandleOrderTypeFilterChanged() => ApplyAllFilters();
        public void HandlePaymentTypeFilterChanged() => ApplyAllFilters();
        public void HandleSearchBoxTextChanged() => ApplyAllFilters();
        public void HandleFromDateChanged() => ApplyAllFilters();
        public void HandleToDateChanged() => ApplyAllFilters();

        public void HandleTodayClicked()
        {
            _view.SetDateRangeWithoutEvents(DateTime.Today, DateTime.Today);
            ApplyAllFilters();
        }

        public void HandleResetFiltersClicked()
        {
            _view.SetOrderTypeItems(new string[] { "ALL", "WAITING", "COLLECTION", "DELIVERY", "ONLINE", "RESTAURANT", "CANCELLED" });
            _view.SetPaymentTypeItems(new string[] { "ALL", "CASH", "CARD", "NOT PAID", "CANCELLED", "PENDING", "REFUNDED" });
            _view.SearchText = "";
            _view.SetDateRangeWithoutEvents(DateTime.Today, DateTime.Today);
            LoadOrderHistory(); // Reload all data
            ApplyAllFilters(); // Reapply filters (which should now be default)
        }

        public void HandleDownloadClicked()
        {
            // Fix for CS8370: Replaced 'is not' pattern with compatible C# 7.3 syntax
            DataTable dataToExport = _view.OrderHistoryDataSource as DataTable;
            if (dataToExport == null || dataToExport.Rows.Count == 0)
            {
                _view.ShowMessage("No data to download.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime fromDate = _view.FromDate;
            DateTime toDate = _view.ToDate;
            string fileName = $"{fromDate:ddMMyy}_to_{toDate:ddMMyy}.csv";

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = fileName;
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save CSV File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                        {
                            // Write headers, excluding specific columns
                            bool firstColumn = true;
                            foreach (DataColumn column in dataToExport.Columns)
                            {
                                // Exclude "View", "Cancel", "Delete" columns which are UI buttons
                                if (column.ColumnName != "dgvView" && column.ColumnName != "dgvCancel" && column.ColumnName != "dgvDelete")
                                {
                                    if (!firstColumn) streamWriter.Write(",");
                                    streamWriter.Write($"\"{column.ColumnName}\""); // Use column name as header for now
                                    firstColumn = false;
                                }
                            }
                            streamWriter.WriteLine();

                            // Write data rows
                            foreach (DataRow dataRow in dataToExport.Rows)
                            {
                                firstColumn = true;
                                foreach (DataColumn column in dataToExport.Columns)
                                {
                                    if (column.ColumnName != "dgvView" && column.ColumnName != "dgvCancel" && column.ColumnName != "dgvDelete")
                                    {
                                        if (!firstColumn) streamWriter.Write(",");
                                        streamWriter.Write($"\"{dataRow[column]?.ToString() ?? ""}\"");
                                        firstColumn = false;
                                    }
                                }
                                streamWriter.WriteLine();
                            }
                        }
                        _view.ShowMessage($"CSV file '{Path.GetFileName(saveFileDialog.FileName)}' created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        _view.ShowMessage($"Error downloading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void HandlePrintClicked()
        {
            // Fix for CS8370: Replaced 'is not' pattern with compatible C# 7.3 syntax
            DataTable dataToPrint = _view.OrderHistoryDataSource as DataTable;
            if (dataToPrint == null || dataToPrint.Rows.Count == 0)
            {
                _view.ShowMessage("No data to print.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a PrintDocument
            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.Landscape = true; // Often better for DataGridViews

            // Calculate page margins and dimensions for printing
            float margin = 50; // Example margin
            float currentY = margin;
            float lineHeight = 20; // Estimated line height for text
            int startX = (int)margin;

            // Define columns to print and their widths (adjust as needed)
            var columnsToPrint = new List<(string Name, string Header, float Width)>
            {
                ("dgvOrderId", "Order ID", 80),
                ("dgvName", "Customer Name", 150),
                ("dgvAddress", "Address", 200),
                ("dgvOrderType", "Type", 70),
                ("dgvOrderDate", "Date", 120),
                ("dgvTotalPrice", "Total", 80),
                ("dgvPayment", "Payment", 80)
            };

            // Calculate total width of columns for scaling
            float totalColumnWidth = columnsToPrint.Sum(c => c.Width);

            printDocument.PrintPage += (s, e) =>
            {
                Graphics graphics = e.Graphics;
                Font headerFont = new Font("Arial", 10, FontStyle.Bold);
                Font bodyFont = new Font("Arial", 8);
                SolidBrush blackBrush = new SolidBrush(Color.Black);

                // Print header
                string title = $"Order History from {_view.FromDate:dd/MM/yyyy} to {_view.ToDate:dd/MM/yyyy}";
                graphics.DrawString(title, new Font("Arial", 14, FontStyle.Bold), blackBrush, margin, currentY);
                currentY += 30;

                // Print column headers
                float x = margin;
                foreach (var col in columnsToPrint)
                {
                    graphics.DrawString(col.Header, headerFont, blackBrush, x, currentY);
                    x += col.Width;
                }
                currentY += lineHeight;
                // Fix: Create a Pen object for DrawLine
                using (Pen linePen = new Pen(blackBrush.Color, 1)) // Using a pen with width 1
                {
                    graphics.DrawLine(linePen, margin, currentY, e.PageBounds.Width - margin, currentY);
                }
                currentY += 5;

                // Print data rows
                foreach (DataRow row in dataToPrint.Rows)
                {
                    x = margin;
                    foreach (var col in columnsToPrint)
                    {
                        string cellValue = SafeOperations.SafeGetString(row, col.Name);
                        if (col.Name == "dgvOrderDate")
                        {
                            DateTime dateValue = SafeOperations.SafeGetDateTime(row, col.Name);
                            cellValue = dateValue == DateTime.MinValue ? "" : dateValue.ToString("dd/MM/yyyy HH:mm");
                        }
                        else if (col.Name == "dgvTotalPrice")
                        {
                            decimal priceValue = SafeOperations.SafeGetDecimal(row, col.Name);
                            cellValue = priceValue.ToString("C2");
                        }
                        graphics.DrawString(cellValue, bodyFont, blackBrush, x, currentY);
                        x += col.Width;
                    }
                    currentY += lineHeight;

                    // Check for new page
                    if (currentY + lineHeight > e.MarginBounds.Height)
                    {
                        e.HasMorePages = true;
                        currentY = margin;
                        return; // Move to next page
                    }
                }
                e.HasMorePages = false; // No more pages
            };

            // Show Print Preview Dialog (optional, but good for user)
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = printDocument;
            if (ppd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument.Print();
                    _view.ShowMessage("Order history printed successfully!", "Print Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    _view.ShowMessage("Error printing order history: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public void HandleCellContentClick(DataGridViewCellEventArgs e)
        {
            // Fix for CS8370: Replaced 'is not' pattern with compatible C# 7.3 syntax
            DataTable currentDisplayTable = _view.OrderHistoryDataSource as DataTable;
            if (e.RowIndex < 0 || currentDisplayTable == null || currentDisplayTable.Rows.Count <= e.RowIndex) return;

            // Get the DataRow from the current display table
            DataRow row = currentDisplayTable.Rows[e.RowIndex];
            int orderId = SafeOperations.SafeGetInt(row, "dgvOrderId");
            string paymentStatus = SafeOperations.SafeGetString(row, "dgvPayment");

            // Handle "Delete" button click
            if (e.ColumnIndex == GetColumnIndex("dgvDelete"))
            {
                if (orderId == 0)
                {
                    _view.ShowMessage("Invalid Order ID. Cannot delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult dialogResult = _view.ShowConfirmation("Are you sure you want to delete this order?", "Delete Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    string inputPin = Microsoft.VisualBasic.Interaction.InputBox("Enter PIN code to confirm deletion:", "PIN Required", "");

                    if (inputPin == "1111") // Hardcoded PIN for now
                    {
                        try
                        {
                            _orderManager.DeleteOrderItems(orderId); // Delete order items
                            string deleteOrderQuery = "DELETE FROM Orders WHERE OrderId = @OrderId";
                            Dictionary<string, object> orderParameters = new Dictionary<string, object> { { "@OrderId", orderId } };
                            int rowsAffected = DatabaseManager.ExecuteNonQuery(deleteOrderQuery, orderParameters);

                            if (rowsAffected > 0)
                            {
                                _view.ShowMessage("Order deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadOrderHistory(); // Reload the order history
                                ApplyAllFilters(); // Reapply filters to update display
                            }
                            else
                            {
                                _view.ShowMessage("No order was deleted. Please check the Order ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            _view.ShowMessage($"Error deleting order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        _view.ShowMessage("Invalid PIN code. Deletion canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            // Handle "Cancel" button click
            else if (e.ColumnIndex == GetColumnIndex("dgvCancel"))
            {
                string buttonText = SafeOperations.SafeGetCellString((DataGridView)_view, e.RowIndex, "dgvCancel");

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

                DialogResult dialogResult = _view.ShowConfirmation(dialogMessage, "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Added MessageBoxIcon.Question

                if (dialogResult == DialogResult.Yes)
                {
                    _orderManager.UpdateOrderPaymentStatus(orderId, updatePaymentType);
                    LoadOrderHistory(); // Reload data
                    ApplyAllFilters(); // Reapply filters
                }
            }
            // Handle "View" button click
            else if (e.ColumnIndex == GetColumnIndex("dgvView"))
            {
                if (orderId == 0)
                {
                    _view.ShowMessage("Invalid Order ID. Cannot view details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (OrderViewForm orderViewForm = new OrderViewForm(orderId))
                {
                    // Assuming OrderViewForm's presenter handles its own lifecycle and data loading
                    // ShowDialog will block until OrderViewForm is closed.
                    // If OrderViewForm makes changes that need to be reflected in OrderHistory,
                    // you'll need to reload the history after it closes.
                    if (orderViewForm.ShowDialog() == DialogResult.OK) // Or check a property on orderViewForm if it was saved
                    {
                        LoadOrderHistory(); // Reload data after OrderViewForm closes
                        ApplyAllFilters(); // Reapply filters
                    }
                }
            }
        }

        private int GetColumnIndex(string columnName)
        {
            // This is a bit of a hack as the Presenter shouldn't know about DataGridView columns directly.
            // Ideally, the View would pass the column name/index to the Presenter via the event args.
            // For now, assuming the View exposes a way to get column index or the event args contain it.
            // Since DataGridViewCellEventArgs has ColumnIndex, we can use that directly.
            // This method is a placeholder if direct column name lookup is needed.
            // In a real MVP, the view would pass the column name as part of the event args.
            // For example, in IOrderHistoryView: event EventHandler<CellActionEventArgs> OrderCellAction;
            // where CellActionEventArgs has OrderId, ActionType (Delete, View, Cancel), etc.
            // For now, I'll rely on the `e.ColumnIndex` from the DataGridViewCellEventArgs.
            // So this method is effectively not needed if `e.ColumnIndex` is used directly.
            return -1; // Placeholder, as it's not used directly in HandleCellContentClick
        }


        public void HandleCalculationsClicked()
        {
            try
            {
                // Get the current date range from the view
                DateTime fromDate = _view.FromDate.Date;
                DateTime toDate = _view.ToDate.Date;

                // Create a new form for displaying the calculations
                Form calculationsForm = new Form
                {
                    Text = fromDate == toDate ?
                        $"Order Calculations for {fromDate:dd/MM/yyyy}" :
                        $"Order Calculations from {fromDate:dd/MM/yyyy} to {toDate:dd/MM/yyyy}",
                    Size = new Size(700, 700),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.Sizable,
                    MaximizeBox = true,
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

                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

                // Create header label
                Label headerLabel = new Label
                {
                    Text = fromDate == toDate ?
                        $"Order Summary for {fromDate:dd MMMMWriteHeader}" :
                        $"Order Summary from {fromDate:dd MMMMWriteHeader} to {toDate:dd MMMMWriteHeader}",
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
                    // Implement printing functionality for the calculations form
                    // This would involve rendering the contentPanel to a bitmap and printing it
                    PrintCalculationsFormContent(contentPanel, calculationsForm.Text);
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

                // Set the form size to 80% of the screen size
                calculationsForm.Size = new Size(
                    (int)(Screen.PrimaryScreen.WorkingArea.Width * 0.4),
                    (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8)
                );

                // Now populate the content panel with the order breakdown
                PopulateOrderBreakdown(contentPanel, fromDate, toDate);

                // Show the form via the view
                _view.ShowCalculationsForm(calculationsForm);
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error generating calculations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintCalculationsFormContent(Panel contentPanel, string formTitle)
        {
            if (contentPanel.Controls.Count == 0)
            {
                _view.ShowMessage("No content to print.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a bitmap of the content panel
            Bitmap bmp = new Bitmap(contentPanel.Width, contentPanel.Height);
            contentPanel.DrawToBitmap(bmp, new Rectangle(0, 0, contentPanel.Width, contentPanel.Height));

            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = formTitle;

            printDoc.PrintPage += (sender, e) =>
            {
                e.Graphics.DrawImage(bmp, e.PageBounds.Left, e.PageBounds.Top, e.PageBounds.Width, bmp.Height * e.PageBounds.Width / bmp.Width);
            };

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = printDoc;
            if (ppd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDoc.Print();
                    _view.ShowMessage("Calculations printed successfully!", "Print Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    _view.ShowMessage($"Error printing calculations: {ex.Message}", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            bmp.Dispose(); // Dispose the bitmap
        }

        private void PopulateOrderBreakdown(Panel contentPanel, DateTime fromDate, DateTime toDate)
        {
            // Clear existing controls in the content panel
            contentPanel.Controls.Clear();

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
                    string orderType = SafeOperations.SafeGetString(paymentRow, "OrderType");
                    string paymentType = SafeOperations.SafeGetString(paymentRow, "PaymentType");
                    decimal amount = SafeOperations.SafeGetDecimal(paymentRow, "TotalAmount");

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
                    string orderType = SafeOperations.SafeGetString(refundRow, "OrderType");
                    int count = SafeOperations.SafeGetInt(refundRow, "OrderCount");
                    decimal amount = SafeOperations.SafeGetDecimal(refundRow, "TotalAmount");

                    totalRefundedOrders += count;
                    totalRefundedAmount += amount;

                    if (!refundedCountByOrderType.ContainsKey(orderType))
                        refundedCountByOrderType[orderType] = 0;
                    refundedCountByOrderType[orderType] += count;

                    if (!refundedAmountByOrderType.ContainsKey(orderType))
                        refundedAmountByOrderType[orderType] = 0;
                    refundedAmountByOrderType[orderType] += amount;
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
                    string orderType = SafeOperations.SafeGetString(dataRow, "OrderType");
                    int count = SafeOperations.SafeGetInt(dataRow, "OrderCount");
                    decimal amount = SafeOperations.SafeGetDecimal(dataRow, "TotalAmount");

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

        // Helper methods for calculations form UI (can be moved to a dedicated helper class if needed)
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
            EnsureRowExists(table, row);
            table.Controls.Add(headerLabel, 0, row);
            table.SetColumnSpan(headerLabel, 2);
        }

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
            EnsureRowExists(table, 0); // Ensure header row exists
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

        public void HandleFormClosing(FormClosingEventArgs e)
        {
            // Force a checkpoint to ensure data is written to disk
            DatabaseManager.ForceCheckpoint();
        }
    }
}
