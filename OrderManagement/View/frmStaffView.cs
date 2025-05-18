using OrderManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public partial class frmStaffView : SampleView
    {
        private readonly string connectionString = DatabaseManager.ConnectionString;

        public frmStaffView()
        {
            InitializeComponent();
            GetData();
        }

        public void GetData()
        {
            string qry = "Select * From staff where forename like '%" + txtSearch.Text + "%' ";
            ListBox lb = new ListBox();

            // Check if lb is not null before iterating over its items
            if (lb != null)
            {
                // Add columns to the DataGridView
                foreach (var columnName in lb.Items)
                {
                    if (!staffViewDataGrid.Columns.Contains(columnName.ToString()))
                    {
                        staffViewDataGrid.Columns.Add(columnName.ToString(), columnName.ToString());
                    }
                }

                // Load data using MainClass.LoadData
                MainClass.LoadData(qry, staffViewDataGrid, lb);
            }
            else
            {
                // Handle the case where lb is null (e.g., log, display a message, etc.)
                MessageBox.Show("ListBox (lb) is not initialized.");
            }
        }

        private void FrmStaffView_Load(object sender, EventArgs e)
        {
            var query = "SELECT * FROM staff";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            staffViewDataGrid.DataSource = dataTable;
            adapter.Fill(dataTable);
            GetData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            frmStaffAdd frm = new frmStaffAdd();
            frm.ShowDialog();
            GetData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void StaffViewDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(staffViewDataGrid.CurrentRow.Cells["dgvid"].Value);

                if (staffViewDataGrid.CurrentCell.OwningColumn.Name == "dgvedit")
                {
                    frmStaffAdd frm = new frmStaffAdd();
                    frm.label1.Text = "Update Staff";
                    frm.id = id;
                    frm.cbRole.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvRole"].Value);
                    frm.txtForename.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvForename"].Value);
                    frm.txtSurname.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvSurname"].Value);
                    frm.txtTelephoneNo.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvTelephoneNo"].Value);
                    frm.txtHouseNameNumber.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvHouseNameNumber"].Value);
                    frm.txtAddressLine1.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvAddressLine1"].Value);
                    frm.txtAddressLine2.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvAddressLine2"].Value);
                    frm.txtAddressLine3.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvAddressLine3"].Value);
                    frm.txtAddressLine4.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvAddressLine4"].Value);
                    frm.txtPostcode.Text = Convert.ToString(staffViewDataGrid.CurrentRow.Cells["dgvPostcode"].Value);
                    frm.ShowDialog();
                    GetData();
                }

                if (staffViewDataGrid.CurrentCell.OwningColumn.Name == "dgvdel")
                {
                    if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string qry = "DELETE FROM staff WHERE id = @id";
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters.Add("@id", id);

                        if (MainClass.SQL(qry, parameters) > 0)
                        {
                            MessageBox.Show("Delete successful.");
                            GetData(); // Refresh the data after deletion
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete. Please try again.");
                        }
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void staffViewDataGrid_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}