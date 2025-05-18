using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public partial class frmCallerView : Form
    {
        // Define a property to get the entered telephone number
        public string EnteredTelephoneNo
        {
            get { return txtCustomerDetails.Text.Trim(); }
        }

        // Define an event to be called when the OK button is clicked
        public event EventHandler OkClicked;

        public frmCallerView()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // When the OK button is clicked, raise the OkClicked event
            OkClicked?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
