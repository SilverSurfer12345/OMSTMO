using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OrderManagement.Model
{
    public partial class frmCategoryAdd : SampleAdd
    {
        public int id = 0;
        public frmCategoryAdd()
        {
            InitializeComponent();
        }
        public override void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Use DatabaseManager.ConnectionString instead of MainClass.conString
                using (SqlConnection con = new SqlConnection(DatabaseManager.ConnectionString))
                {
                    con.Open();

                    // Use a parameterized query to prevent SQL injection
                    string qry = (id == 0) ? "INSERT INTO category VALUES(@catName)" : "UPDATE category SET catName = @catName WHERE catID = @id";

                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@catName", txtName.Text);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Saved Successfully.");

                            id = 0;
                            txtName.Clear();
                            txtName.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Failed to save. Please try again.");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmCategoryAdd_Load(object sender, EventArgs e)
        {
            // Any additional initialization can be done here
        }
    }
}
