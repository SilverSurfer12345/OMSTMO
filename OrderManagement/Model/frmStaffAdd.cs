using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OrderManagement.Model
{
    public partial class frmStaffAdd : SampleAdd
    {
        public frmStaffAdd()
        {
            InitializeComponent();
        }

        public int id = 0;

        public override void btnSave_Click(object sender, EventArgs e)
        {

            // Use a parameterized query to prevent SQL injection
            string qry = (id == 0) ? "INSERT INTO staff (role, forename, surname, telephoneNo, houseNameNumber,AddressLine1, AddressLine2, AddressLine3, AddressLine4, Postcode) VALUES(@role, @forename, @surname, @telephoneNo, @houseNameNumber ,@AddressLine1, @AddressLine2, @AddressLine3, @AddressLine4, @Postcode)" : "UPDATE staff SET role = @role, forename = @forename, surname = @surname, telephoneNo = @telephoneNo, houseNameNumber = @houseNameNumber, AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, AddressLine3 = @AddressLine3, AddressLine4 = @AddressLine4, Postcode = @Postcode WHERE id = @id";

            // Use a Dictionary for parameters instead of Hashtable
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@id", id },
                { "@role", cbRole.Text },
                { "@forename", txtForename.Text },
                { "@surname", txtSurname.Text },
                { "@telephoneNo", txtTelephoneNo.Text },
                { "@houseNameNumber", txtHouseNameNumber.Text },
                { "@AddressLine1", txtAddressLine1.Text },
                { "@AddressLine2", txtAddressLine2.Text },
                { "@AddressLine3", txtAddressLine3.Text },
                { "@AddressLine4", txtAddressLine4.Text },
                { "@Postcode", txtPostcode.Text }
            };

            // Check if the SQL operation was successful
            if (MainClass.SQL(qry, parameters) > 0)
            {
                MessageBox.Show("Saved Successfully.");
                id = 0;
                cbRole.SelectedIndex = -1;
                txtForename.Clear();
                txtSurname.Clear();
                txtTelephoneNo.Clear();
                txtHouseNameNumber.Clear();
                txtAddressLine1.Clear();
                txtAddressLine2.Clear();
                txtAddressLine3.Clear();
                txtAddressLine4.Clear();
                txtPostcode.Clear();
                txtForename.Focus();
            }
            else
            {
                MessageBox.Show("Failed to save. Please try again.");
            }
            this.Close();
        }

    }
}
