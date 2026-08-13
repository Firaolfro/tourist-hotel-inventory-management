//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Tourist_Hotel_Inventory_Management
//{
//    public partial class LoginForm2 : Form
//    {
//        public LoginForm2()
//        {
//            InitializeComponent();
//        }

//        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
//        {

//        }

//        private void LoginForm2_Load(object sender, EventArgs e)
//        {

//        }

//        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
//        {

//        }

//        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
//        {

//        }

//        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
//        {
//            this.Hide();
//            RegisterForm rf = new RegisterForm();
//            rf.Show();
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//             Dashboard frm2 = new Dashboard();
//            frm2.Show();
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Required for SQL Connection

namespace Tourist_Hotel_Inventory_Management
{
    public partial class LoginForm2 : Form
    {
        // Verified connection string for TouristHotelInventoryDB
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;TrustServerCertificate=True";

        public LoginForm2()
        {
            InitializeComponent();
        }

        // Keep your existing layout events to avoid designer errors
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
        private void LoginForm2_Load(object sender, EventArgs e) { }
        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e) { }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            RegisterForm rf = new RegisterForm();
            rf.Show();
        }

        // SIGN IN BUTTON LOGIC
        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Validation: Ensure both fields are filled
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both Username and Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Database verification logic
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Query to check if the Username AND Password match a record
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @user AND Password = @pass";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());

                    int result = (int)cmd.ExecuteScalar();

                    if (result > 0)
                    {
                        // --- UPDATED: Save username to Session before proceeding ---
                        SessionManager.CurrentUsername = txtUsername.Text.Trim();

                        // Success: Go to Dashboard
                        this.Hide();
                        Dashboard frm2 = new Dashboard();
                        frm2.Show();
                    }
                    else
                    {
                        // Failure: Incorrect credentials
                        MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message);
                }
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
    }
}