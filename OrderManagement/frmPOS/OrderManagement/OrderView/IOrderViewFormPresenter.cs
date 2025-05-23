using System;
using System.Windows.Forms;

namespace OrderManagement.Presenter
{
    public interface IOrderViewFormPresenter
    {
        void Initialize(int orderId);
        void HandleViewLoaded();
        void HandleSave();
        void HandleClose();
        void HandleOrderTypeChange();
        void HandlePaymentTypeChange();
        void HandlePreviewInvoice();
        void HandlePrint();
        void HandleEditCurrentOrder();
    }
}
