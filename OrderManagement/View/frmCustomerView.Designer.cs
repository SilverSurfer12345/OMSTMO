namespace OrderManagement.View
{
    partial class frmCustomerView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label3 = new System.Windows.Forms.Label();
            this.customerViewDataGrid = new System.Windows.Forms.DataGridView();
            this.dgvid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvForename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSurname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTelephoneNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHouseNameNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddressLine1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddressLine2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddressLine3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddressLine4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPostcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPreviousOrders = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvedit = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvdel = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerViewDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(963, 18);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(451, 47);
            this.label2.Size = new System.Drawing.Size(0, 38);
            this.label2.Text = "";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(968, 49);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 32);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(32, 32);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, -5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(276, 38);
            this.label3.TabIndex = 6;
            this.label3.Text = "Add/View Customers";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // customerViewDataGrid
            // 
            this.customerViewDataGrid.AllowUserToAddRows = false;
            this.customerViewDataGrid.AllowUserToDeleteRows = false;
            this.customerViewDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customerViewDataGrid.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customerViewDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.customerViewDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customerViewDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvid,
            this.dgvForename,
            this.dgvSurname,
            this.dgvTelephoneNo,
            this.dgvEmail,
            this.dgvHouseNameNumber,
            this.dgvAddressLine1,
            this.dgvAddressLine2,
            this.dgvAddressLine3,
            this.dgvAddressLine4,
            this.dgvPostcode,
            this.dgvPreviousOrders,
            this.dgvedit,
            this.dgvdel});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customerViewDataGrid.DefaultCellStyle = dataGridViewCellStyle8;
            this.customerViewDataGrid.Location = new System.Drawing.Point(0, 138);
            this.customerViewDataGrid.Name = "customerViewDataGrid";
            this.customerViewDataGrid.ReadOnly = true;
            this.customerViewDataGrid.RowHeadersVisible = false;
            this.customerViewDataGrid.RowHeadersWidth = 62;
            this.customerViewDataGrid.RowTemplate.Height = 28;
            this.customerViewDataGrid.Size = new System.Drawing.Size(1456, 805);
            this.customerViewDataGrid.TabIndex = 7;
            this.customerViewDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customerViewDataGrid_CellClick);
            this.customerViewDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customerViewDataGrid_CellContentClick);
            // 
            // dgvid
            // 
            this.dgvid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvid.DataPropertyName = "Id";
            this.dgvid.HeaderText = "ID";
            this.dgvid.MinimumWidth = 8;
            this.dgvid.Name = "dgvid";
            this.dgvid.ReadOnly = true;
            // 
            // dgvForename
            // 
            this.dgvForename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvForename.DataPropertyName = "forename";
            this.dgvForename.HeaderText = "Forename";
            this.dgvForename.MinimumWidth = 8;
            this.dgvForename.Name = "dgvForename";
            this.dgvForename.ReadOnly = true;
            // 
            // dgvSurname
            // 
            this.dgvSurname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvSurname.DataPropertyName = "surname";
            this.dgvSurname.HeaderText = "Surname";
            this.dgvSurname.MinimumWidth = 8;
            this.dgvSurname.Name = "dgvSurname";
            this.dgvSurname.ReadOnly = true;
            // 
            // dgvTelephoneNo
            // 
            this.dgvTelephoneNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvTelephoneNo.DataPropertyName = "telephoneNo";
            this.dgvTelephoneNo.HeaderText = "Telephone Number";
            this.dgvTelephoneNo.MinimumWidth = 8;
            this.dgvTelephoneNo.Name = "dgvTelephoneNo";
            this.dgvTelephoneNo.ReadOnly = true;
            // 
            // dgvEmail
            // 
            this.dgvEmail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvEmail.DataPropertyName = "Email";
            this.dgvEmail.HeaderText = "Email";
            this.dgvEmail.MinimumWidth = 8;
            this.dgvEmail.Name = "dgvEmail";
            this.dgvEmail.ReadOnly = true;
            // 
            // dgvHouseNameNumber
            // 
            this.dgvHouseNameNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvHouseNameNumber.DataPropertyName = "houseNameNumber";
            this.dgvHouseNameNumber.HeaderText = "House Name/Number";
            this.dgvHouseNameNumber.MinimumWidth = 8;
            this.dgvHouseNameNumber.Name = "dgvHouseNameNumber";
            this.dgvHouseNameNumber.ReadOnly = true;
            // 
            // dgvAddressLine1
            // 
            this.dgvAddressLine1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvAddressLine1.DataPropertyName = "AddressLine1";
            this.dgvAddressLine1.HeaderText = "Address Line 1";
            this.dgvAddressLine1.MinimumWidth = 8;
            this.dgvAddressLine1.Name = "dgvAddressLine1";
            this.dgvAddressLine1.ReadOnly = true;
            // 
            // dgvAddressLine2
            // 
            this.dgvAddressLine2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvAddressLine2.DataPropertyName = "AddressLine2";
            this.dgvAddressLine2.HeaderText = "Address Line 2";
            this.dgvAddressLine2.MinimumWidth = 8;
            this.dgvAddressLine2.Name = "dgvAddressLine2";
            this.dgvAddressLine2.ReadOnly = true;
            // 
            // dgvAddressLine3
            // 
            this.dgvAddressLine3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvAddressLine3.DataPropertyName = "AddressLine3";
            this.dgvAddressLine3.HeaderText = "Address Line 3";
            this.dgvAddressLine3.MinimumWidth = 8;
            this.dgvAddressLine3.Name = "dgvAddressLine3";
            this.dgvAddressLine3.ReadOnly = true;
            // 
            // dgvAddressLine4
            // 
            this.dgvAddressLine4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvAddressLine4.DataPropertyName = "AddressLine4";
            this.dgvAddressLine4.HeaderText = "Address Line 4";
            this.dgvAddressLine4.MinimumWidth = 8;
            this.dgvAddressLine4.Name = "dgvAddressLine4";
            this.dgvAddressLine4.ReadOnly = true;
            // 
            // dgvPostcode
            // 
            this.dgvPostcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvPostcode.DataPropertyName = "Postcode";
            this.dgvPostcode.HeaderText = "Postcode";
            this.dgvPostcode.MinimumWidth = 8;
            this.dgvPostcode.Name = "dgvPostcode";
            this.dgvPostcode.ReadOnly = true;
            // 
            // dgvPreviousOrders
            // 
            this.dgvPreviousOrders.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvPreviousOrders.DataPropertyName = "previousOrders";
            this.dgvPreviousOrders.HeaderText = "Previous Orders";
            this.dgvPreviousOrders.MinimumWidth = 8;
            this.dgvPreviousOrders.Name = "dgvPreviousOrders";
            this.dgvPreviousOrders.ReadOnly = true;
            // 
            // dgvedit
            // 
            this.dgvedit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvedit.FillWeight = 50F;
            this.dgvedit.HeaderText = "";
            this.dgvedit.Image = global::OrderManagement.Properties.Resources.pen_tool_icon1;
            this.dgvedit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvedit.MinimumWidth = 50;
            this.dgvedit.Name = "dgvedit";
            this.dgvedit.ReadOnly = true;
            this.dgvedit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvdel
            // 
            this.dgvdel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvdel.FillWeight = 50F;
            this.dgvdel.HeaderText = "";
            this.dgvdel.Image = global::OrderManagement.Properties.Resources.recycle_icon1;
            this.dgvdel.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvdel.MinimumWidth = 50;
            this.dgvdel.Name = "dgvdel";
            this.dgvdel.ReadOnly = true;
            this.dgvdel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(12, 160);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1416, 54);
            this.panel2.TabIndex = 8;
            // 
            // frmCustomerView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1456, 943);
            this.Controls.Add(this.customerViewDataGrid);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAdd);
            this.Name = "frmCustomerView";
            this.Text = "frmCustomerView";
            this.Load += new System.EventHandler(this.frmCustomerView_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtSearch, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.customerViewDataGrid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerViewDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView customerViewDataGrid;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvForename;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSurname;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTelephoneNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvHouseNameNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddressLine1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddressLine2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddressLine3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddressLine4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPostcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPreviousOrders;
        private System.Windows.Forms.DataGridViewImageColumn dgvedit;
        private System.Windows.Forms.DataGridViewImageColumn dgvdel;
        private System.Windows.Forms.PictureBox btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearch;
    }
}