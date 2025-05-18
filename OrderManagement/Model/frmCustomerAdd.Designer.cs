namespace OrderManagement.Model
{
    partial class frmCustomerAdd
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
            this.txtForename = new System.Windows.Forms.TextBox();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.txtTelephoneNo = new System.Windows.Forms.TextBox();
            this.txtHouseNameNumber = new System.Windows.Forms.TextBox();
            this.txtAddressLine1 = new System.Windows.Forms.TextBox();
            this.txtAddressLine2 = new System.Windows.Forms.TextBox();
            this.txtAddressLine3 = new System.Windows.Forms.TextBox();
            this.txtAddressLine4 = new System.Windows.Forms.TextBox();
            this.txtPostcode = new System.Windows.Forms.TextBox();
            this.lblForename = new System.Windows.Forms.TextBox();
            this.lblSurname = new System.Windows.Forms.TextBox();
            this.lblTelephoneNo = new System.Windows.Forms.TextBox();
            this.lblHouseNameNumber = new System.Windows.Forms.TextBox();
            this.lblAddressLine1 = new System.Windows.Forms.TextBox();
            this.lblAddressLine2 = new System.Windows.Forms.TextBox();
            this.lblAddressLine3 = new System.Windows.Forms.TextBox();
            this.lblAddressLine4 = new System.Windows.Forms.TextBox();
            this.lblPostcode = new System.Windows.Forms.TextBox();
            this.btnPostcodeFind = new System.Windows.Forms.Button();
            this.lblEmail = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(187, 37);
            this.label1.Text = "Add Customer";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OrderManagement.Properties.Resources.male_add_icon2;
            // 
            // btnSave
            // 
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtForename
            // 
            this.txtForename.Location = new System.Drawing.Point(289, 187);
            this.txtForename.Name = "txtForename";
            this.txtForename.Size = new System.Drawing.Size(605, 25);
            this.txtForename.TabIndex = 2;
            this.txtForename.TextChanged += new System.EventHandler(this.txtForename_TextChanged);
            // 
            // txtSurname
            // 
            this.txtSurname.Location = new System.Drawing.Point(289, 218);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(605, 25);
            this.txtSurname.TabIndex = 3;
            // 
            // txtTelephoneNo
            // 
            this.txtTelephoneNo.Location = new System.Drawing.Point(289, 249);
            this.txtTelephoneNo.Name = "txtTelephoneNo";
            this.txtTelephoneNo.Size = new System.Drawing.Size(605, 25);
            this.txtTelephoneNo.TabIndex = 4;
            // 
            // txtHouseNameNumber
            // 
            this.txtHouseNameNumber.Location = new System.Drawing.Point(289, 311);
            this.txtHouseNameNumber.Name = "txtHouseNameNumber";
            this.txtHouseNameNumber.Size = new System.Drawing.Size(605, 25);
            this.txtHouseNameNumber.TabIndex = 5;
            this.txtHouseNameNumber.TextChanged += new System.EventHandler(this.txtHouseNameNumber_TextChanged);
            // 
            // txtAddressLine1
            // 
            this.txtAddressLine1.Location = new System.Drawing.Point(289, 342);
            this.txtAddressLine1.Name = "txtAddressLine1";
            this.txtAddressLine1.Size = new System.Drawing.Size(605, 25);
            this.txtAddressLine1.TabIndex = 6;
            // 
            // txtAddressLine2
            // 
            this.txtAddressLine2.Location = new System.Drawing.Point(289, 373);
            this.txtAddressLine2.Name = "txtAddressLine2";
            this.txtAddressLine2.Size = new System.Drawing.Size(605, 25);
            this.txtAddressLine2.TabIndex = 7;
            // 
            // txtAddressLine3
            // 
            this.txtAddressLine3.Location = new System.Drawing.Point(289, 404);
            this.txtAddressLine3.Name = "txtAddressLine3";
            this.txtAddressLine3.Size = new System.Drawing.Size(605, 25);
            this.txtAddressLine3.TabIndex = 8;
            // 
            // txtAddressLine4
            // 
            this.txtAddressLine4.Location = new System.Drawing.Point(289, 435);
            this.txtAddressLine4.Name = "txtAddressLine4";
            this.txtAddressLine4.Size = new System.Drawing.Size(605, 25);
            this.txtAddressLine4.TabIndex = 9;
            // 
            // txtPostcode
            // 
            this.txtPostcode.Location = new System.Drawing.Point(289, 466);
            this.txtPostcode.Name = "txtPostcode";
            this.txtPostcode.Size = new System.Drawing.Size(605, 25);
            this.txtPostcode.TabIndex = 10;
            this.txtPostcode.TextChanged += new System.EventHandler(this.txtPostcode_TextChanged);
            // 
            // lblForename
            // 
            this.lblForename.Enabled = false;
            this.lblForename.Location = new System.Drawing.Point(29, 187);
            this.lblForename.Name = "lblForename";
            this.lblForename.Size = new System.Drawing.Size(246, 25);
            this.lblForename.TabIndex = 11;
            this.lblForename.Text = "Forename";
            // 
            // lblSurname
            // 
            this.lblSurname.Enabled = false;
            this.lblSurname.Location = new System.Drawing.Point(29, 218);
            this.lblSurname.Name = "lblSurname";
            this.lblSurname.Size = new System.Drawing.Size(246, 25);
            this.lblSurname.TabIndex = 12;
            this.lblSurname.Text = "Surname";
            // 
            // lblTelephoneNo
            // 
            this.lblTelephoneNo.Enabled = false;
            this.lblTelephoneNo.Location = new System.Drawing.Point(29, 249);
            this.lblTelephoneNo.Name = "lblTelephoneNo";
            this.lblTelephoneNo.Size = new System.Drawing.Size(246, 25);
            this.lblTelephoneNo.TabIndex = 13;
            this.lblTelephoneNo.Text = "Telephone Number";
            // 
            // lblHouseNameNumber
            // 
            this.lblHouseNameNumber.Enabled = false;
            this.lblHouseNameNumber.Location = new System.Drawing.Point(29, 313);
            this.lblHouseNameNumber.Name = "lblHouseNameNumber";
            this.lblHouseNameNumber.Size = new System.Drawing.Size(246, 25);
            this.lblHouseNameNumber.TabIndex = 14;
            this.lblHouseNameNumber.Text = "House Name/Number";
            // 
            // lblAddressLine1
            // 
            this.lblAddressLine1.Enabled = false;
            this.lblAddressLine1.Location = new System.Drawing.Point(29, 342);
            this.lblAddressLine1.Name = "lblAddressLine1";
            this.lblAddressLine1.Size = new System.Drawing.Size(246, 25);
            this.lblAddressLine1.TabIndex = 15;
            this.lblAddressLine1.Text = "Address Line 1";
            // 
            // lblAddressLine2
            // 
            this.lblAddressLine2.Enabled = false;
            this.lblAddressLine2.Location = new System.Drawing.Point(29, 373);
            this.lblAddressLine2.Name = "lblAddressLine2";
            this.lblAddressLine2.Size = new System.Drawing.Size(246, 25);
            this.lblAddressLine2.TabIndex = 16;
            this.lblAddressLine2.Text = "Address Line 2";
            // 
            // lblAddressLine3
            // 
            this.lblAddressLine3.Enabled = false;
            this.lblAddressLine3.Location = new System.Drawing.Point(29, 404);
            this.lblAddressLine3.Name = "lblAddressLine3";
            this.lblAddressLine3.Size = new System.Drawing.Size(246, 25);
            this.lblAddressLine3.TabIndex = 17;
            this.lblAddressLine3.Text = "Address Line 3";
            // 
            // lblAddressLine4
            // 
            this.lblAddressLine4.Enabled = false;
            this.lblAddressLine4.Location = new System.Drawing.Point(29, 435);
            this.lblAddressLine4.Name = "lblAddressLine4";
            this.lblAddressLine4.Size = new System.Drawing.Size(246, 25);
            this.lblAddressLine4.TabIndex = 18;
            this.lblAddressLine4.Text = "Address Line 4";
            // 
            // lblPostcode
            // 
            this.lblPostcode.Enabled = false;
            this.lblPostcode.Location = new System.Drawing.Point(29, 466);
            this.lblPostcode.Name = "lblPostcode";
            this.lblPostcode.Size = new System.Drawing.Size(246, 25);
            this.lblPostcode.TabIndex = 19;
            this.lblPostcode.Text = "Postcode";
            // 
            // btnPostcodeFind
            // 
            this.btnPostcodeFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPostcodeFind.BackColor = System.Drawing.Color.DarkGray;
            this.btnPostcodeFind.Location = new System.Drawing.Point(897, 460);
            this.btnPostcodeFind.Margin = new System.Windows.Forms.Padding(0);
            this.btnPostcodeFind.Name = "btnPostcodeFind";
            this.btnPostcodeFind.Size = new System.Drawing.Size(75, 34);
            this.btnPostcodeFind.TabIndex = 20;
            this.btnPostcodeFind.Text = "Find";
            this.btnPostcodeFind.UseVisualStyleBackColor = false;
            this.btnPostcodeFind.Click += new System.EventHandler(this.btnPostcodeFind_Click);
            // 
            // lblEmail
            // 
            this.lblEmail.Enabled = false;
            this.lblEmail.Location = new System.Drawing.Point(29, 282);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(246, 25);
            this.lblEmail.TabIndex = 22;
            this.lblEmail.Text = "Email Address";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(289, 280);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(605, 25);
            this.txtEmail.TabIndex = 21;
            // 
            // frmCustomerAdd
            // 
            this.ClientSize = new System.Drawing.Size(1044, 624);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnPostcodeFind);
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
            this.Name = "frmCustomerAdd";
            this.Load += new System.EventHandler(this.frmCustomerAdd_Load_1);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
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
            this.Controls.SetChildIndex(this.btnPostcodeFind, 0);
            this.Controls.SetChildIndex(this.txtEmail, 0);
            this.Controls.SetChildIndex(this.lblEmail, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       
        public System.Windows.Forms.TextBox txtForename;
        public System.Windows.Forms.TextBox txtSurname;
        public System.Windows.Forms.TextBox txtTelephoneNo;
        public System.Windows.Forms.TextBox txtHouseNameNumber;
        public System.Windows.Forms.TextBox txtAddressLine1;
        public System.Windows.Forms.TextBox txtAddressLine2;
        public System.Windows.Forms.TextBox txtAddressLine3;
        public System.Windows.Forms.TextBox txtAddressLine4;
        public System.Windows.Forms.TextBox txtPostcode;
        public System.Windows.Forms.TextBox lblForename;
        public System.Windows.Forms.TextBox lblSurname;
        public System.Windows.Forms.TextBox lblTelephoneNo;
        public System.Windows.Forms.TextBox lblHouseNameNumber;
        public System.Windows.Forms.TextBox lblAddressLine1;
        public System.Windows.Forms.TextBox lblAddressLine2;
        public System.Windows.Forms.TextBox lblAddressLine3;
        public System.Windows.Forms.TextBox lblAddressLine4;
        public System.Windows.Forms.TextBox lblPostcode;
        private System.Windows.Forms.Button btnPostcodeFind;
        public System.Windows.Forms.TextBox lblEmail;
        public System.Windows.Forms.TextBox txtEmail;
    }
}