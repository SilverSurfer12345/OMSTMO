using System;
using System.Windows.Forms;

namespace OrderManagement.Model
{
    public static class DeliveryChargeSetup
    {
        public static void Initialize(Form form)
        {
            try
            {
                // Find the delivery charge button
                Button btnDeliveryChargeAmend = null;
                foreach (Control control in form.Controls)
                {
                    if (control is Button button && button.Name == "btnDeliveryChargeAmend")
                    {
                        btnDeliveryChargeAmend = button;
                        break;
                    }
                }
                
                // Find the delivery charge textbox
                TextBox txtDeliveryCharge = null;
                foreach (Control control in form.Controls)
                {
                    if (control is Panel panel && panel.Name == "pnlBodyBottom")
                    {
                        foreach (Control subControl in panel.Controls)
                        {
                            if (subControl is Panel subPanel && subPanel.Name == "pnlOrderAction")
                            {
                                foreach (Control subSubControl in subPanel.Controls)
                                {
                                    if (subSubControl is Panel subSubPanel && subSubPanel.Name == "pnlPrice")
                                    {
                                        foreach (Control priceControl in subSubPanel.Controls)
                                        {
                                            if (priceControl is TextBox textBox && textBox.Name == "txtDeliveryCharge")
                                            {
                                                txtDeliveryCharge = textBox;
                                                break;
                                            }
                                        }
                                        if (txtDeliveryCharge != null) break;
                                    }
                                }
                                if (txtDeliveryCharge != null) break;
                            }
                        }
                        if (txtDeliveryCharge != null) break;
                    }
                }
                
                // Find the address display textbox
                TextBox addressDisplay = null;
                foreach (Control control in form.Controls)
                {
                    if (control is Panel panel && panel.Name == "panel1")
                    {
                        foreach (Control subControl in panel.Controls)
                        {
                            if (subControl is Panel subPanel && subPanel.Name == "pnlCstDetails")
                            {
                                foreach (Control subSubControl in subPanel.Controls)
                                {
                                    if (subSubControl is TextBox textBox && textBox.Name == "lblAddressDisplay")
                                    {
                                        addressDisplay = textBox;
                                        break;
                                    }
                                }
                                if (addressDisplay != null) break;
                            }
                        }
                        if (addressDisplay != null) break;
                    }
                }
                
                // Set up the delivery charge button
                if (btnDeliveryChargeAmend != null && txtDeliveryCharge != null && addressDisplay != null)
                {
                    SimpleDeliveryCharge.SetupButton(btnDeliveryChargeAmend, txtDeliveryCharge, addressDisplay);
                }
            }
            catch
            {
                // Ignore errors
            }
        }
    }
}