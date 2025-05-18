namespace OrderManagement.View
{
    partial class frmStaffView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.staffViewDataGrid = new System.Windows.Forms.DataGridView();
            this.dgvid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvForename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSurname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTelephoneNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHouseNameNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddressLine1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddressLine2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddressLine3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAddressLine4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPostcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvedit = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvdel = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffViewDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.Location = new System.Drawing.Point(934, 62);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.Location = new System.Drawing.Point(5, 54);
            this.label2.Size = new System.Drawing.Size(82, 25);
            this.label2.Text = "Staff List";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtSearch.Location = new System.Drawing.Point(939, 93);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.Location = new System.Drawing.Point(12, 104);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(1, 165);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1414, 31);
            this.panel2.TabIndex = 6;
            // 
            // staffViewDataGrid
            // 
            this.staffViewDataGrid.AllowUserToAddRows = false;
            this.staffViewDataGrid.AllowUserToDeleteRows = false;
            this.staffViewDataGrid.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.staffViewDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.staffViewDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.staffViewDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvid,
            this.dgvRole,
            this.dgvForename,
            this.dgvSurname,
            this.dgvTelephoneNo,
            this.dgvHouseNameNumber,
            this.dgvAddressLine1,
            this.dgvAddressLine2,
            this.dgvAddressLine3,
            this.dgvAddressLine4,
            this.dgvPostcode,
            this.dgvedit,
            this.dgvdel});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.staffViewDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.staffViewDataGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.staffViewDataGrid.Location = new System.Drawing.Point(0, 219);
            this.staffViewDataGrid.Name = "staffViewDataGrid";
            this.staffViewDataGrid.ReadOnly = true;
            this.staffViewDataGrid.RowHeadersWidth = 62;
            this.staffViewDataGrid.RowTemplate.Height = 28;
            this.staffViewDataGrid.Size = new System.Drawing.Size(1428, 783);
            this.staffViewDataGrid.TabIndex = 8;
            this.staffViewDataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.StaffViewDataGrid_CellContentClick);
            this.staffViewDataGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.staffViewDataGrid_CellContentClick_1);
            // 
            // dgvid
            // 
            this.dgvid.DataPropertyName = "id";
            this.dgvid.HeaderText = "ID";
            this.dgvid.MinimumWidth = 8;
            this.dgvid.Name = "dgvid";
            this.dgvid.ReadOnly = true;
            this.dgvid.Visible = false;
            this.dgvid.Width = 150;
            // 
            // dgvRole
            // 
            this.dgvRole.DataPropertyName = "role";
            this.dgvRole.HeaderText = "Role";
            this.dgvRole.Name = "dgvRole";
            this.dgvRole.ReadOnly = true;
            // 
            // dgvForename
            // 
            this.dgvForename.DataPropertyName = "forename";
            this.dgvForename.HeaderText = "Forename";
            this.dgvForename.MinimumWidth = 8;
            this.dgvForename.Name = "dgvForename";
            this.dgvForename.ReadOnly = true;
            this.dgvForename.Width = 150;
            // 
            // dgvSurname
            // 
            this.dgvSurname.DataPropertyName = "surname";
            this.dgvSurname.HeaderText = "Surname";
            this.dgvSurname.MinimumWidth = 8;
            this.dgvSurname.Name = "dgvSurname";
            this.dgvSurname.ReadOnly = true;
            this.dgvSurname.Width = 150;
            // 
            // dgvTelephoneNo
            // 
            this.dgvTelephoneNo.DataPropertyName = "telephoneNo";
            this.dgvTelephoneNo.HeaderText = "Telephone Number";
            this.dgvTelephoneNo.MinimumWidth = 8;
            this.dgvTelephoneNo.Name = "dgvTelephoneNo";
            this.dgvTelephoneNo.ReadOnly = true;
            this.dgvTelephoneNo.Width = 150;
            // 
            // dgvHouseNameNumber
            // 
            this.dgvHouseNameNumber.DataPropertyName = "houseNameNumber";
            this.dgvHouseNameNumber.HeaderText = "House Name/Number";
            this.dgvHouseNameNumber.MinimumWidth = 8;
            this.dgvHouseNameNumber.Name = "dgvHouseNameNumber";
            this.dgvHouseNameNumber.ReadOnly = true;
            this.dgvHouseNameNumber.Width = 150;
            // 
            // dgvAddressLine1
            // 
            this.dgvAddressLine1.DataPropertyName = "AddressLine1";
            this.dgvAddressLine1.HeaderText = "Address Line 1";
            this.dgvAddressLine1.MinimumWidth = 8;
            this.dgvAddressLine1.Name = "dgvAddressLine1";
            this.dgvAddressLine1.ReadOnly = true;
            this.dgvAddressLine1.Width = 150;
            // 
            // dgvAddressLine2
            // 
            this.dgvAddressLine2.DataPropertyName = "AddressLine2";
            this.dgvAddressLine2.HeaderText = "Address Line 2";
            this.dgvAddressLine2.MinimumWidth = 8;
            this.dgvAddressLine2.Name = "dgvAddressLine2";
            this.dgvAddressLine2.ReadOnly = true;
            this.dgvAddressLine2.Width = 150;
            // 
            // dgvAddressLine3
            // 
            this.dgvAddressLine3.DataPropertyName = "AddressLine3";
            this.dgvAddressLine3.HeaderText = "Address Line 3";
            this.dgvAddressLine3.MinimumWidth = 8;
            this.dgvAddressLine3.Name = "dgvAddressLine3";
            this.dgvAddressLine3.ReadOnly = true;
            this.dgvAddressLine3.Width = 150;
            // 
            // dgvAddressLine4
            // 
            this.dgvAddressLine4.DataPropertyName = "AddressLine4";
            this.dgvAddressLine4.HeaderText = "Address Line 4";
            this.dgvAddressLine4.MinimumWidth = 8;
            this.dgvAddressLine4.Name = "dgvAddressLine4";
            this.dgvAddressLine4.ReadOnly = true;
            this.dgvAddressLine4.Width = 150;
            // 
            // dgvPostcode
            // 
            this.dgvPostcode.DataPropertyName = "Postcode";
            this.dgvPostcode.HeaderText = "Postcode";
            this.dgvPostcode.MinimumWidth = 8;
            this.dgvPostcode.Name = "dgvPostcode";
            this.dgvPostcode.ReadOnly = true;
            this.dgvPostcode.Width = 150;
            // 
            // dgvedit
            // 
            this.dgvedit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvedit.FillWeight = 50F;
            this.dgvedit.HeaderText = "";
            this.dgvedit.Image = global::OrderManagement.Properties.Resources.pen_tool_icon1;
            this.dgvedit.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvedit.MinimumWidth = 50;
            this.dgvedit.Name = "dgvedit";
            this.dgvedit.ReadOnly = true;
            this.dgvedit.Width = 50;
            // 
            // dgvdel
            // 
            this.dgvdel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvdel.FillWeight = 50F;
            this.dgvdel.HeaderText = "";
            this.dgvdel.Image = global::OrderManagement.Properties.Resources.recycle_icon1;
            this.dgvdel.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.dgvdel.MinimumWidth = 50;
            this.dgvdel.Name = "dgvdel";
            this.dgvdel.ReadOnly = true;
            this.dgvdel.Width = 50;
            // 
            // frmStaffView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 1002);
            this.Controls.Add(this.staffViewDataGrid);
            this.Controls.Add(this.panel2);
            this.Name = "frmStaffView";
            this.Text = "frmStaffView";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.txtSearch, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.staffViewDataGrid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffViewDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView staffViewDataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvForename;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSurname;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTelephoneNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvHouseNameNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddressLine1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddressLine2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddressLine3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvAddressLine4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPostcode;
        private System.Windows.Forms.DataGridViewImageColumn dgvedit;
        private System.Windows.Forms.DataGridViewImageColumn dgvdel;
    }
}