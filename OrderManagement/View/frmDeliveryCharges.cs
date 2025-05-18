using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public partial class frmDeliveryCharges : Form
    {
        private DataGridView dgvCharges;
        private Button btnSave;
        private Button btnCancel;
        private string connectionString;

        public frmDeliveryCharges()
        {
            InitializeComponent();

            // Get application directory path and build connection string
            connectionString = OrderManagement.DatabaseManager.ConnectionString;

            // Form settings
            this.Text = "Delivery Charge Management";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;


            // Create controls
            Label lblTitle = new Label
            {
                Text = "Delivery Charges",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 40
            };

            dgvCharges = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = true,
                AllowUserToDeleteRows = true,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                Location = new Point(10, 50),
                Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - 110)
            };

            dgvCharges.Columns.Add("PostcodePrefix", "Postcode Prefix");
            dgvCharges.Columns.Add("Charge", "Delivery Charge (£)");

            Panel pnlButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50
            };

            btnSave = new Button
            {
                Text = "Save",
                Size = new Size(100, 30),
                Location = new Point(this.ClientSize.Width - 220, 10),
                BackColor = Color.FromArgb(241, 85, 126),
                ForeColor = Color.White
            };

            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(100, 30),
                Location = new Point(this.ClientSize.Width - 110, 10),
                BackColor = Color.Gray,
                ForeColor = Color.White
            };

            // Add controls
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Controls.Add(btnCancel);
            this.Controls.Add(dgvCharges);
            this.Controls.Add(lblTitle);
            this.Controls.Add(pnlButtons);

            // Events
            this.Load += frmDeliveryCharges_Load;
            btnSave.Click += btnSave_Click;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            // Fix layout
            this.Resize += (s, e) => {
                btnSave.Location = new Point(this.ClientSize.Width - 220, 10);
                btnCancel.Location = new Point(this.ClientSize.Width - 110, 10);
            };
        }

        private void frmDeliveryCharges_Load(object sender, EventArgs e)
        {
            LoadDeliveryCharges();
        }

        private void LoadDeliveryCharges()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if table exists
                    string checkTableQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'DeliveryCharges'";
                    using (SqlCommand checkCmd = new SqlCommand(checkTableQuery, conn))
                    {
                        int tableExists = (int)checkCmd.ExecuteScalar();

                        if (tableExists == 0)
                        {
                            // Create table if it doesn't exist
                            string createTableQuery = @"
                            CREATE TABLE DeliveryCharges (
                                PostcodePrefix NVARCHAR(10) PRIMARY KEY,
                                Charge DECIMAL(10, 2) NOT NULL
                            )";

                            using (SqlCommand createCmd = new SqlCommand(createTableQuery, conn))
                            {
                                createCmd.ExecuteNonQuery();
                            }

                            // Add default entry
                            string insertDefaultQuery = "INSERT INTO DeliveryCharges (PostcodePrefix, Charge) VALUES ('DEFAULT', 0.00)";
                            using (SqlCommand insertCmd = new SqlCommand(insertDefaultQuery, conn))
                            {
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // Get data from table
                    string query = "SELECT PostcodePrefix, Charge FROM DeliveryCharges";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvCharges.Rows.Clear();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                dgvCharges.Rows.Add(row["PostcodePrefix"], row["Charge"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading delivery charges: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate data
                foreach (DataGridViewRow row in dgvCharges.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Cells[0].Value == null || string.IsNullOrWhiteSpace(row.Cells[0].Value.ToString()))
                    {
                        MessageBox.Show("Postcode prefix cannot be empty");
                        return;
                    }

                    if (row.Cells[1].Value == null || !decimal.TryParse(row.Cells[1].Value.ToString(), out _))
                    {
                        MessageBox.Show("Charge must be a valid number");
                        return;
                    }
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Delete existing data
                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM DeliveryCharges", conn))
                    {
                        deleteCmd.ExecuteNonQuery();
                    }

                    // Insert new data
                    foreach (DataGridViewRow row in dgvCharges.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                        {
                            string prefix = row.Cells[0].Value.ToString().Trim();
                            decimal charge = Convert.ToDecimal(row.Cells[1].Value);

                            string insertQuery = "INSERT INTO DeliveryCharges (PostcodePrefix, Charge) VALUES (@prefix, @charge)";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@prefix", prefix);
                                insertCmd.Parameters.AddWithValue("@charge", charge);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("Delivery charges saved successfully");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving delivery charges: " + ex.Message);
            }
        }
    }
}
