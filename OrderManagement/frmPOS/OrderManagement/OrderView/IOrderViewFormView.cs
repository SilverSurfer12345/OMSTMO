using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public interface IOrderViewFormView
    {
        // Properties to display order and customer details
        string CustomerNameText { get; set; }
        string CustomerTelephoneText { get; set; }
        string CustomerAddressText { get; set; }
        string OrderDateText { get; set; }
        string TotalPriceText { get; set; }
        bool CollectionChecked { get; set; }
        bool DeliveryChecked { get; set; }
        bool OnlineChecked { get; set; }
        bool WaitingChecked { get; set; }
        bool CashChecked { get; set; }
        bool CardChecked { get; set; }
        bool PendingChecked { get; set; }
        bool CancelledChecked { get; set; }
        bool RefundedChecked { get; set; }
        bool ChangesMadeVisible { get; set; }
        bool AddressVisible { get; set; }

        // Data source for order items DataGridView
        object OrderItemsDataSource { get; set; }

        // Methods for UI interaction
        void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        DialogResult ShowConfirmation(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        void CloseView();

        // Events for user actions
        event EventHandler ViewLoaded;
        event EventHandler SaveClicked;
        event EventHandler CloseClicked;
        event EventHandler OrderTypeChanged;
        event EventHandler PaymentTypeChanged;
        event EventHandler PreviewInvoiceClicked;
        event EventHandler PrintClicked;
        event EventHandler EditCurrentOrderClicked;
    }
}
