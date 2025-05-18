namespace OrderManagement.View
{
    partial class frmPayment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCash = new System.Windows.Forms.Button();
            this.pnlPayment = new System.Windows.Forms.Panel();
            this.lblCustomerToPay = new System.Windows.Forms.Label();
            this.lblPaymentAmount = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.pnlPaymentOptions = new System.Windows.Forms.Panel();
            this.btnRefund = new System.Windows.Forms.Button();
            this.btnCard = new System.Windows.Forms.Button();
            this.lblSelectedCustomer = new System.Windows.Forms.Label();
            this.lblPaymentOption = new System.Windows.Forms.Label();
            this.lblSelectedOption = new System.Windows.Forms.Label();
            this.pnlPaymentTopPanel = new System.Windows.Forms.Panel();
            this.lblPaymentTitle = new System.Windows.Forms.Label();
            this.picPOS = new System.Windows.Forms.PictureBox();
            this.btnPending = new System.Windows.Forms.Button();
            this.pnlPayment.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlPaymentOptions.SuspendLayout();
            this.pnlPaymentTopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPOS)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCash
            // 
            this.btnCash.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnCash.ForeColor = System.Drawing.Color.Black;
            this.btnCash.Location = new System.Drawing.Point(44, 17);
            this.btnCash.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCash.Name = "btnCash";
            this.btnCash.Size = new System.Drawing.Size(144, 57);
            this.btnCash.TabIndex = 37;
            this.btnCash.Text = "CASH";
            this.btnCash.UseVisualStyleBackColor = false;
            this.btnCash.Click += new System.EventHandler(this.btnCash_Click);
            // 
            // pnlPayment
            // 
            this.pnlPayment.Controls.Add(this.lblCustomerToPay);
            this.pnlPayment.Controls.Add(this.lblPaymentAmount);
            this.pnlPayment.Controls.Add(this.lblTotalAmount);
            this.pnlPayment.Controls.Add(this.panel1);
            this.pnlPayment.Controls.Add(this.pnlPaymentOptions);
            this.pnlPayment.Controls.Add(this.lblSelectedCustomer);
            this.pnlPayment.Controls.Add(this.lblPaymentOption);
            this.pnlPayment.Controls.Add(this.lblSelectedOption);
            this.pnlPayment.Controls.Add(this.pnlPaymentTopPanel);
            this.pnlPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPayment.Location = new System.Drawing.Point(0, 0);
            this.pnlPayment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlPayment.Name = "pnlPayment";
            this.pnlPayment.Size = new System.Drawing.Size(1200, 692);
            this.pnlPayment.TabIndex = 38;
            // 
            // lblCustomerToPay
            // 
            this.lblCustomerToPay.AutoSize = true;
            this.lblCustomerToPay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerToPay.Location = new System.Drawing.Point(291, 286);
            this.lblCustomerToPay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomerToPay.Name = "lblCustomerToPay";
            this.lblCustomerToPay.Size = new System.Drawing.Size(143, 32);
            this.lblCustomerToPay.TabIndex = 51;
            this.lblCustomerToPay.Text = "John Doe";
            // 
            // lblPaymentAmount
            // 
            this.lblPaymentAmount.AutoSize = true;
            this.lblPaymentAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentAmount.Location = new System.Drawing.Point(300, 417);
            this.lblPaymentAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPaymentAmount.Name = "lblPaymentAmount";
            this.lblPaymentAmount.Size = new System.Drawing.Size(91, 32);
            this.lblPaymentAmount.TabIndex = 50;
            this.lblPaymentAmount.Text = "£0.00";
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(42, 417);
            this.lblTotalAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(216, 32);
            this.lblTotalAmount.TabIndex = 49;
            this.lblTotalAmount.Text = "Amount To Pay:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnConfirm);
            this.panel1.Location = new System.Drawing.Point(898, 615);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(297, 68);
            this.panel1.TabIndex = 48;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(4, 11);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 57);
            this.btnCancel.TabIndex = 48;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnConfirm.ForeColor = System.Drawing.Color.Black;
            this.btnConfirm.Location = new System.Drawing.Point(158, 11);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(144, 57);
            this.btnConfirm.TabIndex = 47;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // pnlPaymentOptions
            // 
            this.pnlPaymentOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPaymentOptions.Controls.Add(this.btnPending);
            this.pnlPaymentOptions.Controls.Add(this.btnRefund);
            this.pnlPaymentOptions.Controls.Add(this.btnCash);
            this.pnlPaymentOptions.Controls.Add(this.btnCard);
            this.pnlPaymentOptions.Location = new System.Drawing.Point(4, 609);
            this.pnlPaymentOptions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlPaymentOptions.Name = "pnlPaymentOptions";
            this.pnlPaymentOptions.Size = new System.Drawing.Size(699, 78);
            this.pnlPaymentOptions.TabIndex = 46;
            // 
            // btnRefund
            // 
            this.btnRefund.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRefund.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnRefund.ForeColor = System.Drawing.Color.Black;
            this.btnRefund.Location = new System.Drawing.Point(537, 17);
            this.btnRefund.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRefund.Name = "btnRefund";
            this.btnRefund.Size = new System.Drawing.Size(144, 57);
            this.btnRefund.TabIndex = 40;
            this.btnRefund.Text = "REFUND";
            this.btnRefund.UseVisualStyleBackColor = false;
            // 
            // btnCard
            // 
            this.btnCard.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnCard.ForeColor = System.Drawing.Color.Black;
            this.btnCard.Location = new System.Drawing.Point(196, 17);
            this.btnCard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCard.Name = "btnCard";
            this.btnCard.Size = new System.Drawing.Size(144, 57);
            this.btnCard.TabIndex = 38;
            this.btnCard.Text = "CARD";
            this.btnCard.UseVisualStyleBackColor = false;
            this.btnCard.Click += new System.EventHandler(this.btnCard_Click);
            // 
            // lblSelectedCustomer
            // 
            this.lblSelectedCustomer.AutoSize = true;
            this.lblSelectedCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedCustomer.Location = new System.Drawing.Point(42, 286);
            this.lblSelectedCustomer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectedCustomer.Name = "lblSelectedCustomer";
            this.lblSelectedCustomer.Size = new System.Drawing.Size(240, 32);
            this.lblSelectedCustomer.TabIndex = 44;
            this.lblSelectedCustomer.Text = "Customer To Pay:";
            // 
            // lblPaymentOption
            // 
            this.lblPaymentOption.AutoSize = true;
            this.lblPaymentOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentOption.Location = new System.Drawing.Point(300, 478);
            this.lblPaymentOption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPaymentOption.Name = "lblPaymentOption";
            this.lblPaymentOption.Size = new System.Drawing.Size(153, 32);
            this.lblPaymentOption.TabIndex = 43;
            this.lblPaymentOption.Text = "NOT PAID";
            // 
            // lblSelectedOption
            // 
            this.lblSelectedOption.AutoSize = true;
            this.lblSelectedOption.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedOption.Location = new System.Drawing.Point(42, 478);
            this.lblSelectedOption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectedOption.Name = "lblSelectedOption";
            this.lblSelectedOption.Size = new System.Drawing.Size(236, 32);
            this.lblSelectedOption.TabIndex = 42;
            this.lblSelectedOption.Text = "Payment Method:";
            // 
            // pnlPaymentTopPanel
            // 
            this.pnlPaymentTopPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.pnlPaymentTopPanel.Controls.Add(this.lblPaymentTitle);
            this.pnlPaymentTopPanel.Controls.Add(this.picPOS);
            this.pnlPaymentTopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPaymentTopPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlPaymentTopPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlPaymentTopPanel.Name = "pnlPaymentTopPanel";
            this.pnlPaymentTopPanel.Size = new System.Drawing.Size(1200, 234);
            this.pnlPaymentTopPanel.TabIndex = 41;
            // 
            // lblPaymentTitle
            // 
            this.lblPaymentTitle.AutoSize = true;
            this.lblPaymentTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentTitle.ForeColor = System.Drawing.Color.White;
            this.lblPaymentTitle.Location = new System.Drawing.Point(252, 62);
            this.lblPaymentTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPaymentTitle.Name = "lblPaymentTitle";
            this.lblPaymentTitle.Size = new System.Drawing.Size(192, 60);
            this.lblPaymentTitle.TabIndex = 1;
            this.lblPaymentTitle.Text = "Payment";
            // 
            // picPOS
            // 
            this.picPOS.Image = global::OrderManagement.Properties.Resources.pos_terminal_icon;
            this.picPOS.Location = new System.Drawing.Point(12, 12);
            this.picPOS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picPOS.Name = "picPOS";
            this.picPOS.Size = new System.Drawing.Size(228, 197);
            this.picPOS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPOS.TabIndex = 0;
            this.picPOS.TabStop = false;
            // 
            // btnPending
            // 
            this.btnPending.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPending.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnPending.ForeColor = System.Drawing.Color.Black;
            this.btnPending.Location = new System.Drawing.Point(348, 17);
            this.btnPending.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPending.Name = "btnPending";
            this.btnPending.Size = new System.Drawing.Size(144, 57);
            this.btnPending.TabIndex = 41;
            this.btnPending.Text = "PENDING";
            this.btnPending.UseVisualStyleBackColor = false;
            this.btnPending.Click += new System.EventHandler(this.btnPending_Click);
            // 
            // frmPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.ControlBox = false;
            this.Controls.Add(this.pnlPayment);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmPayment";
            this.Text = "frmPayment";
            this.pnlPayment.ResumeLayout(false);
            this.pnlPayment.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlPaymentOptions.ResumeLayout(false);
            this.pnlPaymentTopPanel.ResumeLayout(false);
            this.pnlPaymentTopPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPOS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btnCash;
        public System.Windows.Forms.Panel pnlPayment;
        public System.Windows.Forms.Button btnCard;
        public System.Windows.Forms.Panel pnlPaymentTopPanel;
        public System.Windows.Forms.Label lblPaymentTitle;
        public System.Windows.Forms.PictureBox picPOS;
        public System.Windows.Forms.Label lblSelectedCustomer;
        public System.Windows.Forms.Label lblPaymentOption;
        public System.Windows.Forms.Label lblSelectedOption;
        public System.Windows.Forms.Panel pnlPaymentOptions;
        public System.Windows.Forms.Button btnConfirm;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label lblPaymentAmount;
        public System.Windows.Forms.Label lblTotalAmount;
        public System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Label lblCustomerToPay;
        public System.Windows.Forms.Button btnRefund;
        public System.Windows.Forms.Button btnPending;
    }
}