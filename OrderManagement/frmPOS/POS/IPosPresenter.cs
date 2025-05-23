// IPosPresenter.cs
using System;
using System.Windows.Forms;

namespace OrderManagement.Presenter
{
    public interface IPosPresenter
    {
        void Initialize();
        void HandleViewLoaded();
        void HandleFormClosing(FormClosingEventArgs e);
        void HandleBackToMain();
        void HandleNewOrder();
        void HandleSaveOrder();
        void HandlePayment();
        void HandleKotPrint();
        void HandleClearBasket();
        void HandleCustomerAction();
        void HandleCallMe();
        void HandleBill();
        void HandleNotificationConfig();
        void HandleAutoAddPresetCharges();
        void HandleDeliveryChargeAmend();
        void HandleSearchFoodItemTextChange(string searchText);
        void HandleCustomerTelephoneTextChange(string telephoneNo);
        void HandleCustomerDetailsTextChange(string customerDetails);
        void HandleCategorySelected(string category);
        void HandleBasketCellContentClick(int rowIndex, string columnName);
        void HandleCustomerSearchItemSelected(int selectedIndex);
        void HandleDiscountSelected(string selectedDiscount);
        void HandleOrderAlertTimerTick();
        void HandleAlertTimerTick();
        void AddItemToBasket(string itemName, decimal itemPrice); // Method for food item buttons
    }
}