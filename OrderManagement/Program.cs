using System;
using System.Windows.Forms;
using OrderManagement.Model;

namespace OrderManagement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Start with POS form directly for testing
            frmPOS posForm = new frmPOS();
            
            // Add delivery charge functionality
            posForm.Load += (sender, e) => {
                try
                {
                }
                catch
                {
                    // Ignore errors
                }
            };
            
            Application.Run(posForm);
        }
    }
}