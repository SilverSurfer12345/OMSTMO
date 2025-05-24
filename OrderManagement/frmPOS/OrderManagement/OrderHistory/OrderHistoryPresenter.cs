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
            // Event subscription updated to use the custom EventArgs
            _view.CellContentClicked += (s, e) => HandleCellContentClick((OrderHistoryCellClickEventArgs)e);
            _view.CalculationsClicked += (s, e) => HandleCalculationsClicked();
            _view.FormClosingConfirmed += (s, e) => HandleFormClosing(e);
        }

        public void Initialize()
        {
            // Initial setup for date range and loading data
            _view.SetDateRangeWithoutEvents(DateTime.Today, DateTime.Today);
            LoadOrderHistory(); // Load all data
            ApplyAllFilters(); // Apply initial filters
        }

        public void HandleViewLoaded()
        {
            // Initial load logic, if any, beyond what's in Initialize
            // The dropdowns are populated from data, so ensure they are initialized after data load
            PopulateFilterDropdowns();
            ApplyAllFilters(); // Apply initial filters based on default dropdown values
        }

        /// <summary>
        /// Loads all order history data from the database into _allOrderHistoryData.
        /// This method does NOT update the DataGridView directly.
        /// </summary>
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
                // Removed direct DataGridView update here. ApplyAllFilters will handle it.
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error loading order history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _allOrderHistoryData = new DataTable(); // Ensure it's not null on error
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

        /// <summary>
        /// Applies all active filters (date, order type, payment type, search text)
        /// to the _allOrderHistoryData and updates the DataGridView.
        /// </summary>
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
            
            // Define the desired column order array
            string[] columnOrder = new string[] 
            { 
                "dgvOrderId", "dgvName", "dgvAddress", "dgvOrderType", 
                "dgvOrderDate", "dgvTotalPrice", "dgvPayment", 
                "dgvView", "dgvCancel", "dgvDelete" 
            };
            
            // Restore column order after setting the DataSource
            _view.SetColumnOrder(columnOrder);
            
            _view.RefreshOrderHistoryGrid();
            UpdateTotalValue(); // Update total value after filtering
        }

        /// <summary>
        /// Updates the total value displayed in the view based on the currently
        /// filtered and displayed orders in the DataGridView.
        /// Excludes cancelled and refunded orders from the total.
        /// </summary>
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
            // Reset dropdowns and search text to default
            _view.SetOrderTypeItems(new string[] { "ALL", "WAITING", "COLLECTION", "DELIVERY", "ONLINE", "RESTAURANT", "CANCELLED" });
            _view.SetPaymentTypeItems(new string[] { "ALL", "CASH", "CARD", "NOT PAID", "CANCELLED", "PENDING", "REFUNDED" });
            _view.SearchText = "";
            _view.SetDateRangeWithoutEvents(DateTime.Today, DateTime.Today); // Reset date range
            LoadOrderHistory(); // Reload all data from DB to ensure _allOrderHistoryData is fresh
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
                        // Removed success message as per user request
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

                    // Check if more pages are needed
                    if (currentY + lineHeight > e.PageBounds.Height - margin)
                    {
                        e.HasMorePages = true;
                        currentY = margin; // Reset for next page
                        return; // Stop printing on current page
                    }
                }
                e.HasMorePages = false; // No more pages
            };

            // Show the Print Preview Dialog
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();
        }


        public void HandleCalculationsClicked()
        {
            if (_allOrderHistoryData == null || _allOrderHistoryData.Rows.Count == 0)
            {
                _view.ShowMessage("No data available for calculations.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get the current filter date range
            DateTime fromDate = _view.FromDate;
            DateTime toDate = _view.ToDate;

            // Filter orders based on the current date range
            var filteredOrders = _allOrderHistoryData.AsEnumerable()
                .Where(r => r.Field<DateTime>("dgvOrderDate").Date >= fromDate.Date &&
                            r.Field<DateTime>("dgvOrderDate").Date <= toDate.Date);

            if (!filteredOrders.Any())
            {
                _view.ShowMessage("No orders found in the selected date range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Calculate order type statistics
            Dictionary<string, int> orderTypeCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, decimal> orderTypeTotal = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            // Calculate payment type statistics
            Dictionary<string, int> paymentTypeCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, decimal> paymentTypeTotal = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            int totalOrders = 0;
            decimal grandTotal = 0;

            foreach (var order in filteredOrders)
            {
                string orderType = SafeOperations.SafeGetString(order, "dgvOrderType");
                string paymentType = SafeOperations.SafeGetString(order, "dgvPayment");
                decimal totalPrice = SafeOperations.SafeGetDecimal(order, "dgvTotalPrice");

                // Skip cancelled orders for grand total calculation
                if (!string.Equals(paymentType, "CANCELLED", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(paymentType, "REFUNDED", StringComparison.OrdinalIgnoreCase))
                {
                    grandTotal += totalPrice;
                    totalOrders++;
                }

                // Order Type Statistics
                if (!string.IsNullOrEmpty(orderType))
                {
                    // Increment count
                    if (!orderTypeCount.ContainsKey(orderType))
                    {
                        orderTypeCount[orderType] = 0;
                        orderTypeTotal[orderType] = 0;
                    }
                    orderTypeCount[orderType]++;
                    orderTypeTotal[orderType] += totalPrice;
                }

                // Payment Type Statistics
                if (!string.IsNullOrEmpty(paymentType))
                {
                    // Increment count
                    if (!paymentTypeCount.ContainsKey(paymentType))
                    {
                        paymentTypeCount[paymentType] = 0;
                        paymentTypeTotal[paymentType] = 0;
                    }
                    paymentTypeCount[paymentType]++;
                    paymentTypeTotal[paymentType] += totalPrice;
                }
            }

            // Create and show the sales report form
            frmSalesReport salesReportForm = new frmSalesReport();
            salesReportForm.LoadReportData(
                fromDate, toDate,
                orderTypeCount, orderTypeTotal,
                paymentTypeCount, paymentTypeTotal,
                totalOrders, grandTotal
            );

            _view.ShowCalculationsForm(salesReportForm);
        }

        public void HandleCellContentClick(OrderHistoryCellClickEventArgs e)
        {
            // Use the OrderId and ColumnName passed from the view
            string columnName = e.ColumnName;
            int orderId = e.OrderId;

            if (orderId == 0)
            {
                _view.ShowMessage("Could not retrieve Order ID for the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (columnName == "dgvView")
            {
                _view.ShowOrderViewForm(orderId);
            }
            else if (columnName == "dgvCancel")
            {
                // Find the row in the in-memory DataTable
                DataRow rowToUpdate = _allOrderHistoryData.AsEnumerable()
                                        .FirstOrDefault(r => SafeOperations.SafeGetInt(r, "dgvOrderId") == orderId);

                if (rowToUpdate != null)
                {
                    string currentPaymentStatus = SafeOperations.SafeGetString(rowToUpdate, "dgvPayment");

                    if (currentPaymentStatus.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase))
                    {
                        // If currently CANCELLED, uncancel it (set to PENDING or NOT PAID based on business logic)
                        _orderManager.UpdateOrderPaymentStatus(orderId, "PENDING"); // Update DB
                        rowToUpdate["dgvPayment"] = "PENDING"; // Update in-memory DataTable
                        rowToUpdate["dgvOrderType"] = "PENDING"; // Assuming order type also changes
                    }
                    else
                    {
                        // If not CANCELLED, cancel it
                        _orderManager.CancelOrder(orderId); // Update DB
                        rowToUpdate["dgvPayment"] = "CANCELLED"; // Update in-memory DataTable
                        rowToUpdate["dgvOrderType"] = "CANCELLED"; // Assuming order type also changes
                    }
                    ApplyAllFilters(); // Reapply filters to update display and total value
                }
            }
            else if (columnName == "dgvDelete")
            {
                // Show confirmation dialog before deleting
                DialogResult result = _view.ShowConfirmation(
                    "Are you sure you want to delete this order? This action cannot be undone.",
                    "Confirm Deletion",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    _orderManager.DeleteOrder(orderId); // Delete from DB

                    // Remove the row from the in-memory DataTable
                    DataRow rowToDelete = _allOrderHistoryData.AsEnumerable()
                                            .FirstOrDefault(r => SafeOperations.SafeGetInt(r, "dgvOrderId") == orderId);
                    if (rowToDelete != null)
                    {
                        _allOrderHistoryData.Rows.Remove(rowToDelete);
                        _allOrderHistoryData.AcceptChanges(); // Commit the removal
                    }
                    ApplyAllFilters(); // Reapply filters to update display and total value
                }
            }
        }

        private void AddLineSeparator(TableLayoutPanel table, int row, int columnSpan)
        {
            Panel separatorPanel = new Panel
            {
                Height = 2, // Thickness of the line
                BackColor = Color.LightGray, // Color of the line
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 0, 5) // Padding around the line
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
