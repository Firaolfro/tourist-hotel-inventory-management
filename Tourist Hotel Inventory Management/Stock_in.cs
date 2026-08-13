 

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
    public partial class Stock_in : UserControl
    {
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;TrustServerCertificate=True";

        public Stock_in()
        {
            InitializeComponent();
            this.Load += new EventHandler(Stock_in_Load);

            // UI FIX: Prevents the list from flipping upward by limiting height
            comboBox1.MaxDropDownItems = 6;
            comboBox2.MaxDropDownItems = 6;
        }

        private void Stock_in_Load(object sender, EventArgs e)
        {
            LoadItemsIntoComboBox();
            LoadSuppliersIntoComboBox();
            dtpReceiveDate.Value = DateTime.Now;

            // Real-world logic: Default expiry to 1 year from now
            dtpExpiryDate.Value = DateTime.Now.AddYears(1);
        }

        private void LoadItemsIntoComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // This query cleans the display name but keeps the raw ItemName for the database link
                    //string query = @"SELECT 
                    //         ItemName, 
                    //         TRIM(REPLACE(REPLACE(REPLACE(REPLACE(ItemName, '500ml', ''), '1L', ''), '2L', ''), '330ml', '')) AS SimpleName 
                    //         FROM Items 
                    //         ORDER BY SimpleName ASC";

                    string query = @"SELECT 
                 MAX(ItemName) AS ItemName, 
                 TRIM(REPLACE(REPLACE(REPLACE(REPLACE(ItemName, '500ml', ''), '1L', ''), '2L', ''), '330ml', '')) AS SimpleName 
                 FROM Items 
                 GROUP BY TRIM(REPLACE(REPLACE(REPLACE(REPLACE(ItemName, '500ml', ''), '1L', ''), '2L', ''), '330ml', ''))
                 ORDER BY SimpleName ASC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "SimpleName"; // Shows "Ambo Mineral Water"
                    comboBox1.ValueMember = "ItemName";     // Keeps "Ambo Mineral Water 500ml" for the UPDATE

                    comboBox1.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading items: " + ex.Message);
                }
            }
        }

        private void LoadSuppliersIntoComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT DISTINCT SupplierName FROM Suppliers ORDER BY SupplierName ASC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBox2.DataSource = null;
                    comboBox2.DisplayMember = "SupplierName";
                    comboBox2.ValueMember = "SupplierName";
                    comboBox2.DataSource = dt;

                    comboBox2.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Supplier Load Error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please select an item and enter quantity.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox2.Text, out int qtyReceived) || qtyReceived <= 0)
            {
                MessageBox.Show("Please enter a valid positive number for Quantity.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Logic for Expiry Date
            DateTime receiveDate = dtpReceiveDate.Value;
            DateTime expiryDate = dtpExpiryDate.Value;

            // Simple validation: Expiry should not be in the past
            if (expiryDate.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Expiry date cannot be in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string itemName = comboBox1.SelectedValue.ToString();
            string supplierName = comboBox2.SelectedIndex != -1 ? comboBox2.Text : "Default Supplier";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Check if item exists
                            string checkQuery = "SELECT COUNT(*) FROM Items WHERE ItemName = @name";
                            using (SqlCommand cmdCheck = new SqlCommand(checkQuery, conn, trans))
                            {
                                cmdCheck.Parameters.AddWithValue("@name", itemName);
                                int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                                if (count == 0)
                                {
                                    MessageBox.Show("Selected item does not exist in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    trans.Rollback();
                                    return;
                                }
                            }

                            // 2. Update Items table
                            string updateItems = "UPDATE Items SET Quantity = Quantity + @qty WHERE ItemName = @name";
                            using (SqlCommand cmdUpdate = new SqlCommand(updateItems, conn, trans))
                            {
                                cmdUpdate.Parameters.AddWithValue("@qty", qtyReceived);
                                cmdUpdate.Parameters.AddWithValue("@name", itemName);
                                cmdUpdate.ExecuteNonQuery();
                            }

                            // 3. Insert into transaction history (Including Expiry Date)
                            string insertLog = @"INSERT INTO StockInTransactions 
                                                (ItemName, QuantityReceived, ReceivedDate, SupplierName, ExpiryDate) 
                                                VALUES (@name, @qty, @date, @supplier, @expiry)";

                            using (SqlCommand cmdLog = new SqlCommand(insertLog, conn, trans))
                            {
                                cmdLog.Parameters.AddWithValue("@name", itemName);
                                cmdLog.Parameters.AddWithValue("@qty", qtyReceived);
                                cmdLog.Parameters.AddWithValue("@date", receiveDate);
                                cmdLog.Parameters.AddWithValue("@supplier", supplierName);
                                cmdLog.Parameters.AddWithValue("@expiry", expiryDate);
                                cmdLog.ExecuteNonQuery();
                            }

                            trans.Commit();
                            MessageBox.Show($"Stock updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Reset UI
                            comboBox1.SelectedIndex = -1;
                            comboBox2.SelectedIndex = -1;
                            textBox2.Clear();
                            dtpReceiveDate.Value = DateTime.Now;
                            dtpExpiryDate.Value = DateTime.Now.AddYears(1);
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            MessageBox.Show($"Transaction failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (!int.TryParse(textBox2.Text, out int qty) || qty <= 0)
                    textBox2.ForeColor = Color.Red;
                else
                    textBox2.ForeColor = SystemColors.WindowText;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button1_Click(sender, e); e.SuppressKeyPress = true; }
        }

        // --- HANDLERS ---
        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dtpReceiveDate_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox1_Click(object sender, EventArgs e) { }
        private void Stock_in_Click(object sender, EventArgs e) { }
        private void Stock_in_Load_1(object sender, EventArgs e) { }
        private void dtpReceiveDate_ValueChanged(object sender, EventArgs e) { }
        private void dtpExpiryDate_ValueChanged(object sender, EventArgs e) { }
    }
}