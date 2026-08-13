using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist_Hotel_Inventory_Management
{
    public partial class UC_user_setting : UserControl
    {
        public UC_user_setting()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel20_Paint(object sender, PaintEventArgs e)
        {
            //// Find the main Dashboard form hosting this UserControl
            //Dashboard mainDash = (Dashboard)this.FindForm();

            //if (mainDash != null)
            //{
            //    // Switch the view back to User Management
            //    mainDash.addUserControl(new UC_User_Management());
            //}
        }

        private void tableLayoutPanel25_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Find the main Dashboard form hosting this UserControl
            Dashboard mainDash = (Dashboard)this.FindForm();

            if (mainDash != null)
            {
                // Switch the view back to User Management
                mainDash.addUserControl(new UC_User_Management());
            }
        }

        private void tableLayoutPanel54_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel9_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
