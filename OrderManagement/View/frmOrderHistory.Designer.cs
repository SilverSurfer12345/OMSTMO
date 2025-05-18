namespace OrderManagement.View
{
    partial class frmOrderHistory
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlGridHolder = new System.Windows.Forms.Panel();
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPaymentType = new System.Windows.Forms.ComboBox();
            this.cmbOrderType = new System.Windows.Forms.ComboBox();
            this.dgvOrderHistoryView = new System.Windows.Forms.DataGridView();
            this.dgvOrderId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOrderType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOrderDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvView = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvCancel = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnCalculations = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotalValue = new System.Windows.Forms.TextBox();
            this.btnToday = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.pnlGridHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderHistoryView)).BeginInit();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.pnlHeader.Controls.Add(this.label1);
            this.pnlHeader.Controls.Add(this.pictureBox1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1499, 126);
            this.pnlHeader.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(152, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 60);
            this.label1.TabIndex = 1;
            this.label1.Text = "Order History";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OrderManagement.Properties.Resources.map_search_icon1;
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 92);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 949);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1499, 82);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(1336, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(157, 63);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlGridHolder
            // 
            this.pnlGridHolder.Controls.Add(this.txtSearchBox);
            this.pnlGridHolder.Controls.Add(this.label7);
            this.pnlGridHolder.Controls.Add(this.label3);
            this.pnlGridHolder.Controls.Add(this.label2);
            this.pnlGridHolder.Controls.Add(this.cmbPaymentType);
            this.pnlGridHolder.Controls.Add(this.cmbOrderType);
            this.pnlGridHolder.Controls.Add(this.dgvOrderHistoryView);
            this.pnlGridHolder.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlGridHolder.Location = new System.Drawing.Point(0, 126);
            this.pnlGridHolder.Name = "pnlGridHolder";
            this.pnlGridHolder.Size = new System.Drawing.Size(1096, 823);
            this.pnlGridHolder.TabIndex = 3;
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchBox.Location = new System.Drawing.Point(776, 16);
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(288, 34);
            this.txtSearchBox.TabIndex = 6;
            this.txtSearchBox.TextChanged += new System.EventHandler(this.txtSearchBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(704, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 21);
            this.label7.TabIndex = 5;
            this.label7.Text = "Search";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(376, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Payment";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Order Type";
            // 
            // cmbPaymentType
            // 
            this.cmbPaymentType.Font = new System.Drawing.Font("Segoe UI Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPaymentType.FormattingEnabled = true;
            this.cmbPaymentType.Items.AddRange(new object[] {
            "All",
            "CASH",
            "CARD",
            "NOT PAID",
            "CANCELLED",
            "PENDING"});
            this.cmbPaymentType.Location = new System.Drawing.Point(456, 16);
            this.cmbPaymentType.Name = "cmbPaymentType";
            this.cmbPaymentType.Size = new System.Drawing.Size(216, 29);
            this.cmbPaymentType.TabIndex = 2;
            this.cmbPaymentType.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentType_SelectedIndexChanged);
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.Font = new System.Drawing.Font("Segoe UI Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOrderType.FormattingEnabled = true;
            this.cmbOrderType.Items.AddRange(new object[] {
            "All",
            "Collection",
            "Waiting",
            "Delivery",
            "Online",
            "Restaurant",
            "Cancelled"});
            this.cmbOrderType.Location = new System.Drawing.Point(120, 16);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new System.Drawing.Size(216, 29);
            this.cmbOrderType.TabIndex = 1;
            this.cmbOrderType.SelectedIndexChanged += new System.EventHandler(this.cmbOrderType_SelectedIndexChanged);
            // 
            // dgvOrderHistoryView
            // 
            this.dgvOrderHistoryView.AllowUserToAddRows = false;
            this.dgvOrderHistoryView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrderHistoryView.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrderHistoryView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderHistoryView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvOrderId,
            this.dgvName,
            this.dgvAddress,
            this.dgvOrderType,
            this.dgvOrderDate,
            this.dgvTotalPrice,
            this.dgvPayment,
            this.dgvView,
            this.dgvCancel});
            this.dgvOrderHistoryView.GridColor = System.Drawing.Color.White;
            this.dgvOrderHistoryView.Location = new System.Drawing.Point(8, 64);
            this.dgvOrderHistoryView.Name = "dgvOrderHistoryView";
            this.dgvOrderHistoryView.RowHeadersVisible = false;
            this.dgvOrderHistoryView.RowHeadersWidth = 62;
            this.dgvOrderHistoryView.RowTemplate.Height = 28;
            this.dgvOrderHistoryView.Size = new System.Drawing.Size(1056, 752);
            this.dgvOrderHistoryView.TabIndex = 0;
            this.dgvOrderHistoryView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrderHistoryView_CellContentClick);
            // 
            // dgvOrderId
            // 
            this.dgvOrderId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvOrderId.DataPropertyName = "dgvOrderId";
            this.dgvOrderId.HeaderText = "Order ID";
            this.dgvOrderId.MinimumWidth = 8;
            this.dgvOrderId.Name = "dgvOrderId";
            this.dgvOrderId.Visible = false;
            // 
            // dgvName
            // 
            this.dgvName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvName.DataPropertyName = "dgvName";
            this.dgvName.HeaderText = "Customer Name";
            this.dgvName.MinimumWidth = 8;
            this.dgvName.Name = "dgvName";
            // 
            // dgvAddress
            // 
            this.dgvAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvAddress.DataPropertyName = "dgvAddress";
            this.dgvAddress.HeaderText = "Address";
            this.dgvAddress.MinimumWidth = 8;
            this.dgvAddress.Name = "dgvAddress";
            // 
            // dgvOrderType
            // 
            this.dgvOrderType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvOrderType.DataPropertyName = "dgvOrderType";
            this.dgvOrderType.HeaderText = "Order Type";
            this.dgvOrderType.MinimumWidth = 8;
            this.dgvOrderType.Name = "dgvOrderType";
            // 
            // dgvOrderDate
            // 
            this.dgvOrderDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvOrderDate.DataPropertyName = "dgvOrderDate";
            this.dgvOrderDate.HeaderText = "Order Date";
            this.dgvOrderDate.MinimumWidth = 8;
            this.dgvOrderDate.Name = "dgvOrderDate";
            // 
            // dgvTotalPrice
            // 
            this.dgvTotalPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvTotalPrice.DataPropertyName = "dgvTotalPrice";
            this.dgvTotalPrice.HeaderText = "Total Price";
            this.dgvTotalPrice.MinimumWidth = 8;
            this.dgvTotalPrice.Name = "dgvTotalPrice";
            // 
            // dgvPayment
            // 
            this.dgvPayment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvPayment.DataPropertyName = "dgvPayment";
            this.dgvPayment.HeaderText = "Payment";
            this.dgvPayment.MinimumWidth = 8;
            this.dgvPayment.Name = "dgvPayment";
            // 
            // dgvView
            // 
            this.dgvView.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvView.DataPropertyName = "dgvView";
            this.dgvView.HeaderText = "";
            this.dgvView.MinimumWidth = 8;
            this.dgvView.Name = "dgvView";
            this.dgvView.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvView.Text = "View";
            this.dgvView.UseColumnTextForButtonValue = true;
            // 
            // dgvCancel
            // 
            this.dgvCancel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvCancel.DataPropertyName = "dgvCancel";
            this.dgvCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.dgvCancel.HeaderText = "";
            this.dgvCancel.MinimumWidth = 8;
            this.dgvCancel.Name = "dgvCancel";
            this.dgvCancel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCancel.Text = "CANCEL";
            this.dgvCancel.UseColumnTextForButtonValue = true;
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnCalculations);
            this.pnlActions.Controls.Add(this.btnReset);
            this.pnlActions.Controls.Add(this.lblTotal);
            this.pnlActions.Controls.Add(this.txtTotalValue);
            this.pnlActions.Controls.Add(this.btnToday);
            this.pnlActions.Controls.Add(this.btnDownload);
            this.pnlActions.Controls.Add(this.btnPrint);
            this.pnlActions.Controls.Add(this.lblToDate);
            this.pnlActions.Controls.Add(this.dtpToDate);
            this.pnlActions.Controls.Add(this.lblFromDate);
            this.pnlActions.Controls.Add(this.dtpFromDate);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlActions.Location = new System.Drawing.Point(1091, 126);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(408, 823);
            this.pnlActions.TabIndex = 0;
            // 
            // btnCalculations
            // 
            this.btnCalculations.Location = new System.Drawing.Point(112, 398);
            this.btnCalculations.Name = "btnCalculations";
            this.btnCalculations.Size = new System.Drawing.Size(184, 72);
            this.btnCalculations.TabIndex = 13;
            this.btnCalculations.Text = "View Report";
            this.btnCalculations.UseVisualStyleBackColor = true;
            this.btnCalculations.Click += new System.EventHandler(this.btnCalculations_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(216, 320);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(168, 72);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI Black", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(8, 776);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(99, 38);
            this.lblTotal.TabIndex = 11;
            this.lblTotal.Text = "Total:";
            // 
            // txtTotalValue
            // 
            this.txtTotalValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalValue.Location = new System.Drawing.Point(112, 776);
            this.txtTotalValue.Name = "txtTotalValue";
            this.txtTotalValue.Size = new System.Drawing.Size(288, 39);
            this.txtTotalValue.TabIndex = 10;
            // 
            // btnToday
            // 
            this.btnToday.Location = new System.Drawing.Point(16, 320);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(184, 72);
            this.btnToday.TabIndex = 6;
            this.btnToday.Text = "Today";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(216, 240);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(168, 72);
            this.btnDownload.TabIndex = 5;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(16, 240);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(184, 72);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Font = new System.Drawing.Font("Segoe UI Black", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(8, 128);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(135, 38);
            this.lblToDate.TabIndex = 3;
            this.lblToDate.Text = "To Date:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Location = new System.Drawing.Point(16, 176);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(376, 40);
            this.dtpToDate.TabIndex = 2;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Font = new System.Drawing.Font("Segoe UI Black", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(8, 24);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(171, 38);
            this.lblFromDate.TabIndex = 1;
            this.lblFromDate.Text = "From Date:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Location = new System.Drawing.Point(16, 72);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(376, 40);
            this.dtpFromDate.TabIndex = 0;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dtpFromDate_ValueChanged);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::OrderManagement.Properties.Resources.pen_tool_icon2;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn1.MinimumWidth = 8;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 150;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::OrderManagement.Properties.Resources.find_product_icon1;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.MinimumWidth = 8;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.Width = 150;
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.HeaderText = "";
            this.dataGridViewImageColumn3.Image = global::OrderManagement.Properties.Resources.red_x_line_icon;
            this.dataGridViewImageColumn3.MinimumWidth = 8;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.Width = 150;
            // 
            // frmOrderHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1499, 1031);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlGridHolder);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmOrderHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmOrderHistory";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlGridHolder.ResumeLayout(false);
            this.pnlGridHolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderHistoryView)).EndInit();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlGridHolder;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.ComboBox cmbPaymentType;
        private System.Windows.Forms.ComboBox cmbOrderType;
        private System.Windows.Forms.DataGridView dgvOrderHistoryView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtTotalValue;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSearchBox;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvOrderId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvOrderType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvOrderDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTotalPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPayment;
        private System.Windows.Forms.DataGridViewButtonColumn dgvView;
        private System.Windows.Forms.DataGridViewButtonColumn dgvCancel;
        private System.Windows.Forms.Button btnCalculations;
    }
}