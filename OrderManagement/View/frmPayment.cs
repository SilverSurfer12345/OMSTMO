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


namespace OrderManagement.View
{
    public partial class frmPayment : Form
    {
        public event Action<string> Confirm;
        private bool isButtonClicked = false;
        private bool isOrderSaved = false;
        public DataGridView BasketGrid { get; set; }

        // Properties
        public string CustomerName { get; set; }
        public int OrderId { get; set; }


        public string TelephoneNo { get; set; }
        public string OrderType { get; set; }
        public decimal TotalPrice { get; set; }
        public string Address { get; set; }

        private string selectedPaymentMethod = "";

        // Default constructor
        public frmPayment()
        {
            InitializeComponent();
            SetupEventHandlers();
        }

        private System.Windows.Forms.TextBox txtTotalPrice;
        private System.Windows.Forms.Label lblOrderType;
        private System.Windows.Forms.Label lblOrderId;



        // New constructor with parameters
        public frmPayment(int orderId, decimal totalPrice, string orderType)
        {
            InitializeComponent();
            this.OrderId = orderId;
            this.TotalPrice = totalPrice;
            this.OrderType = orderType;

            // Add validation for TelephoneNo
            if (string.IsNullOrEmpty(TelephoneNo))
            {
                MessageBox.Show("Telephone number is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Update UI elements
            if (txtTotalPrice != null)
                txtTotalPrice.Text = totalPrice.ToString("C2");
            if (lblOrderType != null)
                lblOrderType.Text = orderType;
            if (lblOrderId != null)
                lblOrderId.Text = orderId.ToString();

            SetupEventHandlers();
        }



        private void SetupEventHandlers()
        {
            btnCash.Click += (s, e) => { UpdatePaymentOption("CASH", btnCash); isButtonClicked = true; };
            btnCard.Click += (s, e) => { UpdatePaymentOption("CARD", btnCard); isButtonClicked = true; };
            btnRefund.Click += (s, e) => { UpdatePaymentOption("REFUNDED", btnCard); isButtonClicked = true; };
            btnPending.Click += (s, e) => { UpdatePaymentOption("PENDING", btnPending); isButtonClicked = true; };
            btnCancel.Click += (s, e) =>
            {
                if (!isOrderSaved)
                {
                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to cancel? The order will not be saved.",
                        "Confirm Cancel",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        this.Close();
                    }
                }
                else
                {
                    this.Close();
                }
            };
        }


        private void UpdatePaymentOption(string option, Button selectedButton)
        {
            lblPaymentOption.Text = option;
            selectedPaymentMethod = option;

            // Reset all buttons to default color
            btnCash.BackColor = Color.FromArgb(241, 85, 126);
            btnCard.BackColor = Color.FromArgb(241, 85, 126);
            btnRefund.BackColor = Color.FromArgb(241, 85, 126);
            btnPending.BackColor = Color.FromArgb(241, 85, 126);

            // Highlight the selected button
            selectedButton.BackColor = Color.Green;

            // Enable the confirm button
            btnConfirm.Enabled = true;
        }


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedPaymentMethod))
            {
                MessageBox.Show("Please select a payment method first.", "Payment Method Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Update the payment type in the database if we have an order ID
                if (OrderId > 0)
                {
                    string updateQuery = "UPDATE Orders SET PaymentType = @PaymentType WHERE OrderId = @OrderId";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@PaymentType", selectedPaymentMethod },
                { "@OrderId", OrderId }
            };

                    DatabaseManager.ExecuteNonQuery(updateQuery, parameters);

                    // Force a checkpoint to ensure data is written to disk
                    DatabaseManager.ForceCheckpoint();
                }

                // Trigger the Confirm event
                Confirm?.Invoke(selectedPaymentMethod);

                // Close the form
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing payment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private int GetOrCreateCustomer()
        {
            // If no telephone number, just create a customer with other details
            if (string.IsNullOrEmpty(TelephoneNo))
            {
                return AddCustomerToDatabase(CustomerName, null);
            }

            // If telephone exists, try to find existing customer
            string query = "SELECT Id FROM customers WHERE telephoneNo = @telephoneNo";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@telephoneNo", TelephoneNo.Trim() }
    };

            DataTable dt = MainClass.getDataFromTable(query, parameters);

            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["Id"]);
            }

            // If no existing customer found, create new one
            return AddCustomerToDatabase(CustomerName, TelephoneNo);
        }

        private int AddCustomerToDatabase(string customerName, string telephoneNo)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Customer name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            string[] nameParts = customerName.Trim().Split(' ');
            string forename = nameParts[0];
            string surname = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

            string query = "INSERT INTO customers (forename, surname, telephoneNo) VALUES (@forename, @surname, @telephoneNo); SELECT SCOPE_IDENTITY();";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@forename", forename },
        { "@surname", surname },
        { "@telephoneNo", (object)telephoneNo ?? DBNull.Value }  // Handle null telephone numbers
    };

            try
            {
                object result = MainClass.ExecuteScalar(query, parameters);
                return (result == null || result == DBNull.Value) ? -1 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer to database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }



        private void SaveOrderItems(int orderId)
        {
            if (BasketGrid == null)
            {
                MessageBox.Show("Basket data is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (BasketGrid.Rows.Count == 0)
            {
                MessageBox.Show("No items in basket to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                foreach (DataGridViewRow row in BasketGrid.Rows)
                {
                    if (row.Cells["dgvName"].Value == null ||
                        row.Cells["dgvPrice"].Value == null ||
                        row.Cells["dgvQty"].Value == null)
                    {
                        MessageBox.Show("Invalid data in basket.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    string itemName = row.Cells["dgvName"].Value.ToString();
                    decimal itemPrice = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
                    int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);

                    MainClass.SaveOrderItem(orderId, itemName, itemPrice, quantity);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving order items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUnpaid_Click(object sender, EventArgs e)
        {

        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            // Just select the payment method, don't confirm yet
            selectedPaymentMethod = "CASH";

            // Highlight the selected button
            btnCash.BackColor = Color.Green;
            btnCard.BackColor = SystemColors.Control;

            // Enable the confirm button
            btnConfirm.Enabled = true;
        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            // Just select the payment method, don't confirm yet
            selectedPaymentMethod = "CARD";

            // Highlight the selected button
            btnCard.BackColor = Color.Green;
            btnCash.BackColor = SystemColors.Control;

            // Enable the confirm button
            btnConfirm.Enabled = true;
        }

        private int GetOrderIdFromBasket()
        {
            try
            {
                // If we have an OrderId property set directly, use that
                if (OrderId > 0)
                {
                    return OrderId;
                }

                // If we don't have a telephone number, try to find the most recent order
                if (string.IsNullOrEmpty(TelephoneNo))
                {
                    string query = "SELECT TOP 1 OrderId FROM Orders ORDER BY OrderDate DESC";
                    object result = DatabaseManager.ExecuteScalar(query, null);
                    return result == null ? -1 : Convert.ToInt32(result);
                }

                // Get the latest order for the customer by telephone number
                string customerQuery = "SELECT TOP 1 OrderId FROM Orders WHERE CustomerId = (SELECT TOP 1 Id FROM customers WHERE telephoneNo = @TelephoneNo) ORDER BY OrderDate DESC";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@TelephoneNo", TelephoneNo }
        };

                object orderResult = DatabaseManager.ExecuteScalar(customerQuery, parameters);
                if (orderResult != null)
                {
                    return Convert.ToInt32(orderResult);
                }

                // If we still don't have an order ID, try to find the most recent order
                string fallbackQuery = "SELECT TOP 1 OrderId FROM Orders ORDER BY OrderDate DESC";
                object fallbackResult = DatabaseManager.ExecuteScalar(fallbackQuery, null);
                return fallbackResult == null ? -1 : Convert.ToInt32(fallbackResult);
            }
            catch (Exception)
            {
                // Silently handle the error and return -1
                return -1;
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Force a checkpoint to ensure data is written to disk
            DatabaseManager.ForceCheckpoint();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        private void frmPayment_Load(object sender, EventArgs e)
        {
            // Disable the confirm button initially
            btnConfirm.Enabled = false;
        }

        private void btnPending_Click(object sender, EventArgs e)
        {
            // Just select the payment method, don't confirm yet
            selectedPaymentMethod = "PENDING";

            // Highlight the selected button
            btnPending.BackColor = Color.Green;
            btnCash.BackColor = Color.FromArgb(241, 85, 126);
            btnCard.BackColor = Color.FromArgb(241, 85, 126);
            btnRefund.BackColor = Color.FromArgb(241, 85, 126);

            // Enable the confirm button
            btnConfirm.Enabled = true;
        }

    }
}

