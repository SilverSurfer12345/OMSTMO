// frmPOS.cs (Modified)
using OrderManagement.Model;
using OrderManagement.Presenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing; // For KOT printing
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public partial class frmPOS : Form, IPosView
    {
        private IPosPresenter _presenter;
        private Timer alertTimer;
        private Timer orderAlertTimer;
        private Dictionary<string, Image> imageCache = new Dictionary<string, Image>();

        // Constructor for new order
        public frmPOS()
        {
            InitializeComponent();
            _presenter = new PosPresenter(this);
            InitializeCommon();
        }

        // Constructor for edit order
        public frmPOS(int orderId)
        {
            InitializeComponent();
            _presenter = new PosPresenter(this, orderId);
            InitializeCommon();
        }

        private void InitializeCommon()
        {
            this.FormClosing += (s, e) => FormClosingConfirmed?.Invoke(s, e); // Wire up event
            SetupOrderTypeButtons(); // Still handles UI button setup
            cbCustomDiscount.SelectedIndexChanged += (s, e) => DiscountSelected?.Invoke(s, cbCustomDiscount.SelectedItem.ToString());
            flpPreviousOrders.Paint += flpPreviousOrders_Paint; // Keep UI drawing logic here

            // Initialize and start timers (View's responsibility for UI-related timers)
            alertTimer = new Timer();
            alertTimer.Interval = 2 * 60 * 1000;
            alertTimer.Tick += (s, e) => AlertTimerTick?.Invoke(s, e);
            alertTimer.Start();
            AlertTimerTick?.Invoke(alertTimer, EventArgs.Empty); // Trigger immediately

            orderAlertTimer = new Timer();
            orderAlertTimer.Interval = 1000;
            orderAlertTimer.Tick += (s, e) => OrderAlertTimerTick?.Invoke(s, e);
            orderAlertTimer.Start();

            // Initial setup from presenter
            _presenter.Initialize();
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            basketGridView.BorderStyle = BorderStyle.FixedSingle;
            lstSearchResult.HorizontalScrollbar = false;
            lstSearchResult.IntegralHeight = false;
            lstSearchResult.Height = 150;
            DeliveryChargeManager.Initialize(this); // This should be handled by Presenter or a dedicated service
            ViewLoaded?.Invoke(this, EventArgs.Empty);
        }

        // --- IPosView Implementation ---

        // Properties for displaying data
        public string CustomerDetailsText { get => txtCustomerDetails.Text; set => txtCustomerDetails.Text = value; }
        public string CustomerTelephoneText { get => txtCstTelephone.Text; set => txtCstTelephone.Text = value; }
        public string AddressDisplayText { get => txtAddressDisplay.Text; set => txtAddressDisplay.Text = value; }
        public string PreviousOrdersText { get => txtPreviousOrders.Text; set => txtPreviousOrders.Text = value; }
        public string TotalPriceText { get => txtTotalPrice.Text; set => txtTotalPrice.Text = value; }
        public string PaymentOptionText { get => lblPaymentOption.Text; set => lblPaymentOption.Text = value; }
        public string SaveOrderButtonText { get => btnSaveOrder.Text; set => btnSaveOrder.Text = value; }
        public bool IsDeliveryChargeVisible { get => txtDeliveryCharge.Visible; set => txtDeliveryCharge.Visible = value; }
        public bool IsDeliveryChargeAmendButtonVisible { get => btnDeliveryChargeAmend.Visible; set => btnDeliveryChargeAmend.Visible = value; }
        public bool IsCustomerAddressVisible { get => lblCstAddress.Visible; set => lblCstAddress.Visible = value; }
        public bool IsAddressDisplayVisible { get => txtAddressDisplay.Visible; set => txtAddressDisplay.Visible = value; }
        public string CustomerActionButtonText { get => btnCustomerAction.Text; set => btnCustomerAction.Text = value; }
        public int SelectedDiscountIndex { get => cbCustomDiscount.SelectedIndex; set => cbCustomDiscount.SelectedIndex = value; }
        public Color CurrentOrdersButtonBackColor { get => btnCurrentOrders.BackColor; set => btnCurrentOrders.BackColor = value; }
        public Color CurrentOrdersButtonForeColor { get => btnCurrentOrders.ForeColor; set => btnCurrentOrders.ForeColor = value; }


        // Methods for displaying/updating UI
        public void ClearBasket() => basketGridView.Rows.Clear();
        public void AddBasketItem(string itemName, decimal originalPrice, decimal price, int quantity, decimal extraCharge)
        {
            int rowIndex = basketGridView.Rows.Add();
            DataGridViewRow row = basketGridView.Rows[rowIndex];
            row.Cells["dgvName"].Value = itemName;
            row.Cells["dgvOriginalPrice"].Value = originalPrice;
            row.Cells["dgvPrice"].Value = price;
            row.Cells["dgvQty"].Value = quantity;
            row.Cells["dgvExtraChargeValue"].Value = extraCharge;
        }
        public void UpdateBasketItemQuantity(int rowIndex, int quantity)
        {
            if (rowIndex >= 0 && rowIndex < basketGridView.Rows.Count)
            {
                basketGridView.Rows[rowIndex].Cells["dgvQty"].Value = quantity;
            }
        }
        public void UpdateBasketItemPriceAndExtraCharge(int rowIndex, decimal originalPrice, decimal newExtraCharge)
        {
            if (rowIndex >= 0 && rowIndex < basketGridView.Rows.Count)
            {
                var row = basketGridView.Rows[rowIndex];
                row.Cells["dgvExtraChargeValue"].Value = newExtraCharge;
                row.Cells["dgvPrice"].Value = originalPrice + newExtraCharge;
            }
        }
        public void RemoveBasketItem(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < basketGridView.Rows.Count)
            {
                basketGridView.Rows.RemoveAt(rowIndex);
            }
        }
        public void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon) => MessageBox.Show(message, title, buttons, icon);
        public DialogResult ShowConfirmation(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon) => MessageBox.Show(message, title, buttons, icon);

        public void LoadCategories(IEnumerable<string> categories)
        {
            flpCategoryBtns.Controls.Clear();
            foreach (string category in categories)
            {
                Button btn = new Button
                {
                    Text = category,
                    Tag = category,
                    BackColor = Color.FromArgb(241, 85, 126),
                    Font = new Font("Segoe UI Black", 10F, FontStyle.Bold),
                    Size = new Size(180, 50),
                    UseVisualStyleBackColor = false,
                    Margin = new Padding(5),
                    FlatStyle = FlatStyle.Flat
                };
                btn.Click += (s, e) => CategorySelected?.Invoke(s, category);
                flpCategoryBtns.Controls.Add(btn);
            }
        }

        public void LoadFoodItems(IEnumerable<FoodItemDisplayDto> items)
        {
            flpItemView.Controls.Clear();
            foreach (var item in items)
            {
                Button btn = CreateFoodItemButton(item.ItemName, item.ItemPrice, item.ImageData, (s, e) =>
                {
                    dynamic tag = ((Button)s).Tag;
                    _presenter.AddItemToBasket(tag.ItemName, tag.ItemPrice);
                });
                flpItemView.Controls.Add(btn);
            }
        }

        public void LoadPreviousOrderButtons(IEnumerable<PreviousOrderItemDisplayDto> items)
        {
            flpPreviousOrders.Controls.Clear();
            flpPreviousOrders.AutoScroll = true;
            flpPreviousOrders.FlowDirection = FlowDirection.TopDown;
            flpPreviousOrders.WrapContents = false;

            flpPreviousOrders.Controls.Add(new Label
            {
                Text = "Previously Ordered Items",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(5),
                ForeColor = Color.FromArgb(50, 55, 89),
                Dock = DockStyle.Top
            });
            flpPreviousOrders.Controls.Add(new Panel
            {
                Height = 2,
                BackColor = Color.FromArgb(241, 85, 126),
                Margin = new Padding(5, 0, 5, 10),
                Width = flpPreviousOrders.Width - 30,
                Dock = DockStyle.Top
            });

            TableLayoutPanel tablePanel = new TableLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Margin = new Padding(5),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };
            int buttonWidth = 112;
            int buttonMargin = 10;
            int totalButtonWidth = buttonWidth + buttonMargin;
            int availableWidth = flpPreviousOrders.Width - 30;
            int buttonsPerRow = Math.Max(1, availableWidth / totalButtonWidth);
            tablePanel.ColumnCount = buttonsPerRow;
            for (int i = 0; i < buttonsPerRow; i++) { tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / buttonsPerRow)); }

            int rowIndex = 0;
            int colIndex = 0;
            foreach (var item in items)
            {
                Button btn = CreateFoodItemButton(item.ItemName, item.ItemPrice, item.ImageData, (s, e) =>
                {
                    dynamic tag = ((Button)s).Tag;
                    _presenter.AddItemToBasket(tag.ItemName, tag.ItemPrice);
                });
                tablePanel.Controls.Add(btn, colIndex, rowIndex);
                colIndex++;
                if (colIndex >= buttonsPerRow) { colIndex = 0; rowIndex++; }
            }
            flpPreviousOrders.Controls.Add(tablePanel);
        }

        public void ClearCustomerSearchList() => lstSearchResult.Items.Clear();
        public void AddCustomerSearchItem(string customerDetails) => lstSearchResult.Items.Add(customerDetails);
        public void SetCustomerSearchListVisibility(bool visible) => lstSearchResult.Visible = visible;
        public void SetCustomerSearchListWidth(int width) => lstSearchResult.Width = width;

        public void SetOrderTypeButtonColor(string orderType, Color color)
        {
            // Use a helper method to set specific button colors
            ResetOrderTypeButtons(); // Reset all to default first
            switch (orderType.ToUpperInvariant())
            {
                case "DELIVERY": btnDel.BackColor = color; break;
                case "COLLECTION": btnCol.BackColor = color; break;
                case "WAITING": btnWaiting.BackColor = color; break;
                case "DINE IN": btnDineIn.BackColor = color; break;
            }
        }

        public void ResetAllOrderTypeButtonsColor(Color defaultColor)
        {
            btnDel.BackColor = defaultColor;
            btnCol.BackColor = defaultColor;
            btnWaiting.BackColor = defaultColor;
            btnDineIn.BackColor = defaultColor;
        }

        public void HideCustomerAddressFields()
        {
            lblCstAddress.Visible = false;
            txtAddressDisplay.Visible = false;
        }

        public void ShowCustomerAddressFields()
        {
            lblCstAddress.Visible = true;
            txtAddressDisplay.Visible = true;
        }

        public void CloseView() => this.Close();


        // --- Events (View raises these) ---
        public event EventHandler ViewLoaded;
        public event EventHandler FormClosingConfirmed;
        public event EventHandler BackToMainClicked;
        public event EventHandler NewOrderClicked;
        public event EventHandler SaveOrderClicked;
        public event EventHandler PaymentClicked;
        public event EventHandler KotClicked;
        public event EventHandler ClearBasketClicked;
        public event EventHandler CustomerActionClicked;
        public event EventHandler CallMeClicked;
        public event EventHandler BillClicked;
        public event EventHandler NotificationConfigClicked;
        public event EventHandler AutoAddPresetClicked;
        public event EventHandler DeliveryChargeAmendClicked;
        public event EventHandler<string> SearchFoodItemTextChanged;
        public event EventHandler<string> CustomerTelephoneTextChanged;
        public event EventHandler<string> CustomerDetailsTextChanged;
        public event EventHandler<string> CategorySelected;
        public event EventHandler<BasketItemInteractionEventArgs> BasketCellContentClicked;
        public event EventHandler<int> CustomerSearchItemSelected;
        public event EventHandler<string> DiscountSelected;
        public event EventHandler OrderAlertTimerTick;
        public event EventHandler AlertTimerTick;
        public event System.EventHandler<string> OrderTypeChanged;
        // --- Original UI Event Handlers (now just raise Presenter events) ---

        private void pictureBox2_Click(object sender, EventArgs e) => BackToMainClicked?.Invoke(sender, e);
        private void btnNew_Click(object sender, EventArgs e) => NewOrderClicked?.Invoke(sender, e);
        private void btnSaveOrder_Click(object sender, EventArgs e) => SaveOrderClicked?.Invoke(sender, e);
        private void btnPayment_Click(object sender, EventArgs e) => PaymentClicked?.Invoke(sender, e);
        private void btnKot_Click(object sender, EventArgs e) => KotClicked?.Invoke(sender, e);
        private void btnClear_click(object sender, EventArgs e) => ClearBasketClicked?.Invoke(sender, e);
        private void btnCustomerAction_Click(object sender, EventArgs e) => CustomerActionClicked?.Invoke(sender, e);
        private void btnCallMe_Click(object sender, EventArgs e) => CallMeClicked?.Invoke(sender, e);
        private void btnBill_Click(object sender, EventArgs e) => BillClicked?.Invoke(sender, e);
        private void btnNotificationConfig_Click(object sender, EventArgs e) => NotificationConfigClicked?.Invoke(sender, e);
        private void btnAutoAdd_Click(object sender, EventArgs e) => AutoAddPresetClicked?.Invoke(sender, e);
        private void btnDeliveryChargeAmend_Click(object sender, EventArgs e) => DeliveryChargeAmendClicked?.Invoke(sender, e);

        private void txtSearchFoodItem_TextChanged(object sender, EventArgs e) => SearchFoodItemTextChanged?.Invoke(sender, txtSearchFoodItem.Text);
        private void txtCstTelephone_TextChanged(object sender, EventArgs e) => CustomerTelephoneTextChanged?.Invoke(sender, txtCstTelephone.Text);
        private void txtCustomerDetails_TextChanged(object sender, EventArgs e) => CustomerDetailsTextChanged?.Invoke(sender, txtCustomerDetails.Text);
        private void lstSearchResult_SelectedIndexChanged(object sender, EventArgs e) => CustomerSearchItemSelected?.Invoke(sender, lstSearchResult.SelectedIndex);
        private void basketGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                BasketCellContentClicked?.Invoke(sender, new BasketItemInteractionEventArgs(e.RowIndex, basketGridView.Columns[e.ColumnIndex].Name));
            }
        }

        // --- Helper Methods (UI-specific) ---

        private void SetupOrderTypeButtons()
        {
            // Remove existing event handlers to prevent multiple subscriptions
            btnDel.Click -= HandleOrderTypeButtonClick;
            btnCol.Click -= HandleOrderTypeButtonClick;
            btnWaiting.Click -= HandleOrderTypeButtonClick;
            btnDineIn.Click -= HandleOrderTypeButtonClick;

            // Add new event handlers
            btnDel.Click += HandleOrderTypeButtonClick;
            btnCol.Click += HandleOrderTypeButtonClick;
            btnWaiting.Click += HandleOrderTypeButtonClick;
            btnDineIn.Click += HandleOrderTypeButtonClick;

            // Set tags for easy identification
            btnDel.Tag = "DELIVERY";
            btnCol.Tag = "COLLECTION";
            btnWaiting.Tag = "WAITING";
            btnDineIn.Tag = "DINE IN";
        }

        private void HandleOrderTypeButtonClick(object sender, EventArgs e)
{
    Button clickedButton = sender as Button;
    if (clickedButton != null && clickedButton.Tag is string orderType)
    {
        ResetOrderTypeButtons();
        clickedButton.BackColor = Color.Green;
        
        // Notify the presenter about the order type change
        OrderTypeChanged?.Invoke(this, orderType);
        
        // Handle visibility of delivery-related controls
        bool isDelivery = orderType == "DELIVERY";
        IsDeliveryChargeVisible = isDelivery;
        IsDeliveryChargeAmendButtonVisible = isDelivery;
        IsCustomerAddressVisible = isDelivery;
        IsAddressDisplayVisible = isDelivery;
    }
}

        private void ResetOrderTypeButtons()
        {
            Color defaultColor = Color.FromArgb(241, 85, 126);
            btnDel.BackColor = defaultColor;
            btnCol.BackColor = defaultColor;
            btnWaiting.BackColor = defaultColor;
            btnDineIn.BackColor = defaultColor;
        }

        private Button CreateFoodItemButton(string itemName, decimal itemPrice, byte[] imageData, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = $"{itemName}\n{itemPrice:C2}",
                Tag = new { ItemName = itemName, ItemPrice = itemPrice },
                Font = new Font("Segoe UI Black", 10F, FontStyle.Bold),
                Size = new Size(112, 88),
                UseVisualStyleBackColor = false,
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.BottomCenter
            };

            try
            {
                if (imageCache.ContainsKey(itemName))
                {
                    btn.BackgroundImage = imageCache[itemName];
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderSize = 1;
                }
                else if (imageData != null && imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image img = Image.FromStream(ms);
                        imageCache[itemName] = img;
                        btn.BackgroundImage = img;
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                        btn.ForeColor = Color.White;
                        btn.FlatAppearance.BorderSize = 1;
                    }
                }
                else
                {
                    btn.BackColor = Color.FromArgb(241, 85, 126);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image for {itemName}: {ex.Message}");
                btn.BackColor = Color.FromArgb(241, 85, 126);
            }

            btn.Click += onClick;
            return btn;
        }


        // The original `flpPreviousOrders_Paint` is purely UI drawing and can remain here.
        private void flpPreviousOrders_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.FromArgb(241, 85, 126), 2))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, flpPreviousOrders.Width - 1, flpPreviousOrders.Height - 1);
            }
            if (flpPreviousOrders.Controls.Count == 0) // Check if only the title/separator exist
            {
                using (Font font = new Font("Segoe UI", 12, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, 55, 89)))
                {
                    e.Graphics.DrawString("Previous Items", font, brush, 10, 10);
                }
            }
        }

        // Other empty or minor event handlers can be removed or kept if they serve a future purpose
        private void txtTotalPrice_TextChanged(object sender, EventArgs e) { /* Presenter will handle total price updates */ }
        private void lblPaymentOption_Click(object sender, EventArgs e) { /* No action needed */ }
        private void txtDeliveryCharge_TextChanged(object sender, EventArgs e) { /* No action needed */ }
        private void btnHold_Click(object sender, EventArgs e) { /* No action needed */ }
        private void btnWaiting_Click_1(object sender, EventArgs e) { /* No action needed */ }

        // Dispose timers on form closing
        private void frmPOS_FormClosing(object sender, FormClosingEventArgs e)
        {
            alertTimer?.Stop();
            alertTimer?.Dispose();
            orderAlertTimer?.Stop();
            orderAlertTimer?.Dispose();
            // flashTimer is managed by Presenter, so it should handle its disposal
        }

        private void btnCurrentOrders_Click(object sender, EventArgs e)
        {
            // Open the Order Progress form as a dialog
            using (var orderProgressForm = new frmOrderProgress())
            {
                orderProgressForm.ShowDialog(this);
            }
        }
    }
}