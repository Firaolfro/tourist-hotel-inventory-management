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
//    public partial class RegisterForm : Form
//    {
//        public RegisterForm()
//        {
//            InitializeComponent();
//        }

//        private void RegisterForm_Load(object sender, EventArgs e)
//        {

//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            this.Hide();
//            LoginForm2 form2 = new LoginForm2();
//            form2.Show();
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {

//            this.Hide();
//            Dashboard form2 = new Dashboard();
//            form2.Show();
//        }
//    }
//}



using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient; // Required for database connection

namespace Tourist_Hotel_Inventory_Management
{
    public partial class RegisterForm : Form
    {
        // Use your verified connection string
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;TrustServerCertificate=True";

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            // Clear fields on load to ensure a fresh start
            txtFullName.Clear();
            txtUsername.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            cmbRole.SelectedIndex = -1;
        }

        // BACK TO LOGIN BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm2 form2 = new LoginForm2();
            form2.Show();
        }

        // REGISTER BUTTON LOGIC
        private void button2_Click(object sender, EventArgs e)
        {
            // 1. Basic Validation: Ensure no fields are empty
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(cmbRole.Text))
            {
                MessageBox.Show("Please fill in all required fields (*).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Password Confirmation Check
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Database Operation
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Query to insert new user based on your updated table schema
                    string query = "INSERT INTO Users (Username, Password, FullName, Email, Phone, Role) " +
                                   "VALUES (@user, @pass, @name, @email, @phone, @role)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@pass", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@name", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@role", cmbRole.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registration Successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Redirect to Login Page after successful registration
                    this.Hide();
                    LoginForm2 login = new LoginForm2();
                    login.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}