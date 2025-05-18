using OrderManagement.Model;
using OrderManagement.View;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OrderManagement
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            this.FormClosing += frmMain_FormClosing;
            lblUser.Text = "Welcome: " + MainClassHelpers.uName;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Force a database checkpoint to ensure data is written to disk
                DatabaseManager.ForceCheckpoint();

                // Close all database connections
                DatabaseManager.CloseConnections();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in FormClosing: " + ex.Message);
            }
        }
        public void AddControls(Form f)
        {
            ControlsPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            ControlsPanel.Controls.Add(f);
            f.Show();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            AddControls(new frmHome());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            AddControls(new frmCategoryView());
        }

        private void btnPos_Click(object sender, EventArgs e)
        {
            frmPOS frm = new frmPOS();
            frm.Show();
            this.Close();

        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            frmLogin frm = Application.OpenForms.OfType<frmLogin>().FirstOrDefault();
            if (frm != null)
            {
                frm.Show();
            }
            else
            {
                frm = new frmLogin();
                frm.Show();
            }
            frm.Show();
            MessageBox.Show(MainClassHelpers.username + " has been logged out", "Log out successful");
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            AddControls(new frmCustomerView());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AddControls(new frmStaffView());
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
