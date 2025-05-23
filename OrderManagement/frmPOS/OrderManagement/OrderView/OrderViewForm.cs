using OrderManagement.Model;
using OrderManagement.Presenter;
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
    public partial class OrderViewForm : Form, IOrderViewFormView
    {
        private IOrderViewFormPresenter _presenter;

        // Delivery charge display controls (if not already defined in designer)
        // private Label lblDeliveryChargeText; // Assuming these are defined in designer or dynamically
        // private Label lblDeliveryChargeValue; // Assuming these are defined in designer or dynamically

        public OrderViewForm(int orderId)
        {
            InitializeComponent();
            _presenter = new OrderViewFormPresenter(this);
            _presenter.Initialize(orderId);

            // Wire up UI events to presenter events
            this.Load += (s, e) => ViewLoaded?.Invoke(s, e);
            btnSave.Click += (s, e) => SaveClicked?.Invoke(s, e);
            btnClose.Click += (s, e) => CloseClicked?.Invoke(s, e);
            rbCollection.CheckedChanged += (s, e) => OrderTypeChanged?.Invoke(s, e);
            rbDelivery.CheckedChanged += (s, e) => OrderTypeChanged?.Invoke(s, e);
            rbOnline.CheckedChanged += (s, e) => OrderTypeChanged?.Invoke(s, e);
            rbWaiting.CheckedChanged += (s, e) => OrderTypeChanged?.Invoke(s, e);
            rbCash.CheckedChanged += (s, e) => PaymentTypeChanged?.Invoke(s, e);
            rbCard.CheckedChanged += (s, e) => PaymentTypeChanged?.Invoke(s, e);
            rbPending.CheckedChanged += (s, e) => PaymentTypeChanged?.Invoke(s, e);
            rbCancelled.CheckedChanged += (s, e) => PaymentTypeChanged?.Invoke(s, e);
            rbRefunded.CheckedChanged += (s, e) => PaymentTypeChanged?.Invoke(s, e);
            btnPreviewInvoice.Click += (s, e) => PreviewInvoiceClicked?.Invoke(s, e);
            btnPrint.Click += (s, e) => PrintClicked?.Invoke(s, e);
            btnEditCurrentOrder.Click += (s, e) => EditCurrentOrderClicked?.Invoke(s, e);

            // Set DataGridView columns to read-only and hide unused columns
            foreach (DataGridViewColumn col in dgvOrderItems.Columns)
            {
                col.ReadOnly = true;
            }
            if (dgvOrderItems.Columns.Contains("dgvOrderItemId"))
                dgvOrderItems.Columns["dgvOrderItemId"].Visible = false;

            // Ensure the lblChangesMade is initially hidden
            lblChangesMade.Visible = false;
        }

        // --- IOrderViewFormView Implementation ---

        public string CustomerNameText { get => lblNameText.Text; set => lblNameText.Text = value; }
        public string CustomerTelephoneText { get => lblTelephoneText.Text; set => lblTelephoneText.Text = value; }
        public string CustomerAddressText { get => lblAddress.Text; set => lblAddress.Text = value; }
        public string OrderDateText { get => lblOrderDateText.Text; set => lblOrderDateText.Text = value; }
        public string TotalPriceText { get => txtTotalPrice.Text; set => txtTotalPrice.Text = value; }

        public bool CollectionChecked { get => rbCollection.Checked; set => rbCollection.Checked = value; }
        public bool DeliveryChecked { get => rbDelivery.Checked; set => rbDelivery.Checked = value; }
        public bool OnlineChecked { get => rbOnline.Checked; set => rbOnline.Checked = value; }
        public bool WaitingChecked { get => rbWaiting.Checked; set => rbWaiting.Checked = value; }

        public bool CashChecked { get => rbCash.Checked; set => rbCash.Checked = value; }
        public bool CardChecked { get => rbCard.Checked; set => rbCard.Checked = value; }
        public bool PendingChecked { get => rbPending.Checked; set => rbPending.Checked = value; }
        public bool CancelledChecked { get => rbCancelled.Checked; set => rbCancelled.Checked = value; }
        public bool RefundedChecked { get => rbRefunded.Checked; set => rbRefunded.Checked = value; }

        public bool ChangesMadeVisible { get => lblChangesMade.Visible; set => lblChangesMade.Visible = value; }
        public bool AddressVisible { get => lblAddress.Visible; set => lblAddress.Visible = value; }

        public object OrderItemsDataSource { get => dgvOrderItems.DataSource; set => dgvOrderItems.DataSource = value; }

        public void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon) => MessageBox.Show(message, title, buttons, icon);
        public DialogResult ShowConfirmation(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon) => MessageBox.Show(message, title, buttons, icon);
        public void CloseView() => this.Close();

        // --- Events (View raises these) ---
        public event EventHandler ViewLoaded;
        public event EventHandler SaveClicked;
        public event EventHandler CloseClicked;
        public event EventHandler OrderTypeChanged;
        public event EventHandler PaymentTypeChanged;
        public event EventHandler PreviewInvoiceClicked;
        public event EventHandler PrintClicked;
        public event EventHandler EditCurrentOrderClicked;

        // --- Remaining UI-specific event handlers (simplified as they now just raise events) ---

        private void rbCollection_CheckedChanged(object sender, EventArgs e) => OrderTypeChanged?.Invoke(sender, e);
        private void rbDelivery_CheckedChanged(object sender, EventArgs e) => OrderTypeChanged?.Invoke(sender, e);
        private void rbOnline_CheckedChanged(object sender, EventArgs e) => OrderTypeChanged?.Invoke(sender, e);
        private void rbWaiting_CheckedChanged(object sender, EventArgs e) => OrderTypeChanged?.Invoke(sender, e);

        private void rbCash_CheckedChanged(object sender, EventArgs e) => PaymentTypeChanged?.Invoke(sender, e);
        private void rbCard_CheckedChanged(object sender, EventArgs e) => PaymentTypeChanged?.Invoke(sender, e);
        private void rbPending_CheckedChanged(object sender, EventArgs e) => PaymentTypeChanged?.Invoke(sender, e);
        private void rbCancelled_CheckedChanged(object sender, EventArgs e) => PaymentTypeChanged?.Invoke(sender, e);
        private void rbRefunded_CheckedChanged(object sender, EventArgs e) => PaymentTypeChanged?.Invoke(sender, e);

        private void btnSave_Click(object sender, EventArgs e) => SaveClicked?.Invoke(sender, e);
        private void btnClose_Click(object sender, EventArgs e) => CloseClicked?.Invoke(sender, e);
        private void btnPreviewInvoice_Click(object sender, EventArgs e) => PreviewInvoiceClicked?.Invoke(sender, e);
        private void btnPrint_Click(object sender, EventArgs e) => PrintClicked?.Invoke(sender, e);
        private void btnEditCurrentOrder_Click(object sender, EventArgs e) => EditCurrentOrderClicked?.Invoke(sender, e);

        // This method is now purely for UI display and should not contain business logic
        private void dgvOrderItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // If you need to handle clicks for special columns (like an "Edit" button in the grid)
            // you would raise an event here, e.g.:
            // if (e.ColumnIndex == dgvEditItemButtonColumn.Index)
            // {
            //     EditOrderItemClicked?.Invoke(sender, new OrderItemEventArgs(e.RowIndex));
            // }
            // Otherwise, this can be removed if no interaction is needed for display-only grid.
        }

        // The following methods are now handled by the presenter and should be removed from the view
        // private void LoadOrderDetails() { ... }
        // private void ShowOrderSummaryLabels() { ... }
        // private void UpdateDeliveryChargeDisplay() { ... }
        // private string GetSelectedOrderType() { ... }
        // private string GetSelectedPaymentType() { ... }
        // private decimal GetItemPrice(int itemId) { ... }
        // private decimal CalculateTotalFromGrid() { ... }
        // private bool HasUnsavedChanges() { ... }
        // private bool SaveOrder() { ... }

        private void pnlCustomerInfo_Paint(object sender, PaintEventArgs e)
        {
            // UI drawing logic can remain here
        }
    }
}
