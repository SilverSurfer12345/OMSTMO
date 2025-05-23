// Program.cs (Conceptual)
using OrderManagement.Presenter;
using OrderManagement.View;
using System;
using System.Windows.Forms;

namespace OrderManagement
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Example of how to launch frmPOS
            // For a new order:
            frmPOS posForm = new frmPOS();
            Application.Run(posForm);

            // For editing an existing order (e.g., from frmMain or frmOrderHistory):
            // int orderIdToEdit = 123; // Get this from user selection or another form
            // frmPOS posEditForm = new frmPOS(orderIdToEdit);
            // Application.Run(posEditForm);
        }
    }
}