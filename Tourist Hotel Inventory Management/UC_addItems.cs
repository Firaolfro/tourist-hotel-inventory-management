
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
    public partial class UC_addItems : UserControl
    {
        // Global connection string for this control
        //ststring connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";
        public UC_addItems()
        {
            InitializeComponent();

        }

        // --- THE MISSING METHOD THAT FIXES YOUR ERROR ---
        public void FillDataForEdit(string name, string category, string qty, string unit, string price, string minStock)
        {
            txtItemName.Text = name;
            cmbCategory.Text = category;
            txtQuantity.Text = qty;
            cmbUnit.Text = unit;
            txtUnitPrice.Text = price;
            txtMinStock.Text = minStock;

            // Change button text so the code knows we are UPDATING, not INSERTING
            button10.Text = "Update Item";

            // Make the name read-only so the user doesn't break the SQL link
            txtItemName.ReadOnly = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query;

                    // Check if we are updating an existing item or adding a new one
                    if (button10.Text == "Update Item")
                    {
                        query = "UPDATE Items SET Category=@cat, Quantity=@qty, Unit=@unit, UnitPrice=@price, MinStock=@min WHERE ItemName=@name";
                    }
                    else
                    {
                        query = "INSERT INTO Items (ItemName, Category, Quantity, Unit, UnitPrice, MinStock) " +
                                "VALUES (@name, @cat, @qty, @unit, @price, @min)";
                    }

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@name", txtItemName.Text);
                    cmd.Parameters.AddWithValue("@cat", cmbCategory.Text);
                    cmd.Parameters.AddWithValue("@qty", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@unit", cmbUnit.Text);
                    cmd.Parameters.AddWithValue("@price", txtUnitPrice.Text);
                    cmd.Parameters.AddWithValue("@min", txtMinStock.Text);

                    cmd.ExecuteNonQuery();

                    // Update Dashboard Counts
                    Dashboard mainDash = (Dashboard)this.FindForm();
                    if (mainDash != null)
                    {
                        mainDash.LoadDashboardStatistics();
                    }

                    //MessageBox.Show(button10.Text == "Update Item" ? "Item Updated Successfully!" : "New Item Added Successfully!",
                                    //"Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Go back to the list
                    button5_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message);
                }
            }
        }

        // --- NAVIGATION & STUBS ---

        private void button5_Click(object sender, EventArgs e) // Back / Cancel button
        {
            UC_Items itemsList = new UC_Items();
            Dashboard mainDash = (Dashboard)this.FindForm();
            if (mainDash != null)
            {
                mainDash.addUserControl(itemsList);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Additional Back button
        {
            button5_Click(sender, e);
        }

        private void UC_addItems_Load(object sender, EventArgs e) { }
        private void pnlContent_Paint(object sender, PaintEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e) { }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}