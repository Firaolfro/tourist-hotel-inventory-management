using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Tourist_Hotel_Inventory_Management
{
    public partial class UC_Settings_Change_Password : UserControl
    {
        // Connection string matching your Dashboard configuration
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";

        public UC_Settings_Change_Password()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {
            // Boilerplate event maintained
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // --- CHANGE PASSWORD LOGIC ---

            // 1. Validation: Check if any fields are empty
            if (string.IsNullOrWhiteSpace(txtCurrentPass.Text) ||
                string.IsNullOrWhiteSpace(txtNewPass.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPass.Text))
            {
                MessageBox.Show("Please fill in all password fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validation: Check if New Password and Confirmation match
            if (txtNewPass.Text != txtConfirmPass.Text)
            {
                MessageBox.Show("The new password and confirmation do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Database Operation
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // UPDATED: Using SessionManager.CurrentUsername instead of hardcoded 'admin'
                    string query = "UPDATE Users SET Password = @newPass WHERE Username = @currentUser AND Password = @oldPass";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Add parameters to prevent SQL Injection
                        cmd.Parameters.AddWithValue("@oldPass", txtCurrentPass.Text);
                        cmd.Parameters.AddWithValue("@newPass", txtNewPass.Text);
                        cmd.Parameters.AddWithValue("@currentUser", SessionManager.CurrentUsername);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Password updated successfully for {SessionManager.CurrentUsername}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear fields after success
                            txtCurrentPass.Clear();
                            txtNewPass.Clear();
                            txtConfirmPass.Clear();
                        }
                        else
                        {
                            // This triggers if Username + OldPassword combination doesn't exist
                            MessageBox.Show("Current password is incorrect.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Clear all the text fields
            txtCurrentPass.Clear();
            txtNewPass.Clear();
            txtConfirmPass.Clear();

            // 2. Return to the User Account list (Navigation Back)
            // We clear the panel that holds this control and put the User List back in
            if (this.Parent != null)
            {
                this.Parent.Controls.Clear();
                UC_Settings_user_acc userAcc = new UC_Settings_user_acc();
                userAcc.Dock = DockStyle.Fill;
                this.Parent.Controls.Add(userAcc);
                userAcc.LoadUserList(); // Refresh the list
            }
        }

        private void txtCurrentPass_TextChanged(object sender, EventArgs e)
        {
            // Event maintained
        }

        private void txtNewPass_TextChanged(object sender, EventArgs e)
        {
            // Event maintained
        }

        private void txtConfirmPass_TextChanged(object sender, EventArgs e)
        {
            // Event maintained
        }
    }
}