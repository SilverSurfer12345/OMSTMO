using Microsoft.VisualBasic;
using Newtonsoft.Json;
using OrderManagement.Model;
using OrderManagement.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderManagement.Model
{

    public partial class frmPOS : Form

    {

        public string totalPrice;
        private bool isDiscountApplied = false;
        private decimal discountRate = 0m;
        private Dictionary<string, List<DataGridViewRow>> basketItemsByCategory = new Dictionary<string, List<DataGridViewRow>>();
        private string orderType = "";
        private bool isOrderSaved = false;
        private static List<frmPOS> openEditingForms = new List<frmPOS>();

        private int id;
        private int orderIdValue;
        private string customerForename;
        private string customerSurname;
        private string customerTelephoneNo;
        private string customerEmail;
        private string customerHouseNameNumber;
        private string customerAddressLine1;
        private string customerAddressLine2;
        private string customerAddressLine3;
        private string customerAddressLine4;
        private string customerPostcode;
        private string currentOrderType = "WAITING";
        public string TelephoneNo { get; set; }
        private Dictionary<string, Image> imageCache = new Dictionary<string, Image>();



        private Boolean customerExists = false;
        private Boolean loadedPreviousOrder = false;

        private DataTable customerSearchDataTable = new DataTable();

        private Timer alertTimer;
        private Timer flashTimer;
        private Timer orderAlertTimer;
        private bool isFlashing = false;

        private bool isEditMode = false;
        private int editOrderId = 0;

        // Add a constructor that takes an orderId parameter






        public frmPOS()
        {

            InitializeComponent();
            this.FormClosing += frmPOS_FormClosing;
            SetupOrderTypeButtons(false);
            loadCategories();
            cbCustomDiscount.SelectedIndex = 0;
            alertTimer = new Timer();
            alertTimer.Interval = 2 * 60 * 1000; // 2 minutes in milliseconds
            alertTimer.Tick += AlertTimer_Tick;
            alertTimer.Start();
            flpPreviousOrders.Paint += flpPreviousOrders_Paint;
            txtDeliveryCharge.Visible = false; // Hide by default
            btnDeliveryChargeAmend.Visible = false;

            // Trigger the Tick event immediately
            AlertTimer_Tick(alertTimer, EventArgs.Empty);

            // Setup order alert timer
            orderAlertTimer = new Timer();
            orderAlertTimer.Interval = 1000; // 1 second
            orderAlertTimer.Tick += OrderAlertTimer_Tick;
            orderAlertTimer.Start();


        }


        public frmPOS(int orderId)
        {
            InitializeComponent();

            // Set edit mode
            isEditMode = true;
            editOrderId = orderId;
            txtDeliveryCharge.Visible = false; // Hide by default
            btnDeliveryChargeAmend.Visible = false;
            // Register this form as an editing form
            openEditingForms.Add(this);

            // Setup the form
            this.FormClosing += frmPOS_FormClosing;
            SetupOrderTypeButtons(true);
            loadCategories();
            cbCustomDiscount.SelectedIndex = 0;

            // Load the order details
            LoadOrderForEditing(orderId);

            // Update the form title to indicate edit mode
            this.Text = "POS - Edit Order #" + orderId;

            // Change the Save Order button text
            btnSaveOrder.Text = "Update Order";
        }





        private void LoadOrderForEditing(int orderId)
        {
            try
            {
                var orderManager = new OrderManager();
                var order = orderManager.GetOrderDetails(orderId);

                if (order == null)
                {
                    MessageBox.Show("Order not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }


                // Set fields from DTO
                orderIdValue = order.OrderId;
                id = order.CustomerId;
                customerForename = order.Forename;
                customerSurname = order.Surname;
                customerTelephoneNo = order.TelephoneNo;
                customerEmail = order.Email;
                customerHouseNameNumber = order.HouseNameNumber;
                customerAddressLine1 = order.AddressLine1;
                customerAddressLine2 = order.AddressLine2;
                customerAddressLine3 = order.AddressLine3;
                customerAddressLine4 = order.AddressLine4;
                customerPostcode = order.Postcode;
                currentOrderType = order.OrderType;

                // Direct UI updates instead of PerformClick
                Color selectedColor = Color.Green;

                // Reset all buttons first
                ResetOrderTypeButtons();

                // Now set the appropriate button and UI based on order type
                if (string.Equals(currentOrderType, "DELIVERY", StringComparison.OrdinalIgnoreCase))
                {
                    btnDel.BackColor = selectedColor;
                    lblCstAddress.Visible = true;
                    lblAddressDisplay.Visible = true;
                    txtDeliveryCharge.Visible = true;
                    btnDeliveryChargeAmend.Visible = true;
                    OrderManagement.Model.DeliveryChargeManager.AutoCalculateDeliveryCharge(this);
                }
                else if (string.Equals(currentOrderType, "COLLECTION", StringComparison.OrdinalIgnoreCase))
                {
                    btnCol.BackColor = selectedColor;
                    lblCstAddress.Visible = false;
                    lblAddressDisplay.Visible = false;
                    txtDeliveryCharge.Visible = false;
                    btnDeliveryChargeAmend.Visible = false;
                }
                else if (string.Equals(currentOrderType, "WAITING", StringComparison.OrdinalIgnoreCase))
                {
                    btnWaiting.BackColor = selectedColor;
                    lblCstAddress.Visible = false;
                    lblAddressDisplay.Visible = false;
                    txtDeliveryCharge.Visible = false;
                    btnDeliveryChargeAmend.Visible = false;
                }
                else if (string.Equals(currentOrderType, "DINE IN", StringComparison.OrdinalIgnoreCase))
                {
                    btnDineIn.BackColor = selectedColor;
                    lblCstAddress.Visible = false;
                    lblAddressDisplay.Visible = false;
                    txtDeliveryCharge.Visible = false;
                    btnDeliveryChargeAmend.Visible = false;
                }

                // Continue with the rest of the method
                txtCustomerDetails.Text = $"{customerForename} {customerSurname}";
                txtCstTelephone.Text = customerTelephoneNo;
                lblAddressDisplay.Text = order.Address;

                // Load order items
                var items = orderManager.GetOrderItems(orderId);
                basketGridView.Rows.Clear();
                basketItemsByCategory.Clear();
                foreach (var item in items)
                {
                    int rowIndex = basketGridView.Rows.Add();
                    DataGridViewRow row = basketGridView.Rows[rowIndex];

                    row.Cells["dgvName"].Value = item.ItemName;
                    row.Cells["dgvPrice"].Value = item.ItemPrice;
                    row.Cells["dgvQty"].Value = item.Quantity; // Set the correct quantity
                    row.Cells["dgvOriginalPrice"].Value = item.ItemPrice;

                    // Organize items by category if using that feature
                    string category = "General"; // Default category
                    if (!basketItemsByCategory.ContainsKey(category))
                        basketItemsByCategory[category] = new List<DataGridViewRow>();
                    basketItemsByCategory[category].Add(row);

                }

                CalculateTotal();
                LoadPreviousOrderItems(id);
                isOrderSaved = true;
                isEditMode = true;
                editOrderId = orderId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void frmPOS_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Remove this form from the openEditingForms list if it's there
                if (isEditMode)
                {
                    openEditingForms.Remove(this);
                }

                // Check if we're in edit mode with unsaved changes
                if (isEditMode && !isOrderSaved && basketGridView.Rows.Count > 0)
                {
                    DialogResult result = MessageBox.Show(
                        "You have unsaved changes to this order. Do you want to save before closing?",
                        "Unsaved Changes",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Trigger the save button click
                        btnSaveOrder_Click(this, EventArgs.Empty);

                        // If saving failed, cancel the form closing
                        if (!isOrderSaved)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        // User canceled, don't close the form
                        e.Cancel = true;
                        return;
                    }
                    // If No, just continue closing - DO NOT reopen any forms
                }

                // Stop the timers
                if (alertTimer != null)
                {
                    alertTimer.Stop();
                    alertTimer.Dispose();
                }

                if (orderAlertTimer != null)
                {
                    orderAlertTimer.Stop();
                    orderAlertTimer.Dispose();
                }

                if (flashTimer != null)
                {
                    flashTimer.Stop();
                    flashTimer.Dispose();
                }

                // Force a database checkpoint to ensure data is written to disk
                DatabaseManager.ForceCheckpoint();

                // Close all database connections
                DatabaseManager.CloseConnections();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FormClosing: " + ex.Message);
            }
        }






        private void OrderAlertTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Check if there are any critical orders
                bool hasCriticalOrders = CheckForCriticalOrders();

                if (hasCriticalOrders)
                {
                    // Toggle the button color for flashing effect
                    isFlashing = !isFlashing;

                    if (isFlashing)
                    {
                        btnCurrentOrders.BackColor = Color.Red;
                        btnCurrentOrders.ForeColor = Color.White;
                    }
                    else
                    {
                        btnCurrentOrders.BackColor = SystemColors.Control;
                        btnCurrentOrders.ForeColor = Color.Black;
                    }
                }
                else
                {
                    // Reset to normal appearance if no critical orders
                    if (btnCurrentOrders.BackColor != SystemColors.Control || btnCurrentOrders.ForeColor != Color.Black)
                    {
                        btnCurrentOrders.BackColor = SystemColors.Control;
                        btnCurrentOrders.ForeColor = Color.Black;
                        isFlashing = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't show a message box to avoid interrupting the user
                Console.WriteLine("Error in OrderAlertTimer_Tick: " + ex.Message);

                // Reset button to normal state in case of error
                btnCurrentOrders.BackColor = SystemColors.Control;
                btnCurrentOrders.ForeColor = Color.Black;
                isFlashing = false;
            }
        }


        private bool CheckForCriticalOrders()
        {
            try
            {
                // Query to check for critical orders
                string query = @"
            SELECT COUNT(*) FROM Orders O
            WHERE O.completion = 'no' 
            AND CAST(O.OrderDate AS DATE) = CAST(GETDATE() AS DATE)
            AND (
                (O.OrderType = 'COLLECTION' AND DATEDIFF(MINUTE, O.OrderDate, GETDATE()) > 20)
                OR
                (O.OrderType = 'DELIVERY' AND DATEDIFF(MINUTE, O.OrderDate, GETDATE()) > 60)
            )";

                object result = DatabaseManager.ExecuteScalar(query);
                int criticalOrderCount = result == null ? 0 : Convert.ToInt32(result);

                Console.WriteLine($"Critical order count: {criticalOrderCount}"); // Debug output

                return criticalOrderCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking for critical orders: " + ex.Message);
                return false;
            }
        }




        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // If in edit mode, check for unsaved changes
            if (isEditMode && !isOrderSaved && basketGridView.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "You have unsaved changes to this order. Do you want to save before closing?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Trigger the save button click
                    btnSaveOrder_Click(this, EventArgs.Empty);

                    // If saving failed, don't proceed
                    if (!isOrderSaved)
                    {
                        return;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    // User canceled, don't close the form
                    return;
                }
                // If No, just continue closing - DO NOT reopen any forms
            }

            // Create and show the main form
            frmMain frm = new frmMain();
            frm.Show();

            // Always hide this form regardless of edit mode
            this.Hide();
        }









        private void frmPOS_Load(object sender, EventArgs e)
        {
            basketGridView.BorderStyle = BorderStyle.FixedSingle;
            lstSearchResult.HorizontalScrollbar = false; // Optional
            lstSearchResult.IntegralHeight = false; // Avoids clipping on resizing
            lstSearchResult.Height = 150; // Adjust as needed
            DeliveryChargeManager.Initialize(this);

        }

        private void SetupOrderTypeButtons(bool skipDefaultSelection = false)
        {
            // Set default button colors
            Color defaultColor = Color.FromArgb(241, 85, 126);
            Color selectedColor = Color.Green;

            btnDel.BackColor = defaultColor;
            btnCol.BackColor = defaultColor;
            btnWaiting.BackColor = defaultColor;
            btnDineIn.BackColor = defaultColor;

            // Remove existing event handlers if any
            btnDel.Click -= new EventHandler(btnDel_Click);
            btnCol.Click -= new EventHandler(btnCol_Click);
            btnWaiting.Click -= new EventHandler(btnWaiting_Click);
            btnDineIn.Click -= new EventHandler(btnDineIn_Click);

            // Add click handlers (your existing click handlers here)
            btnDel.Click += (s, e) => {

                if (currentOrderType != "DELIVERY")
                {
                    ResetOrderTypeButtons();
                    btnDel.BackColor = selectedColor;
                    currentOrderType = "DELIVERY";
                    lblCstAddress.Visible = true;
                    lblAddressDisplay.Visible = true;

                    // Centralized delivery charge UI and calculation
                    OrderManagement.Model.DeliveryChargeManager.OrderTypeChanged(this, "DELIVERY");
                    CalculateTotal();
                }
            };

            btnCol.Click += (s, e) => {
                if (currentOrderType != "COLLECTION")
                {
                    ResetOrderTypeButtons();
                    btnCol.BackColor = selectedColor;
                    currentOrderType = "COLLECTION";
                    lblCstAddress.Visible = false;
                    lblAddressDisplay.Visible = false;

                    // Hide delivery charge controls
                    txtDeliveryCharge.Visible = false;
                    btnDeliveryChargeAmend.Visible = false;

                    CalculateTotal();
                }
            };

            btnWaiting.Click += (s, e) => {
                if (currentOrderType != "WAITING")
                {
                    ResetOrderTypeButtons();
                    btnWaiting.BackColor = selectedColor;
                    currentOrderType = "WAITING";
                    lblCstAddress.Visible = false;
                    lblAddressDisplay.Visible = false;

                    // Hide delivery charge controls
                    txtDeliveryCharge.Visible = false;
                    btnDeliveryChargeAmend.Visible = false;

                    CalculateTotal();
                }
            };

            btnDineIn.Click += (s, e) => {
                if (currentOrderType != "DINE IN")
                {
                    ResetOrderTypeButtons();
                    btnDineIn.BackColor = selectedColor;
                    currentOrderType = "DINE IN";
                    lblCstAddress.Visible = false;
                    lblAddressDisplay.Visible = false;

                    // Hide delivery charge controls
                    txtDeliveryCharge.Visible = false;
                    btnDeliveryChargeAmend.Visible = false;

                    CalculateTotal();
                }
            };

            if (!skipDefaultSelection)
            {
                // Set default selected button
                btnWaiting.BackColor = selectedColor;
                currentOrderType = "WAITING";

                // Hide address fields for collection (default)
                lblCstAddress.Visible = false;
                lblAddressDisplay.Visible = false;
            }
        }

        // Define empty methods to avoid errors if they're still referenced somewhere
        private void btnDel_Click(object sender, EventArgs e) { }
        private void btnCol_Click(object sender, EventArgs e) { }
        private void btnWaiting_Click(object sender, EventArgs e) { }
        private void btnDineIn_Click(object sender, EventArgs e) { }


        private void ResetOrderTypeButtons()
        {
            Color defaultColor = Color.FromArgb(241, 85, 126); // Your default color
            btnDel.BackColor = defaultColor;
            btnCol.BackColor = defaultColor;
            btnWaiting.BackColor = defaultColor;
            btnDineIn.BackColor = defaultColor;
        }


        private int GetExistingItemIndex(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return -1;
            }

            for (int i = 0; i < basketGridView.Rows.Count; i++)
            {
                string cellValue = SafeOperations.SafeGetCellString(basketGridView, i, "dgvName");
                if (cellValue == itemName)
                {
                    return i;
                }
            }
            return -1;
        }


        // Add this method right after your existing CreateFoodItemButton method
        private Button CreateFoodItemButton(string itemName, decimal itemPrice, EventHandler onClick)
        {
            // Call the 4-parameter version with null image data
            return CreateFoodItemButton(itemName, itemPrice, null, onClick);
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
                // First check if image is in cache
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

                        // Store in cache
                        imageCache[itemName] = img;

                        btn.BackgroundImage = img;
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                        btn.ForeColor = Color.White;
                        btn.FlatAppearance.BorderSize = 1;
                    }
                }
                else
                {
                    // No image data available, set default background color
                    btn.BackColor = Color.FromArgb(241, 85, 126);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image for {itemName}: {ex.Message}");
                btn.BackColor = Color.FromArgb(241, 85, 126); // Default color as fallback
            }

            btn.Click += onClick;
            return btn;
        }





        private void SetButtonBackgroundImageAsync(Button btn, byte[] imageData, string itemName)
        {
            if (imageData == null || imageData.Length == 0)
                return;

            // Create a placeholder image immediately
            btn.BackColor = Color.LightGray;

            // Load the real image in the background
            Task.Run(() => {
                try
                {
                    Image img;
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        img = Image.FromStream(ms);
                        imageCache[itemName] = img;
                    }

                    // Update UI on the main thread
                    if (!btn.IsDisposed && btn.InvokeRequired)
                    {
                        btn.Invoke(new Action(() => {
                            btn.BackgroundImage = img;
                            btn.BackgroundImageLayout = ImageLayout.Stretch;
                            btn.ForeColor = Color.White;
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image for {itemName}: {ex.Message}");
                }
            });
        }





        private void txtSearchFoodItem_TextChanged(object sender, EventArgs e)
        {
            // Implement your search/filter logic here.
            // For example:
            string searchText = txtSearchFoodItem.Text.Trim();

            // Clear the item view panel
            flpItemView.Controls.Clear();

            if (!string.IsNullOrEmpty(searchText))
            {
                // Query your food items by search text
                string query = "SELECT Item, price FROM foodItems WHERE Item LIKE @searchText";
                var parameters = new Dictionary<string, object>
        {
            { "@searchText", "%" + searchText + "%" }
        };
                DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string itemName = row["Item"].ToString();
                        decimal itemPrice = Convert.ToDecimal(row["price"]);
                        Button btn = CreateFoodItemButton(itemName, itemPrice, (s, args) =>
                        {
                            dynamic tag = ((Button)s).Tag;
                            AddItemToBasket(tag.ItemName, tag.ItemPrice);
                            txtSearchFoodItem.Text = ""; // Optionally clear search after selection
                        });
                        flpItemView.Controls.Add(btn);
                    }
                }
            }
            else
            {
                // Optionally reload default items or clear the view
            }
        }



        private void UpdateExistingItemInBasket(int rowIndex, decimal itemPrice)
        {
            if (rowIndex < 0 || rowIndex >= basketGridView.Rows.Count)
            {
                return;
            }

            int currentQty = SafeOperations.SafeGetCellInt(basketGridView, rowIndex, "dgvQty");
            decimal currentPrice = SafeOperations.SafeGetCellDecimal(basketGridView, rowIndex, "dgvPrice");

            basketGridView.Rows[rowIndex].Cells["dgvQty"].Value = currentQty + 1;
            basketGridView.Rows[rowIndex].Cells["dgvPrice"].Value = currentPrice + itemPrice;
        }


        // C# OrderManagement\Model\frmPOS.cs
        private void AddNewItemToBasket(string itemName, decimal itemPrice)
        {
            try
            {
                // Get the latest price from the database
                string query = "SELECT price FROM foodItems WHERE Item = @ItemName";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@ItemName", itemName }
        };

                object result = DatabaseManager.ExecuteScalar(query, parameters);
                decimal dbPrice = result != null ? Convert.ToDecimal(result) : itemPrice;

                // Add a new row to the basket
                int rowIndex = basketGridView.Rows.Add();
                DataGridViewRow row = basketGridView.Rows[rowIndex];

                // Set the values for the new row
                row.Cells["dgvName"].Value = itemName;
                row.Cells["dgvOriginalPrice"].Value = dbPrice; // Use price from database
                row.Cells["dgvPrice"].Value = dbPrice; // Initial price is the same as original
                row.Cells["dgvQty"].Value = 1;
                row.Cells["dgvExtraChargeValue"].Value = 0; // Initialize extra charge to 0

                // Organize items by category if using that feature
                string category = "General"; // Default category
                if (!basketItemsByCategory.ContainsKey(category))
                    basketItemsByCategory[category] = new List<DataGridViewRow>();
                basketItemsByCategory[category].Add(row);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding item to basket: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void basketGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle Remove button
            if (e.RowIndex >= 0 && basketGridView.Columns[e.ColumnIndex].Name == "dgvDeleteBasketItem")
            {
                var row = basketGridView.Rows[e.RowIndex];
                int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);

                if (quantity > 1)
                {
                    // Just decrement the quantity by 1
                    row.Cells["dgvQty"].Value = quantity - 1;

                    // Do NOT recalculate the price - leave dgvPrice unchanged

                    // Mark as unsaved since we modified the basket
                    isOrderSaved = false;
                }
                else
                {
                    // If quantity is 1, remove the row from the basket
                    basketGridView.Rows.RemoveAt(e.RowIndex);
                    isOrderSaved = false;
                }

                // Update the total price label
                UpdateTotalPriceLabel();
            }

            // Handle Extra Charge button
            if (basketGridView.Columns[e.ColumnIndex].Name == "dgvExtraCharge" && e.RowIndex >= 0)
            {
                using (var popup = new OrderManagement.View.frmExtraChargePopup())
                {
                    var row = basketGridView.Rows[e.RowIndex];

                    // Get the current extra charge value
                    decimal currentExtra = 0;
                    decimal.TryParse(row.Cells["dgvExtraChargeValue"].Value?.ToString(), out currentExtra);
                    popup.SetCurrentExtraCharge(currentExtra);

                    if (popup.ShowDialog(this) == DialogResult.OK)
                    {
                        // Get the new extra charge from the popup
                        decimal newExtra = popup.GetSelectedAmount();

                        // Store the new extra charge value
                        row.Cells["dgvExtraChargeValue"].Value = newExtra;

                        // Update the price with the extra charge
                        decimal originalPrice = Convert.ToDecimal(row.Cells["dgvOriginalPrice"].Value);
                        row.Cells["dgvPrice"].Value = originalPrice + newExtra;

                        // Update the total price
                        UpdateTotalPriceLabel();

                        // Mark as unsaved since we modified the basket
                        isOrderSaved = false;
                    }
                }
            }
        }













        public decimal UpdateBasketPrices(decimal price, int quantity)
        {
            return quantity * price;
        }

        private void UpdateCustomerDetails(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            this.id = Convert.ToInt32(dt.Rows[0]["Id"]);
            this.customerForename = dt.Rows[0]["forename"].ToString();
            this.customerSurname = dt.Rows[0]["surname"].ToString();
            this.customerTelephoneNo = dt.Rows[0]["telephoneNo"].ToString();
            this.customerEmail = dt.Columns.Contains("Email") ? dt.Rows[0]["Email"].ToString() : "";
            this.customerHouseNameNumber = dt.Rows[0]["houseNameNumber"].ToString();
            this.customerAddressLine1 = dt.Rows[0]["AddressLine1"].ToString();
            this.customerAddressLine2 = dt.Rows[0]["AddressLine2"].ToString();
            this.customerAddressLine3 = dt.Rows[0]["AddressLine3"].ToString();
            this.customerAddressLine4 = dt.Rows[0]["AddressLine4"].ToString();
            this.customerPostcode = dt.Rows[0]["Postcode"].ToString();

            txtCustomerDetails.Text = customerForename + " " + customerSurname;
            txtCstTelephone.Text = customerTelephoneNo;
            txtPreviousOrders.Text = dt.Rows[0]["previousOrders"].ToString();

            lblAddressDisplay.Text = $"{customerHouseNameNumber} {customerAddressLine1}, " +
                                    $"{customerAddressLine2}, {customerAddressLine3}, " +
                                    $"{customerAddressLine4}, {customerPostcode}";

            customerExists = true;
            btnCustomerAction.Text = "Update Customer";
            LoadPreviousOrderItems(id);

            // Always update delivery charge and total price
            if (currentOrderType == "DELIVERY")
            {
                OrderManagement.Model.DeliveryChargeManager.AutoCalculateDeliveryCharge(this);
            }
            // Always recalculate the total price to reflect any delivery charge change
            CalculateTotal();
        }


        private void btnCustomerAction_Click(object sender, EventArgs e)
        {
            frmCustomerAdd customerAddForm = new frmCustomerAdd();
            customerAddForm.txtTelephoneNo.Text = txtCstTelephone.Text;
            string telephoneNo = customerAddForm.txtTelephoneNo.Text;

            if (!customerExists)
            {
                customerAddForm.id = 0;
                customerAddForm.label1.Text = "Add New Customer";

                if (!string.IsNullOrEmpty(txtCstTelephone.Text))
                {
                    customerAddForm.txtTelephoneNo.Text = txtCstTelephone.Text;
                }

                if (!string.IsNullOrEmpty(txtCustomerDetails.Text))
                {
                    string[] nameParts = txtCustomerDetails.Text.Split(' ');
                    customerAddForm.txtForename.Text = nameParts[0];

                    if (nameParts.Length > 1)
                    {
                        customerAddForm.txtSurname.Text = string.Join(" ", nameParts.Skip(1));
                    }
                }
            }
            else
            {
                customerAddForm.label1.Text = "Update Customer";
                customerAddForm.id = Convert.ToInt32(id);
                customerAddForm.txtForename.Text = customerForename;
                customerAddForm.txtSurname.Text = customerSurname;

                if (!string.IsNullOrEmpty(customerTelephoneNo))
                {
                    customerAddForm.txtTelephoneNo.Text = customerTelephoneNo;
                }

                customerAddForm.txtEmail.Text = customerEmail;
                customerAddForm.txtHouseNameNumber.Text = customerHouseNameNumber;
                customerAddForm.txtAddressLine1.Text = customerAddressLine1;
                customerAddForm.txtAddressLine2.Text = customerAddressLine2;
                customerAddForm.txtAddressLine3.Text = customerAddressLine3;
                customerAddForm.txtAddressLine4.Text = customerAddressLine4;
                customerAddForm.txtPostcode.Text = customerPostcode;
            }

            customerAddForm.SaveClicked += (s, args) =>
            {
                telephoneNo = frmCustomerAdd.telephoneNumber;
                string query = "SELECT * FROM customers WHERE telephoneNo = @telephoneNo";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@telephoneNo", telephoneNo }
        };

                DataTable dt = MainClass.getDataFromTable(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    UpdateCustomerDetails(dt);
                }
            };

            customerAddForm.ShowDialog();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            // If in edit mode, prompt the user before discarding changes
            if (isEditMode)
            {
                DialogResult result = MessageBox.Show(
                    "You are currently editing an order. Are you sure you want to start a new order?",
                    "Confirm New Order",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return; // User chose to continue editing
                }

                // User confirmed, exit edit mode and close this form
                // This will trigger the FormClosing event which handles cleanup
                this.Close();

                // Open a new non-editing POS form
                frmPOS newPosForm = new frmPOS();
                newPosForm.Show();

                // Don't continue with the rest of this method
                return;
            }

            // If not in edit mode, just reset the form for a new order
            CloseRelatedForms();
            ResetFormState();
            basketItemsByCategory.Clear();
            isOrderSaved = false;
        }





        // Add this method to close related forms
        private void CloseRelatedForms()
        {
            // Create a list to store forms that need to be closed
            List<Form> formsToClose = new List<Form>();

            // Find all related forms
            foreach (Form form in Application.OpenForms)
            {
                // Skip the current form (we don't want to close ourselves)
                if (form == this)
                    continue;

                // Check for other frmPOS instances that are in edit mode
                if (form is frmPOS otherPosForm)
                {
                    // Only close if it's in edit mode
                    if (otherPosForm.IsInEditMode())
                    {
                        formsToClose.Add(form);
                    }
                }
                // Check for OrderViewForm
                else if (form is OrderViewForm)
                {
                    formsToClose.Add(form);
                }
                // Check for frmOrderHistory
                else if (form is frmOrderHistory)
                {
                    formsToClose.Add(form);
                }
                // Check for frmPayment
                else if (form is frmPayment)
                {
                    formsToClose.Add(form);
                }
                // Check for frmCustomerAdd
                else if (form is frmCustomerAdd)
                {
                    formsToClose.Add(form);
                }
                // Check for any other editing forms
                else if (form.Name.Contains("Edit") || form.Name.Contains("Add"))
                {
                    formsToClose.Add(form);
                }
            }

            // Close all the forms we found
            foreach (Form form in formsToClose)
            {
                try
                {
                    form.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error closing form {form.Name}: {ex.Message}");
                }
            }
        }



        public bool IsInEditMode()
        {
            return isEditMode;
        }



        // C# OrderManagement\Model\frmPOS.cs
        private void ResetFormState()
        {
            basketGridView.Rows.Clear();

            txtCustomerDetails.Text = string.Empty;
            txtCstTelephone.Text = string.Empty;
            lblAddressDisplay.Text = string.Empty;
            txtPreviousOrders.Text = string.Empty;
            txtTotalPrice.Text = "£0.00"; // <-- Reset total price

            btnDeliveryChargeAmend.Visible = false;
            cbCustomDiscount.SelectedIndex = -1;

            // Reset order type buttons
            ResetOrderTypeButtons();
            btnWaiting.BackColor = Color.Green; // Set Waiting as default
            currentOrderType = "WAITING";       // <-- Default to WAITING

            lblCstAddress.Visible = false;
            lblAddressDisplay.Visible = false;
            lblPaymentOption.Text = string.Empty;

            id = 0;
            orderIdValue = 0;
            customerForename = string.Empty;
            customerSurname = string.Empty;
            customerTelephoneNo = string.Empty;
            customerEmail = string.Empty;
            customerHouseNameNumber = string.Empty;
            customerAddressLine1 = string.Empty;
            customerAddressLine2 = string.Empty;
            customerAddressLine3 = string.Empty;
            customerAddressLine4 = string.Empty;
            customerPostcode = string.Empty;

            customerExists = false;
            btnCustomerAction.Text = "Add Customer";

            flpPreviousOrders.Controls.Clear();
            isOrderSaved = false;

            txtDeliveryCharge.Visible = false;
            btnDeliveryChargeAmend.Visible = false;
            txtDeliveryCharge.Text = "£0.00"; // <-- Reset delivery charge
        }


        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (basketGridView.Rows.Cast<DataGridViewRow>().Count(r => r.Cells["dgvName"].Value != null && !string.IsNullOrWhiteSpace(r.Cells["dgvName"].Value.ToString())) == 0)
                {
                    MessageBox.Show("Please add items to the basket before saving the order.", "Empty Basket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int customerId;

                // Check if we already have a customer selected
                if (customerExists && id > 0)
                {
                    customerId = id;
                }
                else
                {
                    // Parse customer name
                    string customerName = txtCustomerDetails.Text.Trim();
                    string forename = customerName;
                    string surname = "";

                    if (customerName.Contains(" "))
                    {
                        string[] nameParts = customerName.Split(new[] { ' ' }, 2);
                        forename = nameParts[0];
                        surname = nameParts[1];
                    }

                    // Check if customer exists by phone number
                    if (!string.IsNullOrEmpty(txtCstTelephone.Text))
                    {
                        string query = "SELECT Id FROM customers WHERE telephoneNo = @telephoneNo";
                        Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@telephoneNo", txtCstTelephone.Text.Trim() }
                };

                        DataTable dt = MainClass.getDataFromTable(query, parameters);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            customerId = Convert.ToInt32(dt.Rows[0]["Id"]);
                        }
                        else
                        {
                            // Create new customer
                            customerId = MainClass.CreateCustomer(
                                forename,
                                surname,
                                txtCstTelephone.Text.Trim(),
                                lblAddressDisplay.Text.Trim()
                            );
                        }
                    }
                    else if (!string.IsNullOrEmpty(customerName))
                    {
                        // Create customer with just name
                        customerId = MainClass.CreateCustomer(
                            forename,
                            surname,
                            null,
                            lblAddressDisplay.Text.Trim()
                        );
                    }
                    else
                    {
                        // Create a guest customer
                        customerId = MainClass.CreateCustomer(
                            "Guest",
                            "Customer",
                            null,
                            null
                        );
                    }
                }

                if (customerId <= 0)
                {
                    MessageBox.Show("Failed to create or find customer. Please check customer details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get order details
                DateTime currentTime = DateTime.Now;

                // CHANGE HERE: Always use CalculateTotal() to ensure accurate pricing
                decimal totalPrice = CalculateTotal();

                string orderType = currentOrderType; // Use the current order type
                if (string.IsNullOrEmpty(orderType))
                {
                    orderType = "COLLECTION"; // Default to collection if not set
                }

                string paymentType = lblPaymentOption.Text;
                if (string.IsNullOrEmpty(paymentType))
                {
                    paymentType = "PENDING"; // Default payment status
                }

                int orderId;

                if (isEditMode && editOrderId > 0)
                {
                    // Parse the delivery charge
                    decimal deliveryCharge = OrderManagement.Model.DeliveryChargeManager.GetDeliveryCharge(this);

                    // Update existing order
                    string updateOrderQuery = @"
        UPDATE Orders 
        SET 
            CustomerId = @CustomerId, 
            OrderType = @OrderType, 
            TotalPrice = @TotalPrice, 
            PaymentType = @PaymentType, 
            Address = @Address,
            DeliveryCharge = @DeliveryCharge
        WHERE 
            OrderId = @OrderId";

                    Dictionary<string, object> updateOrderParams = new Dictionary<string, object>
    {
        { "@CustomerId", customerId },
        { "@OrderType", orderType },
        { "@TotalPrice", totalPrice },
        { "@PaymentType", paymentType },
        { "@Address", lblAddressDisplay.Text },
        { "@DeliveryCharge", deliveryCharge },
        { "@OrderId", editOrderId }
    };

                    int result = DatabaseManager.ExecuteNonQuery(updateOrderQuery, updateOrderParams);

                    if (result <= 0)
                    {
                        MessageBox.Show("Failed to update order. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    orderId = editOrderId;

                    // Delete existing order items
                    string deleteItemsQuery = "DELETE FROM OrderItems WHERE OrderId = @OrderId";
                    Dictionary<string, object> deleteItemsParams = new Dictionary<string, object>
            {
                { "@OrderId", orderId }
            };

                    DatabaseManager.ExecuteNonQuery(deleteItemsQuery, deleteItemsParams);
                }
                else
                {
                    // Save new order
                    // Parse the delivery charge from the textbox
                    decimal deliveryCharge = OrderManagement.Model.DeliveryChargeManager.GetDeliveryCharge(this);
                    decimal presetCharges = OrderManagement.View.frmPresetCharges.GetTotalPresetCharges();
                    orderId = MainClass.SaveOrder(customerId, orderType, totalPrice, currentTime, paymentType, lblAddressDisplay.Text, deliveryCharge, presetCharges);

                    if (orderId == -1)
                    {
                        MessageBox.Show("Failed to save order. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Save each item in the basket
                bool allItemsSaved = true;
                foreach (DataGridViewRow row in basketGridView.Rows)
                {
                    if (row.Cells["dgvName"].Value == null) continue;

                    string itemName = row.Cells["dgvName"].Value.ToString();
                    decimal itemPrice = Convert.ToDecimal(row.Cells["dgvOriginalPrice"].Value);
                    int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);
                    decimal extraCharge = row.Cells["dgvExtraChargeValue"].Value != null ?
                        Convert.ToDecimal(row.Cells["dgvExtraChargeValue"].Value) : 0m;

                    int result = MainClass.SaveOrderItem(orderId, itemName, itemPrice, quantity, extraCharge);
                    if (result == -1)
                    {
                        allItemsSaved = false;
                        MessageBox.Show($"Failed to save item: {itemName}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }



                if (allItemsSaved)
                {
                    try
                    {
                        // Force a database checkpoint to ensure data is written to disk
                        DatabaseManager.ForceCheckpoint();

                        // Force garbage collection to release connections
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error forcing checkpoint: " + ex.Message);
                    }

                    // Set isOrderSaved to true to prevent the unsaved changes prompt
                    isOrderSaved = true;

                    string message = isEditMode ? "Order updated successfully!" : "Order saved successfully!";
                    MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // If in edit mode, just close the form without showing any other forms
                    if (isEditMode)
                    {
                        this.Close();
                    }
                    else
                    {
                        // For new orders, clear the form for the next order
                        ResetFormState();
                        basketGridView.Rows.Clear();
                        ClearCustomerDetails();
                        CalculateTotal();
                        txtDeliveryCharge.Text = "£0.00";

                        // Reset isOrderSaved for the new order
                        isOrderSaved = false;
                    }
                }
                else
                {
                    MessageBox.Show("Some items failed to save. Please check the order details.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }














        // Helper method to create  a new customer
        private int CreateNewCustomer(string customerName, string telephoneNo)
        {
            try
            {
                // Split the customer name into forename and surname
                string[] nameParts = customerName.Split(new[] { ' ' }, 2);
                string forename = nameParts[0];
                string surname = nameParts.Length > 1 ? nameParts[1] : string.Empty;

                // Create the SQL query
                string query = @"
            INSERT INTO customers (forename, surname, telephoneNo, AddressLine1) 
            VALUES (@forename, @surname, @telephoneNo, @address);
            SELECT SCOPE_IDENTITY();";

                // Set up the parameters
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@forename", forename },
            { "@surname", surname },
            { "@telephoneNo", string.IsNullOrEmpty(telephoneNo) ? DBNull.Value : (object)telephoneNo },
            { "@address", string.IsNullOrEmpty(lblAddressDisplay.Text) ? DBNull.Value : (object)lblAddressDisplay.Text }
        };

                // Execute the query and get the new customer ID
                object result = MainClass.ExecuteScalar(query, parameters);

                // Return the new customer ID
                return result == null || result == DBNull.Value ? -1 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating customer: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        // Add this method to clear all customer details
        private void ClearCustomerDetails()
        {
            // Clear text fields
            txtCustomerDetails.Text = string.Empty;
            txtCstTelephone.Text = string.Empty;
            lblAddressDisplay.Text = string.Empty;
            txtPreviousOrders.Text = string.Empty;

            // Reset customer properties
            id = 0;
            customerForename = string.Empty;
            customerSurname = string.Empty;
            customerTelephoneNo = string.Empty;
            customerEmail = string.Empty;
            customerHouseNameNumber = string.Empty;
            customerAddressLine1 = string.Empty;
            customerAddressLine2 = string.Empty;
            customerAddressLine3 = string.Empty;
            customerAddressLine4 = string.Empty;
            customerPostcode = string.Empty;

            // Reset customer exists flag
            customerExists = false;

            // Update button text
            btnCustomerAction.Text = "Add Customer";

            // Clear previous orders panel if it exists
            if (flpPreviousOrders != null)
            {
                flpPreviousOrders.Controls.Clear();
            }
        }





        private void EnsureOrderTypeSelected()
        {
            if (string.IsNullOrEmpty(currentOrderType))
            {
                // Default to Collection if no order type is selected
                currentOrderType = "WAITING";
                btnCol.BackColor = Color.Green;
                btnDel.BackColor = Color.FromArgb(241, 85, 126);
                btnWaiting.BackColor = Color.FromArgb(241, 85, 126);
                btnDineIn.BackColor = Color.FromArgb(241, 85, 126);

                // Hide address fields for collection
                lblCstAddress.Visible = false;
                lblAddressDisplay.Visible = false;
            }
        }








        private decimal CalculateTotal()
        {
            try
            {
                decimal total = 0;
                if (basketGridView.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in basketGridView.Rows)
                    {
                        if (row.Cells["dgvPrice"].Value != null && row.Cells["dgvQty"].Value != null)
                        {
                            decimal price = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
                            int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);
                            total += price * quantity;
                        }
                    }
                }

                // Only add delivery charge if in DELIVERY mode
                if (currentOrderType == "DELIVERY")
                {
                    decimal deliveryCharge = OrderManagement.Model.DeliveryChargeManager.GetDeliveryCharge(this);
                    total += deliveryCharge;
                }

                // Add preset charges
                decimal presetTotal = OrderManagement.View.frmPresetCharges.GetTotalPresetCharges();
                total += presetTotal;

                txtTotalPrice.Text = total.ToString("C2"); // Update the total in the textbox
                return total;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }









        private string GetOrderType()
        {
            // Ensure an order type is selected
            EnsureOrderTypeSelected();
            return currentOrderType;
        }





        private int GetOrCreateCustomer()
        {
            try
            {
                string phone = txtCstTelephone.Text.Trim();
                if (string.IsNullOrEmpty(phone))
                {
                    MessageBox.Show("Please enter a phone number.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }

                // Check if customer exists
                string checkQuery = "SELECT Id FROM customers WHERE telephoneNo = @phone";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@phone", phone }
        };

                DataTable dt = MainClass.getDataFromTable(checkQuery, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["Id"]);
                }

                // Split customer details into forename and surname
                string[] nameParts = txtCustomerDetails.Text.Trim().Split(new[] { ' ' }, 2);
                string forename = nameParts.Length > 0 ? nameParts[0] : "";
                string surname = nameParts.Length > 1 ? nameParts[1] : "";

                // Create new customer if not exists
                string insertQuery = @"
            INSERT INTO customers (forename, surname, telephoneNo, AddressLine1, previousOrders)
            VALUES (@forename, @surname, @phone, @address, 0);
            SELECT SCOPE_IDENTITY();";

                parameters = new Dictionary<string, object>
        {
            { "@forename", forename },
            { "@surname", surname },
            { "@phone", phone },
            { "@address", lblAddressDisplay.Text.Trim() }
        };

                object result = MainClass.ExecuteScalar(insertQuery, parameters);
                return result == null ? -1 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing customer information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }


















        private int AddCustomerToDatabase(string customerName, string telephoneNo)
        {
            string forename = customerName; // Save the name directly into the forename column
            string surname = string.Empty; // No surname, leave it empty

            // Check if the customer already exists by their name
            string checkExistingQuery = "SELECT Id FROM customers WHERE forename = @forename AND surname = @surname";
            Dictionary<string, object> checkExistingParameters = new Dictionary<string, object>
    {
        { "@forename", forename },
        { "@surname", surname }
    };

            // Execute the query to check if the customer already exists
            DataTable existingCustomerTable = MainClass.getDataFromTable(checkExistingQuery, checkExistingParameters);

            if (existingCustomerTable != null && existingCustomerTable.Rows.Count > 0)
            {
                // Customer already exists, return a new ID to avoid using the same ID
                return GenerateNewCustomerId();
            }

            // Proceed to add a new customer record
            string query = "INSERT INTO customers (forename, surname, telephoneNo) VALUES (@forename, @surname, @telephoneNo); SELECT SCOPE_IDENTITY();";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@forename", forename },
        { "@surname", surname },
        { "@telephoneNo", string.IsNullOrEmpty(telephoneNo) ? DBNull.Value : (object)telephoneNo }
    };

            // Execute the insert query and get the newly inserted customer ID
            object result = MainClass.ExecuteScalar(query, parameters);

            // Explicitly cast the result to int
            int newCustomerId = (result == null || result == DBNull.Value) ? -1 : Convert.ToInt32(result);

            return newCustomerId;
        }


        private int GenerateNewCustomerId()
        {
            // Generate a new unique ID for the customer
            string query = "SELECT MAX(Id) + 1 AS NewId FROM customers";
            object result = MainClass.ExecuteScalar(query, new Dictionary<string, object>());

            return (result == null || result == DBNull.Value) ? 1 : Convert.ToInt32(result);
        }










        private void SafeUpdateCustomerDetails(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No customer data available to update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DataRow row = dt.Rows[0];

                this.id = SafeOperations.SafeGetInt(row, "Id");
                this.customerForename = SafeOperations.SafeGetString(row, "forename");
                this.customerSurname = SafeOperations.SafeGetString(row, "surname");
                this.customerTelephoneNo = SafeOperations.SafeGetString(row, "telephoneNo");
                this.customerEmail = SafeOperations.SafeGetString(row, "Email");
                this.customerHouseNameNumber = SafeOperations.SafeGetString(row, "houseNameNumber");
                this.customerAddressLine1 = SafeOperations.SafeGetString(row, "AddressLine1");
                this.customerAddressLine2 = SafeOperations.SafeGetString(row, "AddressLine2");
                this.customerAddressLine3 = SafeOperations.SafeGetString(row, "AddressLine3");
                this.customerAddressLine4 = SafeOperations.SafeGetString(row, "AddressLine4");
                this.customerPostcode = SafeOperations.SafeGetString(row, "Postcode");

                txtCustomerDetails.Text = customerForename + " " + customerSurname;
                txtCstTelephone.Text = customerTelephoneNo;
                txtPreviousOrders.Text = SafeOperations.SafeGetString(row, "previousOrders");

                lblAddressDisplay.Text = $"{customerHouseNameNumber} {customerAddressLine1}, " +
                                        $"{customerAddressLine2}, {customerAddressLine3}, " +
                                        $"{customerAddressLine4}, {customerPostcode}";

                customerExists = true;
                btnCustomerAction.Text = "Update Customer";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }








        private void UpdateOrAddOrderItems(int orderId)
        {
            List<int> updatedOrderItemIds = new List<int>();
            List<string> updatedOrderItemNames = new List<string>();

            foreach (DataGridViewRow row in basketGridView.Rows)
            {
                //get item name from basketGridView
                string itemName = row.Cells["dgvName"].Value.ToString();

                //Get the item price from the foodItems database table
                string itemParamName = "@itemName";
                string foodItemPriceQuery = "SELECT price FROM foodItems WHERE Item = @itemName";
                decimal foodItemPrice = MainClass.getItemPriceFromDatabase(foodItemPriceQuery, itemParamName, itemName);
                int currentBasketQuantity = Convert.ToInt32(row.Cells["dgvQty"].Value);

                decimal itemPrice = foodItemPrice * currentBasketQuantity;

                // Check if the item already exists in OrderItems for the given order
                // within the last 12 hours
                string query = @"
            SELECT TOP 1 OrderItemId, Quantity
            FROM OrderItems
            WHERE OrderId = @OrderId
                AND ItemName = @ItemName
                AND DateAdded >= DATEADD(HOUR, -12, GETDATE())
            ORDER BY DateAdded DESC
        ";
                // pass in the OrderId and itemName as parameters from the basketGridView
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    {"@OrderId", orderId},
                    {"@ItemName", itemName}
                };

                // Execute the query to get the latest entry for the given order and item from OrderItems table
                DataTable dt = MainClass.ExecuteQuery(query, parameters);

                // Check if the query returned any results
                if (dt != null && dt.Rows.Count > 0)
                {

                    int orderItemId = Convert.ToInt32(dt.Rows[0]["OrderItemId"]);

                    MainClass.UpdateOrderItemQuantity(orderItemId, currentBasketQuantity);

                    // Track the updated OrderItemIds
                    updatedOrderItemIds.Add(orderItemId);
                    updatedOrderItemNames.Add(itemName);
                }
                else
                {

                    int newOrderItemId = MainClass.SaveOrderItem(orderId, itemName, itemPrice, currentBasketQuantity);

                    // Track the new OrderItemId
                    updatedOrderItemIds.Add(newOrderItemId);
                    updatedOrderItemNames.Add(itemName);
                }
            }

            // Retrieve existing OrderItemIds associated with the given orderId
            List<int> existingOrderItemIds = MainClass.GetOrderItemIds(orderId);

            // Identify items to be removed
            List<int> itemsToRemove = existingOrderItemIds.Except(updatedOrderItemIds).ToList();

            // Remove items that are not in the current basket from OrderItems
            foreach (int id in itemsToRemove)
            {
                string itemNameToRemove = MainClass.GetItemName(id);
                if (!updatedOrderItemNames.Contains(itemNameToRemove))
                {
                    MainClass.RemoveOrderItem(orderId, id);
                }
            }

            UpdateTotalPriceLabel();

            decimal totalPrice = Convert.ToDecimal(txtTotalPrice.Text.Replace("£", ""));

            MainClass.UpdateOrders(orderId, totalPrice, orderType, DateTime.Now, "NOT PAID");
            loadPreviousOrders();
        }

        private void loadPreviousOrders()
        {
            // This is a wrapper method that calls LoadPreviousOrdersIntoButtons if a customer is selected
            if (id > 0)
            {
                LoadPreviousOrdersIntoButtons(id);
            }
            else
            {
                MessageBox.Show("Please select a customer first.", "No Customer Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int GetCustomerId(string telephoneNo, DateTime currentTime)
        {
            string query;
            Dictionary<string, object> parameters;
            DataTable dt;

            if (string.IsNullOrEmpty(telephoneNo))
            {
                return 1;
            }
            else
            {
                query = "SELECT Id FROM customers WHERE telephoneNo = @telephoneNo AND RegistrationTime = @currentTime";
                parameters = new Dictionary<string, object>
        {
            { "@telephoneNo", telephoneNo },
            { "@currentTime", currentTime }
        };

                dt = MainClass.getDataFromTable(query, parameters);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return -1;
                }
                return Convert.ToInt32(dt.Rows[0]["Id"]);
            }
        }

        private void SaveOrderItems(int orderId)
        {
            foreach (DataGridViewRow row in basketGridView.Rows)
            {
                string itemName = row.Cells["dgvName"].Value.ToString();
                decimal itemPrice = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
                int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);

                // Save the order item directly
                MainClass.SaveOrderItem(orderId, itemName, itemPrice, quantity);
            }
        }





        private void btnLoadPreviousOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a customer is selected
                if (id <= 0)
                {
                    MessageBox.Show("Please select a customer first.", "No Customer Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Open the previous orders form
                using (frmPreviousOrders previousOrdersForm = new frmPreviousOrders(id))
                {
                    if (previousOrdersForm.ShowDialog() == DialogResult.OK)
                    {
                        // Get the selected order items
                        DataTable selectedItems = previousOrdersForm.SelectedOrderItems;

                        if (selectedItems != null && selectedItems.Rows.Count > 0)
                        {
                            // Ask if the user wants to clear the current basket
                            DialogResult result = MessageBox.Show(
                                "Do you want to clear the current basket before adding these items?",
                                "Clear Basket",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                // Clear the basket
                                basketGridView.Rows.Clear();
                                basketItemsByCategory.Clear();
                            }

                            // Add the selected items to the basket
                            foreach (DataRow row in selectedItems.Rows)
                            {
                                string itemName = row["ItemName"].ToString();
                                decimal itemPrice = Convert.ToDecimal(row["ItemPrice"]);
                                int quantity = Convert.ToInt32(row["Quantity"]);

                                // Add the item to the basket
                                for (int i = 0; i < quantity; i++)
                                {
                                    AddItemToBasket(itemName, itemPrice);
                                }
                            }

                            // Update the total price
                            UpdateTotalPriceLabel();

                            MessageBox.Show("Items added to basket successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading previous order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Helper method to add an item to the basket
        private void AddItemToBasket(string itemName, decimal itemPrice)
        {
            // Check if the item already exists in the basket
            int existingItemIndex = GetExistingItemIndex(itemName);

            if (existingItemIndex >= 0)
            {
                // Item exists, just increment quantity
                var row = basketGridView.Rows[existingItemIndex];
                int currentQty = Convert.ToInt32(row.Cells["dgvQty"].Value) + 1;
                row.Cells["dgvQty"].Value = currentQty;

                // Do NOT update the price - leave dgvPrice unchanged
            }
            else
            {
                // Add new item to basket
                AddNewItemToBasket(itemName, itemPrice);
            }

            // Update the total price label
            UpdateTotalPriceLabel();

            // Set isOrderSaved to false as the basket has changed
            isOrderSaved = false;
        }









        private void btnPayment_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(currentOrderType))
                {
                    MessageBox.Show("Please select either collection or delivery from the top button panel");
                    return;
                }

                if (basketGridView.Rows.Count == 0)
                {
                    MessageBox.Show("The basket is empty.");
                    return;
                }

                // First, save the order if it hasn't been saved yet
                if (!isOrderSaved || orderIdValue <= 0)
                {
                    // Save the order using the same logic as in btnSaveOrder_Click
                    int customerId;

                    // Check if we already have a customer selected
                    if (customerExists && id > 0)
                    {
                        customerId = id;
                    }
                    else
                    {
                        // Parse customer name
                        string customerName = txtCustomerDetails.Text.Trim();
                        string forename = customerName;
                        string surname = "";

                        if (customerName.Contains(" "))
                        {
                            string[] nameParts = customerName.Split(new[] { ' ' }, 2);
                            forename = nameParts[0];
                            surname = nameParts[1];
                        }

                        // Check if customer exists by phone number
                        if (!string.IsNullOrEmpty(txtCstTelephone.Text))
                        {
                            string query = "SELECT Id FROM customers WHERE telephoneNo = @telephoneNo";
                            Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@telephoneNo", txtCstTelephone.Text.Trim() }
                    };

                            DataTable dt = MainClass.getDataFromTable(query, parameters);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                customerId = Convert.ToInt32(dt.Rows[0]["Id"]);
                            }
                            else
                            {
                                // Create new customer
                                customerId = MainClass.CreateCustomer(
                                    forename,
                                    surname,
                                    txtCstTelephone.Text.Trim(),
                                    lblAddressDisplay.Text.Trim()
                                );
                            }
                        }
                        else if (!string.IsNullOrEmpty(customerName))
                        {
                            // Create customer with just name
                            customerId = MainClass.CreateCustomer(
                                forename,
                                surname,
                                null,
                                lblAddressDisplay.Text.Trim()
                            );
                        }
                        else
                        {
                            // Create a guest customer
                            customerId = MainClass.CreateCustomer(
                                "Guest",
                                "Customer",
                                null,
                                null
                            );
                        }
                    }

                    if (customerId <= 0)
                    {
                        MessageBox.Show("Failed to create or find customer. Please check customer details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Get order details
                    DateTime currentTime = DateTime.Now;

                    // CHANGE HERE: Always use CalculateTotal() to ensure accurate pricing
                    decimal totalPrice = CalculateTotal();

                    string orderType = currentOrderType; // Use the current order type
                    if (string.IsNullOrEmpty(orderType))
                    {
                        orderType = "COLLECTION"; // Default to collection if not set
                    }

                    string paymentType = "PENDING"; // Default payment status

                    // Save the order

                    // Parse the delivery charge from the textbox
                    decimal deliveryCharge = OrderManagement.Model.DeliveryChargeManager.GetDeliveryCharge(this);
                    decimal presetCharges = OrderManagement.View.frmPresetCharges.GetTotalPresetCharges();
                    orderIdValue = MainClass.SaveOrder(customerId, orderType, totalPrice, currentTime, paymentType, lblAddressDisplay.Text, deliveryCharge, presetCharges);
                    if (orderIdValue == -1)
                    {
                        MessageBox.Show("Failed to save order. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Save each item in the basket
                    bool allItemsSaved = true;
                    foreach (DataGridViewRow row in basketGridView.Rows)
                    {
                        if (row.Cells["dgvName"].Value == null) continue;

                        string itemName = row.Cells["dgvName"].Value.ToString();
                        decimal itemPrice = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
                        int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);

                        int result = MainClass.SaveOrderItem(orderIdValue, itemName, itemPrice, quantity);
                        if (result == -1)
                        {
                            allItemsSaved = false;
                            MessageBox.Show($"Failed to save item: {itemName}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    if (allItemsSaved)
                    {
                        try
                        {
                            // Force a database checkpoint to ensure data is written to disk
                            DatabaseManager.ForceCheckpoint();

                            // Force garbage collection to release connections
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error forcing checkpoint: " + ex.Message);
                        }

                        // Mark the order as saved
                        isOrderSaved = true;
                    }
                }

                // Now open the payment form
                using (frmPayment paymentForm = new frmPayment())
                {
                    // Create a new DataGridView with the same columns
                    DataGridView basketCopy = new DataGridView();
                    foreach (DataGridViewColumn col in basketGridView.Columns)
                    {
                        basketCopy.Columns.Add(col.Clone() as DataGridViewColumn);
                    }

                    // Copy the data
                    foreach (DataGridViewRow row in basketGridView.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            DataGridViewRow newRow = (DataGridViewRow)row.Clone();
                            for (int i = 0; i < row.Cells.Count; i++)
                            {
                                newRow.Cells[i].Value = row.Cells[i].Value;
                            }
                            basketCopy.Rows.Add(newRow);
                        }
                    }

                    paymentForm.BasketGrid = basketCopy;
                    paymentForm.lblPaymentAmount.Text = txtTotalPrice.Text;
                    paymentForm.CustomerName = txtCustomerDetails.Text;
                    paymentForm.TelephoneNo = txtCstTelephone.Text;
                    paymentForm.OrderType = currentOrderType;

                    // CHANGE HERE: Use CalculateTotal() for accurate pricing
                    paymentForm.TotalPrice = CalculateTotal();

                    paymentForm.Address = lblAddressDisplay.Text;

                    // Set the OrderId property
                    paymentForm.OrderId = orderIdValue;

                    paymentForm.Confirm += paymentOption =>
                    {
                        lblPaymentOption.Text = paymentOption;

                        // Show success message
                        MessageBox.Show("Payment processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // If in edit mode, close this form after payment is confirmed
                        if (isEditMode)
                        {
                            // Close any related forms
                            CloseRelatedForms();

                            // Close this form
                            this.Close();
                        }
                        else
                        {
                            // For new orders, just reset the form
                            ResetFormState();
                            txtDeliveryCharge.Text = "£0.00";
                        }
                    };


                    paymentForm.lblCustomerToPay.Text = customerForename + " " + customerSurname;
                    paymentForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing payment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }









        private void btnKot_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentOrderType))
            {
                MessageBox.Show("Please select either collection or delivery from the top button panel");
                return;
            }

            if (basketGridView.Rows.Count == 0)
            {
                MessageBox.Show("The basket is empty.");
                return;
            }
            try
            {
                // First, save the order (similar to btnSaveOrder_Click)
                int customerId;

                // Check if we already have a customer selected
                if (customerExists && id > 0)
                {
                    customerId = id;
                }
                else
                {
                    // Parse customer name
                    string customerName = txtCustomerDetails.Text.Trim();
                    string forename = customerName;
                    string surname = "";

                    if (customerName.Contains(" "))
                    {
                        string[] nameParts = customerName.Split(new[] { ' ' }, 2);
                        forename = nameParts[0];
                        surname = nameParts[1];
                    }

                    // Check if customer exists by phone number
                    if (!string.IsNullOrEmpty(txtCstTelephone.Text))
                    {
                        string query = "SELECT Id FROM customers WHERE telephoneNo = @telephoneNo";
                        Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@telephoneNo", txtCstTelephone.Text.Trim() }
                };

                        DataTable dt = MainClass.getDataFromTable(query, parameters);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            customerId = Convert.ToInt32(dt.Rows[0]["Id"]);
                        }
                        else
                        {
                            // Create new customer
                            customerId = MainClass.CreateCustomer(
                                forename,
                                surname,
                                txtCstTelephone.Text.Trim(),
                                lblAddressDisplay.Text.Trim()
                            );
                        }
                    }
                    else if (!string.IsNullOrEmpty(customerName))
                    {
                        // Create customer with just name
                        customerId = MainClass.CreateCustomer(
                            forename,
                            surname,
                            null,
                            lblAddressDisplay.Text.Trim()
                        );
                    }
                    else
                    {
                        // Create a guest customer
                        customerId = MainClass.CreateCustomer(
                            "Guest",
                            "Customer",
                            null,
                            null
                        );
                    }
                }

                if (customerId <= 0)
                {
                    MessageBox.Show("Failed to create or find customer. Please check customer details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get order details
                DateTime currentTime = DateTime.Now;

                // CHANGE HERE: Always use CalculateTotal() to ensure accurate pricing
                decimal totalPrice = CalculateTotal();

                string orderType = currentOrderType; // Use the current order type
                if (string.IsNullOrEmpty(orderType))
                {
                    orderType = "COLLECTION"; // Default to collection if not set
                }

                string paymentType = lblPaymentOption.Text;
                if (string.IsNullOrEmpty(paymentType))
                {
                    paymentType = "PENDING"; // Default payment status
                }

                int orderId;

                if (isEditMode && editOrderId > 0)
                {
                    // Parse the delivery charge
                    decimal deliveryCharge = OrderManagement.Model.DeliveryChargeManager.GetDeliveryCharge(this);

                    // Update existing order
                    string updateOrderQuery = @"
        UPDATE Orders 
        SET 
            CustomerId = @CustomerId, 
            OrderType = @OrderType, 
            TotalPrice = @TotalPrice, 
            PaymentType = @PaymentType, 
            Address = @Address,
            DeliveryCharge = @DeliveryCharge
        WHERE 
            OrderId = @OrderId";

                    Dictionary<string, object> updateOrderParams = new Dictionary<string, object>
    {
        { "@CustomerId", customerId },
        { "@OrderType", orderType },
        { "@TotalPrice", totalPrice },
        { "@PaymentType", paymentType },
        { "@Address", lblAddressDisplay.Text },
        { "@DeliveryCharge", deliveryCharge },
        { "@OrderId", editOrderId }
    };

                    int result = DatabaseManager.ExecuteNonQuery(updateOrderQuery, updateOrderParams);

                    if (result <= 0)
                    {
                        MessageBox.Show("Failed to update order. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    orderId = editOrderId;

                    // Delete existing order items
                    string deleteItemsQuery = "DELETE FROM OrderItems WHERE OrderId = @OrderId";
                    Dictionary<string, object> deleteItemsParams = new Dictionary<string, object>
            {
                { "@OrderId", orderId }
            };

                    DatabaseManager.ExecuteNonQuery(deleteItemsQuery, deleteItemsParams);
                }
                else
                {
                    // Save new order
                    decimal deliveryCharge = OrderManagement.Model.DeliveryChargeManager.GetDeliveryCharge(this);
                    decimal presetCharges = OrderManagement.View.frmPresetCharges.GetTotalPresetCharges();
                    orderId = MainClass.SaveOrder(customerId, orderType, totalPrice, currentTime, paymentType, lblAddressDisplay.Text, deliveryCharge, presetCharges);
                    if (orderId == -1)
                    {
                        MessageBox.Show("Failed to save order. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Save each item in the basket
                bool allItemsSaved = true;
                foreach (DataGridViewRow row in basketGridView.Rows)
                {
                    if (row.Cells["dgvName"].Value == null) continue;

                    string itemName = row.Cells["dgvName"].Value.ToString();
                    decimal itemPrice = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
                    int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);

                    int result = MainClass.SaveOrderItem(orderId, itemName, itemPrice, quantity);
                    if (result == -1)
                    {
                        allItemsSaved = false;
                        MessageBox.Show($"Failed to save item: {itemName}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (allItemsSaved)
                {
                    // Now print the order
                    PrintDocument pd = new PrintDocument();
                    pd.DefaultPageSettings.PaperSize = new PaperSize("Thermal44", 44 * 10, 1000);

                    pd.PrintPage += (s, ev) =>
                    {
                        const int margin = 5;
                        int pageWidth = ev.PageBounds.Width - margin * 2;

                        Font font = new Font("Arial", 10);
                        Font titleFont = new Font("Arial", 12, FontStyle.Bold);
                        Font resFont = new Font("Arial", 8);
                        Font resTelFont = new Font("Arial", 8, FontStyle.Bold);
                        Font cusFont = new Font("Arial", 10);
                        Font cusBoldFont = new Font("Arial", 10, FontStyle.Bold);

                        float currentHeight = margin;

                        // Title
                        string title = "TAKE MY ORDER";
                        int titleWidth = (int)ev.Graphics.MeasureString(title, titleFont).Width;
                        ev.Graphics.DrawString(title, titleFont, Brushes.Black, (pageWidth - titleWidth) / 2, currentHeight);
                        currentHeight += 20;

                        // Restaurant Details
                        string resAddress = "123 Restaurant Street, Newcastle Upon Tyne, NE99 9AA";
                        SizeF resAddressSize = ev.Graphics.MeasureString(resAddress, resFont);
                        float resAddressX = (pageWidth - resAddressSize.Width) / 2;
                        ev.Graphics.DrawString(resAddress, resFont, Brushes.Black, resAddressX, currentHeight);
                        currentHeight += resAddressSize.Height;

                        string resTelephone = "01910000000";
                        SizeF resTelephoneSize = ev.Graphics.MeasureString(resTelephone, resFont);
                        float resTelephoneX = (pageWidth - resTelephoneSize.Width) / 2;
                        ev.Graphics.DrawString(resTelephone, resTelFont, Brushes.Black, resTelephoneX, currentHeight);
                        currentHeight += resTelephoneSize.Height;

                        // Draw the first line to split the page
                        ev.Graphics.DrawLine(Pens.Black, 0, currentHeight, pageWidth, currentHeight);
                        currentHeight += 10;

                        // Order Type
                        string orderTypeDisplay = currentOrderType?.ToUpperInvariant() ?? "ORDER";
                        Font orderTypeFont = new Font("Arial", 12, FontStyle.Bold);
                        SizeF orderTypeSize = ev.Graphics.MeasureString(orderTypeDisplay, orderTypeFont);
                        float orderTypeX = (pageWidth - orderTypeSize.Width) / 2;
                        ev.Graphics.DrawString(orderTypeDisplay, orderTypeFont, Brushes.Black, orderTypeX, currentHeight);
                        currentHeight += orderTypeSize.Height + 5; // Add some spacing

                        // Customer Details - only print if available
                        bool hasCustomerName = !string.IsNullOrEmpty(customerForename) || !string.IsNullOrEmpty(customerSurname);
                        if (hasCustomerName)
                        {
                            string cusName = $"{customerForename} {customerSurname}".Trim();
                            ev.Graphics.DrawString(cusName, cusBoldFont, Brushes.Black, margin, currentHeight);
                            currentHeight += ev.Graphics.MeasureString(cusName, cusBoldFont).Height;
                        }

                        // Only print address for delivery orders and if address is available
                        if (currentOrderType.Equals("delivery", StringComparison.OrdinalIgnoreCase))
                        {
                            StringBuilder addressBuilder = new StringBuilder();

                            if (!string.IsNullOrEmpty(customerHouseNameNumber))
                                addressBuilder.AppendLine(customerHouseNameNumber);

                            if (!string.IsNullOrEmpty(customerAddressLine1))
                                addressBuilder.AppendLine(customerAddressLine1);

                            if (!string.IsNullOrEmpty(customerAddressLine2))
                                addressBuilder.AppendLine(customerAddressLine2);

                            if (!string.IsNullOrEmpty(customerAddressLine3))
                                addressBuilder.AppendLine(customerAddressLine3);

                            if (!string.IsNullOrEmpty(customerAddressLine4))
                                addressBuilder.AppendLine(customerAddressLine4);

                            if (!string.IsNullOrEmpty(customerPostcode))
                                addressBuilder.Append(customerPostcode);

                            string cusAddress = addressBuilder.ToString().Trim();

                            if (!string.IsNullOrEmpty(cusAddress))
                            {
                                ev.Graphics.DrawString(cusAddress, cusFont, Brushes.Black, margin, currentHeight);
                                currentHeight += ev.Graphics.MeasureString(cusAddress, cusFont).Height;
                            }
                        }

                        // Customer Telephone Number - only print if available
                        if (!string.IsNullOrEmpty(customerTelephoneNo))
                        {
                            ev.Graphics.DrawString(customerTelephoneNo, cusBoldFont, Brushes.Black, margin, currentHeight);
                            currentHeight += ev.Graphics.MeasureString(customerTelephoneNo, cusBoldFont).Height;
                        }

                        // Draw the second line to split the page
                        ev.Graphics.DrawLine(Pens.Black, 0, currentHeight, pageWidth, currentHeight);
                        currentHeight += 10;

                        // Items
                        foreach (DataGridViewRow row in basketGridView.Rows)
                        {
                            if (row.Cells["dgvName"].Value == null) continue;

                            string itemName = row.Cells["dgvName"].Value.ToString().PadRight(20);
                            decimal originalPrice = Convert.ToDecimal(row.Cells["dgvOriginalPrice"].Value);
                            decimal extraCharge = row.Cells["dgvExtraChargeValue"].Value != null ?
                                Convert.ToDecimal(row.Cells["dgvExtraChargeValue"].Value) : 0m;
                            int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);
                            decimal totalItemPrice = Convert.ToDecimal(row.Cells["dgvPrice"].Value);

                            ev.Graphics.DrawString(quantity.ToString() + " x " + itemName, font, Brushes.Black, margin, currentHeight);
                            float itemPriceWidth = ev.Graphics.MeasureString("£" + totalItemPrice.ToString("F2"), font).Width;
                            ev.Graphics.DrawString("£" + totalItemPrice.ToString("F2"), font, Brushes.Black, pageWidth - margin - itemPriceWidth, currentHeight);
                            currentHeight += 20;

                            // Show extra charge if any
                            if (extraCharge > 0)
                            {
                                ev.Graphics.DrawString("   + Extra Charge", font, Brushes.Black, margin + 10, currentHeight);
                                float extraChargeWidth = ev.Graphics.MeasureString("£" + extraCharge.ToString("F2"), font).Width;
                                ev.Graphics.DrawString("£" + extraCharge.ToString("F2"), font, Brushes.Black, pageWidth - margin - extraChargeWidth, currentHeight);
                                currentHeight += 20;
                            }
                        }


                        // Total
                        string totalLabel = "Total".PadRight(10);

                        // CHANGE HERE: Calculate the total price correctly
                        decimal calculatedTotal = CalculateTotal();
                        string totalPriceStr = calculatedTotal.ToString("F2");

                        ev.Graphics.DrawString(totalLabel, font, Brushes.Black, margin, currentHeight);
                        float totalPriceWidth = ev.Graphics.MeasureString("£" + totalPriceStr, font).Width;
                        ev.Graphics.DrawString("£" + totalPriceStr, font, Brushes.Black, pageWidth - margin - totalPriceWidth, currentHeight);
                        currentHeight += 20;

                        // Draw the third line to split the page
                        ev.Graphics.DrawLine(Pens.Black, 0, currentHeight, pageWidth, currentHeight);
                        currentHeight += 10;

                        // Payment
                        if (!string.IsNullOrEmpty(paymentType))
                        {
                            string paymentLabel = "PENDING:";
                            ev.Graphics.DrawString(paymentLabel, cusBoldFont, Brushes.Black, margin, currentHeight);
                            float paymentValueWidth = ev.Graphics.MeasureString(paymentType, cusBoldFont).Width;
                            ev.Graphics.DrawString(paymentType, cusBoldFont, Brushes.Black, pageWidth - margin - paymentValueWidth, currentHeight);
                            currentHeight += 20;
                        }

                        // Order ID
                        ev.Graphics.DrawString("Order #: " + orderId, cusBoldFont, Brushes.Black, margin, currentHeight);
                        currentHeight += 20;

                        // Date & Time
                        string timeLine = "Time: " + currentTime.ToString("hh:mm tt");
                        string dateLine = "Date: " + currentTime.ToString("dd/MM/yyyy");
                        ev.Graphics.DrawString(timeLine, font, Brushes.Black, margin, currentHeight);
                        currentHeight += 20;
                        ev.Graphics.DrawString(dateLine, font, Brushes.Black, margin, currentHeight);
                        currentHeight += 20;

                        ev.HasMorePages = false;
                    };

                    pd.Print();

                    // Reset form state after printing
                    ResetFormState();
                    basketItemsByCategory.Clear();
                    isOrderSaved = false;
                    orderIdValue = 0;
                    isEditMode = false; // Exit edit mode after printing
                    txtDeliveryCharge.Text = "£0.00";

                    // Close any open OrderViewForm and frmOrderHistory instances
                    CloseRelatedForms();

                    // If in edit mode, close this form too
                    if (isEditMode)
                    {
                        this.Close();
                    }
                    else
                    {
                        // For new orders, reset the form for the next order
                        ResetFormState();
                        basketItemsByCategory.Clear();
                        isOrderSaved = false;
                        txtDeliveryCharge.Text = "£0.00";
                    }

                    // Show a success message
                    MessageBox.Show("Order saved and printed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Some items failed to save. Cannot print order.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







        private void UpdateTotalPriceLabel()
        {
            try
            {
                decimal price = 0;

                if (basketGridView != null)
                {
                    foreach (DataGridViewRow row in basketGridView.Rows)
                    {
                        if (row.Cells["dgvPrice"] != null && row.Cells["dgvPrice"].Value != null &&
                            row.Cells["dgvQty"] != null && row.Cells["dgvQty"].Value != null)
                        {
                            decimal itemPrice = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
                            int quantity = Convert.ToInt32(row.Cells["dgvQty"].Value);
                            price += itemPrice * quantity;
                        }
                    }
                }

                // Apply discount if applicable
                price = price * (1 - discountRate);

                // Add delivery charge if visible and valid
                decimal deliveryCharge = OrderManagement.Model.DeliveryChargeManager.GetDeliveryCharge(this);
                price += deliveryCharge;

                // Add preset charges
                decimal presetTotal = OrderManagement.View.frmPresetCharges.GetTotalPresetCharges();
                price += presetTotal;

                txtTotalPrice.Text = "£" + price.ToString("F2");
                this.totalPrice = txtTotalPrice.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating total price: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTotalPrice.Text = "£0.00";
                this.totalPrice = "£0.00";
            }
        }






        private void cbCustomDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCustomDiscount.SelectedItem != null)
            {
                Dictionary<string, decimal> discountRates = new Dictionary<string, decimal>
        {
            { "No Discount", 0m },
            { "5% Discount", 0.05m },
            { "10% Discount", 0.1m },
            { "20% Discount", 0.2m },
            { "30% Discount", 0.3m },
            { "40% Discount", 0.4m },
            { "50% Discount", 0.5m },
            { "60% Discount", 0.6m },
            { "70% Discount", 0.7m },
            { "80% Discount", 0.8m },
            { "90% Discount", 0.9m },
            { "100% Discount", 1m },
        };

                string selectedDiscount = cbCustomDiscount.SelectedItem.ToString();

                decimal discountRate;
                if (discountRates.TryGetValue(selectedDiscount, out discountRate))
                {
                    this.discountRate = discountRate;
                }
                else
                {
                    this.discountRate = 0m; // Default to no discount
                }

                UpdateTotalPriceLabel();
            }

        }

        private void resetTotalPrice()
        {
            if (basketGridView.Rows.Count > 0)
            {
                decimal price = 0;
                foreach (DataGridViewRow row in basketGridView.Rows)
                {
                    decimal itemPrice = Convert.ToDecimal(row.Cells["dgvPrice"].Value);
                    price += itemPrice;
                }
                price = price * (1 - discountRate); // Apply discount
                txtTotalPrice.Text = "£" + price.ToString("F2");
            }
            else
            {
                txtTotalPrice.Text = "£0.00";
            }
        }

        private void btnClear_click(object sender, EventArgs e)
        {
            var rowsToRemove = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in basketGridView.Rows)
            {
                if (Convert.ToInt32(row.Cells["dgvQty"].Value) == 0)
                    rowsToRemove.Add(row);
            }
            foreach (var row in rowsToRemove)
                basketGridView.Rows.Remove(row);

            basketItemsByCategory.Clear();
            // Update the total price label
            UpdateTotalPriceLabel();

            flpPreviousOrders.Controls.Clear();

            // Set isOrderSaved to false as the basket has changed
            isOrderSaved = false;
        }

        private void ResetAllOrderTypeButtons()
        {
            // Reset all order type buttons to default color
            btnCol.BackColor = Color.FromArgb(241, 85, 126);
            btnDel.BackColor = Color.FromArgb(241, 85, 126);
            btnWaiting.BackColor = Color.FromArgb(241, 85, 126);
            btnDineIn.BackColor = Color.FromArgb(241, 85, 126);
        }







        private void btnCallMe_Click(object sender, EventArgs e)
        {
            // Use the txtCustomerDetails textbox of frmCallerView to get the telephone number
            using (frmCallerView callerViewForm = new frmCallerView())
            {
                // Show the form without the need for InputBox
                callerViewForm.ShowDialog();

                // Retrieve the telephone number from the txtCustomerDetails textbox in frmCallerView
                string telephoneNo = callerViewForm.EnteredTelephoneNo;

                // If a telephone number is entered, update the txtCstTelephone field
                if (!string.IsNullOrEmpty(telephoneNo))
                {
                    txtCstTelephone.Text = telephoneNo;

                    // Query to get the customer details
                    string query = "SELECT * FROM customers WHERE telephoneNo = @telephoneNo";

                    Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@telephoneNo", telephoneNo }
            };

                    DataTable dt = MainClass.getDataFromTable(query, parameters);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        // if customer is found Update customer details in frmPOS directly
                        UpdateCustomerDetails(dt);

                        // get the customer previous orders automatically upon finding thc customer
                        LoadPreviousOrdersIntoButtons(Convert.ToInt32(dt.Rows[0]["Id"]));

                        // Optionally close frmCallerView if needed
                        callerViewForm.Close();
                    }
                    else
                    {
                        // If customer is not found, copy the entered telephone number to txtCstTelephone
                        txtCstTelephone.Text = telephoneNo;
                        // Optionally, you can handle this case differently or show a message
                        MessageBox.Show("Customer not found. Entered telephone number used as customer telephone.");
                    }
                }
            }
        }

        private void txtTotalPrice_TextChanged(object sender, EventArgs e)
        {
            string input = txtTotalPrice.Text.Replace("£", "");
            if (decimal.TryParse(input, out decimal newPrice))
            {
                this.totalPrice = "£" + newPrice.ToString("F2");
            }

        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            using (frmOrderHistory orderHistoryForm = new frmOrderHistory())
            {
                orderHistoryForm.ShowDialog();
            }
        }

        private void LoadPreviousOrdersIntoButtons(int customerId)
        {
            // Clear the FlowLayoutPanel
            flpPreviousOrders.Controls.Clear();

            // Create a HashSet to store the names of the items that have already been added
            HashSet<string> addedItems = new HashSet<string>();

            // Get the previous orders
            DataTable previousOrders = MainClass.GetPreviousOrders(customerId);

            if (previousOrders != null)
            {
                foreach (DataRow row in previousOrders.Rows)
                {
                    string itemName = row["ItemName"].ToString();
                    decimal itemPrice = Convert.ToDecimal(row["ItemPrice"]);
                    int quantity = Convert.ToInt32(row["Quantity"]);

                    // Only add a new button if the item has not already been added
                    if (!addedItems.Contains(itemName))
                    {
                        // Add the item to the HashSet to prevent duplicates
                        addedItems.Add(itemName);

                        // Create a new button
                        Button btn = new Button
                        {
                            Text = $"{itemName}\n{itemPrice:C2}",
                            Tag = new { ItemName = itemName, ItemPrice = itemPrice, Quantity = quantity },
                            BackColor = Color.FromArgb(241, 85, 126),
                            ForeColor = Color.FromArgb(50, 55, 89),
                            Font = new Font("Segoe UI Black", 10F, FontStyle.Bold),
                            Size = new Size(112, 88),
                            UseVisualStyleBackColor = false
                        };

                        // Attach a click event to add item to basket
                        btn.Click += (s, e) =>
                        {
                            dynamic tag = ((Button)s).Tag;
                            AddItemToBasket(tag.ItemName, tag.ItemPrice);
                        };

                        // Add the button to the FlowLayoutPanel
                        flpPreviousOrders.Controls.Add(btn);
                    }
                }
            }
        }


        private void AddItemToBasket(string itemName, decimal itemPrice, int quantity)
        {
            // Check if the item already exists in basketGridView
            int existingItemIndex = GetExistingItemIndex(itemName);

            if (existingItemIndex >= 0)
            {
                // Update the quantity and price in basketGridView
                UpdateExistingItemInBasket(existingItemIndex, itemPrice);
            }
            else
            {
                // Add a new row to basketGridView
                AddNewItemToBasket(itemName, itemPrice);
            }

            // Update the total price label
            UpdateTotalPriceLabel();
            // Set isOrderSaved to false as the basket has changed
            isOrderSaved = true;
        }

        private void lstSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSearchResult.SelectedIndex != -1)
            {
                string selectedCustomerDetails = lstSearchResult.SelectedItem.ToString();
                string[] details = selectedCustomerDetails.Split(new[] { " - " }, StringSplitOptions.None);

                if (details.Length >= 2)
                {
                    lstSearchResult.Visible = false;
                    UpdateCustomerDetails(customerSearchDataTable);
                    txtCustomerDetails.Text = details[0].Trim();
                }
            }
        }


        private void txtCustomerDetails_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                string[] parts = textBox.Text.Split(new[] { ' ' }, 2);
                customerForename = parts[0];
                customerSurname = parts.Length > 1 ? parts[1] : string.Empty;
            }
            else
            {
                customerForename = string.Empty;
                customerSurname = string.Empty;
            }
        }
        private void txtCstTelephone_TextChanged(object sender, EventArgs e)
        {
            string inputText = txtCstTelephone.Text.Trim();
            customerTelephoneNo = inputText;

            // Optionally remove this condition if address display shouldn't block search
            if (string.IsNullOrWhiteSpace(inputText))
            {
                lstSearchResult.Items.Clear();
                lstSearchResult.Visible = false;
                return;
            }

            string connectionString = OrderManagement.DatabaseManager.ConnectionString;
            string queryString = "SELECT TOP 10 * FROM customers WHERE telephoneNo LIKE @telephoneNo + '%'";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@telephoneNo", SqlDbType.VarChar).Value = inputText;

                    customerSearchDataTable.Clear();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(customerSearchDataTable);
                    }

                    lstSearchResult.Items.Clear();

                    foreach (DataRow row in customerSearchDataTable.Rows)
                    {
                        string forename = row["forename"]?.ToString();
                        string surname = row["surname"]?.ToString();

                        var addressParts = new[]
                        {
                    row["houseNameNumber"],
                    row["AddressLine1"],
                    row["AddressLine2"],
                    row["AddressLine3"],
                    row["AddressLine4"],
                    row["Postcode"]
                }
                        .Where(p => !string.IsNullOrWhiteSpace(p?.ToString()))
                        .Select(p => p.ToString());

                        string fullAddress = string.Join(", ", addressParts);
                        string customerDetails = $"{forename} {surname} - {fullAddress}";

                        lstSearchResult.Items.Add(customerDetails);
                    }

                    // Dynamically adjust ListBox width based on content
                    if (lstSearchResult.Items.Count > 0)
                    {
                        using (Graphics g = lstSearchResult.CreateGraphics())
                        {
                            int maxWidth = 0;

                            foreach (var item in lstSearchResult.Items)
                            {
                                int itemWidth = (int)g.MeasureString(item.ToString(), lstSearchResult.Font).Width;
                                if (itemWidth > maxWidth)
                                    maxWidth = itemWidth;
                            }

                            // Add some padding and constrain to form width
                            lstSearchResult.Width = Math.Min(maxWidth + 30, this.Width - lstSearchResult.Left - 20);
                        }

                        lstSearchResult.Visible = true;
                    }
                    else
                    {
                        lstSearchResult.Visible = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error while searching customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lstSearchResult.Items.Clear();
                lstSearchResult.Visible = false;
            }
        }







        private void categoryList_SelectedIndexChanged(object sender, EventArgs e, string selectedCategory)
        {
            // Clear the FlowLayoutPanel
            flpItemView.Controls.Clear();

            // Ensure that a category is selected
            if (string.IsNullOrEmpty(selectedCategory))
            {
                // Handle the case where no category is selected
                return;
            }

            string query = "SELECT Id, Item, price, icon FROM foodItems WHERE category = @category";
            Dictionary<string, object> parameters = new Dictionary<string, object>
    {
        { "@category", selectedCategory }
    };

            DataTable dt = DatabaseManager.ExecuteQuery(query, parameters);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string itemName = row["Item"].ToString();
                    decimal itemPrice = Convert.ToDecimal(row["price"]);
                    byte[] imageData = null;

                    // Get image data from database
                    if (row["icon"] != DBNull.Value)
                    {
                        imageData = (byte[])row["icon"];
                        Console.WriteLine($"Found image data for {itemName}: {imageData.Length} bytes");
                    }
                    else
                    {
                        Console.WriteLine($"No image data found for {itemName}");
                    }

                    // Create button with image data
                    Button btn = CreateFoodItemButton(itemName, itemPrice, imageData, (s, ev) =>
                    {
                        dynamic tag = ((Button)s).Tag;
                        AddItemToBasket(tag.ItemName, tag.ItemPrice);
                    });

                    flpItemView.Controls.Add(btn);
                }
            }
        }





        private void LoadItemsByCategory(string category)
        {
            try
            {
                flpItemView.Controls.Clear();
                string query = "SELECT Id, Item, price, icon FROM foodItems WHERE category = @category ORDER BY Item";
                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@category", category }
        };

                DataTable items = DatabaseManager.ExecuteQuery(query, parameters);
                Console.WriteLine($"Loading {category} items: found {(items != null ? items.Rows.Count : 0)} items");

                if (items != null && items.Rows.Count > 0)
                {
                    foreach (DataRow row in items.Rows)
                    {
                        string itemName = row["Item"].ToString();
                        decimal itemPrice = Convert.ToDecimal(row["price"]);
                        byte[] imageData = null;

                        // Get image data from database
                        if (row["icon"] != DBNull.Value)
                        {
                            imageData = (byte[])row["icon"];
                            Console.WriteLine($"Found image data for {itemName}: {imageData.Length} bytes");
                        }
                        else
                        {
                            Console.WriteLine($"No image data found for {itemName}");
                        }

                        // Create button with image data
                        Button btn = CreateFoodItemButton(itemName, itemPrice, imageData, (s, e) =>
                        {
                            dynamic tag = ((Button)s).Tag;
                            AddItemToBasket(tag.ItemName, tag.ItemPrice);
                        });

                        flpItemView.Controls.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadCategories()
        {
            try
            {
                // Clear the FlowLayoutPanel
                flpCategoryBtns.Controls.Clear();

                // Get categories from the database
                string query = "SELECT DISTINCT category FROM foodItems ORDER BY category";
                DataTable categories = DatabaseManager.ExecuteQuery(query);

                if (categories != null && categories.Rows.Count > 0)
                {
                    foreach (DataRow row in categories.Rows)
                    {
                        string category = row["category"].ToString();

                        // Create a button for each category
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

                        // Add click event to load items for this category
                        btn.Click += (s, e) => LoadItemsByCategory(category);

                        // Add the button to the FlowLayoutPanel
                        flpCategoryBtns.Controls.Add(btn);
                    }

                    // If there are categories, load items for the first one
                    if (flpCategoryBtns.Controls.Count > 0)
                    {
                        string firstCategory = flpCategoryBtns.Controls[0].Tag.ToString();
                        LoadItemsByCategory(firstCategory);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCurrentOrders_Click(object sender, EventArgs e)
        {
            frmOrderProgress orderProgressForm = new frmOrderProgress();
            orderProgressForm.Show();
        }

        private void btnNotificationConfig_Click(object sender, EventArgs e)
        {
            try
            {
                string configPath = NotificationConfigManager.GetConfigPath();
                if (string.IsNullOrEmpty(configPath))
                {
                    MessageBox.Show("Could not determine configuration path.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var configManager = new NotificationConfigManager(configPath);
                var config = configManager.LoadConfig();

                // Create a simple form without any resource dependencies
                Form form = new Form();
                form.Text = "Notification Configuration";
                form.Size = new Size(400, 400);
                form.StartPosition = FormStartPosition.CenterParent;

                // Create a FlowLayoutPanel
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.Dock = DockStyle.Fill;
                flowLayoutPanel.AutoScroll = true;
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.Padding = new Padding(10);

                // Create labels and TextBox controls for each setting
                Label collectionHighAlertMinutesLabel = new Label
                {
                    Text = "Collection High Alert Minutes:",
                    Width = 190
                };
                TextBox collectionHighAlertMinutesTextBox = new TextBox
                {
                    Text = config.CollectionOrder.HighAlertMinutes.ToString(),
                    Width = 250
                };

                Label deliveryHighAlertMinutesLabel = new Label
                {
                    Text = "Delivery High Alert Minutes:",
                    Width = 190
                };
                TextBox deliveryHighAlertMinutesTextBox = new TextBox
                {
                    Text = config.DeliveryOrder.HighAlertMinutes.ToString(),
                    Width = 250
                };

                Label collectionMediumAlertMinutesLabel = new Label
                {
                    Text = "Collection Medium Alert Minutes:",
                    Width = 190
                };
                TextBox collectionMediumAlertMinutesTextBox = new TextBox
                {
                    Text = config.CollectionOrder.MediumAlertMinutes.ToString(),
                    Width = 250
                };

                Label deliveryMediumAlertMinutesLabel = new Label
                {
                    Text = "Delivery Medium Alert Minutes:",
                    Width = 190
                };
                TextBox deliveryMediumAlertMinutesTextBox = new TextBox
                {
                    Text = config.DeliveryOrder.MediumAlertMinutes.ToString(),
                    Width = 250
                };

                CheckBox enabledCheckBox = new CheckBox
                {
                    Text = "Notifications Enabled:",
                    Checked = config.Enabled == "Yes",
                    Width = 250
                };

                Button saveButton = new Button
                {
                    Text = "Save",
                    Width = 250
                };

                saveButton.Click += (s, args) =>
                {
                    try
                    {
                        // Validate inputs
                        if (!int.TryParse(collectionHighAlertMinutesTextBox.Text, out int collectionHigh) || collectionHigh <= 0)
                        {
                            MessageBox.Show("Collection High Alert Minutes must be a positive number.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (!int.TryParse(deliveryHighAlertMinutesTextBox.Text, out int deliveryHigh) || deliveryHigh <= 0)
                        {
                            MessageBox.Show("Delivery High Alert Minutes must be a positive number.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (!int.TryParse(collectionMediumAlertMinutesTextBox.Text, out int collectionMedium) || collectionMedium <= 0)
                        {
                            MessageBox.Show("Collection Medium Alert Minutes must be a positive number.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (!int.TryParse(deliveryMediumAlertMinutesTextBox.Text, out int deliveryMedium) || deliveryMedium <= 0)
                        {
                            MessageBox.Show("Delivery Medium Alert Minutes must be a positive number.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Update the configuration with the new values
                        config.CollectionOrder.HighAlertMinutes = collectionHigh;
                        config.DeliveryOrder.HighAlertMinutes = deliveryHigh;
                        config.CollectionOrder.MediumAlertMinutes = collectionMedium;
                        config.DeliveryOrder.MediumAlertMinutes = deliveryMedium;

                        // Update the Enabled setting
                        config.Enabled = enabledCheckBox.Checked ? "Yes" : "No";

                        // Save the configuration
                        configManager.SaveConfig(config);

                        // Refresh notification status
                        AlertTimer_Tick(alertTimer, EventArgs.Empty);

                        // Close the form
                        form.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving configuration: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                // Add the labels and controls to the form
                flowLayoutPanel.Controls.Add(collectionHighAlertMinutesLabel);
                flowLayoutPanel.Controls.Add(collectionHighAlertMinutesTextBox);

                flowLayoutPanel.Controls.Add(deliveryHighAlertMinutesLabel);
                flowLayoutPanel.Controls.Add(deliveryHighAlertMinutesTextBox);

                flowLayoutPanel.Controls.Add(collectionMediumAlertMinutesLabel);
                flowLayoutPanel.Controls.Add(collectionMediumAlertMinutesTextBox);

                flowLayoutPanel.Controls.Add(deliveryMediumAlertMinutesLabel);
                flowLayoutPanel.Controls.Add(deliveryMediumAlertMinutesTextBox);

                flowLayoutPanel.Controls.Add(enabledCheckBox);
                flowLayoutPanel.Controls.Add(saveButton);

                // Add the FlowLayoutPanel to the form
                form.Controls.Add(flowLayoutPanel);

                // Show the form
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void AlertTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string configPath = NotificationConfigManager.GetConfigPath();
                if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
                {
                    StopFlashing();
                    return;
                }

                var configManager = new NotificationConfigManager(configPath);
                var config = configManager.LoadConfig();

                if (config == null || config.Enabled == "No")
                {
                    StopFlashing();
                    return;
                }

                // Define the query string
                string query = @"
            SELECT 
                O.OrderId, 
                O.OrderType,
                O.DesiredCompletionTime,
                O.completion,
                CAST(DATEDIFF(MINUTE, 
                    DATEADD(SECOND, 
                        DATEPART(HOUR, O.DesiredCompletionTime) * 3600 + 
                        DATEPART(MINUTE, O.DesiredCompletionTime) * 60 + 
                        DATEPART(SECOND, O.DesiredCompletionTime), 
                        CAST(O.OrderDate AS DATETIME)
                    ), 
                    GETDATE()
                ) AS INT) AS TimeSinceDesiredCompletion
            FROM 
                Orders O
            WHERE 
                O.completion = 'no' 
                AND CAST(O.OrderDate AS DATE) = CAST(GETDATE() AS DATE)
            AND O.OrderType IN ('COLLECTION', 'DELIVERY')";

                var orders = MainClass.GetOrdersFromDatabase(query);

                // Check if there are any orders at all
                if (orders == null || orders.Count == 0)
                {
                    StopFlashing();
                    return;
                }

                bool highAlertTriggered = false;
                bool mediumAlertTriggered = false;

                foreach (var order in orders)
                {
                    if (order == null) continue;

                    string orderType = (order["OrderType"] ?? "").ToString().ToUpper();

                    // Safely parse the time difference
                    if (order["TimeSinceDesiredCompletion"] != null &&
                        order["TimeSinceDesiredCompletion"] != DBNull.Value)
                    {
                        int timeSince;
                        if (int.TryParse(order["TimeSinceDesiredCompletion"].ToString(), out timeSince))
                        {
                            if ((orderType == "COLLECTION" && timeSince > config.CollectionOrder.HighAlertMinutes) ||
                                (orderType == "DELIVERY" && timeSince > config.DeliveryOrder.HighAlertMinutes))
                            {
                                highAlertTriggered = true;
                                break;
                            }

                            if ((orderType == "COLLECTION" && timeSince > config.CollectionOrder.MediumAlertMinutes) ||
                                (orderType == "DELIVERY" && timeSince > config.DeliveryOrder.MediumAlertMinutes))
                            {
                                mediumAlertTriggered = true;
                            }
                        }
                    }
                }

                // Update button color based on alert status
                if (btnCurrentOrders != null && !IsDisposed)
                {
                    if (highAlertTriggered)
                    {
                        StartFlashing(Color.Red);
                    }
                    else if (mediumAlertTriggered)
                    {
                        StartFlashing(Color.Orange);
                    }
                    else
                    {
                        StopFlashing();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AlertTimer_Tick: {ex.Message}");
                StopFlashing();
            }
        }

        private void StartFlashing(Color flashColor)
        {
            try
            {
                if (btnCurrentOrders == null || IsDisposed) return;

                if (flashTimer != null)
                {
                    flashTimer.Stop();
                    flashTimer.Dispose();
                }

                flashTimer = new Timer();
                flashTimer.Interval = 500; // Set the flash interval (in milliseconds)
                flashTimer.Tick += (s, e) =>
                {
                    if (btnCurrentOrders != null && !IsDisposed)
                    {
                        btnCurrentOrders.BackColor = (btnCurrentOrders.BackColor == flashColor)
                            ? DefaultBackColor
                            : flashColor;
                    }
                };
                flashTimer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StartFlashing: {ex.Message}");
            }
        }

        private void StopFlashing()
        {
            try
            {
                if (flashTimer != null)
                {
                    flashTimer.Stop();
                    flashTimer.Dispose();
                    flashTimer = null;
                }

                if (btnCurrentOrders != null && !IsDisposed)
                {
                    btnCurrentOrders.BackColor = DefaultBackColor;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in StopFlashing: {ex.Message}");
            }
        }








        private void LoadPreviousOrderItems(int customerId)
        {
            try
            {
                // Clear the FlowLayoutPanel
                flpPreviousOrders.Controls.Clear();

                // Enable auto-scroll for the main panel
                flpPreviousOrders.AutoScroll = true;
                flpPreviousOrders.FlowDirection = FlowDirection.TopDown;
                flpPreviousOrders.WrapContents = false;

                // Create a label for the section
                Label titleLabel = new Label
                {
                    Text = "Previously Ordered Items",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(5),
                    ForeColor = Color.FromArgb(50, 55, 89),
                    Dock = DockStyle.Top
                };
                flpPreviousOrders.Controls.Add(titleLabel);

                // Create a panel to span the full width for a separator line
                Panel separatorPanel = new Panel
                {
                    Height = 2,
                    BackColor = Color.FromArgb(241, 85, 126),
                    Margin = new Padding(5, 0, 5, 10),
                    Width = flpPreviousOrders.Width - 30, // Account for scrollbar
                    Dock = DockStyle.Top
                };
                flpPreviousOrders.Controls.Add(separatorPanel);

                // Query to get the most frequently ordered items by this customer
                string query = @"
            SELECT 
                OI.ItemName, 
                MAX(OI.ItemPrice) AS ItemPrice, 
                COUNT(*) AS OrderCount,
                MAX(O.OrderDate) AS LastOrdered,
                F.icon AS ItemImage
            FROM 
                OrderItems OI
            JOIN 
                Orders O ON OI.OrderId = O.OrderId
            LEFT JOIN
                foodItems F ON OI.ItemName = F.Item
            WHERE 
                O.CustomerId = @CustomerId
            GROUP BY 
                OI.ItemName, F.icon
            ORDER BY 
                COUNT(*) DESC, 
                MAX(O.OrderDate) DESC";

                Dictionary<string, object> parameters = new Dictionary<string, object>
        {
            { "@CustomerId", customerId }
        };

                DataTable previousItems = DatabaseManager.ExecuteQuery(query, parameters);

                if (previousItems != null && previousItems.Rows.Count > 0)
                {
                    // Create a table layout panel for buttons
                    TableLayoutPanel tablePanel = new TableLayoutPanel
                    {
                        AutoSize = true,
                        Dock = DockStyle.Top,
                        Margin = new Padding(5),
                        CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                    };

                    // Calculate how many buttons to show per row based on the panel width
                    int buttonWidth = 112; // Width of a button
                    int buttonMargin = 10; // Total left/right margin of a button
                    int totalButtonWidth = buttonWidth + buttonMargin;
                    int availableWidth = flpPreviousOrders.Width - 30; // Account for scrollbar
                    int buttonsPerRow = Math.Max(1, availableWidth / totalButtonWidth);

                    // Set up the table layout
                    tablePanel.ColumnCount = buttonsPerRow;
                    tablePanel.RowCount = (int)Math.Ceiling((double)previousItems.Rows.Count / buttonsPerRow);

                    // Set column and row styles
                    float columnWidth = 100f / buttonsPerRow;
                    for (int i = 0; i < buttonsPerRow; i++)
                    {
                        tablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnWidth));
                    }

                    // Add buttons to the table
                    int rowIndex = 0;
                    int colIndex = 0;

                    foreach (DataRow row in previousItems.Rows)
                    {
                        string itemName = row["ItemName"].ToString();
                        decimal itemPrice = Convert.ToDecimal(row["ItemPrice"]);
                        int orderCount = Convert.ToInt32(row["OrderCount"]);
                        DateTime lastOrdered = Convert.ToDateTime(row["LastOrdered"]);

                        // Get image data if available
                        byte[] imageData = null;
                        if (row["ItemImage"] != DBNull.Value)
                        {
                            imageData = (byte[])row["ItemImage"];
                        }

                        // Create a button for each item using the CreateFoodItemButton method
                        Button btn = CreateFoodItemButton(itemName, itemPrice, imageData, (s, e) =>
                        {
                            dynamic tag = ((Button)s).Tag;
                            AddItemToBasket(tag.ItemName, tag.ItemPrice);
                        });

                        // Add the button to the table
                        tablePanel.Controls.Add(btn, colIndex, rowIndex);

                        // Move to the next column
                        colIndex++;

                        // If we've filled a row, move to the next row
                        if (colIndex >= buttonsPerRow)
                        {
                            colIndex = 0;
                            rowIndex++;
                        }
                    }

                    // Add the table panel to the main panel
                    flpPreviousOrders.Controls.Add(tablePanel);
                }
                else
                {
                    // No previous orders found
                    Label noOrdersLabel = new Label
                    {
                        Text = "No previous orders found for this customer.",
                        Font = new Font("Segoe UI", 10, FontStyle.Italic),
                        AutoSize = true,
                        Margin = new Padding(10),
                        ForeColor = Color.Gray
                    };
                    flpPreviousOrders.Controls.Add(noOrdersLabel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading previous order items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void flpPreviousOrders_Paint(object sender, PaintEventArgs e)
        {
            // Draw a border around the panel
            using (Pen pen = new Pen(Color.FromArgb(241, 85, 126), 2))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, flpPreviousOrders.Width - 1, flpPreviousOrders.Height - 1);
            }

            // Add a title if the panel is empty
            if (flpPreviousOrders.Controls.Count == 0)
            {
                using (Font font = new Font("Segoe UI", 12, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, 55, 89)))
                {
                    e.Graphics.DrawString("Previous Items", font, brush, 10, 10);
                }
            }
        }

        private void lblPaymentOption_Click(object sender, EventArgs e)
        {

        }

        private void btnDel_Click_1(object sender, EventArgs e)
        {

        }

        private void btnDeliveryChargeAmend_Click(object sender, EventArgs e)
        {
            OrderManagement.Model.DeliveryChargeManager.ManageDeliveryCharges(this);
        }






        private void AutoCalculateDeliveryCharge()
        {
            OrderManagement.Model.DeliveryChargeManager.AutoCalculateDeliveryCharge(this);
        }




        private void txtDeliveryCharge_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAutoAdd_Click(object sender, EventArgs e)
        {
            using (var presetForm = new OrderManagement.View.frmPresetCharges())
            {
                if (presetForm.ShowDialog() == DialogResult.OK)
                {
                    UpdateTotalPrice(); // This method should exist in frmPOS
                }
            }
        }

        private void AddPresetChargesToTotal()
        {
            decimal presetTotal = OrderManagement.View.frmPresetCharges.GetTotalPresetCharges();
            decimal currentTotal = 0;
            decimal.TryParse(txtTotalPrice.Text.Replace("£", ""), out currentTotal);
            currentTotal += presetTotal;
            txtTotalPrice.Text = $"£{currentTotal:0.00}";
        }

        private void UpdateTotalPrice()
        {
            // Calculate basket total
            decimal basketTotal = 0;
            // Example: If you have a DataGridView for basket items, sum the prices
            foreach (DataGridViewRow row in basketGridView.Rows)
            {
                if (row.IsNewRow) continue;
                decimal price = 0;
                int qty = 1;
                decimal.TryParse(row.Cells["dgvPrice"].Value?.ToString(), out price);
                int.TryParse(row.Cells["dgvQty"].Value?.ToString(), out qty);
                basketTotal += price * qty;
            }

            // Calculate delivery charge
            decimal deliveryCharge = OrderManagement.Model.DeliveryChargeManager.GetDeliveryCharge(this);

            // Get preset charges
            decimal presetCharges = OrderManagement.View.frmPresetCharges.GetTotalPresetCharges();

            // Calculate total
            decimal total = basketTotal + deliveryCharge + presetCharges;

            // Update the total price textbox
            txtTotalPrice.Text = $"£{total:0.00}";
        }

        private void btnHold_Click(object sender, EventArgs e)
        {

        }

        private void btnWaiting_Click_1(object sender, EventArgs e)
        {

        }



    }
}
