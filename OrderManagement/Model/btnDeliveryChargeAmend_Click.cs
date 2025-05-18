using System;
using System.Windows.Forms;

namespace OrderManagement.Model
{
    public static class DeliveryChargeCalculator
    {
        public static void btnDeliveryChargeAmend_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the button that was clicked
                Button button = sender as Button;
                if (button == null) return;

                // Get the form that contains the button
                Form form = button.FindForm();
                if (form == null) return;

                // Find the delivery charge textbox
                TextBox txtDeliveryCharge = null;
                foreach (Control control in form.Controls)
                {
                    if (control is TextBox textBox && textBox.Name == "txtDeliveryCharge")
                    {
                        txtDeliveryCharge = textBox;
                        break;
                    }

                    // Also search in panels
                    if (control is Panel panel)
                    {
                        foreach (Control panelControl in panel.Controls)
                        {
                            if (panelControl is TextBox panelTextBox && panelTextBox.Name == "txtDeliveryCharge")
                            {
                                txtDeliveryCharge = panelTextBox;
                                break;
                            }

                            // Search in nested panels
                            if (panelControl is Panel nestedPanel)
                            {
                                foreach (Control nestedControl in nestedPanel.Controls)
                                {
                                    if (nestedControl is TextBox nestedTextBox && nestedTextBox.Name == "txtDeliveryCharge")
                                    {
                                        txtDeliveryCharge = nestedTextBox;
                                        break;
                                    }
                                }
                                if (txtDeliveryCharge != null) break;
                            }
                        }
                        if (txtDeliveryCharge != null) break;
                    }
                }

                if (txtDeliveryCharge == null)
                {
                    MessageBox.Show("Could not find txtDeliveryCharge control.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
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

                if (addressDisplay == null)
                {
                    MessageBox.Show("Could not find address display control.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Extract postcode from address
                string postcode = ExtractPostcode(addressDisplay.Text);

                // Calculate delivery charge
                decimal charge = CalculateDeliveryCharge(postcode);

                // Update the delivery charge textbox
                txtDeliveryCharge.Text = $"£{charge:0.00}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating delivery charge: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string ExtractPostcode(string address)
        {
            if (string.IsNullOrEmpty(address))
                return string.Empty;

            string[] parts = address.Split(',');
            if (parts.Length > 0)
            {
                return parts[parts.Length - 1].Trim();
            }

            return string.Empty;
        }

        public static decimal CalculateDeliveryCharge(string postcode)
        {
            // Default charge
            decimal baseCharge = 2.50m;

            if (string.IsNullOrWhiteSpace(postcode))
                return baseCharge;

            // Clean up the postcode
            postcode = postcode.ToUpper().Replace(" ", "");

            // Check for specific postcode areas
            if (postcode.StartsWith("NE16"))
                return 2.50m;
            if (postcode.StartsWith("NE15") || postcode.StartsWith("NE17"))
                return 3.00m;
            if (postcode.StartsWith("NE11") || postcode.StartsWith("NE21") || postcode.StartsWith("NE40"))
                return 3.50m;
            if (postcode.StartsWith("NE8") || postcode.StartsWith("NE9") || postcode.StartsWith("NE41") || postcode.StartsWith("NE42"))
                return 4.00m;
            if (postcode.StartsWith("NE10"))
                return 4.50m;
            if (postcode.StartsWith("NE1") || postcode.StartsWith("NE2") || postcode.StartsWith("NE4") || postcode.StartsWith("NE20"))
                return 5.00m;
            if (postcode.StartsWith("NE3") || postcode.StartsWith("NE5"))
                return 5.50m;
            if (postcode.StartsWith("NE6") || postcode.StartsWith("NE7") || postcode.StartsWith("NE28"))
                return 6.00m;
            if (postcode.StartsWith("NE12"))
                return 6.50m;
            if (postcode.StartsWith("NE13") || postcode.StartsWith("NE29"))
                return 7.00m;
            if (postcode.StartsWith("NE22") || postcode.StartsWith("NE30"))
                return 7.50m;
            if (postcode.StartsWith("DH"))
                return 8.50m;
            if (postcode.StartsWith("SR"))
                return 9.00m;
            if (postcode.StartsWith("TS"))
                return 10.00m;

            // Default to maximum charge if no match found
            return 10.00m;
        }
    }
}
