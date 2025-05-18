using System;

namespace OrderManagement.Model
{
    partial class frmStaffAdd
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
            this.cbRole = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPostcode = new System.Windows.Forms.TextBox();
            this.lblAddressLine4 = new System.Windows.Forms.TextBox();
            this.lblAddressLine3 = new System.Windows.Forms.TextBox();
            this.lblAddressLine2 = new System.Windows.Forms.TextBox();
            this.lblAddressLine1 = new System.Windows.Forms.TextBox();
            this.lblHouseNameNumber = new System.Windows.Forms.TextBox();
            this.lblTelephoneNo = new System.Windows.Forms.TextBox();
            this.lblSurname = new System.Windows.Forms.TextBox();
            this.lblForename = new System.Windows.Forms.TextBox();
            this.txtPostcode = new System.Windows.Forms.TextBox();
            this.txtAddressLine4 = new System.Windows.Forms.TextBox();
            this.txtAddressLine3 = new System.Windows.Forms.TextBox();
            this.txtAddressLine2 = new System.Windows.Forms.TextBox();
            this.txtAddressLine1 = new System.Windows.Forms.TextBox();
            this.txtHouseNameNumber = new System.Windows.Forms.TextBox();
            this.txtTelephoneNo = new System.Windows.Forms.TextBox();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.txtForename = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Size = new System.Drawing.Size(898, 134);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 575);
            this.panel2.Size = new System.Drawing.Size(898, 98);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(182, 37);
            this.label1.Text = "Staff Add/Edit";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OrderManagement.Properties.Resources.male_add_icon5;

            // 
            // cbRole
            // 
            this.cbRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRole.FormattingEnabled = true;
            this.cbRole.Items.AddRange(new object[] {
            "Manager",
            "Admin",
            "Waiter",
            "Chef",
            "Cook",
            "Cleaner",
            "Driver",
            "Other"});
            this.cbRole.Location = new System.Drawing.Point(12, 177);
            this.cbRole.Name = "cbRole";
            this.cbRole.Size = new System.Drawing.Size(246, 25);
            this.cbRole.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "Role";
            // 
            // lblPostcode
            // 
            this.lblPostcode.Enabled = false;
            this.lblPostcode.Location = new System.Drawing.Point(12, 482);
            this.lblPostcode.Name = "lblPostcode";
            this.lblPostcode.Size = new System.Drawing.Size(246, 25);
            this.lblPostcode.TabIndex = 37;
            this.lblPostcode.Text = "Postcode";
            // 
            // lblAddressLine4
            // 
            this.lblAddressLine4.Enabled = false;
            this.lblAddressLine4.Location = new System.Drawing.Point(12, 451);
            this.lblAddressLine4.Name = "lblAddressLine4";
            this.lblAddressLine4.Size = new System.Drawing.Size(246, 25);
            this.lblAddressLine4.TabIndex = 36;
            this.lblAddressLine4.Text = "Address Line 4";
            // 
            // lblAddressLine3
            // 
            this.lblAddressLine3.Enabled = false;
            this.lblAddressLine3.Location = new System.Drawing.Point(12, 420);
            this.lblAddressLine3.Name = "lblAddressLine3";
            this.lblAddressLine3.Size = new System.Drawing.Size(246, 25);
            this.lblAddressLine3.TabIndex = 35;
            this.lblAddressLine3.Text = "Address Line 3";
            // 
            // lblAddressLine2
            // 
            this.lblAddressLine2.Enabled = false;
            this.lblAddressLine2.Location = new System.Drawing.Point(12, 389);
            this.lblAddressLine2.Name = "lblAddressLine2";
            this.lblAddressLine2.Size = new System.Drawing.Size(246, 25);
            this.lblAddressLine2.TabIndex = 34;
            this.lblAddressLine2.Text = "Address Line 2";
            // 
            // lblAddressLine1
            // 
            this.lblAddressLine1.Enabled = false;
            this.lblAddressLine1.Location = new System.Drawing.Point(12, 358);
            this.lblAddressLine1.Name = "lblAddressLine1";
            this.lblAddressLine1.Size = new System.Drawing.Size(246, 25);
            this.lblAddressLine1.TabIndex = 33;
            this.lblAddressLine1.Text = "Address Line 1";
            // 
            // lblHouseNameNumber
            // 
            this.lblHouseNameNumber.Enabled = false;
            this.lblHouseNameNumber.Location = new System.Drawing.Point(12, 329);
            this.lblHouseNameNumber.Name = "lblHouseNameNumber";
            this.lblHouseNameNumber.Size = new System.Drawing.Size(246, 25);
            this.lblHouseNameNumber.TabIndex = 32;
            this.lblHouseNameNumber.Text = "House Name/Number";
            // 
            // lblTelephoneNo
            // 
            this.lblTelephoneNo.Enabled = false;
            this.lblTelephoneNo.Location = new System.Drawing.Point(12, 294);
            this.lblTelephoneNo.Name = "lblTelephoneNo";
            this.lblTelephoneNo.Size = new System.Drawing.Size(246, 25);
            this.lblTelephoneNo.TabIndex = 31;
            this.lblTelephoneNo.Text = "Telephone Number";
            // 
            // lblSurname
            // 
            this.lblSurname.Enabled = false;
            this.lblSurname.Location = new System.Drawing.Point(12, 263);
            this.lblSurname.Name = "lblSurname";
            this.lblSurname.Size = new System.Drawing.Size(246, 25);
            this.lblSurname.TabIndex = 30;
            this.lblSurname.Text = "Surname";
            // 
            // lblForename
            // 
            this.lblForename.Enabled = false;
            this.lblForename.Location = new System.Drawing.Point(12, 232);
            this.lblForename.Name = "lblForename";
            this.lblForename.Size = new System.Drawing.Size(246, 25);
            this.lblForename.TabIndex = 29;
            this.lblForename.Text = "Forename";
            // 
            // txtPostcode
            // 
            this.txtPostcode.Location = new System.Drawing.Point(272, 482);
            this.txtPostcode.Name = "txtPostcode";
            this.txtPostcode.Size = new System.Drawing.Size(605, 25);
            this.txtPostcode.TabIndex = 28;
            // 
            // txtAddressLine4
            // 
            this.txtAddressLine4.Location = new System.Drawing.Point(272, 451);
            this.txtAddressLine4.Name = "txtAddressLine4";
            this.txtAddressLine4.Size = new System.Drawing.Size(605, 25);
            this.txtAddressLine4.TabIndex = 27;
            // 
            // txtAddressLine3
            // 
            this.txtAddressLine3.Location = new System.Drawing.Point(272, 420);
            this.txtAddressLine3.Name = "txtAddressLine3";
            this.txtAddressLine3.Size = new System.Drawing.Size(605, 25);
            this.txtAddressLine3.TabIndex = 26;
            // 
            // txtAddressLine2
            // 
            this.txtAddressLine2.Location = new System.Drawing.Point(272, 389);
            this.txtAddressLine2.Name = "txtAddressLine2";
            this.txtAddressLine2.Size = new System.Drawing.Size(605, 25);
            this.txtAddressLine2.TabIndex = 25;
            // 
            // txtAddressLine1
            // 
            this.txtAddressLine1.Location = new System.Drawing.Point(272, 358);
            this.txtAddressLine1.Name = "txtAddressLine1";
            this.txtAddressLine1.Size = new System.Drawing.Size(605, 25);
            this.txtAddressLine1.TabIndex = 24;
            // 
            // txtHouseNameNumber
            // 
            this.txtHouseNameNumber.Location = new System.Drawing.Point(272, 327);
            this.txtHouseNameNumber.Name = "txtHouseNameNumber";
            this.txtHouseNameNumber.Size = new System.Drawing.Size(605, 25);
            this.txtHouseNameNumber.TabIndex = 23;
            // 
            // txtTelephoneNo
            // 
            this.txtTelephoneNo.Location = new System.Drawing.Point(272, 294);
            this.txtTelephoneNo.Name = "txtTelephoneNo";
            this.txtTelephoneNo.Size = new System.Drawing.Size(605, 25);
            this.txtTelephoneNo.TabIndex = 22;
            // 
            // txtSurname
            // 
            this.txtSurname.Location = new System.Drawing.Point(272, 263);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(605, 25);
            this.txtSurname.TabIndex = 21;
            // 
            // txtForename
            // 
            this.txtForename.Location = new System.Drawing.Point(272, 232);
            this.txtForename.Name = "txtForename";
            this.txtForename.Size = new System.Drawing.Size(605, 25);
            this.txtForename.TabIndex = 20;
            // 
            // frmStaffAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 673);
            this.Controls.Add(this.lblPostcode);
            this.Controls.Add(this.lblAddressLine4);
            this.Controls.Add(this.lblAddressLine3);
            this.Controls.Add(this.lblAddressLine2);
            this.Controls.Add(this.lblAddressLine1);
            this.Controls.Add(this.lblHouseNameNumber);
            this.Controls.Add(this.lblTelephoneNo);
            this.Controls.Add(this.lblSurname);
            this.Controls.Add(this.lblForename);
            this.Controls.Add(this.txtPostcode);
            this.Controls.Add(this.txtAddressLine4);
            this.Controls.Add(this.txtAddressLine3);
            this.Controls.Add(this.txtAddressLine2);
            this.Controls.Add(this.txtAddressLine1);
            this.Controls.Add(this.txtHouseNameNumber);
            this.Controls.Add(this.txtTelephoneNo);
            this.Controls.Add(this.txtSurname);
            this.Controls.Add(this.txtForename);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbRole);
            this.Name = "frmStaffAdd";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.cbRole, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtForename, 0);
            this.Controls.SetChildIndex(this.txtSurname, 0);
            this.Controls.SetChildIndex(this.txtTelephoneNo, 0);
            this.Controls.SetChildIndex(this.txtHouseNameNumber, 0);
            this.Controls.SetChildIndex(this.txtAddressLine1, 0);
            this.Controls.SetChildIndex(this.txtAddressLine2, 0);
            this.Controls.SetChildIndex(this.txtAddressLine3, 0);
            this.Controls.SetChildIndex(this.txtAddressLine4, 0);
            this.Controls.SetChildIndex(this.txtPostcode, 0);
            this.Controls.SetChildIndex(this.lblForename, 0);
            this.Controls.SetChildIndex(this.lblSurname, 0);
            this.Controls.SetChildIndex(this.lblTelephoneNo, 0);
            this.Controls.SetChildIndex(this.lblHouseNameNumber, 0);
            this.Controls.SetChildIndex(this.lblAddressLine1, 0);
            this.Controls.SetChildIndex(this.lblAddressLine2, 0);
            this.Controls.SetChildIndex(this.lblAddressLine3, 0);
            this.Controls.SetChildIndex(this.lblAddressLine4, 0);
            this.Controls.SetChildIndex(this.lblPostcode, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox lblPostcode;
        public System.Windows.Forms.TextBox lblAddressLine4;
        public System.Windows.Forms.TextBox lblAddressLine3;
        public System.Windows.Forms.TextBox lblAddressLine2;
        public System.Windows.Forms.TextBox lblAddressLine1;
        public System.Windows.Forms.TextBox lblHouseNameNumber;
        public System.Windows.Forms.TextBox lblTelephoneNo;
        public System.Windows.Forms.TextBox lblSurname;
        public System.Windows.Forms.TextBox lblForename;
        public System.Windows.Forms.TextBox txtPostcode;
        public System.Windows.Forms.TextBox txtAddressLine4;
        public System.Windows.Forms.TextBox txtAddressLine3;
        public System.Windows.Forms.TextBox txtAddressLine2;
        public System.Windows.Forms.TextBox txtAddressLine1;
        public System.Windows.Forms.TextBox txtHouseNameNumber;
        public System.Windows.Forms.TextBox txtTelephoneNo;
        public System.Windows.Forms.TextBox txtSurname;
        public System.Windows.Forms.TextBox txtForename;
        public System.Windows.Forms.ComboBox cbRole;
    }
}