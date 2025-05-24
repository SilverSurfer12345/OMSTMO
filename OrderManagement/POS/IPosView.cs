// IPosView.cs
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public interface IPosView
    {
        // Properties for displaying data (Presenter sets these)
        string CustomerDetailsText { get; set; }
        string CustomerTelephoneText { get; set; }
        string AddressDisplayText { get; set; }
        string PreviousOrdersText { get; set; }
        string TotalPriceText { get; set; }
        string PaymentOptionText { get; set; }
        string SaveOrderButtonText { get; set; }
        bool IsDeliveryChargeVisible { get; set; }
        bool IsDeliveryChargeAmendButtonVisible { get; set; }
        bool IsCustomerAddressVisible { get; set; }
        bool IsAddressDisplayVisible { get; set; }
        string CustomerActionButtonText { get; set; }
        int SelectedDiscountIndex { get; set; }
        Color CurrentOrdersButtonBackColor { get; set; }
        Color CurrentOrdersButtonForeColor { get; set; }
        string DeliveryChargeText { get; set; }

        // Methods for displaying/updating UI (Presenter calls these)
        void ClearBasket();
        void AddBasketItem(string itemName, decimal originalPrice, decimal price, int quantity, decimal extraCharge);
        void UpdateBasketItemQuantity(int rowIndex, int quantity);
        void UpdateBasketItemPriceAndExtraCharge(int rowIndex, decimal originalPrice, decimal newExtraCharge);
        void RemoveBasketItem(int rowIndex);
        void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        DialogResult ShowConfirmation(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        void LoadCategories(IEnumerable<string> categories);
        void LoadFoodItems(IEnumerable<FoodItemDisplayDto> items);
        void LoadPreviousOrderButtons(IEnumerable<PreviousOrderItemDisplayDto> items);
        void ClearCustomerSearchList();
        void AddCustomerSearchItem(string customerDetails);
        void SetCustomerSearchListVisibility(bool visible);
        void SetCustomerSearchListWidth(int width);
        void SetOrderTypeButtonColor(string orderType, Color color);
        void ResetAllOrderTypeButtonsColor(Color defaultColor);
        void HideCustomerAddressFields();
        void ShowCustomerAddressFields();
        void CloseView(); // To allow Presenter to close the view

        // Events for user actions (View raises these, Presenter subscribes)
        event System.EventHandler ViewLoaded;
        event System.EventHandler FormClosingConfirmed;
        event System.EventHandler BackToMainClicked;
        event System.EventHandler NewOrderClicked;
        event System.EventHandler SaveOrderClicked;
        event System.EventHandler PaymentClicked;
        event System.EventHandler KotClicked;
        event System.EventHandler ClearBasketClicked;
        event System.EventHandler CustomerActionClicked;
        event System.EventHandler CallMeClicked;
        event System.EventHandler BillClicked;
        event System.EventHandler NotificationConfigClicked;
        event System.EventHandler AutoAddPresetClicked;
        event System.EventHandler DeliveryChargeAmendClicked;
        event System.EventHandler<string> SearchFoodItemTextChanged;
        event System.EventHandler<string> CustomerTelephoneTextChanged;
        event System.EventHandler<string> CustomerDetailsTextChanged;
        event System.EventHandler<string> CategorySelected;
        event System.EventHandler<BasketItemInteractionEventArgs> BasketCellContentClicked;
        event System.EventHandler<int> CustomerSearchItemSelected;
        event System.EventHandler<string> DiscountSelected;
        event System.EventHandler OrderAlertTimerTick;
        event System.EventHandler AlertTimerTick;
        event System.EventHandler<string> OrderTypeChanged;
    }

    // DTOs for displaying data
    public class FoodItemDisplayDto
    {
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public byte[] ImageData { get; set; }
    }

    public class PreviousOrderItemDisplayDto
    {
        public string ItemName { get; set; }
        public decimal ItemPrice { get; set; }
        public byte[] ImageData { get; set; } // For previous items too
    }

    public class BasketItemInteractionEventArgs : System.EventArgs
    {
        public int RowIndex { get; }
        public string ColumnName { get; }

        public BasketItemInteractionEventArgs(int rowIndex, string columnName)
        {
            RowIndex = rowIndex;
            ColumnName = columnName;
        }
    }
}