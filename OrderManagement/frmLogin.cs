using System;
using System.Windows.Forms;

namespace OrderManagement
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (MainClass.IsValidUser(txtUser.Text, txtPass.Text) == false)
            {
                MessageBox.Show("Incorrect username or password", "Login Failed");
            }
            else
            {
                this.Hide();
                txtPass.Clear();
                frmMain frmMain = new frmMain();
                frmMain.Show();

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle password visibility based on CheckBox state
            txtPass.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUser.Clear();
            txtPass.Clear();
        }

    }
}
