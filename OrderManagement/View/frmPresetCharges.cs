using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace OrderManagement.View
{
    public partial class frmPresetCharges : Form
    {
        private DataGridView dgvCharges;
        private Button btnOK;
        private string connectionString;

        public frmPresetCharges()
        {
            connectionString = OrderManagement.DatabaseManager.ConnectionString;

            this.Text = "Preset Charges";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            dgvCharges = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 200,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            dgvCharges.Columns.Add("ChargeName", "Charge Name");
            dgvCharges.Columns.Add("ChargeValue", "Charge Value (£)");

            btnOK = new Button
            {
                Text = "OK",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnOK.Click += BtnOK_Click;

            this.Controls.Add(dgvCharges);
            this.Controls.Add(btnOK);

            LoadCharges();
        }

        private void LoadCharges()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ChargeName, ChargeValue FROM PresetCharges";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCharges.Rows.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvCharges.Rows.Add(row["ChargeName"], row["ChargeValue"]);
                    }
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (DataGridViewRow row in dgvCharges.Rows)
                {
                    if (row.IsNewRow) continue;
                    string name = row.Cells[0].Value.ToString();
                    decimal value = Convert.ToDecimal(row.Cells[1].Value);
                    string update = "UPDATE PresetCharges SET ChargeValue = @value WHERE ChargeName = @name";
                    using (SqlCommand cmd = new SqlCommand(update, conn))
                    {
                        cmd.Parameters.AddWithValue("@value", value);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public static decimal GetTotalPresetCharges()
        {
            decimal total = 0;
            string connectionString = OrderManagement.DatabaseManager.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT SUM(ChargeValue) FROM PresetCharges";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                        total = Convert.ToDecimal(result);
                }
            }
            return total;
        }

        public static decimal CalculateTotal(decimal basketTotal, decimal deliveryCharge, decimal presetCharges)
        {
            decimal total = basketTotal + deliveryCharge + presetCharges;
            return total;
        }
    }
}