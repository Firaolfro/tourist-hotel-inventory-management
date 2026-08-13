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
//    public partial class Stock_Out : UserControl
//    {
//        public Stock_Out()
//        {
//            InitializeComponent();
//        }

//        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e)
//        {

//        }

//        private void textBox1_TextChanged(object sender, EventArgs e)
//        {
//            //date
//        }

//        private void label1_Click(object sender, EventArgs e)
//        {

//        }

//        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
//        {

//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            //record  stock out button
//        }

//        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            //dapartment
//        }

//        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            //Item 
//        }

//        private void textBox2_TextChanged(object sender, EventArgs e)
//        {
//            //quantity
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist_Hotel_Inventory_Management
{
    public partial class Stock_Out : UserControl
    {
        // Connection string matches your Stock In page
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;TrustServerCertificate=True";

        public Stock_Out()
        {
            InitializeComponent();
            this.Load += new EventHandler(Stock_Out_Load);
        }

        private void Stock_Out_Load(object sender, EventArgs e)
        {
            LoadItemsIntoComboBox();
            LoadDepartments();
            dtpUsedDate.Value = DateTime.Now; // Assuming you have a DateTimePicker named dtpUsedDate
        }

        private void LoadItemsIntoComboBox()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Professional Query: No duplicates, clean display names, keeps full name for DB update
                    string query = @"SELECT 
                                     MAX(ItemName) AS ItemName, 
                                     TRIM(REPLACE(REPLACE(REPLACE(REPLACE(ItemName, '500ml', ''), '1L', ''), '2L', ''), '330ml', '')) AS SimpleName 
                                     FROM Items 
                                     GROUP BY TRIM(REPLACE(REPLACE(REPLACE(REPLACE(ItemName, '500ml', ''), '1L', ''), '2L', ''), '330ml', ''))
                                     ORDER BY SimpleName ASC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBox1.DataSource = null;
                    comboBox1.DisplayMember = "SimpleName";
                    comboBox1.ValueMember = "ItemName";
                    comboBox1.DataSource = dt;
                    comboBox1.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading items: " + ex.Message);
                }
            }
        }

        private void LoadDepartments()
        {
            // Standard Hotel Departments
            string[] departments = { "Housekeeping", "Restaurant", "Bar", "Kitchen", "Front Office", "Maintenance" };
            comboBox2.DataSource = departments;
            comboBox2.SelectedIndex = -1;
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    // --- VALIDATION ---
        //    if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || string.IsNullOrEmpty(textBox2.Text))
        //    {
        //        MessageBox.Show("Please fill all fields.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    if (!int.TryParse(textBox2.Text, out int qtyUsed) || qtyUsed <= 0)
        //    {
        //        MessageBox.Show("Enter a valid quantity.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    string itemName = comboBox1.SelectedValue.ToString();
        //    string department = comboBox2.SelectedItem.ToString();
        //    DateTime usedDate = dtpUsedDate.Value;

        //    // --- DATABASE TRANSACTION ---
        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            using (SqlTransaction trans = conn.BeginTransaction())
        //            {
        //                try
        //                {
        //                    // 1. CHECK CURRENT STOCK (Real-world safety check)
        //                    string checkQty = "SELECT Quantity FROM Items WHERE ItemName = @name";
        //                    SqlCommand cmdCheck = new SqlCommand(checkQty, conn, trans);
        //                    cmdCheck.Parameters.AddWithValue("@name", itemName);

        //                    object result = cmdCheck.ExecuteScalar();
        //                    int currentStock = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

        //                    if (currentStock < qtyUsed)
        //                    {
        //                        MessageBox.Show($"Insufficient Stock! Available: {currentStock}", "Shortage", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                        trans.Rollback();
        //                        return;
        //                    }

        //                    // 2. SUBTRACT STOCK FROM ITEMS TABLE
        //                    string updateQuery = "UPDATE Items SET Quantity = Quantity - @qty WHERE ItemName = @name";
        //                    SqlCommand cmdUpdate = new SqlCommand(updateQuery, conn, trans);
        //                    cmdUpdate.Parameters.AddWithValue("@qty", qtyUsed);
        //                    cmdUpdate.Parameters.AddWithValue("@name", itemName);
        //                    cmdUpdate.ExecuteNonQuery();

        //                    // 3. LOG TRANSACTION IN STOCKOUT TABLE
        //                    string logQuery = @"INSERT INTO StockOutTransactions (ItemName, QuantityUsed, UsedDate, Department) 
        //                                        VALUES (@name, @qty, @date, @dept)";
        //                    SqlCommand cmdLog = new SqlCommand(logQuery, conn, trans);
        //                    cmdLog.Parameters.AddWithValue("@name", itemName);
        //                    cmdLog.Parameters.AddWithValue("@qty", qtyUsed);
        //                    cmdLog.Parameters.AddWithValue("@date", usedDate);
        //                    cmdLog.Parameters.AddWithValue("@dept", department);
        //                    cmdLog.ExecuteNonQuery();

        //                    trans.Commit();
        //                    MessageBox.Show("Stock usage recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                    // Reset UI
        //                    comboBox1.SelectedIndex = -1;
        //                    comboBox2.SelectedIndex = -1;
        //                    textBox2.Clear();
        //                }
        //                catch (Exception ex)
        //                {
        //                    trans.Rollback();
        //                    MessageBox.Show("Transaction Failed: " + ex.Message);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Database Error: " + ex.Message);
        //        }
        //    }
        //}


        private void button1_Click(object sender, EventArgs e)
        {
            // --- VALIDATION ---
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please fill all fields.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox2.Text, out int qtyUsed) || qtyUsed <= 0)
            {
                MessageBox.Show("Enter a valid quantity.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string itemName = comboBox1.SelectedValue.ToString();
            string department = comboBox2.SelectedItem.ToString();
            DateTime usedDate = dtpUsedDate.Value;

            // --- DATABASE TRANSACTION ---
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. CHECK CURRENT STOCK (Real-world safety check)
                            string checkQty = "SELECT Quantity FROM Items WHERE ItemName = @name";
                            SqlCommand cmdCheck = new SqlCommand(checkQty, conn, trans);
                            cmdCheck.Parameters.AddWithValue("@name", itemName);

                            object result = cmdCheck.ExecuteScalar();
                            int currentStock = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                            if (currentStock < qtyUsed)
                            {
                                MessageBox.Show($"Insufficient Stock! Available: {currentStock}", "Shortage", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                trans.Rollback();
                                return;
                            }

                            // 2. SUBTRACT STOCK FROM ITEMS TABLE
                            string updateQuery = "UPDATE Items SET Quantity = Quantity - @qty WHERE ItemName = @name";
                            SqlCommand cmdUpdate = new SqlCommand(updateQuery, conn, trans);
                            cmdUpdate.Parameters.AddWithValue("@qty", qtyUsed);
                            cmdUpdate.Parameters.AddWithValue("@name", itemName);
                            cmdUpdate.ExecuteNonQuery();

                            // 3. LOG TRANSACTION IN STOCKOUT TABLE
                            string logQuery = @"INSERT INTO StockOutTransactions (ItemName, QuantityUsed, UsedDate, Department) 
                                        VALUES (@name, @qty, @date, @dept)";
                            SqlCommand cmdLog = new SqlCommand(logQuery, conn, trans);
                            cmdLog.Parameters.AddWithValue("@name", itemName);
                            cmdLog.Parameters.AddWithValue("@qty", qtyUsed);
                            cmdLog.Parameters.AddWithValue("@date", usedDate);
                            cmdLog.Parameters.AddWithValue("@dept", department);
                            cmdLog.ExecuteNonQuery();

                            trans.Commit();
                            MessageBox.Show("Stock usage recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // --- NEW PRINT TRIGGER ---
                            // Ask if the user wants to print the "Issue Slip" for the Chef/Barman to sign
                            DialogResult printResult = MessageBox.Show("Would you like to print the Issue Slip for signature?", "Print Receipt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (printResult == DialogResult.Yes)
                            {
                                // Ensure printDialog1 and printDocument1 are dragged onto your form
                                printDialog1.Document = printDocument1;
                                if (printDialog1.ShowDialog() == DialogResult.OK)
                                {
                                    printDocument1.Print();
                                }
                            }

                            // Reset UI
                            comboBox1.SelectedIndex = -1;
                            comboBox2.SelectedIndex = -1;
                            textBox2.Clear();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            MessageBox.Show("Transaction Failed: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Error: " + ex.Message);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Simple validation: Color red if invalid number
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (!int.TryParse(textBox2.Text, out int val) || val <= 0)
                    textBox2.ForeColor = Color.Red;
                else
                    textBox2.ForeColor = Color.Black;
            }
        }

        // --- PRESERVED HANDLERS (Unused but kept for Designer) ---
        private void tableLayoutPanel10_Paint(object sender, PaintEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {

        }
    }
}