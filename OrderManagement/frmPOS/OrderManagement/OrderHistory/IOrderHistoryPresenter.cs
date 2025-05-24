using System;
using System.Windows.Forms;
using OrderManagement.View; // Needed for OrderHistoryCellClickEventArgs

namespace OrderManagement.Presenter
{
    public interface IOrderHistoryPresenter
    {
        void Initialize();
        void HandleViewLoaded();
        void HandleOrderTypeFilterChanged();
        void HandlePaymentTypeFilterChanged();
        void HandleSearchBoxTextChanged();
        void HandleFromDateChanged();
        void HandleToDateChanged();
        void HandleTodayClicked();
        void HandleDownloadClicked();
        void HandlePrintClicked();
        void HandleResetFiltersClicked();
        // Modified signature to use the custom EventArgs
        void HandleCellContentClick(OrderHistoryCellClickEventArgs e);
        void HandleCalculationsClicked();
        void HandleFormClosing(FormClosingEventArgs e);
    }
}
