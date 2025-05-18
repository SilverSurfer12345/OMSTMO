using OrderManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Data.SqlClient;

namespace OrderManagement.View
{
    public partial class OrderViewForm : Form
    {
        private int orderId;

        private string customerName;
        private string customerTelephoneNumber;

        private string customerHouseNameNumber;
        private string customerAddressLine1;
        private string customerAddressLine2;
        private string customerAddressLine3;
        private string customerAddressLine4;
        private string customerPostcode;

        private string orderType;
        private string paymentType;
        private string totalPriceValue;
        private decimal baseTotalPrice;
        private decimal deliveryCharge;
        private decimal presetCharges;
        private decimal dbTotalPrice; // The total price as stored in the DB (includes delivery charge if delivery)
        private decimal dbDeliveryCharge; // The delivery charge as stored in the DB
        private decimal dbPresetCharges; // The preset charges as stored in the DB
        private DateTime orderDateTime;
        private System.Windows.Forms.DateTimePicker dtpOrderDate;

        // Delivery charge display controls
        private Label lblDeliveryChargeText;
        private Label lblDeliveryChargeValue;

        public OrderViewForm(int orderId)
        {
            InitializeComponent();

            // If you create dgvOrderItems in code, do it here:
            // dgvOrderItems = new DataGridView();
            // this.Controls.Add(dgvOrderItems);

            this.StartPosition = FormStartPosition.CenterScreen;
            this.orderId = orderId;

            LoadOrderDetails();
            UpdateDeliveryChargeDisplay();
        }

        private void LoadOrderDetails()
        {
            string getOrderDetailsQuery = "SELECT * FROM Orders WHERE OrderId = @orderId";
            var orderParameters = new Dictionary<string, object> { { "@orderId", orderId } };
            DataTable orderDetails = MainClass.getDataFromTable(getOrderDetailsQuery, orderParameters);

            if (orderDetails != null && orderDetails.Rows.Count > 0)
            {
                int customerId = Convert.ToInt32(orderDetails.Rows[0]["CustomerId"]);
                deliveryCharge = 0;
                if (orderDetails.Columns.Contains("DeliveryCharge") && orderDetails.Rows[0]["DeliveryCharge"] != DBNull.Value)
                    deliveryCharge = Convert.ToDecimal(orderDetails.Rows[0]["DeliveryCharge"]);

                presetCharges = 0;
                if (orderDetails.Columns.Contains("PresetCharges") && orderDetails.Rows[0]["PresetCharges"] != DBNull.Value)
                    presetCharges = Convert.ToDecimal(orderDetails.Rows[0]["PresetCharges"]);

                baseTotalPrice = Convert.ToDecimal(orderDetails.Rows[0]["TotalPrice"]);

                dbTotalPrice = Convert.ToDecimal(orderDetails.Rows[0]["TotalPrice"]);
                dbDeliveryCharge = orderDetails.Columns.Contains("DeliveryCharge") && orderDetails.Rows[0]["DeliveryCharge"] != DBNull.Value
                    ? Convert.ToDecimal(orderDetails.Rows[0]["DeliveryCharge"])
                    : 0;
                dbPresetCharges = orderDetails.Columns.Contains("PresetCharges") && orderDetails.Rows[0]["PresetCharges"] != DBNull.Value
                    ? Convert.ToDecimal(orderDetails.Rows[0]["PresetCharges"])
                    : 0;

                string getCustomerDetailsQuery = "SELECT * FROM customers WHERE Id = @customerId";
                var customerParameters = new Dictionary<string, object> { { "@customerId", customerId } };
                DataTable customerDetails = MainClass.getDataFromTable(getCustomerDetailsQuery, customerParameters);

                if (customerDetails != null && customerDetails.Rows.Count > 0)
                {
                    lblNameText.Text = "Name: " + customerDetails.Rows[0]["forename"].ToString() + " " + customerDetails.Rows[0]["surname"].ToString();
                    customerName = lblNameText.Text;
                    if (customerDetails.Rows[0]["telephoneNo"].ToString() == "")
                        lblTelephoneText.Text = "Telephone: no telephone number recorded";
                    else
                        lblTelephoneText.Text = "Telephone: " + customerDetails.Rows[0]["telephoneNo"].ToString();
                    customerTelephoneNumber = lblTelephoneText.Text;

                    customerHouseNameNumber = customerDetails.Rows[0]["houseNameNumber"].ToString();
                    customerAddressLine1 = customerDetails.Rows[0]["AddressLine1"].ToString();
                    customerAddressLine2 = customerDetails.Rows[0]["AddressLine2"].ToString();
                    customerAddressLine3 = customerDetails.Rows[0]["AddressLine3"].ToString();
                    customerAddressLine4 = customerDetails.Rows[0]["AddressLine4"].ToString();
                    customerPostcode = customerDetails.Rows[0]["Postcode"].ToString();

                    // Show address for delivery orders
                    if (String.Equals(orderDetails.Rows[0]["OrderType"].ToString(), "DELIVERY", StringComparison.OrdinalIgnoreCase))
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

                        lblAddress.Text = string.Join(Environment.NewLine, addressLines);
                        lblAddress.Visible = true;
                    }
                    else
                    {
                        lblAddress.Text = "";
                        lblAddress.Visible = false;
                    }

                    lblOrderDateText.Text = "Order Date: " + orderDetails.Rows[0]["OrderDate"].ToString();
                    if (orderDetails.Rows[0]["OrderDate"] != DBNull.Value)
                        orderDateTime = Convert.ToDateTime(orderDetails.Rows[0]["OrderDate"]);

                    lblOrderTypeText.Text = "Order Type";
                    orderType = orderDetails.Rows[0]["OrderType"].ToString();

                    if (String.Equals(orderType, "COLLECTION", StringComparison.OrdinalIgnoreCase))
                        rbCollection.Checked = true;
                    else if (String.Equals(orderType, "DELIVERY", StringComparison.OrdinalIgnoreCase))
                        rbDelivery.Checked = true;
                    else if (String.Equals(orderType, "ONLINE", StringComparison.OrdinalIgnoreCase))
                        rbOnline.Checked = true;
                    else if (String.Equals(orderType, "WAITING", StringComparison.OrdinalIgnoreCase))
                        rbWaiting.Checked = true;

                    lblPaymentTypeText.Text = "Payment Type: ";
                    paymentType = orderDetails.Rows[0]["PaymentType"]?.ToString() ?? "";

                    rbCash.Checked = false;
                    rbCard.Checked = false;
                    rbPending.Checked = false;
                    rbCancelled.Checked = false;
                    rbRefunded.Checked = false;

                    if (!string.IsNullOrEmpty(paymentType))
                    {
                        switch (paymentType.ToUpper())
                        {
                            case "CASH": rbCash.Checked = true; break;
                            case "CARD": rbCard.Checked = true; break;
                            case "PENDING": rbPending.Checked = true; break;
                            case "CANCELLED": rbCancelled.Checked = true; break;
                            case "REFUNDED": rbRefunded.Checked = true; break;
                            default: MessageBox.Show($"Unknown payment type: '{paymentType}'"); break;
                        }
                    }

                    lblTotalPriceText.Text = "Total Price: ";
                    txtTotalPrice.Text = orderDetails.Rows[0]["TotalPrice"].ToString();
                    totalPriceValue = txtTotalPrice.Text;
                    baseTotalPrice = Convert.ToDecimal(orderDetails.Rows[0]["TotalPrice"]);
                    if (String.Equals(orderType, "DELIVERY", StringComparison.OrdinalIgnoreCase))
                    {
                        baseTotalPrice -= deliveryCharge;
                        if (baseTotalPrice < 0) baseTotalPrice = 0;
                    }


                    string getOrderItemsQuery = "SELECT OrderItemId AS dgvOrderItemId, itemName AS dgvItemName, itemPrice AS dgvItemPrice, Quantity AS dgvQuantity FROM OrderItems WHERE OrderId = @orderId";
                    var orderItemsParameters = new Dictionary<string, object> { { "@orderId", orderId } };
                    DataTable orderItems = MainClass.getDataFromTable(getOrderItemsQuery, orderItemsParameters);

                    if (orderItems != null && orderItems.Rows.Count > 0)
                    {
                        dgvOrderItems.DataSource = orderItems;

                        // Set columns to read-only and hide unused columns
                        foreach (DataGridViewColumn col in dgvOrderItems.Columns)
                        {
                            col.ReadOnly = true;
                        }
                        if (dgvOrderItems.Columns.Contains("dgvOrderItemId"))
                            dgvOrderItems.Columns["dgvOrderItemId"].Visible = false;

                        // Add preset charges as rows in the DataGridView
                        if (dgvOrderItems.DataSource is DataTable dt)
                        {
                            // Remove any existing preset charge rows to avoid duplicates
                            var presetChargeNames = new List<string>();
                            using (var conn = new SqlConnection(OrderManagement.DatabaseManager.ConnectionString))
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
                            var rowsToRemove = dt.AsEnumerable()
                                .Where(r => r["dgvItemName"] != null && presetChargeNames.Contains(r["dgvItemName"].ToString()))
                                .ToList();
                            foreach (var row in rowsToRemove)
                                dt.Rows.Remove(row);

                            // Now add each preset charge as a row
                            using (var conn = new SqlConnection(OrderManagement.DatabaseManager.ConnectionString))
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
                                        if (chargeValue > 0)
                                        {
                                            DataRow presetRow = dt.NewRow();
                                            presetRow["dgvItemName"] = chargeName;
                                            presetRow["dgvItemPrice"] = chargeValue;
                                            presetRow["dgvQuantity"] = 1;
                                            dt.Rows.Add(presetRow);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Customer details not found.");
                }
            }
            else
            {
                MessageBox.Show("Order details not found.");
            }
            // Show summary panel (delivery, preset, total) below the grid
            ShowOrderSummaryLabels();
        }

        private void ShowOrderSummaryLabels()
        {
            // Remove any previous summary labels
            var oldSummaryLabels = this.Controls.OfType<Label>()
                .Where(l => l.Tag != null && l.Tag.ToString() == "OrderSummary")
                .ToList();
            foreach (var lbl in oldSummaryLabels)
                this.Controls.Remove(lbl);

            int summaryY = lblAddress.Bottom + 10;
            int summaryX = lblAddress.Left;
            var summaryFont = new Font("Segoe UI", 10, FontStyle.Bold);

            // Show each preset charge with value > 0
            using (var conn = new SqlConnection(OrderManagement.DatabaseManager.ConnectionString))
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
                        var lbl = new Label
                        {
                            Text = $"{chargeName}: £{chargeValue:0.00}",
                            Font = summaryFont,
                            AutoSize = true,
                            Location = new Point(summaryX, summaryY),
                            Tag = "OrderSummary"
                        };
                        this.Controls.Add(lbl);
                        summaryY += lbl.Height + 4;
                    }
                }
            }

            // Only show delivery charge if delivery
            if (String.Equals(GetSelectedOrderType(), "DELIVERY", StringComparison.OrdinalIgnoreCase) && dbDeliveryCharge > 0)
            {
                var lblDelivery = new Label
                {
                    Text = $"Delivery Charge: £{dbDeliveryCharge:0.00}",
                    Font = summaryFont,
                    AutoSize = true,
                    Location = new Point(summaryX, summaryY),
                    Tag = "OrderSummary"
                };
                this.Controls.Add(lblDelivery);
                summaryY += lblDelivery.Height + 4;
            }

            var lblTotal = new Label
            {
                Text = $"Total: £{txtTotalPrice.Text}",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(summaryX, summaryY),
                Tag = "OrderSummary"
            };
            this.Controls.Add(lblTotal);
        }

        private void UpdateDeliveryChargeDisplay()
        {
            bool isDelivery = String.Equals(GetSelectedOrderType(), "DELIVERY", StringComparison.OrdinalIgnoreCase);

            if (lblDeliveryChargeText != null)
                lblDeliveryChargeText.Visible = isDelivery;
            if (lblDeliveryChargeValue != null)
                lblDeliveryChargeValue.Visible = isDelivery;

            // Defensive null check for dgvOrderItems
            if (dgvOrderItems != null && dgvOrderItems.DataSource is DataTable dt)
            {
                var rowsToRemove = dt.AsEnumerable()
                    .Where(r => r["dgvItemName"] != null && r["dgvItemName"].ToString() == "Delivery Charge")
                    .ToList();
                foreach (var row in rowsToRemove)
                    dt.Rows.Remove(row);

                // Add delivery charge row if delivery is selected
                if (isDelivery && dbDeliveryCharge > 0)
                {
                    DataRow deliveryRow = dt.NewRow();
                    deliveryRow["dgvItemName"] = "Delivery Charge";
                    deliveryRow["dgvItemPrice"] = dbDeliveryCharge;
                    deliveryRow["dgvQuantity"] = 1;
                    dt.Rows.Add(deliveryRow);
                }
            }

            // Calculate total from grid
            decimal gridTotal = CalculateTotalFromGrid();

            if (lblDeliveryChargeValue != null)
                lblDeliveryChargeValue.Text = isDelivery ? $"£{dbDeliveryCharge:0.00}" : "£0.00";

            txtTotalPrice.Text = gridTotal.ToString("0.00");
            totalPriceValue = txtTotalPrice.Text;

            ShowOrderSummaryLabels();
        }

        private void rbCollection_CheckedChanged(object sender, EventArgs e)
        {
            lblChangesMade.Visible = true;
            lblAddress.Visible = false;
            UpdateDeliveryChargeDisplay();
        }

        private void rbDelivery_CheckedChanged(object sender, EventArgs e)
        {
            lblChangesMade.Visible = true;
            // Show and update address label for delivery
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

            lblAddress.Text = string.Join(Environment.NewLine, addressLines);
            lblAddress.Visible = true;
            UpdateDeliveryChargeDisplay();
        }

        private void rbOnline_CheckedChanged(object sender, EventArgs e)
        {
            lblChangesMade.Visible = true;
            lblAddress.Visible = false;
            UpdateDeliveryChargeDisplay();
        }

        private void rbWaiting_CheckedChanged(object sender, EventArgs e)
        {
            lblChangesMade.Visible = true;
            lblAddress.Visible = false;
            UpdateDeliveryChargeDisplay();
        }

        private string GetSelectedOrderType()
        {
            if (rbCollection.Checked) return "COLLECTION";
            if (rbDelivery.Checked) return "DELIVERY";
            if (rbOnline.Checked) return "ONLINE";
            if (rbWaiting.Checked) return "WAITING";
            return null;
        }

        private string GetSelectedPaymentType()
        {
            if (rbCash.Checked) return "CASH";
            if (rbCard.Checked) return "CARD";
            if (rbPending.Checked) return "PENDING";
            if (rbCancelled.Checked) return "CANCELLED";
            if (rbRefunded.Checked) return "REFUNDED";
            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string selectedOrderType = GetSelectedOrderType();
            if (string.IsNullOrEmpty(selectedOrderType))
            {
                MessageBox.Show("Please select an order type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedPaymentType = GetSelectedPaymentType();
            decimal totalPrice = Convert.ToDecimal(txtTotalPrice.Text);

            string getOrderDetailsQuery = "SELECT * FROM Orders WHERE OrderId = @orderId";
            var orderParameters = new Dictionary<string, object> { { "@orderId", orderId } };
            DataTable orderDetails = MainClass.getDataFromTable(getOrderDetailsQuery, orderParameters);

            if (orderDetails != null && orderDetails.Rows.Count > 0)
            {
                string currentOrderType = orderDetails.Rows[0]["OrderType"].ToString();
                string currentPaymentType = orderDetails.Rows[0]["PaymentType"].ToString();
                decimal currentTotalPrice = Convert.ToDecimal(orderDetails.Rows[0]["TotalPrice"]);

                if (selectedOrderType != currentOrderType)
                {
                    string updateOrderQuery = "UPDATE Orders SET OrderType = @orderType WHERE OrderId = @orderId";
                    var updateParameters = new Dictionary<string, object> {
                        { "@orderType", selectedOrderType },
                        { "@orderId", orderId } };
                    MainClass.UpdateTableData(updateOrderQuery, updateParameters);
                }

                if (totalPrice != currentTotalPrice)
                {
                    string updateOrderQuery = "UPDATE Orders SET TotalPrice = @totalPrice WHERE OrderId = @orderId";
                    var updateParameters = new Dictionary<string, object> {
                        { "@totalPrice", totalPrice },
                        { "@orderId", orderId } };
                    MainClass.UpdateTableData(updateOrderQuery, updateParameters);
                }

                if (selectedPaymentType != currentPaymentType)
                {
                    string updateOrderQuery = "UPDATE Orders SET PaymentType = @paymentType WHERE OrderId = @orderId";
                    var updateParameters = new Dictionary<string, object> {
                        { "@paymentType", selectedPaymentType },
                        { "@orderId", orderId } };
                    MainClass.UpdateTableData(updateOrderQuery, updateParameters);
                }

                if (selectedPaymentType == currentPaymentType && selectedOrderType == currentOrderType && totalPrice == currentTotalPrice)
                {
                    MessageBox.Show("No Changes found");
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void pnlCustomerInfo_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnPreviewInvoice_Click(object sender, EventArgs e)
        {
            using (var popup = new InvoicePopupForm(
                customerName,
                customerTelephoneNumber,
                customerHouseNameNumber,
                customerAddressLine1,
                customerAddressLine2,
                customerAddressLine3,
                customerAddressLine4,
                customerPostcode,
                orderType,
                paymentType,
                orderDateTime,
                dgvOrderItems.DataSource as DataTable,
                deliveryCharge,
                presetCharges,
                totalPriceValue
            ))
            {
                popup.ShowDialog(this);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (HasUnsavedChanges())
            {
                if (!SaveOrder())
                    return; // If save fails, do not proceed
            }

            // Implement print logic if needed
        }

        private void btnEditCurrentOrder_Click(object sender, EventArgs e)
        {
            if (HasUnsavedChanges())
            {
                if (!SaveOrder())
                    return; // If save fails, do not proceed
            }

            try
            {
                Form parentOrderHistory = null;
                foreach (Form form in Application.OpenForms)
                {
                    if (form is frmOrderHistory)
                    {
                        parentOrderHistory = form;
                        break;
                    }
                }

                this.DialogResult = DialogResult.OK;

                if (parentOrderHistory != null)
                {
                    parentOrderHistory.Close();
                }

                this.Close();

                using (frmPOS posForm = new frmPOS(orderId))
                {
                    posForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error editing order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvOrderItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvItemName.Index)
            {
                // Get selected item id
                int itemId = (int)dgvOrderItems.Rows[e.RowIndex].Cells["dgvOrderItemId"].Value;

                // Lookup price 
                decimal price = GetItemPrice(itemId);

                // Update price cell
                dgvOrderItems.Rows[e.RowIndex].Cells["dgvItemPrice"].Value = price;
            }
        }

        private decimal GetItemPrice(int itemId)
        {
            string sql = "SELECT price FROM foodItems WHERE Id = @ItemId";
            var parameters = new Dictionary<string, object> { { "@ItemId", itemId } };

            DataTable result = MainClass.getDataFromTable(sql, parameters);
            if (result != null && result.Rows.Count > 0)
            {
                return Convert.ToDecimal(result.Rows[0]["price"]);
            }
            else
            {
                // Handle the case where no matching item was found in the database
                throw new Exception("Item not found");
            }
        }

        private decimal CalculateTotalFromGrid()
        {
            decimal total = 0;
            if (dgvOrderItems != null && dgvOrderItems.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvOrderItems.Rows)
                {
                    if (row.Cells["dgvItemPrice"].Value != null && row.Cells["dgvQuantity"].Value != null)
                    {
                        decimal price = 0;
                        int qty = 1;
                        decimal.TryParse(row.Cells["dgvItemPrice"].Value.ToString(), out price);
                        int.TryParse(row.Cells["dgvQuantity"].Value.ToString(), out qty);
                        total += price * qty;
                    }
                }
            }
            return total;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Do you want to save before closing?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Ensure UI is up-to-date
                    this.Validate();
                    Application.DoEvents();

                    bool saveResult = SaveOrder();
                    if (!saveResult)
                    {
                        MessageBox.Show("Failed to save changes. The form will not close.", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Only close if save succeeded
                    }
                    else
                    {
                        MessageBox.Show("Order changes saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK; // Notify parent for refresh
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Do not close
                }
                // If No, just close without saving
            }
            this.Close();
        }

        // Add this helper method to check for unsaved changes
        private bool HasUnsavedChanges()
        {
            // Get current values from UI
            string selectedOrderType = GetSelectedOrderType();
            string selectedPaymentType = GetSelectedPaymentType();
            decimal totalPrice = Convert.ToDecimal(txtTotalPrice.Text);

            // Get original values from DB
            string getOrderDetailsQuery = "SELECT * FROM Orders WHERE OrderId = @orderId";
            var orderParameters = new Dictionary<string, object> { { "@orderId", orderId } };
            DataTable orderDetails = MainClass.getDataFromTable(getOrderDetailsQuery, orderParameters);

            if (orderDetails != null && orderDetails.Rows.Count > 0)
            {
                string currentOrderType = orderDetails.Rows[0]["OrderType"].ToString();
                string currentPaymentType = orderDetails.Rows[0]["PaymentType"].ToString();
                decimal currentTotalPrice = Convert.ToDecimal(orderDetails.Rows[0]["TotalPrice"]);

                // Compare UI values to DB values
                if (selectedOrderType != currentOrderType ||
                    selectedPaymentType != currentPaymentType ||
                    totalPrice != currentTotalPrice)
                {
                    return true;
                }
            }

            return false;
        }

        // Add this helper method to save the order
        private bool SaveOrder()
        {
            try
            {
                // Reuse your btnSave_Click logic, but without showing "No Changes found"
                string selectedOrderType = GetSelectedOrderType();
                string selectedPaymentType = GetSelectedPaymentType();
                decimal totalPrice = Convert.ToDecimal(txtTotalPrice.Text);

                string getOrderDetailsQuery = "SELECT * FROM Orders WHERE OrderId = @orderId";
                var orderParameters = new Dictionary<string, object> { { "@orderId", orderId } };
                DataTable orderDetails = MainClass.getDataFromTable(getOrderDetailsQuery, orderParameters);

                if (orderDetails != null && orderDetails.Rows.Count > 0)
                {
                    string currentOrderType = orderDetails.Rows[0]["OrderType"].ToString();
                    string currentPaymentType = orderDetails.Rows[0]["PaymentType"].ToString();
                    decimal currentTotalPrice = Convert.ToDecimal(orderDetails.Rows[0]["TotalPrice"]);

                    bool changed = false;

                    if (selectedOrderType != currentOrderType)
                    {
                        string updateOrderQuery = "UPDATE Orders SET OrderType = @orderType WHERE OrderId = @orderId";
                        var updateParameters = new Dictionary<string, object> {
                    { "@orderType", selectedOrderType },
                    { "@orderId", orderId } };
                        MainClass.UpdateTableData(updateOrderQuery, updateParameters);
                        changed = true;
                    }

                    if (totalPrice != currentTotalPrice)
                    {
                        string updateOrderQuery = "UPDATE Orders SET TotalPrice = @totalPrice WHERE OrderId = @orderId";
                        var updateParameters = new Dictionary<string, object> {
                    { "@totalPrice", totalPrice },
                    { "@orderId", orderId } };
                        MainClass.UpdateTableData(updateOrderQuery, updateParameters);
                        changed = true;
                    }

                    if (selectedPaymentType != currentPaymentType)
                    {
                        string updateOrderQuery = "UPDATE Orders SET PaymentType = @paymentType WHERE OrderId = @orderId";
                        var updateParameters = new Dictionary<string, object> {
                    { "@paymentType", selectedPaymentType },
                    { "@orderId", orderId } };
                        MainClass.UpdateTableData(updateOrderQuery, updateParameters);
                        changed = true;
                    }

                    if (changed)
                    {
                        lblChangesMade.Visible = false; // Mark as saved
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}