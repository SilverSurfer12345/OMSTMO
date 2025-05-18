using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace OrderManagement.View
{
    public class InvoicePopupForm : Form
    {
        private Button btnClose;

        public InvoicePopupForm(
            string customerName,
            string customerTelephoneNumber,
            string customerHouseNameNumber,
            string customerAddressLine1,
            string customerAddressLine2,
            string customerAddressLine3,
            string customerAddressLine4,
            string customerPostcode,
            string orderType,
            string paymentType,
            DateTime orderDateTime,
            DataTable orderItems,
            decimal deliveryCharge,
            decimal presetCharges,
            string totalPriceValue)
        {
            this.Text = "Invoice Preview";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(480, 700);
            this.MinimumSize = new Size(400, 400);

            // Main scrollable panel
            var pnlScroll = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            // Main vertical layout
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 0
            };

            // Title
            var lblTitle = new Label
            {
                Text = "TAKE MY ORDER",
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                AutoSize = true
            };
            mainLayout.Controls.Add(lblTitle);

            // Restaurant details
            var lblResAddress = new Label
            {
                Text = "123 Restaurant Street, Newcastle Upon Tyne, NE99 9AA",
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                AutoSize = true
            };
            mainLayout.Controls.Add(lblResAddress);

            var lblResTel = new Label
            {
                Text = "01910000000",
                Font = new Font("Arial", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                AutoSize = true
            };
            mainLayout.Controls.Add(lblResTel);

            // Separator
            mainLayout.Controls.Add(CreateSeparator());

            // Customer details
            var lblCusName = new Label
            {
                Text = customerName?.Replace("Name: ", "") ?? "No Name",
                Font = new Font("Arial", 13, FontStyle.Bold),
                Dock = DockStyle.Top,
                AutoSize = true
            };
            mainLayout.Controls.Add(lblCusName);

            if (string.Equals(orderType, "DELIVERY", StringComparison.OrdinalIgnoreCase))
            {
                var addressLines = new List<string>();
                string firstLine = "";
                if (!string.IsNullOrWhiteSpace(customerHouseNameNumber))
                    firstLine += customerHouseNameNumber.Trim();
                if (!string.IsNullOrWhiteSpace(customerAddressLine1))
                {
                    if (firstLine.Length > 0)
                        firstLine += " ";
                    firstLine += customerAddressLine1.Trim();
                }
                if (!string.IsNullOrWhiteSpace(firstLine))
                    addressLines.Add(firstLine);
                if (!string.IsNullOrWhiteSpace(customerAddressLine2)) addressLines.Add(customerAddressLine2.Trim());
                if (!string.IsNullOrWhiteSpace(customerAddressLine3)) addressLines.Add(customerAddressLine3.Trim());
                if (!string.IsNullOrWhiteSpace(customerAddressLine4)) addressLines.Add(customerAddressLine4.Trim());
                if (!string.IsNullOrWhiteSpace(customerPostcode)) addressLines.Add(customerPostcode.Trim());

                var lblCusAddress = new Label
                {
                    Text = string.Join(Environment.NewLine, addressLines),
                    Font = new Font("Arial", 13, FontStyle.Bold),
                    Dock = DockStyle.Top,
                    AutoSize = true
                };
                mainLayout.Controls.Add(lblCusAddress);

                // Remove any existing "Delivery Charge" row from orderItems so it doesn't show in the items table
                if (orderItems != null)
                {
                    var rowsToRemove = orderItems.AsEnumerable()
                        .Where(r => r["dgvItemName"] != null && r["dgvItemName"].ToString() == "Delivery Charge")
                        .ToList();
                    foreach (var removeRow in rowsToRemove)
                        orderItems.Rows.Remove(removeRow);

                    // Remove preset charge rows (e.g., Bag Charge, Service Charge, etc.)
                    var presetChargeNames = new List<string>();
                    string connectionString = OrderManagement.DatabaseManager.ConnectionString;
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT ChargeName FROM PresetCharges WHERE ChargeValue > 0";
                        using (var cmd = new SqlCommand(query, conn))
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                presetChargeNames.Add(reader["ChargeName"].ToString());
                            }
                        }
                    }
                    var presetRowsToRemove = orderItems.AsEnumerable()
                        .Where(r => r["dgvItemName"] != null && presetChargeNames.Contains(r["dgvItemName"].ToString()))
                        .ToList();
                    foreach (var removeRow in presetRowsToRemove)
                        orderItems.Rows.Remove(removeRow);
                }
            }

            var lblCusTel = new Label
            {
                Text = customerTelephoneNumber?.Replace("Telephone: ", "") ?? "No Telephone Number",
                Font = new Font("Arial", 13, FontStyle.Bold),
                Dock = DockStyle.Top,
                AutoSize = true
            };
            mainLayout.Controls.Add(lblCusTel);

            // Separator
            mainLayout.Controls.Add(CreateSeparator());

            // Items Table
            var itemsTable = new TableLayoutPanel
            {
                ColumnCount = 3,
                AutoSize = true,
                Dock = DockStyle.Top,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None // No borders
            };

            itemsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50)); // Qty
            itemsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // Name
            itemsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80)); // Price

            // Header row
            itemsTable.Controls.Add(new Label { Text = "Qty", Font = new Font("Arial", 13, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, 0);
            itemsTable.Controls.Add(new Label { Text = "Item", Font = new Font("Arial", 13, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 1, 0);
            itemsTable.Controls.Add(new Label { Text = "Price", Font = new Font("Arial", 13, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 2, 0);

            int row = 1;
            bool separatorAdded = false;
            if (orderItems != null)
            {
                var itemFont = new Font("Arial", 13, FontStyle.Bold);
                var deliveryFont = new Font("Arial", 10, FontStyle.Regular);

                foreach (DataRow itemRow in orderItems.Rows)
                {
                    string itemName = itemRow["dgvItemName"]?.ToString();
                    string itemPrice = itemRow["dgvItemPrice"]?.ToString();
                    string quantity = itemRow["dgvQuantity"]?.ToString();

                    if (string.IsNullOrWhiteSpace(itemName) || string.IsNullOrWhiteSpace(itemPrice) || string.IsNullOrWhiteSpace(quantity))
                        continue;

                    bool isDeliveryCharge = itemName.Trim().Equals("Delivery Charge", StringComparison.OrdinalIgnoreCase);

                    // Insert a separator before the delivery charge row
                    if (isDeliveryCharge && !separatorAdded)
                    {
                        var separator = new Panel
                        {
                            Height = 1,
                            Dock = DockStyle.Top,
                            BackColor = Color.Gray,
                            Margin = new Padding(0, 5, 0, 5)
                        };
                        // Add separator as a full-width row
                        itemsTable.Controls.Add(separator, 0, row);
                        itemsTable.SetColumnSpan(separator, 3);
                        row++;
                        separatorAdded = true;
                    }

                    var fontToUse = isDeliveryCharge ? deliveryFont : itemFont;

                    itemsTable.Controls.Add(new Label { Text = quantity, Font = fontToUse, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill }, 0, row);
                    itemsTable.Controls.Add(new Label { Text = itemName, Font = fontToUse, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 1, row);
                    itemsTable.Controls.Add(new Label { Text = $"£{itemPrice}", Font = fontToUse, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill }, 2, row);
                    row++;

                    foreach (DataRow presetRow in orderItems.Rows)
                    {
                        // Additional logic for preset rows can be added here
                    }
                }
            }

            mainLayout.Controls.Add(itemsTable);

            // Add a separator after the items table
            mainLayout.Controls.Add(CreateSeparator());

            // --- Summary Section: Preset Charges, Delivery Charge, Total ---
            var summaryPanel = new TableLayoutPanel
            {
                ColumnCount = 3,
                AutoSize = true,
                Dock = DockStyle.Top,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            summaryPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120)); // Adjust as needed

            var summaryFont = new Font("Arial", 11, FontStyle.Bold);
            int summaryRow = 0;

            // --- Show each preset charge with value > 0 ---
            try
            {
                string connectionString = OrderManagement.DatabaseManager.ConnectionString;
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ChargeName, ChargeValue FROM PresetCharges WHERE ChargeValue > 0";
                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string chargeName = reader["ChargeName"].ToString();
                            decimal chargeValue = Convert.ToDecimal(reader["ChargeValue"]);
                            summaryPanel.Controls.Add(new Label { Text = "", Dock = DockStyle.Fill }, 0, summaryRow);
                            summaryPanel.Controls.Add(new Label { Text = chargeName, Font = summaryFont, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 1, summaryRow);
                            summaryPanel.Controls.Add(new Label { Text = $"£{chargeValue:0.00}", Font = summaryFont, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 2, summaryRow);
                            summaryRow++;
                        }
                    }
                }
            }
            catch { /* ignore errors for preview */ }

            // Delivery Charge (if any)
            if (deliveryCharge > 0)
            {
                summaryPanel.Controls.Add(new Label { Text = "", Dock = DockStyle.Fill }, 0, summaryRow);
                summaryPanel.Controls.Add(new Label { Text = "Delivery Charge", Font = summaryFont, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 1, summaryRow);
                summaryPanel.Controls.Add(new Label { Text = $"£{deliveryCharge:0.00}", Font = summaryFont, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 2, summaryRow);
                summaryRow++;
            }

            // Total row (LARGER AND BOLD)
            var totalFont = new Font("Arial", 13, FontStyle.Bold);
            summaryPanel.Controls.Add(new Label { Text = "", Dock = DockStyle.Fill }, 0, summaryRow);
            summaryPanel.Controls.Add(new Label { Text = "Total", Font = totalFont, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 1, summaryRow);
            summaryPanel.Controls.Add(new Label { Text = $"£{totalPriceValue}", Font = totalFont, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, AutoSize = false }, 2, summaryRow);

            mainLayout.Controls.Add(summaryPanel);

            // Separator
            mainLayout.Controls.Add(CreateSeparator());

            // Payment type
            var paymentPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Dock = DockStyle.Top
            };
            paymentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            paymentPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            paymentPanel.Controls.Add(new Label { Text = "Payment", Font = new Font("Arial", 10, FontStyle.Bold), Dock = DockStyle.Fill }, 0, 0);
            paymentPanel.Controls.Add(new Label { Text = paymentType?.ToUpper() ?? "NOT SET", Font = new Font("Arial", 10, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight }, 1, 0);

            mainLayout.Controls.Add(paymentPanel);

            // Date & Time
            var dateTimePanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Dock = DockStyle.Top
            };
            dateTimePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            dateTimePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            dateTimePanel.Controls.Add(new Label
            {
                Text = $"Time of Order: {orderDateTime:hh:mm}",
                Font = new Font("Arial", 10),
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, 0);

            dateTimePanel.Controls.Add(new Label
            {
                Text = $"Date of Order: {orderDateTime:dd/MM/yyyy}",
                Font = new Font("Arial", 10),
                Dock = DockStyle.Fill,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight
            }, 1, 0);

            mainLayout.Controls.Add(dateTimePanel);

            // Add main layout to scroll panel
            pnlScroll.Controls.Add(mainLayout);

            // Close button
            btnClose = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(pnlScroll);
            this.Controls.Add(btnClose);
        }

        private Control CreateSeparator()
        {
            return new Panel
            {
                Height = 2,
                Dock = DockStyle.Top,
                BackColor = Color.Black,
                Margin = new Padding(0, 10, 0, 10)
            };
        }
    }
}
