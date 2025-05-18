using System;
using System.Windows.Forms;
using System.Drawing;

namespace OrderManagement.Model
{
    /// <summary>
    /// A minimal implementation for adding delivery charge functionality to the POS form
    /// </summary>
    public class MinimalDeliveryCharge
    {
        // UI controls
        private TextBox txtDeliveryCharge;
        private Label lblDeliveryCharge;
        
        // Constructor
        public MinimalDeliveryCharge(Form posForm)
        {
            try
            {
                // Create delivery charge label
                lblDeliveryCharge = new Label
                {
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(348, 40),
                    Name = "lblDeliveryCharge",
                    Size = new Size(168, 30),
                    Text = "Delivery Charge:",
                    Visible = false
                };

                // Create delivery charge textbox
                txtDeliveryCharge = new TextBox
                {
                    BorderStyle = BorderStyle.None,
                    Font = new Font("Segoe UI Black", 12F, FontStyle.Bold),
                    Location = new Point(520, 40),
                    Name = "txtDeliveryCharge",
                    Size = new Size(100, 30),
                    Text = "£0.00",
                    ReadOnly = true,
                    Visible = false
                };
                
                // Find the price panel
                Panel pricePanel = FindPricePanel(posForm);
                if (pricePanel != null)
                {
                    // Add controls to the panel
                    pricePanel.Controls.Add(lblDeliveryCharge);
                    pricePanel.Controls.Add(txtDeliveryCharge);
                    
                    // Find the delivery button
                    Button btnDel = FindButton(posForm, "btnDel");
                    if (btnDel != null)
                    {
                        // Add click handler to show delivery charge
                        btnDel.Click += (sender, e) => ShowDeliveryCharge(posForm);
                    }
                    
                    // Find other order type buttons
                    Button btnCol = FindButton(posForm, "btnCol");
                    if (btnCol != null)
                    {
                        btnCol.Click += (sender, e) => HideDeliveryCharge();
                    }
                    
                    Button btnWaiting = FindButton(posForm, "btnWaiting");
                    if (btnWaiting != null)
                    {
                        btnWaiting.Click += (sender, e) => HideDeliveryCharge();
                    }
                    
                    Button btnDineIn = FindButton(posForm, "btnDineIn");
                    if (btnDineIn != null)
                    {
                        btnDineIn.Click += (sender, e) => HideDeliveryCharge();
                    }
                }
            }
            catch
            {
                // Ignore errors
            }
        }
        
        // Find the price panel in the form
        private Panel FindPricePanel(Form form)
        {
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
                                    return subSubPanel;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        
        // Find a button in the form
        private Button FindButton(Form form, string buttonName)
        {
            foreach (Control control in form.Controls)
            {
                if (control is Panel panel && panel.Name == "panel1")
                {
                    foreach (Control subControl in panel.Controls)
                    {
                        if (subControl is Button button && button.Name == buttonName)
                        {
                            return button;
                        }
                    }
                }
            }
            return null;
        }
        
        // Show the delivery charge
        private void ShowDeliveryCharge(Form form)
        {
            try
            {
                // Get the postcode from the address display
                string postcode = GetPostcodeFromAddress(form);
                
                // Calculate the delivery charge
                decimal charge = CalculateDeliveryCharge(postcode);
                
                // Update the textbox
                txtDeliveryCharge.Text = $"£{charge:0.00}";
                
                // Make the controls visible
                lblDeliveryCharge.Visible = true;
                txtDeliveryCharge.Visible = true;
            }
            catch
            {
                // Ignore errors
            }
        }
        
        // Show the delivery charge for a specific postcode
        public void ShowDeliveryChargeForPostcode(string postcode)
        {
            decimal charge = CalculateDeliveryCharge(postcode);
            txtDeliveryCharge.Text = $"£{charge:0.00}";
            lblDeliveryCharge.Visible = true;
            txtDeliveryCharge.Visible = true;
        }
        
        // Hide the delivery charge
        private void HideDeliveryCharge()
        {
            lblDeliveryCharge.Visible = false;
            txtDeliveryCharge.Visible = false;
        }
        
        // Get the postcode from the address display
        private string GetPostcodeFromAddress(Form form)
        {
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
                                    // Extract the postcode from the address
                                    string address = textBox.Text;
                                    string[] parts = address.Split(',');
                                    if (parts.Length > 0)
                                    {
                                        return parts[parts.Length - 1].Trim();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
        
        // Calculate the delivery charge based on the postcode
        private decimal CalculateDeliveryCharge(string postcode)
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