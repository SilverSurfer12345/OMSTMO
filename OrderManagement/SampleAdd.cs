using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OrderManagement
{
    public partial class SampleAdd : Form
    {
        public SampleAdd()
        {
            InitializeComponent();
        }
        private void SetRoundedButton(Button button, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, button.Width, button.Height);
            button.Region = new Region(path);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Call the SetRoundedButton method for each button you want to have rounded edges
            SetRoundedButton(btnSave, 15);
            SetRoundedButton(btnClose, 10);
            // Add more buttons as needed
        }
        private void SampleAdd_Load(object sender, EventArgs e)
        {

        }

        public virtual void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public virtual void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
