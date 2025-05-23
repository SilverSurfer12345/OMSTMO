using System;
using System.Windows.Forms;

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
        void HandleCellContentClick(DataGridViewCellEventArgs e);
        void HandleCalculationsClicked();
        void HandleFormClosing(FormClosingEventArgs e);
    }
}
