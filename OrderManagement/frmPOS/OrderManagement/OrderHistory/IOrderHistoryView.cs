using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OrderManagement.View
{
    // New EventArgs class to pass detailed click information
    public class OrderHistoryCellClickEventArgs : EventArgs
    {
        public int RowIndex { get; }
        public int ColumnIndex { get; }
        public string ColumnName { get; }
        public int OrderId { get; }

        public OrderHistoryCellClickEventArgs(int rowIndex, int columnIndex, string columnName, int orderId)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            ColumnName = columnName;
            OrderId = orderId;
        }
    }

    public interface IOrderHistoryView
    {
        // Properties for displaying data
        object OrderHistoryDataSource { get; set; }
        string SelectedOrderType { get; set; }
        string SelectedPaymentType { get; set; }
        string SearchText { get; set; }
        DateTime FromDate { get; set; }
        DateTime ToDate { get; set; }
        string TotalValueText { get; set; }

        // Methods for UI interaction
        void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        DialogResult ShowConfirmation(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        void CloseView();
        void RefreshOrderHistoryGrid();
        void SetOrderTypeItems(IEnumerable<string> items);
        void SetPaymentTypeItems(IEnumerable<string> items);
        void SetDateRangeWithoutEvents(DateTime from, DateTime to);
        void SetColumnOrder(string[] columnOrder);
        void ShowCalculationsForm(Form calculationsForm);

        // New method to open the order view form
        void ShowOrderViewForm(int orderId);

        // Events for user actions
        event EventHandler ViewLoaded;
        event EventHandler OrderTypeFilterChanged;
        event EventHandler PaymentTypeFilterChanged;
        event EventHandler SearchBoxTextChanged;
        event EventHandler FromDateChanged;
        event EventHandler ToDateChanged;
        event EventHandler TodayClicked;
        event EventHandler DownloadClicked;
        event EventHandler PrintClicked;
        event EventHandler ResetFiltersClicked;

        // Modified event signature to use the custom EventArgs
        event EventHandler<OrderHistoryCellClickEventArgs> CellContentClicked;
        event EventHandler CalculationsClicked;
        event FormClosingEventHandler FormClosingConfirmed;
    }
}
