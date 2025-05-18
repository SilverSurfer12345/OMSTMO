using OrderManagement.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration; // Add this namespace

namespace OrderManagement.View
{
    public partial class frmCustomerView : Form
    {
        private void frmCustomerView_Load(object sender, EventArgs e)
        {
            var query = "SELECT * FROM customers";
            SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString);
            SqlCommand command = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            customerViewDataGrid.DataSource = dataTable;
            adapter.Fill(dataTable);
            GetData();
        }

        // Add the missing GetData method
        private void GetData()
        {
            // Implement the logic for GetData here
            MessageBox.Show("GetData method called!");
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Add the desired functionality here, or leave it empty if no action is required
            MessageBox.Show("Label clicked!");
        }

        private void customerViewDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Add your logic here to handle the cell click event.
            // Example: Check if the clicked cell is the edit or delete column.
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (customerViewDataGrid.Columns[e.ColumnIndex].Name == "dgvedit")
                {
                    // Handle edit logic
                    MessageBox.Show("Edit button clicked for row " + e.RowIndex);
                }
                else if (customerViewDataGrid.Columns[e.ColumnIndex].Name == "dgvdel")
                {
                    // Handle delete logic
                    MessageBox.Show("Delete button clicked for row " + e.RowIndex);
                }
            }
        }

        private void customerViewDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Add your logic here for handling the CellContentClick event
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = customerViewDataGrid.Columns[e.ColumnIndex].Name;
                if (columnName == "dgvedit")
                {
                    // Handle edit action
                    MessageBox.Show("Edit button clicked for row " + e.RowIndex);
                }
                else if (columnName == "dgvdel")
                {
                    // Handle delete action
                    MessageBox.Show("Delete button clicked for row " + e.RowIndex);
                }
            }
        }
    }
}
