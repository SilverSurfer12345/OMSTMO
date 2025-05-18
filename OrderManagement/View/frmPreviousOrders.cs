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
    public partial class frmPreviousOrders : Form
    {
        public DataTable SelectedOrderItems { get; private set; }
        private int customerId;

        public frmPreviousOrders(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            LoadPreviousOrders();
        }

        private void LoadPreviousOrders()
        {
            try
            {
                // Query to get all previous orders for this customer
                string query = @"
                    SELECT 
                        O.OrderId, 
                        O.OrderDate, 
                        O.TotalPrice,
                        O.OrderType,
                        O.PaymentType
                    FROM 
                        Orders O
                    WHERE 
                        O.CustomerId = @CustomerId
                    ORDER BY 
                        O.OrderDate DESC";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@CustomerId", customerId }
                };

                DataTable orders = DatabaseManager.ExecuteQuery(query, parameters);

                if (orders != null && orders.Rows.Count > 0)
                {
                    dgvPreviousOrders.DataSource = orders;

                    // Format the columns
                    dgvPreviousOrders.Columns["OrderId"].HeaderText = "Order ID";
                    dgvPreviousOrders.Columns["OrderDate"].HeaderText = "Order Date";
                    dgvPreviousOrders.Columns["TotalPrice"].HeaderText = "Total Price";
                    dgvPreviousOrders.Columns["OrderType"].HeaderText = "Order Type";
                    dgvPreviousOrders.Columns["PaymentType"].HeaderText = "Payment Type";

                    // Format the date column
                    dgvPreviousOrders.Columns["OrderDate"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                    // Format the price column
                    dgvPreviousOrders.Columns["TotalPrice"].DefaultCellStyle.Format = "C2";
                }
                else
                {
                    MessageBox.Show("No previous orders found for this customer.", "No Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading previous orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPreviousOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPreviousOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dgvPreviousOrders.SelectedRows[0].Cells["OrderId"].Value);
                LoadOrderItems(orderId);
            }
        }

        private void LoadOrderItems(int orderId)
        {
            try
            {
                // Query to get all items for the selected order
                string query = @"
                    SELECT 
                        OI.ItemName,
                        OI.ItemPrice,
                        OI.Quantity
                    FROM 
                        OrderItems OI
                    WHERE 
                        OI.OrderId = @OrderId";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@OrderId", orderId }
                };

                DataTable items = DatabaseManager.ExecuteQuery(query, parameters);

                if (items != null && items.Rows.Count > 0)
                {
                    dgvOrderItems.DataSource = items;

                    // Format the columns
                    dgvOrderItems.Columns["ItemName"].HeaderText = "Item Name";
                    dgvOrderItems.Columns["ItemPrice"].HeaderText = "Price";
                    dgvOrderItems.Columns["Quantity"].HeaderText = "Quantity";

                    // Format the price column
                    dgvOrderItems.Columns["ItemPrice"].DefaultCellStyle.Format = "C2";
                }
                else
                {
                    dgvOrderItems.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvPreviousOrders.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dgvPreviousOrders.SelectedRows[0].Cells["OrderId"].Value);

                // Query to get all items for the selected order
                string query = @"
                    SELECT 
                        OI.ItemName,
                        OI.ItemPrice,
                        OI.Quantity
                    FROM 
                        OrderItems OI
                    WHERE 
                        OI.OrderId = @OrderId";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@OrderId", orderId }
                };

                SelectedOrderItems = DatabaseManager.ExecuteQuery(query, parameters);

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Please select an order first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
