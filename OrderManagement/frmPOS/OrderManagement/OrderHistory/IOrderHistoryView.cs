using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OrderManagement.View
{
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
        void ShowCalculationsForm(Form calculationsForm); // To show the new calculations form

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
        event EventHandler<DataGridViewCellEventArgs> CellContentClicked;
        event EventHandler CalculationsClicked;
        event FormClosingEventHandler FormClosingConfirmed;
    }
}
