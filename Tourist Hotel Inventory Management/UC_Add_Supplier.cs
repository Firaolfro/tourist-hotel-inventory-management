

 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist_Hotel_Inventory_Management
{
    public partial class UC_Add_Supplier : UserControl
    {
        public UC_Add_Supplier()
        {
            InitializeComponent();
        }

        // Action to save supplier to the database
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    // Verified connection string to match your local database name
        //    ststring connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";

        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        try
        //        {
        //            conn.Open();

        //            // SQL Query to insert new supplier data matching your UI fields
        //            string query = "INSERT INTO Suppliers (SupplierName, Phone, Email, Address) VALUES (@name, @phone, @email, @address)";

        //            SqlCommand cmd = new SqlCommand(query, conn);

        //            // Parameters mapped to your UI controls
        //            // Note: Ensure txtSupplierName, txtPhone, txtEmail, and cmbAddress match your designer names
        //            cmd.Parameters.AddWithValue("@name", txtSupplierName.Text.Trim());
        //            cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
        //            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
        //            cmd.Parameters.AddWithValue("@address", cmbAddress.Text.Trim());

        //            cmd.ExecuteNonQuery();

        //            MessageBox.Show("Supplier successfully registered!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            // Automatically return to the supplier list view after successful registration
        //            button1_Click(sender, e);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handles database errors such as login failure or table missing
        //            MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //            Dashboard mainDash = (Dashboard)this.FindForm();
        //            if (mainDash != null)
        //            {
        //                mainDash.addUserControl(new UC_Suppliers());
        //            }
        //        }
        //    }
        //}


        private void button2_Click(object sender, EventArgs e)
        {
            // 1. Validation Logic: Ensure the user didn't leave critical fields empty
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text))
            {
                MessageBox.Show("Supplier Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSupplierName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            // 2. Connection String
            string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";

            // 
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // 3. SQL Query - Using parameters to prevent SQL Injection
                    string query = "INSERT INTO Suppliers (SupplierName, Phone, Email, Address) VALUES (@name, @phone, @email, @address)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Clean data by Trimming whitespace before saving
                        cmd.Parameters.AddWithValue("@name", txtSupplierName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", cmbAddress.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }

                    // 4. Success Feedback
                    MessageBox.Show("Supplier successfully registered!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Navigate back to the list
                    button1_Click(sender, e);
                }
                catch (SqlException sqlEx)
                {
                    // Specifically handle SQL errors (like duplicate names or connection timeouts)
                    MessageBox.Show("Database Connection Error: " + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Safety fallback to main list
                    Dashboard mainDash = (Dashboard)this.FindForm();
                    if (mainDash != null)
                    {
                        mainDash.addUserControl(new UC_Suppliers());
                    }
                }
            }
        }

        // Navigate back to Supplier List (Cancel/Back functionality)
        private void button1_Click(object sender, EventArgs e)
        {
            Dashboard mainDash = (Dashboard)this.FindForm();
            if (mainDash != null)
            {
                mainDash.addUserControl(new UC_Suppliers());
            }
        }

        // Secondary navigation cancel button logic
        private void button5_Click(object sender, EventArgs e)
        {
            Dashboard mainDash = (Dashboard)this.FindForm();
            if (mainDash != null)
            {
                mainDash.addUserControl(new UC_Suppliers());
            }
        }

        // Placeholder handlers for Designer compatibility
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e) { }

        private void tableLayoutPanel7_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}