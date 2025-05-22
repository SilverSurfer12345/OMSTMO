using System;
using System.Drawing;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public class frmExtraChargePopup : Form
    {
        public decimal SelectedAmount { get; private set; }
        private TextBox txtCustom;
        private Button btnApply;
        private decimal quickButtonAmount = 0;
        private bool quickButtonClicked = false;

        public frmExtraChargePopup()
        {
            this.Text = "Add Extra Charge";
            this.Size = new Size(320, 260);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 120,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10)
            };

            // Add quick amount buttons
            for (decimal d = 0.25m; d <= 3.00m; d += 0.25m)
            {
                decimal buttonValue = d; // capture the value for closure
                var btn = new Button
                {
                    Text = buttonValue.ToString("0.00"),
                    Width = 60,
                    Height = 40,
                    Margin = new Padding(5)
                };
                btn.Click += (s, e) =>
                {
                    quickButtonAmount = buttonValue;
                    quickButtonClicked = true;
                    txtCustom.Text = ""; // Clear custom input
                    this.DialogResult = DialogResult.OK; // Close and signal selection
                };
                panel.Controls.Add(btn);
            }

            // Custom input
            var lblCustom = new Label
            {
                Text = "Custom Amount:",
                AutoSize = true,
                Top = panel.Bottom + 10,
                Left = 10
            };
            txtCustom = new TextBox
            {
                Width = 100,
                Top = lblCustom.Bottom + 5,
                Left = 10
            };

            // Apply button
            btnApply = new Button
            {
                Text = "Apply",
                Width = 80,
                Top = txtCustom.Bottom + 10,
                Left = 10
            };
            btnApply.Click += (s, e) =>
            {
                if (decimal.TryParse(txtCustom.Text, out decimal val) && val >= 0)
                {
                    SelectedAmount = val;
                    quickButtonClicked = false;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Please enter a valid amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            // Layout
            var mainPanel = new Panel { Dock = DockStyle.Fill };
            panel.Top = 10;
            mainPanel.Controls.Add(panel);
            lblCustom.Top = panel.Bottom + 10;
            mainPanel.Controls.Add(lblCustom);
            txtCustom.Top = lblCustom.Bottom + 5;
            mainPanel.Controls.Add(txtCustom);
            btnApply.Top = txtCustom.Bottom + 10;
            mainPanel.Controls.Add(btnApply);

            this.Controls.Add(mainPanel);
        }

        // Helper to get the selected amount
        public decimal GetSelectedAmount()
        {
            return quickButtonClicked ? quickButtonAmount : SelectedAmount;
        }
        
        // Set the current extra charge value
        public void SetCurrentExtraCharge(decimal currentExtraCharge)
        {
            // If it matches one of our quick buttons, highlight that button
            foreach (Control control in Controls[0].Controls[0].Controls)
            {
                if (control is Button btn && decimal.TryParse(btn.Text, out decimal btnValue))
                {
                    if (btnValue == currentExtraCharge)
                    {
                        quickButtonAmount = currentExtraCharge;
                        quickButtonClicked = true;
                        return;
                    }
                }
            }
            
            // Otherwise set it in the custom field
            txtCustom.Text = currentExtraCharge.ToString("0.00");
            SelectedAmount = currentExtraCharge;
            quickButtonClicked = false;
        }
    }
}