namespace OrderManagement.View
{
    partial class frmPOS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOS));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAutoAdd = new System.Windows.Forms.Button();
            this.lblSearchFoodItem = new System.Windows.Forms.Label();
            this.txtSearchFoodItem = new System.Windows.Forms.TextBox();
            this.btnWaiting = new System.Windows.Forms.Button();
            this.btnNotificationConfig = new System.Windows.Forms.Button();
            this.txtPreviousOrders = new System.Windows.Forms.TextBox();
            this.btnCurrentOrders = new System.Windows.Forms.Button();
            this.pnlBasketHolder = new System.Windows.Forms.Panel();
            this.pnlCstDetails = new System.Windows.Forms.Panel();
            this.lstSearchResult = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCstTelephone = new System.Windows.Forms.TextBox();
            this.lblCstAddress = new System.Windows.Forms.Label();
            this.lblCstName = new System.Windows.Forms.Label();
            this.btnCustomerAction = new System.Windows.Forms.Button();
            this.txtCustomerDetails = new System.Windows.Forms.TextBox();
            this.txtAddressDisplay = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnCol = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.PictureBox();
            this.btnDineIn = new System.Windows.Forms.Button();
            this.btnBill = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblPaymentOption = new System.Windows.Forms.Label();
            this.btnLoadPreviousOrder = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pnlBodyBottom = new System.Windows.Forms.Panel();
            this.pnlOrderAction = new System.Windows.Forms.Panel();
            this.pnlBasketBtns = new System.Windows.Forms.Panel();
            this.btnKot = new System.Windows.Forms.Button();
            this.btnPayment = new System.Windows.Forms.Button();
            this.btnSaveOrder = new System.Windows.Forms.Button();
            this.pnlPrice = new System.Windows.Forms.Panel();
            this.lblAddNote = new System.Windows.Forms.Label();
            this.txtAddNote = new System.Windows.Forms.TextBox();
            this.btnDeliveryChargeAmend = new System.Windows.Forms.Button();
            this.txtDeliveryCharge = new System.Windows.Forms.TextBox();
            this.txtTotalPrice = new System.Windows.Forms.TextBox();
            this.lblTotalPrice = new System.Windows.Forms.Label();
            this.cbCustomDiscount = new System.Windows.Forms.ComboBox();
            this.btnCallMe = new System.Windows.Forms.Button();
            this.basketGridView = new System.Windows.Forms.DataGridView();
            this.dgvid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOriginalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvExtraCharge = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvOrderNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDeliveryCharge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDeleteBasketItem = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvExtraChargeValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlMainGridHolder = new System.Windows.Forms.Panel();
            this.pnlOrderHolder = new System.Windows.Forms.Panel();
            this.flpPreviousOrders = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlItemHolder = new System.Windows.Forms.Panel();
            this.flpCategoryBtns = new System.Windows.Forms.FlowLayoutPanel();
            this.flpItemView = new System.Windows.Forms.FlowLayoutPanel();
            this.rmDataSet1 = new OrderManagement.RMDataSet();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1.SuspendLayout();
            this.pnlCstDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlBodyBottom.SuspendLayout();
            this.pnlOrderAction.SuspendLayout();
            this.pnlBasketBtns.SuspendLayout();
            this.pnlPrice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basketGridView)).BeginInit();
            this.pnlMainGridHolder.SuspendLayout();
            this.pnlOrderHolder.SuspendLayout();
            this.pnlItemHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rmDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.panel1.Controls.Add(this.btnAutoAdd);
            this.panel1.Controls.Add(this.lblSearchFoodItem);
            this.panel1.Controls.Add(this.txtSearchFoodItem);
            this.panel1.Controls.Add(this.btnWaiting);
            this.panel1.Controls.Add(this.btnNotificationConfig);
            this.panel1.Controls.Add(this.txtPreviousOrders);
            this.panel1.Controls.Add(this.btnCurrentOrders);
            this.panel1.Controls.Add(this.pnlBasketHolder);
            this.panel1.Controls.Add(this.pnlCstDetails);
            this.panel1.Controls.Add(this.btnNew);
            this.panel1.Controls.Add(this.btnCol);
            this.panel1.Controls.Add(this.btnDel);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnDineIn);
            this.panel1.Controls.Add(this.btnBill);
            this.panel1.Controls.Add(this.btnHold);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1828, 192);
            this.panel1.TabIndex = 0;
            // 
            // btnAutoAdd
            // 
            this.btnAutoAdd.Location = new System.Drawing.Point(215, 12);
            this.btnAutoAdd.Name = "btnAutoAdd";
            this.btnAutoAdd.Size = new System.Drawing.Size(72, 53);
            this.btnAutoAdd.TabIndex = 59;
            this.btnAutoAdd.Text = "Auto Add";
            this.btnAutoAdd.UseVisualStyleBackColor = true;
            this.btnAutoAdd.Click += new System.EventHandler(this.btnAutoAdd_Click);
            // 
            // lblSearchFoodItem
            // 
            this.lblSearchFoodItem.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblSearchFoodItem.AutoSize = true;
            this.lblSearchFoodItem.Font = new System.Drawing.Font("Segoe UI Black", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchFoodItem.ForeColor = System.Drawing.Color.White;
            this.lblSearchFoodItem.Location = new System.Drawing.Point(12, 154);
            this.lblSearchFoodItem.Name = "lblSearchFoodItem";
            this.lblSearchFoodItem.Size = new System.Drawing.Size(181, 38);
            this.lblSearchFoodItem.TabIndex = 57;
            this.lblSearchFoodItem.Text = "Search Item";
            // 
            // txtSearchFoodItem
            // 
            this.txtSearchFoodItem.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSearchFoodItem.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchFoodItem.Location = new System.Drawing.Point(133, 154);
            this.txtSearchFoodItem.Name = "txtSearchFoodItem";
            this.txtSearchFoodItem.Size = new System.Drawing.Size(256, 35);
            this.txtSearchFoodItem.TabIndex = 56;
            this.txtSearchFoodItem.TextChanged += new System.EventHandler(this.txtSearchFoodItem_TextChanged);
            // 
            // btnWaiting
            // 
            this.btnWaiting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnWaiting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnWaiting.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWaiting.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnWaiting.Location = new System.Drawing.Point(1129, 8);
            this.btnWaiting.Name = "btnWaiting";
            this.btnWaiting.Size = new System.Drawing.Size(112, 88);
            this.btnWaiting.TabIndex = 55;
            this.btnWaiting.TabStop = false;
            this.btnWaiting.Text = "Waiting";
            this.btnWaiting.UseVisualStyleBackColor = false;
            this.btnWaiting.Click += new System.EventHandler(this.btnWaiting_Click_1);
            // 
            // btnNotificationConfig
            // 
            this.btnNotificationConfig.BackgroundImage = global::OrderManagement.Properties.Resources.tools_icon;
            this.btnNotificationConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNotificationConfig.Location = new System.Drawing.Point(300, 19);
            this.btnNotificationConfig.Name = "btnNotificationConfig";
            this.btnNotificationConfig.Size = new System.Drawing.Size(24, 25);
            this.btnNotificationConfig.TabIndex = 54;
            this.btnNotificationConfig.UseVisualStyleBackColor = true;
            this.btnNotificationConfig.Click += new System.EventHandler(this.btnNotificationConfig_Click);
            // 
            // txtPreviousOrders
            // 
            this.txtPreviousOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtPreviousOrders.Location = new System.Drawing.Point(1365, 8);
            this.txtPreviousOrders.Name = "txtPreviousOrders";
            this.txtPreviousOrders.Size = new System.Drawing.Size(34, 34);
            this.txtPreviousOrders.TabIndex = 9;
            this.txtPreviousOrders.Visible = false;
            // 
            // btnCurrentOrders
            // 
            this.btnCurrentOrders.Location = new System.Drawing.Point(330, 8);
            this.btnCurrentOrders.Name = "btnCurrentOrders";
            this.btnCurrentOrders.Size = new System.Drawing.Size(158, 47);
            this.btnCurrentOrders.TabIndex = 53;
            this.btnCurrentOrders.Text = "Current Orders";
            this.btnCurrentOrders.UseVisualStyleBackColor = true;
            this.btnCurrentOrders.Click += new System.EventHandler(this.btnCurrentOrders_Click);
            // 
            // pnlBasketHolder
            // 
            this.pnlBasketHolder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pnlBasketHolder.BackColor = System.Drawing.SystemColors.Highlight;
            this.pnlBasketHolder.Location = new System.Drawing.Point(1065, 192);
            this.pnlBasketHolder.Name = "pnlBasketHolder";
            this.pnlBasketHolder.Size = new System.Drawing.Size(763, 468);
            this.pnlBasketHolder.TabIndex = 16;
            // 
            // pnlCstDetails
            // 
            this.pnlCstDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCstDetails.Controls.Add(this.lstSearchResult);
            this.pnlCstDetails.Controls.Add(this.label2);
            this.pnlCstDetails.Controls.Add(this.txtCstTelephone);
            this.pnlCstDetails.Controls.Add(this.lblCstAddress);
            this.pnlCstDetails.Controls.Add(this.lblCstName);
            this.pnlCstDetails.Controls.Add(this.btnCustomerAction);
            this.pnlCstDetails.Controls.Add(this.txtCustomerDetails);
            this.pnlCstDetails.Controls.Add(this.txtAddressDisplay);
            this.pnlCstDetails.Location = new System.Drawing.Point(838, 102);
            this.pnlCstDetails.Name = "pnlCstDetails";
            this.pnlCstDetails.Size = new System.Drawing.Size(990, 90);
            this.pnlCstDetails.TabIndex = 46;
            // 
            // lstSearchResult
            // 
            this.lstSearchResult.FormattingEnabled = true;
            this.lstSearchResult.ItemHeight = 28;
            this.lstSearchResult.Location = new System.Drawing.Point(631, 28);
            this.lstSearchResult.Name = "lstSearchResult";
            this.lstSearchResult.Size = new System.Drawing.Size(208, 4);
            this.lstSearchResult.TabIndex = 52;
            this.lstSearchResult.Visible = false;
            this.lstSearchResult.SelectedIndexChanged += new System.EventHandler(this.lstSearchResult_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(495, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 30);
            this.label2.TabIndex = 49;
            this.label2.Text = "Telephone Number:";
            // 
            // txtCstTelephone
            // 
            this.txtCstTelephone.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCstTelephone.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCstTelephone.Location = new System.Drawing.Point(631, 3);
            this.txtCstTelephone.Name = "txtCstTelephone";
            this.txtCstTelephone.Size = new System.Drawing.Size(208, 35);
            this.txtCstTelephone.TabIndex = 48;
            this.txtCstTelephone.TextChanged += new System.EventHandler(this.txtCstTelephone_TextChanged);
            // 
            // lblCstAddress
            // 
            this.lblCstAddress.AutoSize = true;
            this.lblCstAddress.BackColor = System.Drawing.SystemColors.Window;
            this.lblCstAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCstAddress.Location = new System.Drawing.Point(111, 51);
            this.lblCstAddress.Name = "lblCstAddress";
            this.lblCstAddress.Size = new System.Drawing.Size(88, 30);
            this.lblCstAddress.TabIndex = 47;
            this.lblCstAddress.Text = "Address:";
            // 
            // lblCstName
            // 
            this.lblCstName.AutoSize = true;
            this.lblCstName.BackColor = System.Drawing.SystemColors.Window;
            this.lblCstName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCstName.Location = new System.Drawing.Point(107, 6);
            this.lblCstName.Name = "lblCstName";
            this.lblCstName.Size = new System.Drawing.Size(159, 30);
            this.lblCstName.TabIndex = 46;
            this.lblCstName.Text = "Customer Name:";
            // 
            // btnCustomerAction
            // 
            this.btnCustomerAction.Location = new System.Drawing.Point(845, 41);
            this.btnCustomerAction.Name = "btnCustomerAction";
            this.btnCustomerAction.Size = new System.Drawing.Size(138, 36);
            this.btnCustomerAction.TabIndex = 45;
            this.btnCustomerAction.Text = "Add New Customer";
            this.btnCustomerAction.UseVisualStyleBackColor = true;
            this.btnCustomerAction.Click += new System.EventHandler(this.btnCustomerAction_Click);
            // 
            // txtCustomerDetails
            // 
            this.txtCustomerDetails.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtCustomerDetails.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCustomerDetails.Location = new System.Drawing.Point(227, 3);
            this.txtCustomerDetails.Name = "txtCustomerDetails";
            this.txtCustomerDetails.Size = new System.Drawing.Size(256, 35);
            this.txtCustomerDetails.TabIndex = 6;
            this.txtCustomerDetails.TextChanged += new System.EventHandler(this.txtCustomerDetails_TextChanged);
            // 
            // txtAddressDisplay
            // 
            this.txtAddressDisplay.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtAddressDisplay.Enabled = false;
            this.txtAddressDisplay.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAddressDisplay.Location = new System.Drawing.Point(180, 48);
            this.txtAddressDisplay.Name = "txtAddressDisplay";
            this.txtAddressDisplay.Size = new System.Drawing.Size(659, 35);
            this.txtAddressDisplay.TabIndex = 43;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnNew.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(507, 8);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(120, 88);
            this.btnNew.TabIndex = 45;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCol
            // 
            this.btnCol.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnCol.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCol.Location = new System.Drawing.Point(1003, 8);
            this.btnCol.Name = "btnCol";
            this.btnCol.Size = new System.Drawing.Size(120, 88);
            this.btnCol.TabIndex = 41;
            this.btnCol.Text = "Collection";
            this.btnCol.UseVisualStyleBackColor = false;
            // 
            // btnDel
            // 
            this.btnDel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnDel.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDel.Location = new System.Drawing.Point(883, 8);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(112, 88);
            this.btnDel.TabIndex = 42;
            this.btnDel.Text = "Delivery";
            this.btnDel.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(1715, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(101, 84);
            this.btnExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnExit.TabIndex = 39;
            this.btnExit.TabStop = false;
            this.btnExit.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // btnDineIn
            // 
            this.btnDineIn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDineIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnDineIn.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDineIn.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnDineIn.Location = new System.Drawing.Point(1247, 8);
            this.btnDineIn.Name = "btnDineIn";
            this.btnDineIn.Size = new System.Drawing.Size(112, 88);
            this.btnDineIn.TabIndex = 37;
            this.btnDineIn.TabStop = false;
            this.btnDineIn.Text = "Dine In";
            this.btnDineIn.UseVisualStyleBackColor = false;
            // 
            // btnBill
            // 
            this.btnBill.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnBill.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBill.Location = new System.Drawing.Point(763, 8);
            this.btnBill.Name = "btnBill";
            this.btnBill.Size = new System.Drawing.Size(112, 88);
            this.btnBill.TabIndex = 33;
            this.btnBill.Text = "Bill List";
            this.btnBill.UseVisualStyleBackColor = false;
            this.btnBill.Click += new System.EventHandler(this.btnBill_Click);
            // 
            // btnHold
            // 
            this.btnHold.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnHold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnHold.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHold.Location = new System.Drawing.Point(635, 8);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(120, 88);
            this.btnHold.TabIndex = 32;
            this.btnHold.Text = "Hold";
            this.btnHold.UseVisualStyleBackColor = false;
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(141, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 54);
            this.label1.TabIndex = 2;
            this.label1.Text = "POS";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(123, 101);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            // 
            // lblPaymentOption
            // 
            this.lblPaymentOption.AutoSize = true;
            this.lblPaymentOption.ForeColor = System.Drawing.SystemColors.Control;
            this.lblPaymentOption.Location = new System.Drawing.Point(582, 99);
            this.lblPaymentOption.Name = "lblPaymentOption";
            this.lblPaymentOption.Size = new System.Drawing.Size(96, 28);
            this.lblPaymentOption.TabIndex = 56;
            this.lblPaymentOption.Text = "PENDING";
            this.lblPaymentOption.Visible = false;
            this.lblPaymentOption.Click += new System.EventHandler(this.lblPaymentOption_Click);
            // 
            // btnLoadPreviousOrder
            // 
            this.btnLoadPreviousOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadPreviousOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnLoadPreviousOrder.Location = new System.Drawing.Point(133, 46);
            this.btnLoadPreviousOrder.Name = "btnLoadPreviousOrder";
            this.btnLoadPreviousOrder.Size = new System.Drawing.Size(197, 101);
            this.btnLoadPreviousOrder.TabIndex = 5;
            this.btnLoadPreviousOrder.Text = "Load Previous Order";
            this.btnLoadPreviousOrder.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(15, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(197, 101);
            this.btnClear.TabIndex = 31;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_click);
            // 
            // pnlBodyBottom
            // 
            this.pnlBodyBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.pnlBodyBottom.Controls.Add(this.pnlOrderAction);
            this.pnlBodyBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBodyBottom.Location = new System.Drawing.Point(0, 1057);
            this.pnlBodyBottom.Name = "pnlBodyBottom";
            this.pnlBodyBottom.Size = new System.Drawing.Size(1828, 125);
            this.pnlBodyBottom.TabIndex = 1;
            // 
            // pnlOrderAction
            // 
            this.pnlOrderAction.Controls.Add(this.pnlBasketBtns);
            this.pnlOrderAction.Controls.Add(this.pnlPrice);
            this.pnlOrderAction.Controls.Add(this.btnCallMe);
            this.pnlOrderAction.Controls.Add(this.btnLoadPreviousOrder);
            this.pnlOrderAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlOrderAction.Location = new System.Drawing.Point(0, -30);
            this.pnlOrderAction.Name = "pnlOrderAction";
            this.pnlOrderAction.Size = new System.Drawing.Size(1828, 155);
            this.pnlOrderAction.TabIndex = 35;
            // 
            // pnlBasketBtns
            // 
            this.pnlBasketBtns.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pnlBasketBtns.Controls.Add(this.btnKot);
            this.pnlBasketBtns.Controls.Add(this.btnPayment);
            this.pnlBasketBtns.Controls.Add(this.btnSaveOrder);
            this.pnlBasketBtns.Controls.Add(this.btnClear);
            this.pnlBasketBtns.Location = new System.Drawing.Point(330, 40);
            this.pnlBasketBtns.Name = "pnlBasketBtns";
            this.pnlBasketBtns.Size = new System.Drawing.Size(945, 110);
            this.pnlBasketBtns.TabIndex = 38;
            // 
            // btnKot
            // 
            this.btnKot.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnKot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnKot.ForeColor = System.Drawing.Color.Black;
            this.btnKot.Location = new System.Drawing.Point(745, 6);
            this.btnKot.Name = "btnKot";
            this.btnKot.Size = new System.Drawing.Size(197, 101);
            this.btnKot.TabIndex = 34;
            this.btnKot.Text = "Print";
            this.btnKot.UseVisualStyleBackColor = false;
            this.btnKot.Click += new System.EventHandler(this.btnKot_Click);
            // 
            // btnPayment
            // 
            this.btnPayment.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnPayment.ForeColor = System.Drawing.Color.Black;
            this.btnPayment.Location = new System.Drawing.Point(498, 6);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(197, 101);
            this.btnPayment.TabIndex = 36;
            this.btnPayment.Text = "Payment";
            this.btnPayment.UseVisualStyleBackColor = false;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            // 
            // btnSaveOrder
            // 
            this.btnSaveOrder.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSaveOrder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(85)))), ((int)(((byte)(126)))));
            this.btnSaveOrder.ForeColor = System.Drawing.Color.Black;
            this.btnSaveOrder.Location = new System.Drawing.Point(252, 6);
            this.btnSaveOrder.Name = "btnSaveOrder";
            this.btnSaveOrder.Size = new System.Drawing.Size(197, 101);
            this.btnSaveOrder.TabIndex = 35;
            this.btnSaveOrder.Text = "Save";
            this.btnSaveOrder.UseVisualStyleBackColor = false;
            this.btnSaveOrder.Click += new System.EventHandler(this.btnSaveOrder_Click);
            // 
            // pnlPrice
            // 
            this.pnlPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPrice.Controls.Add(this.lblAddNote);
            this.pnlPrice.Controls.Add(this.txtAddNote);
            this.pnlPrice.Controls.Add(this.btnDeliveryChargeAmend);
            this.pnlPrice.Controls.Add(this.txtDeliveryCharge);
            this.pnlPrice.Controls.Add(this.lblPaymentOption);
            this.pnlPrice.Controls.Add(this.txtTotalPrice);
            this.pnlPrice.Controls.Add(this.lblTotalPrice);
            this.pnlPrice.Controls.Add(this.cbCustomDiscount);
            this.pnlPrice.Location = new System.Drawing.Point(1402, 30);
            this.pnlPrice.Name = "pnlPrice";
            this.pnlPrice.Size = new System.Drawing.Size(426, 125);
            this.pnlPrice.TabIndex = 37;
            // 
            // lblAddNote
            // 
            this.lblAddNote.AutoSize = true;
            this.lblAddNote.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddNote.ForeColor = System.Drawing.Color.White;
            this.lblAddNote.Location = new System.Drawing.Point(3, 6);
            this.lblAddNote.Name = "lblAddNote";
            this.lblAddNote.Size = new System.Drawing.Size(157, 40);
            this.lblAddNote.TabIndex = 60;
            this.lblAddNote.Text = "Add Note:";
            // 
            // txtAddNote
            // 
            this.txtAddNote.Location = new System.Drawing.Point(102, 6);
            this.txtAddNote.Name = "txtAddNote";
            this.txtAddNote.Size = new System.Drawing.Size(133, 34);
            this.txtAddNote.TabIndex = 59;
            // 
            // btnDeliveryChargeAmend
            // 
            this.btnDeliveryChargeAmend.Location = new System.Drawing.Point(241, 0);
            this.btnDeliveryChargeAmend.Name = "btnDeliveryChargeAmend";
            this.btnDeliveryChargeAmend.Size = new System.Drawing.Size(91, 34);
            this.btnDeliveryChargeAmend.TabIndex = 58;
            this.btnDeliveryChargeAmend.Text = "Del Amend";
            this.btnDeliveryChargeAmend.UseVisualStyleBackColor = true;
            this.btnDeliveryChargeAmend.Click += new System.EventHandler(this.btnDeliveryChargeAmend_Click);
            // 
            // txtDeliveryCharge
            // 
            this.txtDeliveryCharge.Location = new System.Drawing.Point(338, 6);
            this.txtDeliveryCharge.Name = "txtDeliveryCharge";
            this.txtDeliveryCharge.Size = new System.Drawing.Size(81, 34);
            this.txtDeliveryCharge.TabIndex = 57;
            this.txtDeliveryCharge.TextChanged += new System.EventHandler(this.txtDeliveryCharge_TextChanged);
            // 
            // txtTotalPrice
            // 
            this.txtTotalPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalPrice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTotalPrice.Font = new System.Drawing.Font("Segoe UI Black", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalPrice.Location = new System.Drawing.Point(319, 92);
            this.txtTotalPrice.Name = "txtTotalPrice";
            this.txtTotalPrice.Size = new System.Drawing.Size(100, 39);
            this.txtTotalPrice.TabIndex = 14;
            this.txtTotalPrice.Text = "£0.00";
            this.txtTotalPrice.TextChanged += new System.EventHandler(this.txtTotalPrice_TextChanged);
            // 
            // lblTotalPrice
            // 
            this.lblTotalPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalPrice.AutoSize = true;
            this.lblTotalPrice.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalPrice.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPrice.ForeColor = System.Drawing.Color.White;
            this.lblTotalPrice.Location = new System.Drawing.Point(204, 91);
            this.lblTotalPrice.Name = "lblTotalPrice";
            this.lblTotalPrice.Size = new System.Drawing.Size(168, 40);
            this.lblTotalPrice.TabIndex = 10;
            this.lblTotalPrice.Text = "Total Price:";
            // 
            // cbCustomDiscount
            // 
            this.cbCustomDiscount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCustomDiscount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCustomDiscount.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCustomDiscount.FormattingEnabled = true;
            this.cbCustomDiscount.IntegralHeight = false;
            this.cbCustomDiscount.Items.AddRange(new object[] {
            "No Discount",
            "5% Discount",
            "10% Discount",
            "20% Discount",
            "30% Discount",
            "40% Discount",
            "50% Discount",
            "60% Discount",
            "70% Discount",
            "80% Discount",
            "90% Discount",
            "100% Discount"});
            this.cbCustomDiscount.Location = new System.Drawing.Point(53, 91);
            this.cbCustomDiscount.Name = "cbCustomDiscount";
            this.cbCustomDiscount.Size = new System.Drawing.Size(145, 36);
            this.cbCustomDiscount.TabIndex = 13;
            // 
            // btnCallMe
            // 
            this.btnCallMe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCallMe.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnCallMe.Location = new System.Drawing.Point(3, 112);
            this.btnCallMe.Name = "btnCallMe";
            this.btnCallMe.Size = new System.Drawing.Size(120, 37);
            this.btnCallMe.TabIndex = 0;
            this.btnCallMe.Text = "Call Me Test";
            this.btnCallMe.UseVisualStyleBackColor = true;
            this.btnCallMe.Click += new System.EventHandler(this.btnCallMe_Click);
            // 
            // basketGridView
            // 
            this.basketGridView.AllowUserToAddRows = false;
            this.basketGridView.AllowUserToDeleteRows = false;
            this.basketGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.basketGridView.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.basketGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.basketGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.basketGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvid,
            this.dgvName,
            this.dgvQty,
            this.dgvPrice,
            this.dgvOriginalPrice,
            this.dgvExtraCharge,
            this.dgvOrderNotes,
            this.dgvDeliveryCharge,
            this.dgvDeleteBasketItem,
            this.dgvExtraChargeValue});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.basketGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.basketGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.basketGridView.Location = new System.Drawing.Point(0, 0);
            this.basketGridView.Name = "basketGridView";
            this.basketGridView.RowHeadersVisible = false;
            this.basketGridView.RowHeadersWidth = 62;
            this.basketGridView.RowTemplate.Height = 28;
            this.basketGridView.Size = new System.Drawing.Size(429, 543);
            this.basketGridView.TabIndex = 8;
            this.basketGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.basketGridView_CellContentClick);
            // 
            // dgvid
            // 
            this.dgvid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvid.DataPropertyName = "catID";
            this.dgvid.FillWeight = 10F;
            this.dgvid.HeaderText = "id";
            this.dgvid.MinimumWidth = 8;
            this.dgvid.Name = "dgvid";
            this.dgvid.ReadOnly = true;
            this.dgvid.Visible = false;
            this.dgvid.Width = 10;
            // 
            // dgvName
            // 
            this.dgvName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvName.DataPropertyName = "catID";
            this.dgvName.HeaderText = "Item Name";
            this.dgvName.MinimumWidth = 8;
            this.dgvName.Name = "dgvName";
            // 
            // dgvQty
            // 
            this.dgvQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvQty.FillWeight = 50F;
            this.dgvQty.HeaderText = "Qty";
            this.dgvQty.MinimumWidth = 50;
            this.dgvQty.Name = "dgvQty";
            this.dgvQty.ReadOnly = true;
            // 
            // dgvPrice
            // 
            this.dgvPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvPrice.FillWeight = 55F;
            this.dgvPrice.HeaderText = "Price";
            this.dgvPrice.MinimumWidth = 55;
            this.dgvPrice.Name = "dgvPrice";
            this.dgvPrice.ReadOnly = true;
            // 
            // dgvOriginalPrice
            // 
            this.dgvOriginalPrice.HeaderText = "Original Price";
            this.dgvOriginalPrice.MinimumWidth = 8;
            this.dgvOriginalPrice.Name = "dgvOriginalPrice";
            this.dgvOriginalPrice.Visible = false;
            // 
            // dgvExtraCharge
            // 
            this.dgvExtraCharge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvExtraCharge.FillWeight = 50F;
            this.dgvExtraCharge.HeaderText = "";
            this.dgvExtraCharge.MinimumWidth = 50;
            this.dgvExtraCharge.Name = "dgvExtraCharge";
            this.dgvExtraCharge.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExtraCharge.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvExtraCharge.Text = "ADD";
            // 
            // dgvOrderNotes
            // 
            this.dgvOrderNotes.HeaderText = "Order Notes";
            this.dgvOrderNotes.MinimumWidth = 8;
            this.dgvOrderNotes.Name = "dgvOrderNotes";
            this.dgvOrderNotes.Visible = false;
            // 
            // dgvDeliveryCharge
            // 
            this.dgvDeliveryCharge.HeaderText = "Delivery Charge";
            this.dgvDeliveryCharge.MinimumWidth = 8;
            this.dgvDeliveryCharge.Name = "dgvDeliveryCharge";
            this.dgvDeliveryCharge.Visible = false;
            // 
            // dgvDeleteBasketItem
            // 
            this.dgvDeleteBasketItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvDeleteBasketItem.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDeleteBasketItem.FillWeight = 50F;
            this.dgvDeleteBasketItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.dgvDeleteBasketItem.HeaderText = "";
            this.dgvDeleteBasketItem.MinimumWidth = 50;
            this.dgvDeleteBasketItem.Name = "dgvDeleteBasketItem";
            this.dgvDeleteBasketItem.ReadOnly = true;
            this.dgvDeleteBasketItem.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDeleteBasketItem.Text = "REMOVE";
            this.dgvDeleteBasketItem.ToolTipText = "DELETE";
            this.dgvDeleteBasketItem.UseColumnTextForButtonValue = true;
            // 
            // dgvExtraChargeValue
            // 
            this.dgvExtraChargeValue.HeaderText = "Extra Charge";
            this.dgvExtraChargeValue.MinimumWidth = 8;
            this.dgvExtraChargeValue.Name = "dgvExtraChargeValue";
            this.dgvExtraChargeValue.Visible = false;
            // 
            // pnlMainGridHolder
            // 
            this.pnlMainGridHolder.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pnlMainGridHolder.Controls.Add(this.pnlOrderHolder);
            this.pnlMainGridHolder.Controls.Add(this.pnlItemHolder);
            this.pnlMainGridHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainGridHolder.Location = new System.Drawing.Point(0, 192);
            this.pnlMainGridHolder.Name = "pnlMainGridHolder";
            this.pnlMainGridHolder.Size = new System.Drawing.Size(1828, 865);
            this.pnlMainGridHolder.TabIndex = 15;
            // 
            // pnlOrderHolder
            // 
            this.pnlOrderHolder.BackColor = System.Drawing.Color.PaleGreen;
            this.pnlOrderHolder.Controls.Add(this.flpPreviousOrders);
            this.pnlOrderHolder.Controls.Add(this.basketGridView);
            this.pnlOrderHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOrderHolder.Location = new System.Drawing.Point(1399, 0);
            this.pnlOrderHolder.Name = "pnlOrderHolder";
            this.pnlOrderHolder.Size = new System.Drawing.Size(429, 865);
            this.pnlOrderHolder.TabIndex = 14;
            // 
            // flpPreviousOrders
            // 
            this.flpPreviousOrders.AutoScroll = true;
            this.flpPreviousOrders.BackColor = System.Drawing.Color.White;
            this.flpPreviousOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPreviousOrders.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpPreviousOrders.Location = new System.Drawing.Point(0, 543);
            this.flpPreviousOrders.Name = "flpPreviousOrders";
            this.flpPreviousOrders.Size = new System.Drawing.Size(429, 322);
            this.flpPreviousOrders.TabIndex = 10;
            this.flpPreviousOrders.WrapContents = false;
            // 
            // pnlItemHolder
            // 
            this.pnlItemHolder.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlItemHolder.Controls.Add(this.flpCategoryBtns);
            this.pnlItemHolder.Controls.Add(this.flpItemView);
            this.pnlItemHolder.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlItemHolder.Location = new System.Drawing.Point(0, 0);
            this.pnlItemHolder.Name = "pnlItemHolder";
            this.pnlItemHolder.Size = new System.Drawing.Size(1399, 865);
            this.pnlItemHolder.TabIndex = 10;
            // 
            // flpCategoryBtns
            // 
            this.flpCategoryBtns.BackColor = System.Drawing.Color.White;
            this.flpCategoryBtns.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpCategoryBtns.Location = new System.Drawing.Point(0, 0);
            this.flpCategoryBtns.Name = "flpCategoryBtns";
            this.flpCategoryBtns.Size = new System.Drawing.Size(190, 865);
            this.flpCategoryBtns.TabIndex = 53;
            // 
            // flpItemView
            // 
            this.flpItemView.BackColor = System.Drawing.Color.White;
            this.flpItemView.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpItemView.Location = new System.Drawing.Point(190, 0);
            this.flpItemView.Name = "flpItemView";
            this.flpItemView.Size = new System.Drawing.Size(1209, 865);
            this.flpItemView.TabIndex = 11;
            // 
            // rmDataSet1
            // 
            this.rmDataSet1.DataSetName = "RMDataSet";
            this.rmDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewImageColumn2.FillWeight = 50F;
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::OrderManagement.Properties.Resources.recycle_icon;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn2.MinimumWidth = 50;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewImageColumn3.FillWeight = 50F;
            this.dataGridViewImageColumn3.HeaderText = "";
            this.dataGridViewImageColumn3.Image = global::OrderManagement.Properties.Resources.plus_square_icon;
            this.dataGridViewImageColumn3.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dataGridViewImageColumn3.MinimumWidth = 50;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            // 
            // frmPOS
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1828, 1182);
            this.Controls.Add(this.pnlMainGridHolder);
            this.Controls.Add(this.pnlBodyBottom);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPOS";
            this.Text = "frmPOS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPOS_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlCstDetails.ResumeLayout(false);
            this.pnlCstDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlBodyBottom.ResumeLayout(false);
            this.pnlOrderAction.ResumeLayout(false);
            this.pnlBasketBtns.ResumeLayout(false);
            this.pnlPrice.ResumeLayout(false);
            this.pnlPrice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basketGridView)).EndInit();
            this.pnlMainGridHolder.ResumeLayout(false);
            this.pnlOrderHolder.ResumeLayout(false);
            this.pnlItemHolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rmDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlBodyBottom;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDineIn;
        private System.Windows.Forms.Button btnBill;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.PictureBox btnExit;
        private System.Windows.Forms.DataGridView basketGridView;
        private RMDataSet rmDataSet1;
        private System.Windows.Forms.Label lblTotalPrice;
        public System.Windows.Forms.TextBox txtTotalPrice;
        private System.Windows.Forms.Button btnCallMe;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnCol;
        private System.Windows.Forms.TextBox txtPreviousOrders;
        private System.Windows.Forms.TextBox txtCustomerDetails;
        private System.Windows.Forms.Button btnLoadPreviousOrder;
        private System.Windows.Forms.TextBox txtAddressDisplay;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel pnlCstDetails;
        private System.Windows.Forms.Panel pnlOrderAction;
        private System.Windows.Forms.Button btnCustomerAction;
        private System.Windows.Forms.TextBox txtCstTelephone;
        private System.Windows.Forms.Label lblCstAddress;
        private System.Windows.Forms.Label lblCstName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSaveOrder;
        private System.Windows.Forms.Panel pnlBasketHolder;
        private System.Windows.Forms.Panel pnlMainGridHolder;
        private System.Windows.Forms.Panel pnlOrderHolder;
        private System.Windows.Forms.FlowLayoutPanel flpPreviousOrders;
        private System.Windows.Forms.Panel pnlItemHolder;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.Button btnKot;
        private System.Windows.Forms.Button btnPayment;
        public System.Windows.Forms.ComboBox cbCustomDiscount;
        private System.Windows.Forms.Panel pnlPrice;
        private System.Windows.Forms.Panel pnlBasketBtns;
        private System.Windows.Forms.ListBox lstSearchResult;
        private System.Windows.Forms.FlowLayoutPanel flpItemView;
        private System.Windows.Forms.FlowLayoutPanel flpCategoryBtns;
        private System.Windows.Forms.Button btnCurrentOrders;
        private System.Windows.Forms.Button btnNotificationConfig;
        private System.Windows.Forms.Button btnWaiting;
        private System.Windows.Forms.Label lblPaymentOption;
        private System.Windows.Forms.TextBox txtDeliveryCharge;
        private System.Windows.Forms.Button btnDeliveryChargeAmend;
        private System.Windows.Forms.Button btnAutoAdd;
        private System.Windows.Forms.TextBox txtSearchFoodItem;
        private System.Windows.Forms.Label lblSearchFoodItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvOriginalPrice;
        private System.Windows.Forms.DataGridViewButtonColumn dgvExtraCharge;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvOrderNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvDeliveryCharge;
        private System.Windows.Forms.DataGridViewButtonColumn dgvDeleteBasketItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvExtraChargeValue;
        private System.Windows.Forms.TextBox txtAddNote;
        private System.Windows.Forms.Label lblAddNote;
    }
}
