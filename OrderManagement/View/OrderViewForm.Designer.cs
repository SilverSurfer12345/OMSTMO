namespace OrderManagement.View
{
    partial class OrderViewForm
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
            this.lblNameText = new System.Windows.Forms.Label();
            this.lblTelephoneText = new System.Windows.Forms.Label();
            this.lblOrderDateText = new System.Windows.Forms.Label();
            this.lblPaymentTypeText = new System.Windows.Forms.Label();
            this.lblOrderTypeText = new System.Windows.Forms.Label();
            this.lblTotalPriceText = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnEditCurrentOrder = new System.Windows.Forms.Button();
            this.btnPreviewInvoice = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvOrderItems = new System.Windows.Forms.DataGridView();
            this.dgvOrderItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvItemPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbInvoiceBill = new System.Windows.Forms.PictureBox();
            this.pnlCustomerInfo = new System.Windows.Forms.Panel();
            this.lblAddress = new System.Windows.Forms.Label();
            this.pnlPaymentTypeRButtons = new System.Windows.Forms.Panel();
            this.rbRefunded = new System.Windows.Forms.RadioButton();
            this.rbPending = new System.Windows.Forms.RadioButton();
            this.rbCancelled = new System.Windows.Forms.RadioButton();
            this.rbCash = new System.Windows.Forms.RadioButton();
            this.rbCard = new System.Windows.Forms.RadioButton();
            this.txtTotalPrice = new System.Windows.Forms.TextBox();
            this.pnlOrderTypeRbuttons = new System.Windows.Forms.Panel();
            this.rbWaiting = new System.Windows.Forms.RadioButton();
            this.rbCollection = new System.Windows.Forms.RadioButton();
            this.rbOnline = new System.Windows.Forms.RadioButton();
            this.rbDelivery = new System.Windows.Forms.RadioButton();
            this.lblChangesMade = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInvoiceBill)).BeginInit();
            this.pnlCustomerInfo.SuspendLayout();
            this.pnlPaymentTypeRButtons.SuspendLayout();
            this.pnlOrderTypeRbuttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNameText
            // 
            this.lblNameText.AutoSize = true;
            this.lblNameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameText.Location = new System.Drawing.Point(4, 5);
            this.lblNameText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNameText.Name = "lblNameText";
            this.lblNameText.Size = new System.Drawing.Size(247, 33);
            this.lblNameText.TabIndex = 1;
            this.lblNameText.Text = "Name: John Doe";
            // 
            // lblTelephoneText
            // 
            this.lblTelephoneText.AutoSize = true;
            this.lblTelephoneText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelephoneText.Location = new System.Drawing.Point(4, 42);
            this.lblTelephoneText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTelephoneText.Name = "lblTelephoneText";
            this.lblTelephoneText.Size = new System.Drawing.Size(366, 33);
            this.lblTelephoneText.TabIndex = 5;
            this.lblTelephoneText.Text = "Telephone: 01910000000";
            // 
            // lblOrderDateText
            // 
            this.lblOrderDateText.AutoSize = true;
            this.lblOrderDateText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderDateText.Location = new System.Drawing.Point(4, 78);
            this.lblOrderDateText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOrderDateText.Name = "lblOrderDateText";
            this.lblOrderDateText.Size = new System.Drawing.Size(341, 33);
            this.lblOrderDateText.TabIndex = 7;
            this.lblOrderDateText.Text = "Order Date: 01/01/2024";
            // 
            // lblPaymentTypeText
            // 
            this.lblPaymentTypeText.AutoSize = true;
            this.lblPaymentTypeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentTypeText.Location = new System.Drawing.Point(4, 477);
            this.lblPaymentTypeText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPaymentTypeText.Name = "lblPaymentTypeText";
            this.lblPaymentTypeText.Size = new System.Drawing.Size(233, 33);
            this.lblPaymentTypeText.TabIndex = 9;
            this.lblPaymentTypeText.Text = "Payment Type: ";
            // 
            // lblOrderTypeText
            // 
            this.lblOrderTypeText.AutoSize = true;
            this.lblOrderTypeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderTypeText.Location = new System.Drawing.Point(4, 435);
            this.lblOrderTypeText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOrderTypeText.Name = "lblOrderTypeText";
            this.lblOrderTypeText.Size = new System.Drawing.Size(191, 33);
            this.lblOrderTypeText.TabIndex = 11;
            this.lblOrderTypeText.Text = "Order Type: ";
            // 
            // lblTotalPriceText
            // 
            this.lblTotalPriceText.AutoSize = true;
            this.lblTotalPriceText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPriceText.Location = new System.Drawing.Point(4, 518);
            this.lblTotalPriceText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotalPriceText.Name = "lblTotalPriceText";
            this.lblTotalPriceText.Size = new System.Drawing.Size(175, 33);
            this.lblTotalPriceText.TabIndex = 13;
            this.lblTotalPriceText.Text = "Total Price:";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Controls.Add(this.pictureBox1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1566, 209);
            this.pnlHeader.TabIndex = 16;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(258, 88);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(222, 54);
            this.lblHeader.TabIndex = 3;
            this.lblHeader.Text = "Order View";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(4, 5);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(224, 178);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(488, 22);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(213, 111);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.Gainsboro;
            this.pnlFooter.Controls.Add(this.btnEditCurrentOrder);
            this.pnlFooter.Controls.Add(this.btnPreviewInvoice);
            this.pnlFooter.Controls.Add(this.btnPrint);
            this.pnlFooter.Controls.Add(this.btnClose);
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 809);
            this.pnlFooter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1566, 151);
            this.pnlFooter.TabIndex = 18;
            // 
            // btnEditCurrentOrder
            // 
            this.btnEditCurrentOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnEditCurrentOrder.ForeColor = System.Drawing.Color.White;
            this.btnEditCurrentOrder.Location = new System.Drawing.Point(267, 22);
            this.btnEditCurrentOrder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEditCurrentOrder.Name = "btnEditCurrentOrder";
            this.btnEditCurrentOrder.Size = new System.Drawing.Size(213, 111);
            this.btnEditCurrentOrder.TabIndex = 25;
            this.btnEditCurrentOrder.Text = "EDIT ORDER";
            this.btnEditCurrentOrder.UseVisualStyleBackColor = false;
            this.btnEditCurrentOrder.Click += new System.EventHandler(this.btnEditCurrentOrder_Click);
            // 
            // btnPreviewInvoice
            // 
            this.btnPreviewInvoice.BackColor = System.Drawing.Color.RosyBrown;
            this.btnPreviewInvoice.Location = new System.Drawing.Point(1107, 22);
            this.btnPreviewInvoice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPreviewInvoice.Name = "btnPreviewInvoice";
            this.btnPreviewInvoice.Size = new System.Drawing.Size(195, 111);
            this.btnPreviewInvoice.TabIndex = 24;
            this.btnPreviewInvoice.Text = "PREVIEW INVOICE";
            this.btnPreviewInvoice.UseVisualStyleBackColor = false;
            this.btnPreviewInvoice.Click += new System.EventHandler(this.btnPreviewInvoice_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(1311, 22);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(213, 111);
            this.btnPrint.TabIndex = 18;
            this.btnPrint.Text = "PRINT";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(44, 22);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(213, 111);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "SAVE AND CLOSE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvOrderItems
            // 
            this.dgvOrderItems.AllowUserToAddRows = false;
            this.dgvOrderItems.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvOrderItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrderItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvOrderItemId,
            this.dgvItemName,
            this.dgvQuantity,
            this.dgvItemPrice});
            this.dgvOrderItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrderItems.Location = new System.Drawing.Point(0, 0);
            this.dgvOrderItems.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvOrderItems.Name = "dgvOrderItems";
            this.dgvOrderItems.RowHeadersVisible = false;
            this.dgvOrderItems.RowHeadersWidth = 62;
            this.dgvOrderItems.Size = new System.Drawing.Size(646, 600);
            this.dgvOrderItems.TabIndex = 19;
            this.dgvOrderItems.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrderItems_CellContentClick);
            // 
            // dgvOrderItemId
            // 
            this.dgvOrderItemId.DataPropertyName = "dgvOrderItemId";
            this.dgvOrderItemId.HeaderText = "Order Item Id";
            this.dgvOrderItemId.MinimumWidth = 8;
            this.dgvOrderItemId.Name = "dgvOrderItemId";
            this.dgvOrderItemId.Visible = false;
            this.dgvOrderItemId.Width = 150;
            // 
            // dgvItemName
            // 
            this.dgvItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvItemName.DataPropertyName = "dgvItemName";
            this.dgvItemName.HeaderText = "Item Name";
            this.dgvItemName.MinimumWidth = 8;
            this.dgvItemName.Name = "dgvItemName";
            this.dgvItemName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgvQuantity
            // 
            this.dgvQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvQuantity.DataPropertyName = "dgvQuantity";
            this.dgvQuantity.HeaderText = "Quantity";
            this.dgvQuantity.MinimumWidth = 8;
            this.dgvQuantity.Name = "dgvQuantity";
            // 
            // dgvItemPrice
            // 
            this.dgvItemPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvItemPrice.DataPropertyName = "dgvItemPrice";
            this.dgvItemPrice.HeaderText = "Price";
            this.dgvItemPrice.MinimumWidth = 8;
            this.dgvItemPrice.Name = "dgvItemPrice";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pbInvoiceBill);
            this.panel1.Controls.Add(this.dgvOrderItems);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(920, 209);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(646, 600);
            this.panel1.TabIndex = 20;
            // 
            // pbInvoiceBill
            // 
            this.pbInvoiceBill.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pbInvoiceBill.Location = new System.Drawing.Point(0, 0);
            this.pbInvoiceBill.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbInvoiceBill.Name = "pbInvoiceBill";
            this.pbInvoiceBill.Size = new System.Drawing.Size(428, 387);
            this.pbInvoiceBill.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbInvoiceBill.TabIndex = 23;
            this.pbInvoiceBill.TabStop = false;
            this.pbInvoiceBill.Visible = false;
            // 
            // pnlCustomerInfo
            // 
            this.pnlCustomerInfo.Controls.Add(this.lblAddress);
            this.pnlCustomerInfo.Controls.Add(this.pnlPaymentTypeRButtons);
            this.pnlCustomerInfo.Controls.Add(this.txtTotalPrice);
            this.pnlCustomerInfo.Controls.Add(this.pnlOrderTypeRbuttons);
            this.pnlCustomerInfo.Controls.Add(this.lblChangesMade);
            this.pnlCustomerInfo.Controls.Add(this.lblNameText);
            this.pnlCustomerInfo.Controls.Add(this.lblTelephoneText);
            this.pnlCustomerInfo.Controls.Add(this.lblOrderDateText);
            this.pnlCustomerInfo.Controls.Add(this.lblTotalPriceText);
            this.pnlCustomerInfo.Controls.Add(this.lblPaymentTypeText);
            this.pnlCustomerInfo.Controls.Add(this.lblOrderTypeText);
            this.pnlCustomerInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCustomerInfo.Location = new System.Drawing.Point(0, 209);
            this.pnlCustomerInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlCustomerInfo.Name = "pnlCustomerInfo";
            this.pnlCustomerInfo.Size = new System.Drawing.Size(910, 600);
            this.pnlCustomerInfo.TabIndex = 21;
            this.pnlCustomerInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCustomerInfo_Paint);
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.Location = new System.Drawing.Point(4, 115);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(129, 33);
            this.lblAddress.TabIndex = 22;
            this.lblAddress.Text = "Address";
            // 
            // pnlPaymentTypeRButtons
            // 
            this.pnlPaymentTypeRButtons.Controls.Add(this.rbRefunded);
            this.pnlPaymentTypeRButtons.Controls.Add(this.rbPending);
            this.pnlPaymentTypeRButtons.Controls.Add(this.rbCancelled);
            this.pnlPaymentTypeRButtons.Controls.Add(this.rbCash);
            this.pnlPaymentTypeRButtons.Controls.Add(this.rbCard);
            this.pnlPaymentTypeRButtons.Location = new System.Drawing.Point(224, 477);
            this.pnlPaymentTypeRButtons.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlPaymentTypeRButtons.Name = "pnlPaymentTypeRButtons";
            this.pnlPaymentTypeRButtons.Size = new System.Drawing.Size(658, 38);
            this.pnlPaymentTypeRButtons.TabIndex = 21;
            // 
            // rbRefunded
            // 
            this.rbRefunded.AutoSize = true;
            this.rbRefunded.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRefunded.Location = new System.Drawing.Point(549, 5);
            this.rbRefunded.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbRefunded.Name = "rbRefunded";
            this.rbRefunded.Size = new System.Drawing.Size(113, 24);
            this.rbRefunded.TabIndex = 22;
            this.rbRefunded.TabStop = true;
            this.rbRefunded.Text = "Refunded";
            this.rbRefunded.UseVisualStyleBackColor = true;
            // 
            // rbPending
            // 
            this.rbPending.AutoSize = true;
            this.rbPending.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPending.Location = new System.Drawing.Point(176, 5);
            this.rbPending.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbPending.Name = "rbPending";
            this.rbPending.Size = new System.Drawing.Size(101, 24);
            this.rbPending.TabIndex = 21;
            this.rbPending.TabStop = true;
            this.rbPending.Text = "Pending";
            this.rbPending.UseVisualStyleBackColor = true;
            // 
            // rbCancelled
            // 
            this.rbCancelled.AutoSize = true;
            this.rbCancelled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCancelled.Location = new System.Drawing.Point(298, 5);
            this.rbCancelled.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbCancelled.Name = "rbCancelled";
            this.rbCancelled.Size = new System.Drawing.Size(117, 24);
            this.rbCancelled.TabIndex = 20;
            this.rbCancelled.TabStop = true;
            this.rbCancelled.Text = "Cancelled";
            this.rbCancelled.UseVisualStyleBackColor = true;
            // 
            // rbCash
            // 
            this.rbCash.AutoSize = true;
            this.rbCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCash.Location = new System.Drawing.Point(4, 5);
            this.rbCash.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbCash.Name = "rbCash";
            this.rbCash.Size = new System.Drawing.Size(77, 24);
            this.rbCash.TabIndex = 17;
            this.rbCash.TabStop = true;
            this.rbCash.Text = "Cash";
            this.rbCash.UseVisualStyleBackColor = true;
            // 
            // rbCard
            // 
            this.rbCard.AutoSize = true;
            this.rbCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCard.Location = new System.Drawing.Point(93, 5);
            this.rbCard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbCard.Name = "rbCard";
            this.rbCard.Size = new System.Drawing.Size(74, 24);
            this.rbCard.TabIndex = 18;
            this.rbCard.TabStop = true;
            this.rbCard.Text = "Card";
            this.rbCard.UseVisualStyleBackColor = true;
            // 
            // txtTotalPrice
            // 
            this.txtTotalPrice.Location = new System.Drawing.Point(188, 523);
            this.txtTotalPrice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTotalPrice.Name = "txtTotalPrice";
            this.txtTotalPrice.ReadOnly = true;
            this.txtTotalPrice.Size = new System.Drawing.Size(148, 26);
            this.txtTotalPrice.TabIndex = 21;
            // 
            // pnlOrderTypeRbuttons
            // 
            this.pnlOrderTypeRbuttons.Controls.Add(this.rbWaiting);
            this.pnlOrderTypeRbuttons.Controls.Add(this.rbCollection);
            this.pnlOrderTypeRbuttons.Controls.Add(this.rbOnline);
            this.pnlOrderTypeRbuttons.Controls.Add(this.rbDelivery);
            this.pnlOrderTypeRbuttons.Location = new System.Drawing.Point(192, 435);
            this.pnlOrderTypeRbuttons.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlOrderTypeRbuttons.Name = "pnlOrderTypeRbuttons";
            this.pnlOrderTypeRbuttons.Size = new System.Drawing.Size(464, 38);
            this.pnlOrderTypeRbuttons.TabIndex = 20;
            // 
            // rbWaiting
            // 
            this.rbWaiting.AutoSize = true;
            this.rbWaiting.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbWaiting.Location = new System.Drawing.Point(346, 5);
            this.rbWaiting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbWaiting.Name = "rbWaiting";
            this.rbWaiting.Size = new System.Drawing.Size(97, 24);
            this.rbWaiting.TabIndex = 20;
            this.rbWaiting.TabStop = true;
            this.rbWaiting.Text = "Waiting";
            this.rbWaiting.UseVisualStyleBackColor = true;
            // 
            // rbCollection
            // 
            this.rbCollection.AutoSize = true;
            this.rbCollection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCollection.Location = new System.Drawing.Point(4, 5);
            this.rbCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbCollection.Name = "rbCollection";
            this.rbCollection.Size = new System.Drawing.Size(118, 24);
            this.rbCollection.TabIndex = 17;
            this.rbCollection.TabStop = true;
            this.rbCollection.Text = "Collection";
            this.rbCollection.UseVisualStyleBackColor = true;
            this.rbCollection.CheckedChanged += new System.EventHandler(this.rbCollection_CheckedChanged);
            // 
            // rbOnline
            // 
            this.rbOnline.AutoSize = true;
            this.rbOnline.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbOnline.Location = new System.Drawing.Point(250, 5);
            this.rbOnline.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbOnline.Name = "rbOnline";
            this.rbOnline.Size = new System.Drawing.Size(88, 24);
            this.rbOnline.TabIndex = 19;
            this.rbOnline.TabStop = true;
            this.rbOnline.Text = "Online";
            this.rbOnline.UseVisualStyleBackColor = true;
            this.rbOnline.CheckedChanged += new System.EventHandler(this.rbOnline_CheckedChanged);
            // 
            // rbDelivery
            // 
            this.rbDelivery.AutoSize = true;
            this.rbDelivery.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDelivery.Location = new System.Drawing.Point(135, 5);
            this.rbDelivery.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbDelivery.Name = "rbDelivery";
            this.rbDelivery.Size = new System.Drawing.Size(103, 24);
            this.rbDelivery.TabIndex = 18;
            this.rbDelivery.TabStop = true;
            this.rbDelivery.Text = "Delivery";
            this.rbDelivery.UseVisualStyleBackColor = true;
            this.rbDelivery.CheckedChanged += new System.EventHandler(this.rbDelivery_CheckedChanged);
            // 
            // lblChangesMade
            // 
            this.lblChangesMade.AutoSize = true;
            this.lblChangesMade.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChangesMade.ForeColor = System.Drawing.Color.IndianRed;
            this.lblChangesMade.Location = new System.Drawing.Point(38, 565);
            this.lblChangesMade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblChangesMade.Name = "lblChangesMade";
            this.lblChangesMade.Size = new System.Drawing.Size(617, 29);
            this.lblChangesMade.TabIndex = 16;
            this.lblChangesMade.Text = "if you wish to keep your changes please press save!";
            this.lblChangesMade.Visible = false;
            // 
            // OrderViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1566, 960);
            this.Controls.Add(this.pnlCustomerInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "OrderViewForm";
            this.Text = "OrderViewForm";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInvoiceBill)).EndInit();
            this.pnlCustomerInfo.ResumeLayout(false);
            this.pnlCustomerInfo.PerformLayout();
            this.pnlPaymentTypeRButtons.ResumeLayout(false);
            this.pnlPaymentTypeRButtons.PerformLayout();
            this.pnlOrderTypeRbuttons.ResumeLayout(false);
            this.pnlOrderTypeRbuttons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblNameText;
        private System.Windows.Forms.Label lblTelephoneText;
        private System.Windows.Forms.Label lblOrderDateText;
        private System.Windows.Forms.Label lblPaymentTypeText;
        private System.Windows.Forms.Label lblOrderTypeText;
        private System.Windows.Forms.Label lblTotalPriceText;
        public System.Windows.Forms.Panel pnlHeader;
        public System.Windows.Forms.Label lblHeader;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.Panel pnlFooter;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DataGridView dgvOrderItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvOrderItemId;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvItemPrice;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlCustomerInfo;
        private System.Windows.Forms.Label lblChangesMade;
        private System.Windows.Forms.RadioButton rbCollection;
        private System.Windows.Forms.RadioButton rbDelivery;
        private System.Windows.Forms.RadioButton rbOnline;
        private System.Windows.Forms.Panel pnlOrderTypeRbuttons;
        private System.Windows.Forms.TextBox txtTotalPrice;
        private System.Windows.Forms.Panel pnlPaymentTypeRButtons;
        private System.Windows.Forms.RadioButton rbCancelled;
        private System.Windows.Forms.RadioButton rbCash;
        private System.Windows.Forms.RadioButton rbCard;
        private System.Windows.Forms.RadioButton rbPending;
        private System.Windows.Forms.RadioButton rbRefunded;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.PictureBox pbInvoiceBill;
        private System.Windows.Forms.Button btnPreviewInvoice;
        public System.Windows.Forms.Button btnEditCurrentOrder;
        private System.Windows.Forms.RadioButton rbWaiting;
    }
}