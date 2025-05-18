using System;
using System.Drawing;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public class frmExtraCharge : Form
    {
        public decimal ExtraValue { get; private set; }
        private TextBox txtManual;
        private Button btnOk;

        public frmExtraCharge()
        {
            this.Text = "Add Extra Charge";
            this.Size = new Size(350, 180);
            this.StartPosition = FormStartPosition.CenterParent;

            var flp = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10)
            };

            for (decimal d = 0.25m; d <= 3.00m; d += 0.25m)
            {
                var btn = new Button
                {
                    Text = $"£{d:0.00}",
                    Width = 60,
                    Height = 35,
                    Margin = new Padding(5)
                };
                decimal value = d;
                btn.Click += (s, e) =>
                {
                    ExtraValue = value;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                };
                flp.Controls.Add(btn);
            }

            var lbl = new Label
            {
                Text = "Manual Entry:",
                AutoSize = true,
                Margin = new Padding(10, 10, 0, 0)
            };
            txtManual = new TextBox
            {
                Width = 80,
                Margin = new Padding(5, 10, 0, 0)
            };
            btnOk = new Button
            {
                Text = "OK",
                Width = 50,
                Height = 28,
                Margin = new Padding(10, 10, 0, 0)
            };
            btnOk.Click += (s, e) =>
            {
                if (decimal.TryParse(txtManual.Text, out decimal val) && val >= 0)
                {
                    ExtraValue = val;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a valid amount.", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            var manualPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true
            };
            manualPanel.Controls.Add(lbl);
            manualPanel.Controls.Add(txtManual);
            manualPanel.Controls.Add(btnOk);

            this.Controls.Add(flp);
            this.Controls.Add(manualPanel);
        }
    }
}