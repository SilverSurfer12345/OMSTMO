using OrderManagement.Model;
using OrderManagement.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;
using System.IO;

namespace OrderManagement.Presenter
{
    public class OrderViewFormPresenter : IOrderViewFormPresenter
    {
        private readonly IOrderViewFormView _view;
        private readonly OrderManager _orderManager;
        private readonly CustomerManager _customerManager;

        private int _currentOrderId;
        private OrderManager.OrderDto _currentOrderDetails;
        private List<OrderManager.OrderItemDto> _currentOrderItems;

        // Store original values to detect changes
        private string _originalOrderType;
        private string _originalPaymentType;
        private decimal _originalTotalPrice;

        public OrderViewFormPresenter(IOrderViewFormView view)
        {
            _view = view;
            _orderManager = new OrderManager();
            _customerManager = new CustomerManager();

            // Subscribe to view events
            _view.ViewLoaded += (s, e) => HandleViewLoaded();
            _view.SaveClicked += (s, e) => HandleSave();
            _view.CloseClicked += (s, e) => HandleClose();
            _view.OrderTypeChanged += (s, e) => HandleOrderTypeChange();
            _view.PaymentTypeChanged += (s, e) => HandlePaymentTypeChange();
            _view.PreviewInvoiceClicked += (s, e) => HandlePreviewInvoice();
            _view.PrintClicked += (s, e) => HandlePrint();
            _view.EditCurrentOrderClicked += (s, e) => HandleEditCurrentOrder();
        }

        public void Initialize(int orderId)
        {
            _currentOrderId = orderId;
            LoadOrderDetails();
        }

        public void HandleViewLoaded()
        {
            // Any logic that needs to run when the form is fully loaded
            // The order details are already loaded in Initialize, so this might be minimal
        }

        private void LoadOrderDetails()
        {
            try
            {
                _currentOrderDetails = _orderManager.GetOrderDetails(_currentOrderId);
                _currentOrderItems = _orderManager.GetOrderItems(_currentOrderId);

                if (_currentOrderDetails == null)
                {
                    _view.ShowMessage("Order details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _view.CloseView();
                    return;
                }

                // Store original values for change detection
                _originalOrderType = _currentOrderDetails.OrderType;
                _originalPaymentType = _currentOrderDetails.PaymentType;
                _originalTotalPrice = _currentOrderDetails.TotalPrice;

                // Populate customer details
                _view.CustomerNameText = $"Name: {_currentOrderDetails.Forename} {_currentOrderDetails.Surname}";
                _view.CustomerTelephoneText = string.IsNullOrEmpty(_currentOrderDetails.TelephoneNo) ?
                                              "Telephone: no telephone number recorded" :
                                              $"Telephone: {_currentOrderDetails.TelephoneNo}";

                // Populate address for delivery orders
                if (_currentOrderDetails.OrderType.Equals("DELIVERY", StringComparison.OrdinalIgnoreCase))
                {
                    var addressLines = new List<string>();
                    string firstLine = "";
                    if (!string.IsNullOrWhiteSpace(_currentOrderDetails.HouseNameNumber))
                        firstLine += _currentOrderDetails.HouseNameNumber.Trim();
                    if (!string.IsNullOrWhiteSpace(_currentOrderDetails.AddressLine1))
                    {
                        if (firstLine.Length > 0)
                            firstLine += " ";
                        firstLine += _currentOrderDetails.AddressLine1.Trim();
                    }
                    if (!string.IsNullOrWhiteSpace(firstLine))
                        addressLines.Add(firstLine);
                    if (!string.IsNullOrWhiteSpace(_currentOrderDetails.AddressLine2)) addressLines.Add(_currentOrderDetails.AddressLine2.Trim());
                    if (!string.IsNullOrWhiteSpace(_currentOrderDetails.AddressLine3)) addressLines.Add(_currentOrderDetails.AddressLine3.Trim());
                    if (!string.IsNullOrWhiteSpace(_currentOrderDetails.AddressLine4)) addressLines.Add(_currentOrderDetails.AddressLine4.Trim());
                    if (!string.IsNullOrWhiteSpace(_currentOrderDetails.Postcode)) addressLines.Add(_currentOrderDetails.Postcode.Trim());

                    _view.CustomerAddressText = string.Join(Environment.NewLine, addressLines);
                    _view.AddressVisible = true;
                }
                else
                {
                    _view.CustomerAddressText = string.Empty;
                    _view.AddressVisible = false;
                }

                _view.OrderDateText = $"Order Date: {_currentOrderDetails.OrderDate:dd/MM/yyyy HH:mm}";
                _view.TotalPriceText = _currentOrderDetails.TotalPrice.ToString("0.00");

                // Set order type radio buttons
                _view.CollectionChecked = _currentOrderDetails.OrderType.Equals("COLLECTION", StringComparison.OrdinalIgnoreCase);
                _view.DeliveryChecked = _currentOrderDetails.OrderType.Equals("DELIVERY", StringComparison.OrdinalIgnoreCase);
                _view.OnlineChecked = _currentOrderDetails.OrderType.Equals("ONLINE", StringComparison.OrdinalIgnoreCase);
                _view.WaitingChecked = _currentOrderDetails.OrderType.Equals("WAITING", StringComparison.OrdinalIgnoreCase);

                // Set payment type radio buttons
                _view.CashChecked = _currentOrderDetails.PaymentType.Equals("CASH", StringComparison.OrdinalIgnoreCase);
                _view.CardChecked = _currentOrderDetails.PaymentType.Equals("CARD", StringComparison.OrdinalIgnoreCase);
                _view.PendingChecked = _currentOrderDetails.PaymentType.Equals("PENDING", StringComparison.OrdinalIgnoreCase);
                _view.CancelledChecked = _currentOrderDetails.PaymentType.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase);
                _view.RefundedChecked = _currentOrderDetails.PaymentType.Equals("REFUNDED", StringComparison.OrdinalIgnoreCase);

                DisplayOrderItemsAndSummary();
                _view.ChangesMadeVisible = false; // No changes on load
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Error loading order details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _view.CloseView();
            }
        }

        private void DisplayOrderItemsAndSummary()
        {
            // Create a DataTable for the DataGridView
            DataTable dt = new DataTable();
            dt.Columns.Add("dgvItemName", typeof(string));
            dt.Columns.Add("dgvItemPrice", typeof(decimal));
            dt.Columns.Add("dgvQuantity", typeof(int));

            // Add order items to DataTable
            foreach (var item in _currentOrderItems)
            {
                dt.Rows.Add(item.ItemName, item.ItemPrice, item.Quantity);
            }

            // Add preset charges to DataTable
            var presetCharges = GetPresetChargesFromDatabase(); // Fetch current preset charges
            foreach (var charge in presetCharges)
            {
                if (charge.ChargeValue > 0)
                {
                    dt.Rows.Add(charge.ChargeName, charge.ChargeValue, 1);
                }
            }

            // Add delivery charge if applicable
            if (_view.DeliveryChecked && _currentOrderDetails.DeliveryCharge > 0)
            {
                dt.Rows.Add("Delivery Charge", _currentOrderDetails.DeliveryCharge, 1);
            }

            _view.OrderItemsDataSource = dt;

            // Recalculate total based on what's in the grid (including delivery/preset)
            CalculateAndDisplayTotal();
        }

        private decimal CalculateTotalFromCurrentState()
        {
            decimal total = 0;
            foreach (var item in _currentOrderItems)
            {
                total += item.ItemPrice * item.Quantity;
            }

            // Add preset charges
            var presetCharges = GetPresetChargesFromDatabase();
            foreach (var charge in presetCharges)
            {
                if (charge.ChargeValue > 0)
                {
                    total += charge.ChargeValue;
                }
            }

            // Add delivery charge if order type is delivery
            if (_view.DeliveryChecked)
            {
                // Calculate delivery charge based on postcode (if available) or use stored
                decimal calculatedDeliveryCharge = 0m;
                if (!string.IsNullOrEmpty(_currentOrderDetails.Postcode))
                {
                    calculatedDeliveryCharge = DeliveryChargeManager.CalculateDeliveryCharge(_currentOrderDetails.Postcode);
                }
                else
                {
                    calculatedDeliveryCharge = _currentOrderDetails.DeliveryCharge; // Use stored if no postcode
                }
                total += calculatedDeliveryCharge;
            }

            // Apply discount if any (assuming discount is part of OrderDto or a separate field)
            // For now, I'll assume it's part of OrderDto and applied to the base total
            total = total * (1 - _currentOrderDetails.DiscountRate);

            return total;
        }

        private void CalculateAndDisplayTotal()
        {
            decimal total = CalculateTotalFromCurrentState();
            _view.TotalPriceText = total.ToString("0.00");
        }

        public void HandleSave()
        {
            if (!HasUnsavedChanges())
            {
                _view.ShowMessage("No changes found to save.", "No Changes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string selectedOrderType = GetSelectedOrderTypeFromView();
                string selectedPaymentType = GetSelectedPaymentTypeFromView();
                decimal currentTotalPrice = Convert.ToDecimal(_view.TotalPriceText); // Get from UI

                // Update the _currentOrderDetails DTO with new values from the view
                _currentOrderDetails.OrderType = selectedOrderType;
                _currentOrderDetails.PaymentType = selectedPaymentType;
                _currentOrderDetails.TotalPrice = currentTotalPrice; // Update with calculated total

                // Recalculate delivery charge based on current order type selection
                if (selectedOrderType.Equals("DELIVERY", StringComparison.OrdinalIgnoreCase))
                {
                    _currentOrderDetails.DeliveryCharge = DeliveryChargeManager.CalculateDeliveryCharge(_currentOrderDetails.Postcode);
                }
                else
                {
                    _currentOrderDetails.DeliveryCharge = 0m;
                }

                // Update the order in the database
                _orderManager.UpdateOrder(
                    _currentOrderDetails.OrderId,
                    _currentOrderDetails.CustomerId,
                    _currentOrderDetails.OrderType,
                    _currentOrderDetails.TotalPrice,
                    _currentOrderDetails.PaymentType,
                    _currentOrderDetails.Address, // Address might need to be updated if customer details change, but for now use existing
                    _currentOrderDetails.DeliveryCharge,
                    _currentOrderDetails.DiscountRate,
                    _currentOrderDetails.DiscountLabel
                );

                // Update original values after successful save
                _originalOrderType = _currentOrderDetails.OrderType;
                _originalPaymentType = _currentOrderDetails.PaymentType;
                _originalTotalPrice = _currentOrderDetails.TotalPrice;

                _view.ChangesMadeVisible = false;
                _view.ShowMessage("Order updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Error saving order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void HandleClose()
        {
            if (HasUnsavedChanges())
            {
                var result = _view.ShowConfirmation(
                    "You have unsaved changes. Do you want to save before closing?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    HandleSave(); // Attempt to save
                    if (HasUnsavedChanges()) // Check if still unsaved after attempt
                    {
                        _view.ShowMessage("Failed to save changes. The form will not close.", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Do not close
                }
            }
            _view.CloseView();
        }

        public void HandleOrderTypeChange()
        {
            // Update the internal order type based on view selection
            _currentOrderDetails.OrderType = GetSelectedOrderTypeFromView();
            _view.ChangesMadeVisible = HasUnsavedChanges();
            DisplayOrderItemsAndSummary(); // Recalculate total and update display
        }

        public void HandlePaymentTypeChange()
        {
            // Update the internal payment type based on view selection
            _currentOrderDetails.PaymentType = GetSelectedPaymentTypeFromView();
            _view.ChangesMadeVisible = HasUnsavedChanges();
        }

        public void HandlePreviewInvoice()
        {
            // This still opens a new WinForms form directly.
            // Data is passed from the presenter's internal state.
            using (var popup = new InvoicePopupForm(
                $"{_currentOrderDetails.Forename} {_currentOrderDetails.Surname}",
                _currentOrderDetails.TelephoneNo,
                _currentOrderDetails.HouseNameNumber,
                _currentOrderDetails.AddressLine1,
                _currentOrderDetails.AddressLine2,
                _currentOrderDetails.AddressLine3,
                _currentOrderDetails.AddressLine4,
                _currentOrderDetails.Postcode,
                _currentOrderDetails.OrderType,
                _currentOrderDetails.PaymentType,
                _currentOrderDetails.OrderDate,
                _view.OrderItemsDataSource as DataTable, // Pass the DataTable from the view
                _currentOrderDetails.DeliveryCharge, // Use the stored delivery charge
                _orderManager.GetPresetChargesTotal(), // Get total preset charges from manager
                _currentOrderDetails.TotalPrice.ToString("0.00") // Use the stored total price
            ))
            {
                // The InvoicePopupForm needs to be able to be parented by the view
                // This requires the IOrderViewFormView to expose its Form instance, or a method to show dialogs.
                // For now, assuming it can be shown directly.
                popup.ShowDialog(_view as Form);
            }
        }

        public void HandlePrint()
        {
            if (HasUnsavedChanges())
            {
                var result = _view.ShowConfirmation(
                    "You have unsaved changes. Do you want to save before printing?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    HandleSave();
                    if (HasUnsavedChanges()) // Check if still unsaved after attempt
                    {
                        _view.ShowMessage("Failed to save changes. Printing cancelled.", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Do not print
                }
            }

            // Implement print logic here, using data from _currentOrderDetails and _currentOrderItems
            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.PaperSize = new PaperSize("Thermal44", 44 * 10, 1000); // Example thermal paper size

            pd.PrintPage += (s, ev) =>
            {
                Graphics graphics = ev.Graphics;
                Font font = new Font("Courier New", 10);
                float fontHeight = font.GetHeight();
                int startX = 10;
                int startY = 10;
                int offset = 40;

                // Header
                graphics.DrawString("--- Order Details ---", new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY);
                offset += 20;

                graphics.DrawString($"Order ID: {_currentOrderDetails.OrderId}", font, new SolidBrush(Color.Black), startX, startY + offset);
                offset += (int)fontHeight;
                graphics.DrawString($"Date: {_currentOrderDetails.OrderDate:dd/MM/yyyy HH:mm}", font, new SolidBrush(Color.Black), startX, startY + offset);
                offset += (int)fontHeight;
                graphics.DrawString($"Type: {_currentOrderDetails.OrderType}", font, new SolidBrush(Color.Black), startX, startY + offset);
                offset += (int)fontHeight;
                graphics.DrawString($"Payment: {_currentOrderDetails.PaymentType}", font, new SolidBrush(Color.Black), startX, startY + offset);
                offset += (int)fontHeight + 10;

                // Customer Info
                graphics.DrawString("--- Customer Info ---", new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                offset += 20;

                graphics.DrawString($"Name: {_currentOrderDetails.Forename} {_currentOrderDetails.Surname}", font, new SolidBrush(Color.Black), startX, startY + offset);
                offset += (int)fontHeight;
                graphics.DrawString($"Tel: {_currentOrderDetails.TelephoneNo}", font, new SolidBrush(Color.Black), startX, startY + offset);
                offset += (int)fontHeight;
                if (_currentOrderDetails.OrderType.Equals("DELIVERY", StringComparison.OrdinalIgnoreCase))
                {
                    graphics.DrawString($"Address: {_currentOrderDetails.Address}", font, new SolidBrush(Color.Black), startX, startY + offset);
                    offset += (int)fontHeight;
                }
                offset += 10;

                // Order Items
                graphics.DrawString("--- Order Items ---", new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                offset += 20;

                foreach (var item in _currentOrderItems)
                {
                    graphics.DrawString($"{item.ItemName} x{item.Quantity} @ £{item.ItemPrice:0.00} (Extra: £{item.ExtraCharge:0.00})", font, new SolidBrush(Color.Black), startX, startY + offset);
                    offset += (int)fontHeight;
                }
                offset += 10;

                // Summary
                graphics.DrawString("--- Summary ---", new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                offset += 20;

                var presetCharges = _orderManager.GetPresetCharges(); // Assuming a method to get all preset charges
                foreach (var charge in presetCharges)
                {
                    if (charge.ChargeValue > 0)
                    {
                        graphics.DrawString($"{charge.ChargeName}: £{charge.ChargeValue:0.00}", font, new SolidBrush(Color.Black), startX, startY + offset);
                        offset += (int)fontHeight;
                    }
                }

                if (_currentOrderDetails.OrderType.Equals("DELIVERY", StringComparison.OrdinalIgnoreCase) && _currentOrderDetails.DeliveryCharge > 0)
                {
                    graphics.DrawString($"Delivery Charge: £{_currentOrderDetails.DeliveryCharge:0.00}", font, new SolidBrush(Color.Black), startX, startY + offset);
                    offset += (int)fontHeight;
                }

                graphics.DrawString($"Discount: {_currentOrderDetails.DiscountLabel} ({_currentOrderDetails.DiscountRate:P0})", font, new SolidBrush(Color.Black), startX, startY + offset);
                offset += (int)fontHeight;

                graphics.DrawString($"Total: £{_currentOrderDetails.TotalPrice:0.00}", new Font("Courier New", 12, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
                offset += (int)fontHeight;
            };

            try
            {
                pd.Print();
                _view.ShowMessage("Order printed successfully!", "Print Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Error printing order: " + ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void HandleEditCurrentOrder()
        {
            if (HasUnsavedChanges())
            {
                var result = _view.ShowConfirmation(
                    "You have unsaved changes. Do you want to save before editing in POS?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    HandleSave();
                    if (HasUnsavedChanges()) // Check if still unsaved after attempt
                    {
                        _view.ShowMessage("Failed to save changes. Cannot open in POS.", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Do not proceed to POS
                }
            }

            try
            {
                // Signal to the view to close itself and potentially trigger opening frmPOS
                _view.CloseView();
                // This part of navigation (opening frmPOS with orderId) should ideally be handled
                // by a higher-level application coordinator or the main entry point.
                // For now, I'll assume the parent form (e.g., frmOrderHistory) will handle it.
                // If this presenter is directly responsible for opening frmPOS, you'd need to
                // instantiate and show it here.
                // using (frmPOS posForm = new frmPOS(_currentOrderId))
                // {
                //     (this._view as Form)?.Hide(); // Hide current form if it's a Form
                //     posForm.ShowDialog();
                //     (this._view as Form)?.Show(); // Show current form again if needed
                // }
            }
            catch (Exception ex)
            {
                _view.ShowMessage("Error preparing order for editing: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool HasUnsavedChanges()
        {
            string currentOrderType = GetSelectedOrderTypeFromView();
            string currentPaymentType = GetSelectedPaymentTypeFromView();
            decimal currentTotalPrice = Convert.ToDecimal(_view.TotalPriceText); // Get from UI

            // Compare with original loaded values
            if (currentOrderType != _originalOrderType ||
                currentPaymentType != _originalPaymentType ||
                currentTotalPrice != _originalTotalPrice)
            {
                return true;
            }
            return false;
        }

        private string GetSelectedOrderTypeFromView()
        {
            if (_view.CollectionChecked) return "COLLECTION";
            if (_view.DeliveryChecked) return "DELIVERY";
            if (_view.OnlineChecked) return "ONLINE";
            if (_view.WaitingChecked) return "WAITING";
            return null;
        }

        private string GetSelectedPaymentTypeFromView()
        {
            if (_view.CashChecked) return "CASH";
            if (_view.CardChecked) return "CARD";
            if (_view.PendingChecked) return "PENDING";
            if (_view.CancelledChecked) return "CANCELLED";
            if (_view.RefundedChecked) return "REFUNDED";
            return null;
        }

        // Helper to get preset charges (should ideally be in a PresetChargeManager)
        private List<OrderManager.PresetChargeDto> GetPresetChargesFromDatabase()
        {
            // This method now calls the OrderManager to get preset charges
            return _orderManager.GetPresetCharges();
        }
    }
}
