

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
    public partial class UC_Settings_user_acc : UserControl
    {
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            passwordpanel1.Controls.Clear();
            passwordpanel1.Controls.Add(userControl);
            userControl.BringToFront();
        }

        public UC_Settings_user_acc()
        {
            InitializeComponent();
        }

        // CHANGED TO PUBLIC: So Dashboard can call this when button is clicked
        public void LoadUserList()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    string query = "SELECT Username, Role FROM Users";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dgvUsers != null)
                    {
                        dgvUsers.DataSource = dt;
                    }

                    // --- LOGIC ADDED HERE ---
                    // We use the count of rows in the DataTable to update the label
                    if (lblTotalUsers != null)
                    {
                        lblTotalUsers.Text = "Total Registered Users: " + dt.Rows.Count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading user list: " + ex.Message);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill Username, Password, and select a Role!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Users (Username, Password, FullName, Email, Phone, Role) 
                                    VALUES (@user, @pass, '', '', '', @role)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@pass", txtPassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@role", cmbRole.SelectedItem.ToString());

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User Account Created Successfully!");
                        button3_Click(sender, e);
                        LoadUserList();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error saving user: " + ex.Message); }
            }
        }

        private void DeleteUser(string username)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Users WHERE Username = @user";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("User deleted successfully!");
                        LoadUserList();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error deleting user: " + ex.Message); }
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvUsers.Columns[e.ColumnIndex].Name == "btnDeleteCol")
                {
                    string selectedUser = dgvUsers.Rows[e.RowIndex].Cells["Username"].Value.ToString();
                    if (selectedUser.ToLower() == "admin")
                    {
                        MessageBox.Show("The primary admin account cannot be deleted.");
                        return;
                    }

                    if (MessageBox.Show($"Delete user '{selectedUser}'?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DeleteUser(selectedUser);
                    }
                }
            }
        }

        private void UC_Settings_user_acc_Load(object sender, EventArgs e)
        {
            LoadUserList();
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    txtUsername.Clear();
        //    txtPassword.Clear();
        //    if (cmbRole.Items.Count > 0) cmbRole.SelectedIndex = -1;
        //}

        private void button3_Click(object sender, EventArgs e)
        {
            // Clear inputs
            txtUsername.Clear();
            txtPassword.Clear();
            if (cmbRole.Items.Count > 0) cmbRole.SelectedIndex = -1;

            // Refresh the list so the user sees the latest data
            LoadUserList();

            // Move focus back to Username for the next entry
            txtUsername.Focus();
        }

        // Navigation and Boilerplate
        private void button1_Click_1(object sender, EventArgs e) { 
            
            //Change Password button
            
            addUserControl(new UC_Settings_Change_Password()); }
        //private void button2_Click_1(object sender, EventArgs e) { addUserControl(new UC_Settings_user_acc()); }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // 1. Create the instance
            UC_Settings_user_acc userAcc = new UC_Settings_user_acc();

            // 2. Load it into the panel
            addUserControl(userAcc);

            // 3. Manually trigger the data load so the list isn't empty
            userAcc.LoadUserList();
        }

        private void label6_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void tblSettings_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel45_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel21_Paint(object sender, PaintEventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void passwordpanel1_Paint(object sender, PaintEventArgs e) { }
        private void label18_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel52_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel20_Paint(object sender, PaintEventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void txtUsername_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e) { }
        private void tableLayoutPanel46_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel25_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel31_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel32_Paint(object sender, PaintEventArgs e) { }
        private void UC_Settings_user_acc_Load_1(object sender, EventArgs e) { }
    }
}