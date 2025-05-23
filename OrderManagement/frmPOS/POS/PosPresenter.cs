// PosPresenter.cs
using OrderManagement.Model; // Assuming your current Model classes are here
using OrderManagement.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms; // For MessageBox, DialogResult, etc.

namespace OrderManagement.Presenter
{
    public class PosPresenter : IPosPresenter
    {
        private readonly IPosView _view;
        private OrderManager _orderManager; // Assuming this exists in your Model
        private CustomerManager _customerManager; // Assuming this exists
        private NotificationConfigManager _notificationConfigManager; // Assuming this exists
        // Removed: private DeliveryChargeManager _deliveryChargeManager; // DeliveryChargeManager is static

        // Internal state (Model data)
        private int _currentCustomerId;
        private int _currentOrderId;
        private string _currentOrderType = "WAITING"; // Default
        private bool _isEditMode = false;
        private bool _isOrderSaved = false;
        private decimal _discountRate = 0m;
        private DataTable _customerSearchDataTable = new DataTable(); // For search results
        private List<BasketItem> _basketItems = new List<BasketItem>(); // In-memory representation of basket

        // DTO for basket items (can be a nested class or separate file)
        public class BasketItem
        {
            public string ItemName { get; set; }
            public decimal OriginalPrice { get; set; }
            public decimal Price { get; set; } // Price after extra charges
            public int Quantity { get; set; }
            public decimal ExtraCharge { get; set; }
        }

        public PosPresenter(IPosView view)
        {
            _view = view;
            _orderManager = new OrderManager(); // Initialize your managers
            _customerManager = new CustomerManager();
            _notificationConfigManager = new NotificationConfigManager(NotificationConfigManager.GetConfigPath());
            // Removed: _deliveryChargeManager = new DeliveryChargeManager(); // DeliveryChargeManager is static

            // Subscribe to View events
            _view.ViewLoaded += (s, e) => HandleViewLoaded();
            _view.FormClosingConfirmed += (s, e) => HandleFormClosing(new FormClosingEventArgs(CloseReason.UserClosing, false)); // Pass dummy event args
            _view.BackToMainClicked += (s, e) => HandleBackToMain();
            _view.NewOrderClicked += (s, e) => HandleNewOrder();
            _view.SaveOrderClicked += (s, e) => HandleSaveOrder();
            _view.PaymentClicked += (s, e) => HandlePayment();
            _view.KotClicked += (s, e) => HandleKotPrint();
            _view.ClearBasketClicked += (s, e) => HandleClearBasket();
            _view.CustomerActionClicked += (s, e) => HandleCustomerAction();
            _view.CallMeClicked += (s, e) => HandleCallMe();
            _view.BillClicked += (s, e) => HandleBill();
            _view.NotificationConfigClicked += (s, e) => HandleNotificationConfig();
            _view.AutoAddPresetClicked += (s, e) => HandleAutoAddPresetCharges();
            _view.DeliveryChargeAmendClicked += (s, e) => HandleDeliveryChargeAmend();
            _view.SearchFoodItemTextChanged += (s, searchText) => HandleSearchFoodItemTextChange(searchText);
            _view.CustomerTelephoneTextChanged += (s, telNo) => HandleCustomerTelephoneTextChange(telNo);
            _view.CustomerDetailsTextChanged += (s, details) => HandleCustomerDetailsTextChange(details);
            _view.CategorySelected += (s, category) => HandleCategorySelected(category);
            _view.BasketCellContentClicked += (s, args) => HandleBasketCellContentClick(args.RowIndex, args.ColumnName);
            _view.CustomerSearchItemSelected += (s, selectedIndex) => HandleCustomerSearchItemSelected(selectedIndex);
            _view.DiscountSelected += (s, discountLabel) => HandleDiscountSelected(discountLabel);
            _view.OrderAlertTimerTick += (s, e) => HandleOrderAlertTimerTick();
            _view.AlertTimerTick += (s, e) => HandleAlertTimerTick();
        }

        // Constructor for edit mode
        public PosPresenter(IPosView view, int orderId) : this(view)
        {
            _isEditMode = true;
            _currentOrderId = orderId;
            _view.SaveOrderButtonText = "Update Order";
            _view.ShowMessage($"POS - Edit Order #{orderId}", "Edit Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadOrderForEditing(orderId);
        }

        public void Initialize()
        {
            // Initial setup for the view
            _view.IsDeliveryChargeVisible = false;
            _view.IsDeliveryChargeAmendButtonVisible = false;
            _view.IsCustomerAddressVisible = false;
            _view.IsAddressDisplayVisible = false;
            _view.ResetAllOrderTypeButtonsColor(Color.FromArgb(241, 85, 126));
            _view.SetOrderTypeButtonColor("WAITING", Color.Green); // Default
            _currentOrderType = "WAITING";
            _view.TotalPriceText = "£0.00";
            _view.SelectedDiscountIndex = 0; // "No Discount"
            LoadCategories();
            CalculateAndDisplayTotal(); // Initial total calculation
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _orderManager.GetAllCategories();
                _view.LoadCategories(categories);
                if (categories.Any())
                {
                    HandleCategorySelected(categories.First()); // Load items for the first category
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFoodItemsByCategory(string category)
        {
            try
            {
                var items = _orderManager.GetFoodItemsByCategory(category)
                                         .Select(item => new FoodItemDisplayDto
                                         {
                                             ItemName = item.Item,
                                             ItemPrice = item.Price,
                                             ImageData = item.Icon // Assuming icon is byte[]
                                         });
                _view.LoadFoodItems(items);
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error loading items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPreviousOrderItemsForCustomer(int customerId)
        {
            try
            {
                var previousItems = _orderManager.GetPreviousOrderedItems(customerId)
                                                 .Select(item => new PreviousOrderItemDisplayDto
                                                 {
                                                     ItemName = item.ItemName,
                                                     ItemPrice = item.ItemPrice,
                                                     ImageData = item.ItemImage // Assuming ItemImage is byte[]
                                                 });
                _view.LoadPreviousOrderButtons(previousItems);
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error loading previous order items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Event Handlers from View ---

        public void HandleViewLoaded()
        {
            // Any logic that needs to run when the form is fully loaded
            // DeliveryChargeManager.Initialize(_view as frmPOS); // This needs a way to pass the view context
        }

        public void HandleFormClosing(FormClosingEventArgs e)
        {
            if (_isEditMode && !_isOrderSaved && _basketItems.Any())
            {
                DialogResult result = _view.ShowConfirmation(
                    "You have unsaved changes to this order. Do you want to save before closing?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    HandleSaveOrder();
                    if (!_isOrderSaved)
                    {
                        e.Cancel = true; // Saving failed, cancel closing
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // User canceled, don't close
                }
            }

            // Clean up resources (timers, connections)
            // This part might still need to be in the View's FormClosing event,
            // or the Presenter could expose methods to stop/dispose timers
            DatabaseManager.ForceCheckpoint();
            DatabaseManager.CloseConnections();
        }

        public void HandleBackToMain()
        {
            if (_isEditMode && !_isOrderSaved && _basketItems.Any())
            {
                DialogResult result = _view.ShowConfirmation(
                    "You have unsaved changes to this order. Do you want to save before closing?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    HandleSaveOrder();
                    if (!_isOrderSaved) return; // If saving failed, don't proceed
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // User canceled, don't close
                }
            }
            // The actual navigation (showing frmMain and hiding current form)
            // should be handled by the View or a higher-level coordinator.
            // Presenter just signals intent.
            _view.CloseView(); // Signal to the View to close itself
        }

        public void HandleNewOrder()
        {
            if (_isEditMode)
            {
                DialogResult result = _view.ShowConfirmation(
                    "You are currently editing an order. Are you sure you want to start a new order?",
                    "Confirm New Order",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return; // User chose to continue editing
                }
                // If Yes, the current form should close and a new one open.
                // This navigation should be handled by the application's main entry point
                // or a coordinator. Presenter just signals.
                _view.CloseView(); // Signal to the View to close itself
                // A new frmPOS should be opened by the main application logic
                return;
            }

            ResetPosState();
        }

        public void HandleSaveOrder()
        {
            if (!_basketItems.Any())
            {
                _view.ShowMessage("Please add items to the basket before saving the order.", "Empty Basket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int customerId = GetOrCreateCustomer();
                if (customerId <= 0)
                {
                    _view.ShowMessage("Failed to create or find customer. Please check customer details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                decimal totalPrice = CalculateTotal();
                string paymentType = string.IsNullOrEmpty(_view.PaymentOptionText) ? "PENDING" : _view.PaymentOptionText;
                string discountLabel = _view.SelectedDiscountIndex >= 0 ? _view.SelectedDiscountIndex.ToString() : "No Discount"; // Assuming index maps to label

                // Get delivery charge before saving
                decimal deliveryCharge = 0m;
                if (_currentOrderType == "DELIVERY")
                {
                    // Call the static method directly, casting to frmPOS
                    deliveryCharge = DeliveryChargeManager.GetDeliveryCharge(_view as frmPOS);
                }

                if (_isEditMode && _currentOrderId > 0)
                {
                    _orderManager.UpdateOrder(_currentOrderId, customerId, _currentOrderType, totalPrice, paymentType, _view.AddressDisplayText, deliveryCharge, _discountRate, discountLabel);
                    _orderManager.DeleteOrderItems(_currentOrderId); // Clear existing items for update
                }
                else
                {
                    decimal presetCharges = frmPresetCharges.GetTotalPresetCharges(); // Assuming frmPresetCharges is accessible
                    _currentOrderId = _orderManager.SaveNewOrder(customerId, _currentOrderType, totalPrice, DateTime.Now, paymentType, _view.AddressDisplayText, deliveryCharge, presetCharges, _discountRate, discountLabel);
                    if (_currentOrderId == -1)
                    {
                        _view.ShowMessage("Failed to save order. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                bool allItemsSaved = true;
                foreach (var item in _basketItems)
                {
                    int result = _orderManager.SaveOrderItem(_currentOrderId, item.ItemName, item.OriginalPrice, item.Quantity, item.ExtraCharge);
                    if (result == -1)
                    {
                        allItemsSaved = false;
                        _view.ShowMessage($"Failed to save item: {item.ItemName}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (allItemsSaved)
                {
                    DatabaseManager.ForceCheckpoint();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    _isOrderSaved = true;
                    string message = _isEditMode ? "Order updated successfully!" : "Order saved successfully!";
                    _view.ShowMessage(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (_isEditMode)
                    {
                        _view.CloseView(); // Signal to close
                    }
                    else
                    {
                        ResetPosState();
                    }
                }
                else
                {
                    _view.ShowMessage("Some items failed to save. Please check the order details.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Error saving order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void HandlePayment()
        {
            if (string.IsNullOrEmpty(_currentOrderType))
            {
                _view.ShowMessage("Please select either collection or delivery from the top button panel", "Order Type Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_basketItems.Any())
            {
                _view.ShowMessage("The basket is empty.", "Empty Basket", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ensure order is saved before proceeding to payment
            if (!_isOrderSaved || _currentOrderId <= 0)
            {
                HandleSaveOrder();
                if (!_isOrderSaved || _currentOrderId <= 0) // Check again if save was successful
                {
                    _view.ShowMessage("Order could not be saved, payment cancelled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Prepare data for payment form (This part still needs to interact with the actual WinForms frmPayment)
            // You'd need to create a DTO for payment details and pass it to the payment form,
            // and the payment form would then update the order status via the Presenter.
            // For now, I'll simulate the interaction with frmPayment.
            using (frmPayment paymentForm = new frmPayment()) // Assuming frmPayment exists
            {
                // Populate paymentForm properties
                // This is where the Presenter needs to extract data from its internal state
                // and push it to the specific WinForms payment form.
                // This is a common challenge with MVP and tightly coupled forms.
                // Ideally, frmPayment would also have an IPaymentView and IPaymentPresenter.
                // For now, I'll assume direct property setting.

                // Create a dummy DataGridView to pass to frmPayment (less ideal, but matches original)
                DataGridView basketCopy = new DataGridView();
                // ... copy columns and rows from _basketItems to basketCopy ...

                paymentForm.BasketGrid = basketCopy;
                paymentForm.lblPaymentAmount.Text = CalculateTotal().ToString("C2");
                paymentForm.CustomerName = _view.CustomerDetailsText; // Get from view
                paymentForm.TelephoneNo = _view.CustomerTelephoneText; // Get from view
                paymentForm.OrderType = _currentOrderType;
                paymentForm.TotalPrice = CalculateTotal();
                paymentForm.Address = _view.AddressDisplayText; // Get from view
                paymentForm.OrderId = _currentOrderId;

                paymentForm.Confirm += paymentOption =>
                {
                    _view.PaymentOptionText = paymentOption;
                    _view.ShowMessage("Payment processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Update order payment status in DB
                    _orderManager.UpdateOrderPaymentStatus(_currentOrderId, paymentOption);

                    if (_isEditMode)
                    {
                        // Close any related forms (this logic is complex and might need a coordinator)
                        _view.CloseView();
                    }
                    else
                    {
                        ResetPosState();
                    }
                };

                // This assumes customerForename and customerSurname are accessible from the Presenter's state
                // You might need to expose them as properties on the Presenter or pass them from the View.
                paymentForm.lblCustomerToPay.Text = $"{_customerManager.GetCustomerForename(_currentCustomerId)} {_customerManager.GetCustomerSurname(_currentCustomerId)}";
                paymentForm.ShowDialog();
            }
        }

        public void HandleKotPrint()
        {
            HandleSaveOrder(); // Ensure order is saved before printing
            if (!_isOrderSaved) return;

            // Printing logic (remains largely the same, but uses Presenter's state)
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.PaperSize = new PaperSize("Thermal44", 44 * 10, 1000);

            pd.PrintPage += (s, ev) =>
            {
                // ... (printing logic using _basketItems, _currentOrderType, _currentCustomerId, _currentOrderId, etc.)
                // Access customer details from _customerManager or properties on Presenter
                // Calculate total using CalculateTotal()
                // Example:
                // string customerForename = _customerManager.GetCustomerForename(_currentCustomerId);
                // string customerSurname = _customerManager.GetCustomerSurname(_currentCustomerId);
                // ...
            };

            try
            {
                pd.Print();
                _view.ShowMessage("Order saved and printed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Error printing KOT: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void HandleClearBasket()
        {
            _basketItems.Clear();
            _view.ClearBasket();
            CalculateAndDisplayTotal();
            _isOrderSaved = false;
            LoadPreviousOrderItemsForCustomer(_currentCustomerId); // Reload previous orders panel
        }

        public void HandleCustomerAction()
        {
            // This still opens a new WinForms form (frmCustomerAdd)
            // Ideally, frmCustomerAdd would also be part of an MVP structure.
            using (frmCustomerAdd customerAddForm = new frmCustomerAdd()) // Assuming frmCustomerAdd exists
            {
                // Populate form fields based on _currentCustomerId or _view.CustomerTelephoneText
                if (_currentCustomerId > 0)
                {
                    var customer = _customerManager.GetCustomerDetails(_currentCustomerId);
                    if (customer != null)
                    {
                        customerAddForm.label1.Text = "Update Customer";
                        customerAddForm.id = _currentCustomerId;
                        customerAddForm.txtForename.Text = customer.Forename;
                        customerAddForm.txtSurname.Text = customer.Surname;
                        customerAddForm.txtTelephoneNo.Text = customer.TelephoneNo;
                        customerAddForm.txtEmail.Text = customer.Email;
                        customerAddForm.txtHouseNameNumber.Text = customer.HouseNameNumber;
                        customerAddForm.txtAddressLine1.Text = customer.AddressLine1;
                        customerAddForm.txtAddressLine2.Text = customer.AddressLine2;
                        customerAddForm.txtAddressLine3.Text = customer.AddressLine3;
                        customerAddForm.txtAddressLine4.Text = customer.AddressLine4;
                        customerAddForm.txtPostcode.Text = customer.Postcode;
                    }
                }
                else if (!string.IsNullOrEmpty(_view.CustomerTelephoneText))
                {
                    customerAddForm.txtTelephoneNo.Text = _view.CustomerTelephoneText;
                }
                else if (!string.IsNullOrEmpty(_view.CustomerDetailsText))
                {
                    string[] nameParts = _view.CustomerDetailsText.Split(' ');
                    customerAddForm.txtForename.Text = nameParts[0];
                    if (nameParts.Length > 1)
                    {
                        customerAddForm.txtSurname.Text = string.Join(" ", nameParts.Skip(1));
                    }
                }

                customerAddForm.SaveClicked += (s, args) =>
                {
                    string telephoneNo = frmCustomerAdd.telephoneNumber; // Get from static property
                    var customer = _customerManager.GetCustomerByTelephone(telephoneNo);
                    if (customer != null)
                    {
                        UpdateCustomerDetailsInPresenter(customer);
                    }
                };

                customerAddForm.ShowDialog();
            }
        }

        public void HandleCallMe()
        {
            using (frmCallerView callerViewForm = new frmCallerView()) // Assuming frmCallerView exists
            {
                callerViewForm.ShowDialog();
                string telephoneNo = callerViewForm.EnteredTelephoneNo;

                if (!string.IsNullOrEmpty(telephoneNo))
                {
                    _view.CustomerTelephoneText = telephoneNo;
                    var customer = _customerManager.GetCustomerByTelephone(telephoneNo);
                    if (customer != null)
                    {
                        UpdateCustomerDetailsInPresenter(customer);
                        LoadPreviousOrderItemsForCustomer(_currentCustomerId);
                    }
                    else
                    {
                        _view.ShowMessage("Customer not found. Entered telephone number used as customer telephone.", "Customer Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        public void HandleBill()
        {
            // Opens another WinForms form. Ideally, this would also be part of MVP.
            using (frmOrderHistory orderHistoryForm = new frmOrderHistory()) // Assuming frmOrderHistory exists
            {
                orderHistoryForm.ShowDialog();
            }
        }

        public void HandleNotificationConfig()
        {
            // This method creates a form dynamically, which is less ideal for MVP.
            // For a proper MVP, you'd have a separate INotificationConfigView and Presenter.
            // Keeping the original logic here for now, but it's a point for further refactoring.
            try
            {
                string configPath = NotificationConfigManager.GetConfigPath();
                if (string.IsNullOrEmpty(configPath) || !System.IO.File.Exists(configPath))
                {
                    _view.ShowMessage("Could not determine configuration path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var configManager = new NotificationConfigManager(configPath);
                var config = configManager.LoadConfig();

                Form form = new Form();
                form.Text = "Notification Configuration";
                form.Size = new Size(400, 400);
                form.StartPosition = FormStartPosition.CenterParent;

                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.Dock = DockStyle.Fill;
                flowLayoutPanel.AutoScroll = true;
                flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel.Padding = new Padding(10);

                Label collectionHighAlertMinutesLabel = new Label { Text = "Collection High Alert Minutes:", Width = 190 };
                TextBox collectionHighAlertMinutesTextBox = new TextBox { Text = config.CollectionOrder.HighAlertMinutes.ToString(), Width = 250 };
                Label deliveryHighAlertMinutesLabel = new Label { Text = "Delivery High Alert Minutes:", Width = 190 };
                TextBox deliveryHighAlertMinutesTextBox = new TextBox { Text = config.DeliveryOrder.HighAlertMinutes.ToString(), Width = 250 };
                Label collectionMediumAlertMinutesLabel = new Label { Text = "Collection Medium Alert Minutes:", Width = 190 };
                TextBox collectionMediumAlertMinutesTextBox = new TextBox { Text = config.CollectionOrder.MediumAlertMinutes.ToString(), Width = 250 };
                Label deliveryMediumAlertMinutesLabel = new Label { Text = "Delivery Medium Alert Minutes:", Width = 190 };
                TextBox deliveryMediumAlertMinutesTextBox = new TextBox { Text = config.DeliveryOrder.MediumAlertMinutes.ToString(), Width = 250 };
                CheckBox enabledCheckBox = new CheckBox { Text = "Notifications Enabled:", Checked = config.Enabled == "Yes", Width = 250 };
                Button saveButton = new Button { Text = "Save", Width = 250 };

                saveButton.Click += (s, args) =>
                {
                    try
                    {
                        if (!int.TryParse(collectionHighAlertMinutesTextBox.Text, out int collectionHigh) || collectionHigh <= 0) { _view.ShowMessage("Collection High Alert Minutes must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (!int.TryParse(deliveryHighAlertMinutesTextBox.Text, out int deliveryHigh) || deliveryHigh <= 0) { _view.ShowMessage("Delivery High Alert Minutes must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (!int.TryParse(collectionMediumAlertMinutesTextBox.Text, out int collectionMedium) || collectionMedium <= 0) { _view.ShowMessage("Collection Medium Alert Minutes must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                        if (!int.TryParse(deliveryMediumAlertMinutesTextBox.Text, out int deliveryMedium) || deliveryMedium <= 0) { _view.ShowMessage("Delivery Medium Alert Minutes must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                        config.CollectionOrder.HighAlertMinutes = collectionHigh;
                        config.DeliveryOrder.HighAlertMinutes = deliveryHigh;
                        config.CollectionOrder.MediumAlertMinutes = collectionMedium;
                        config.DeliveryOrder.MediumAlertMinutes = deliveryMedium;
                        config.Enabled = enabledCheckBox.Checked ? "Yes" : "No";

                        configManager.SaveConfig(config);
                        HandleAlertTimerTick(); // Refresh notification status
                        form.Close();
                    }
                    catch (Exception ex)
                    {
                        _view.ShowMessage("Error saving configuration: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                flowLayoutPanel.Controls.AddRange(new Control[] {
                    collectionHighAlertMinutesLabel, collectionHighAlertMinutesTextBox,
                    deliveryHighAlertMinutesLabel, deliveryHighAlertMinutesTextBox,
                    collectionMediumAlertMinutesLabel, collectionMediumAlertMinutesTextBox,
                    deliveryMediumAlertMinutesLabel, deliveryMediumAlertMinutesTextBox,
                    enabledCheckBox, saveButton
                });
                form.Controls.Add(flowLayoutPanel);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void HandleAutoAddPresetCharges()
        {
            using (var presetForm = new frmPresetCharges()) // Assuming frmPresetCharges exists
            {
                if (presetForm.ShowDialog() == DialogResult.OK)
                {
                    CalculateAndDisplayTotal();
                }
            }
        }

        public void HandleDeliveryChargeAmend()
        {
            // Call the static method directly, casting to frmPOS
            DeliveryChargeManager.ManageDeliveryCharges(_view as frmPOS);
            CalculateAndDisplayTotal();
        }

        public void HandleSearchFoodItemTextChange(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                LoadFoodItemsByCategory(_orderManager.GetAllCategories().FirstOrDefault()); // Reload first category
                return;
            }

            var items = _orderManager.SearchFoodItems(searchText)
                                     .Select(item => new FoodItemDisplayDto
                                     {
                                         ItemName = item.Item,
                                         ItemPrice = item.Price,
                                         ImageData = item.Icon
                                     });
            _view.LoadFoodItems(items);
        }

        public void HandleCustomerTelephoneTextChange(string telephoneNo)
        {
            if (string.IsNullOrWhiteSpace(telephoneNo))
            {
                _view.ClearCustomerSearchList();
                _view.SetCustomerSearchListVisibility(false);
                return;
            }

            try
            {
                _customerSearchDataTable.Clear();
                var customers = _customerManager.SearchCustomersByTelephone(telephoneNo);

                _view.ClearCustomerSearchList();
                int maxWidth = 0;
                foreach (var customer in customers)
                {
                    string fullAddress = $"{customer.HouseNameNumber} {customer.AddressLine1}, {customer.AddressLine2}, {customer.AddressLine3}, {customer.AddressLine4}, {customer.Postcode}";
                    string customerDetails = $"{customer.Forename} {customer.Surname} - {fullAddress}";
                    _view.AddCustomerSearchItem(customerDetails);

                    // Re-measure width (this is UI specific, ideally done in View)
                    // You'd need a way for the Presenter to request this from the View.
                    // For now, assume the View handles its own sizing.
                }

                _view.SetCustomerSearchListVisibility(customers.Any());
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Database error while searching customers: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _view.ClearCustomerSearchList();
                _view.SetCustomerSearchListVisibility(false);
            }
        }

        public void HandleCustomerDetailsTextChange(string customerDetails)
        {
            // This is primarily for updating internal state for saving, not for search
            // The original code had a split logic for forename/surname.
            // This should be handled by the customer manager or a dedicated DTO.
        }

        public void HandleCategorySelected(string category)
        {
            LoadFoodItemsByCategory(category);
        }

        public void HandleBasketCellContentClick(int rowIndex, string columnName)
        {
            if (rowIndex < 0 || rowIndex >= _basketItems.Count) return;

            var item = _basketItems[rowIndex];

            if (columnName == "dgvDeleteBasketItem")
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    _view.UpdateBasketItemQuantity(rowIndex, item.Quantity);
                }
                else
                {
                    _basketItems.RemoveAt(rowIndex);
                    _view.RemoveBasketItem(rowIndex);
                }
                CalculateAndDisplayTotal();
                _isOrderSaved = false;
            }
            else if (columnName == "dgvExtraCharge")
            {
                using (var popup = new frmExtraChargePopup()) // Assuming frmExtraChargePopup exists
                {
                    popup.SetCurrentExtraCharge(item.ExtraCharge);
                    if (popup.ShowDialog(_view as Form) == DialogResult.OK) // Needs actual form instance
                    {
                        decimal newExtra = popup.GetSelectedAmount();
                        item.ExtraCharge = newExtra;
                        item.Price = item.OriginalPrice + newExtra; // Update item's price
                        _view.UpdateBasketItemPriceAndExtraCharge(rowIndex, item.OriginalPrice, newExtra);
                        CalculateAndDisplayTotal();
                        _isOrderSaved = false;
                    }
                }
            }
        }

        public void HandleCustomerSearchItemSelected(int selectedIndex)
        {
            if (selectedIndex != -1 && selectedIndex < _customerSearchDataTable.Rows.Count)
            {
                _view.SetCustomerSearchListVisibility(false);
                DataRow row = _customerSearchDataTable.Rows[selectedIndex];
                var customer = new CustomerManager.CustomerDto // Use the nested DTO from CustomerManager
                {
                    Id = SafeOperations.SafeGetInt(row, "Id"),
                    Forename = SafeOperations.SafeGetString(row, "forename"),
                    Surname = SafeOperations.SafeGetString(row, "surname"),
                    TelephoneNo = SafeOperations.SafeGetString(row, "telephoneNo"),
                    Email = SafeOperations.SafeGetString(row, "Email"),
                    HouseNameNumber = SafeOperations.SafeGetString(row, "houseNameNumber"),
                    AddressLine1 = SafeOperations.SafeGetString(row, "AddressLine1"),
                    AddressLine2 = SafeOperations.SafeGetString(row, "AddressLine2"),
                    AddressLine3 = SafeOperations.SafeGetString(row, "AddressLine3"),
                    AddressLine4 = SafeOperations.SafeGetString(row, "AddressLine4"),
                    Postcode = SafeOperations.SafeGetString(row, "Postcode")
                };
                UpdateCustomerDetailsInPresenter(customer);
                LoadPreviousOrderItemsForCustomer(customer.Id);
            }
        }

        public void HandleDiscountSelected(string selectedDiscount)
        {
            Dictionary<string, decimal> discountRates = new Dictionary<string, decimal>
            {
                { "No Discount", 0m }, { "5% Discount", 0.05m }, { "10% Discount", 0.1m },
                { "20% Discount", 0.2m }, { "30% Discount", 0.3m }, { "40% Discount", 0.4m },
                { "50% Discount", 0.5m }, { "60% Discount", 0.6m }, { "70% Discount", 0.7m },
                { "80% Discount", 0.8m }, { "90% Discount", 0.9m }, { "100% Discount", 1m },
            };

            if (discountRates.TryGetValue(selectedDiscount, out decimal rate))
            {
                _discountRate = rate;
            }
            else
            {
                _discountRate = 0m;
            }
            CalculateAndDisplayTotal();
        }

        public void HandleOrderAlertTimerTick()
        {
            bool hasCriticalOrders = _orderManager.CheckForCriticalOrders(); // Assuming this method exists in OrderManager
            if (hasCriticalOrders)
            {
                // Toggle color (Presenter decides color, View applies it)
                _view.CurrentOrdersButtonBackColor = (_view.CurrentOrdersButtonBackColor == Color.Red) ? SystemColors.Control : Color.Red;
                _view.CurrentOrdersButtonForeColor = (_view.CurrentOrdersButtonForeColor == Color.White) ? Color.Black : Color.White;
            }
            else
            {
                _view.CurrentOrdersButtonBackColor = SystemColors.Control;
                _view.CurrentOrdersButtonForeColor = Color.Black;
            }
        }

        public void HandleAlertTimerTick()
        {
            try
            {
                string configPath = NotificationConfigManager.GetConfigPath();
                if (string.IsNullOrEmpty(configPath) || !System.IO.File.Exists(configPath))
                {
                    StopFlashing();
                    return;
                }

                var config = _notificationConfigManager.LoadConfig();

                if (config == null || config.Enabled == "No")
                {
                    StopFlashing();
                    return;
                }

                var orders = _orderManager.GetAlertableOrders(); // Assuming this method exists and returns relevant data

                bool highAlertTriggered = false;
                bool mediumAlertTriggered = false;

                foreach (var order in orders)
                {
                    string orderType = (order["OrderType"] ?? "").ToString().ToUpper();
                    if (order["TimeSinceDesiredCompletion"] != null && order["TimeSinceDesiredCompletion"] != DBNull.Value)
                    {
                        int timeSince = Convert.ToInt32(order["TimeSinceDesiredCompletion"]);
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AlertTimerTick: {ex.Message}");
                StopFlashing();
            }
        }

        // --- Core Business Logic Methods ---

        public void AddItemToBasket(string itemName, decimal itemPrice)
        {
            // Get the latest price from the database (Model responsibility)
            decimal dbPrice = _orderManager.GetFoodItemPrice(itemName);

            var existingItem = _basketItems.FirstOrDefault(bi => bi.ItemName == itemName);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                // Update the UI for this specific row
                // This requires the View to have a method to update a specific row by index
                int rowIndex = _basketItems.IndexOf(existingItem); // This is fragile if basketGridView isn't perfectly synced
                _view.UpdateBasketItemQuantity(rowIndex, existingItem.Quantity);
            }
            else
            {
                var newItem = new BasketItem
                {
                    ItemName = itemName,
                    OriginalPrice = dbPrice,
                    Price = dbPrice, // Initial price is original price
                    Quantity = 1,
                    ExtraCharge = 0
                };
                _basketItems.Add(newItem);
                // Add new row to UI
                _view.AddBasketItem(newItem.ItemName, newItem.OriginalPrice, newItem.Price, newItem.Quantity, newItem.ExtraCharge);
            }

            CalculateAndDisplayTotal();
            _isOrderSaved = false;
        }

        private decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in _basketItems)
            {
                total += item.Price * item.Quantity;
            }

            if (_currentOrderType == "DELIVERY")
            {
                // Call the static method directly, casting to frmPOS
                total += DeliveryChargeManager.GetDeliveryCharge(_view as frmPOS);
            }

            total += frmPresetCharges.GetTotalPresetCharges(); // Still directly calling static method

            total = total * (1 - _discountRate);
            return total;
        }

        private void CalculateAndDisplayTotal()
        {
            decimal total = CalculateTotal();
            _view.TotalPriceText = total.ToString("C2");
        }

        private int GetOrCreateCustomer()
        {
            string phone = _view.CustomerTelephoneText.Trim();
            string customerName = _view.CustomerDetailsText.Trim();

            if (_currentCustomerId > 0) return _currentCustomerId; // Already have a customer

            if (!string.IsNullOrEmpty(phone))
            {
                var customer = _customerManager.GetCustomerByTelephone(phone);
                if (customer != null)
                {
                    _currentCustomerId = customer.Id;
                    return _currentCustomerId;
                }
            }

            // If no customer found by phone or phone is empty, create new
            string forename = customerName;
            string surname = "";
            if (customerName.Contains(" "))
            {
                string[] nameParts = customerName.Split(new[] { ' ' }, 2);
                forename = nameParts[0];
                surname = nameParts[1];
            }

            _currentCustomerId = _customerManager.CreateCustomer(
                forename,
                surname,
                string.IsNullOrEmpty(phone) ? null : phone,
                _view.AddressDisplayText // Get address from view
            );
            return _currentCustomerId;
        }

        private void UpdateCustomerDetailsInPresenter(CustomerManager.CustomerDto customer) // Changed parameter type
        {
            _currentCustomerId = customer.Id;
            _view.CustomerDetailsText = $"{customer.Forename} {customer.Surname}";
            _view.CustomerTelephoneText = customer.TelephoneNo;
            _view.PreviousOrdersText = customer.PreviousOrdersCount.ToString(); // Assuming this property exists
            _view.AddressDisplayText = $"{customer.HouseNameNumber} {customer.AddressLine1}, {customer.AddressLine2}, {customer.AddressLine3}, {customer.AddressLine4}, {customer.Postcode}";
            _view.CustomerActionButtonText = "Update Customer";
            // Set internal state that customer exists
            // This flag might be removed if _currentCustomerId > 0 implies existence.
            CalculateAndDisplayTotal(); // Recalculate total in case delivery charge changes
        }

        private void ResetPosState()
        {
            _basketItems.Clear();
            _view.ClearBasket();

            _view.CustomerDetailsText = string.Empty;
            _view.CustomerTelephoneText = string.Empty;
            _view.AddressDisplayText = string.Empty;
            _view.PreviousOrdersText = string.Empty;
            _view.TotalPriceText = "£0.00";

            _view.IsDeliveryChargeAmendButtonVisible = false;
            _view.SelectedDiscountIndex = 0; // "No Discount"

            _view.ResetAllOrderTypeButtonsColor(Color.FromArgb(241, 85, 126));
            _view.SetOrderTypeButtonColor("WAITING", Color.Green);
            _currentOrderType = "WAITING";

            _view.HideCustomerAddressFields();
            _view.PaymentOptionText = string.Empty;

            _currentCustomerId = 0;
            _currentOrderId = 0;
            _view.CustomerActionButtonText = "Add Customer";

            LoadPreviousOrderItemsForCustomer(0); // Clear previous orders display
            _isOrderSaved = false;

            _view.IsDeliveryChargeVisible = false;
            _view.IsDeliveryChargeAmendButtonVisible = false;
            // Delivery charge text box reset should be handled by the View
        }

        private void LoadOrderForEditing(int orderId)
        {
            try
            {
                var order = _orderManager.GetOrderDetails(orderId);
                if (order == null)
                {
                    _view.ShowMessage("Order not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _view.CloseView();
                    return;
                }

                _currentOrderId = order.OrderId;
                _currentCustomerId = order.CustomerId;
                _currentOrderType = order.OrderType;

                // Update View properties
                _view.CustomerDetailsText = $"{order.Forename} {order.Surname}";
                _view.CustomerTelephoneText = order.TelephoneNo;
                _view.AddressDisplayText = order.Address;

                // Set order type button color
                _view.ResetAllOrderTypeButtonsColor(Color.FromArgb(241, 85, 126));
                _view.SetOrderTypeButtonColor(_currentOrderType, Color.Green);

                // Handle visibility of address/delivery charge based on order type
                if (_currentOrderType.Equals("DELIVERY", StringComparison.OrdinalIgnoreCase))
                {
                    _view.ShowCustomerAddressFields();
                    _view.IsDeliveryChargeVisible = true;
                    _view.IsDeliveryChargeAmendButtonVisible = true;
                    // Call the static method directly, casting to frmPOS
                    DeliveryChargeManager.AutoCalculateDeliveryCharge(_view as frmPOS);
                }
                else
                {
                    _view.HideCustomerAddressFields();
                    _view.IsDeliveryChargeVisible = false;
                    _view.IsDeliveryChargeAmendButtonVisible = false;
                }

                // Load order items into internal basket and then update View
                _basketItems.Clear();
                _view.ClearBasket();
                var items = _orderManager.GetOrderItems(orderId);
                foreach (var item in items)
                {
                    var basketItem = new BasketItem
                    {
                        ItemName = item.ItemName,
                        OriginalPrice = item.ItemPrice,
                        Quantity = item.Quantity,
                        ExtraCharge = item.ExtraCharge,
                        Price = item.ItemPrice + item.ExtraCharge // Calculate price with extra charge
                    };
                    _basketItems.Add(basketItem);
                    _view.AddBasketItem(basketItem.ItemName, basketItem.OriginalPrice, basketItem.Price, basketItem.Quantity, basketItem.ExtraCharge);
                }

                CalculateAndDisplayTotal();
                LoadPreviousOrderItemsForCustomer(_currentCustomerId);
                _isOrderSaved = true;
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Error loading order for editing: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _view.CloseView();
            }
        }

        // Flashing logic (can be extracted to a helper class if complex)
        private Timer _flashTimer;
        private Color _defaultButtonBackColor; // Store original back color for btnCurrentOrders

        private void StartFlashing(Color flashColor)
        {
            if (_flashTimer != null)
            {
                _flashTimer.Stop();
                _flashTimer.Dispose();
            }

            _defaultButtonBackColor = _view.CurrentOrdersButtonBackColor; // Capture current color
            _flashTimer = new Timer();
            _flashTimer.Interval = 500;
            _flashTimer.Tick += (s, e) =>
            {
                _view.CurrentOrdersButtonBackColor = (_view.CurrentOrdersButtonBackColor == flashColor) ? _defaultButtonBackColor : flashColor;
            };
            _flashTimer.Start();
        }

        private void StopFlashing()
        {
            if (_flashTimer != null)
            {
                _flashTimer.Stop();
                _flashTimer.Dispose();
                _flashTimer = null;
            }
            _view.CurrentOrdersButtonBackColor = _defaultButtonBackColor; // Reset to original
            _view.CurrentOrdersButtonForeColor = Color.Black; // Ensure text is black
        }
    }
}
