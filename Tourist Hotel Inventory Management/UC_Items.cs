
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Drawing;
//using System.Windows.Forms;

//namespace Tourist_Hotel_Inventory_Management
//{
//    public partial class UC_Items : UserControl
//    {
//        // Database Connection String
//        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

//        // Dictionary to translate Database Categories to Amharic
//        Dictionary<string, string> categoryTranslations = new Dictionary<string, string>
//        {
//            {"Alcohol", "አልኮል"},
//            {"Beverage", "መጠጥ"},
//            {"Soft Drink", "ለስላሳ መጠጥ"},
//            {"Food & Beverage", "ምግብ እና መጠጥ"},
//            {"Cleaning Supplies", "የጽዳት ዕቃዎች"},
//            {"Fruits & Vegetables", "አትክልትና ፍራፍሬ"},
//            {"Guest Amenities", "የእንግዳ መገልገያዎች"},
//            {"Meat & Poultry", "ስጋ እና ዶሮ"}
//        };

//        public UC_Items()
//        {
//            InitializeComponent();
//            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dgvInventory.MultiSelect = false;

//            // Wire up the CellFormatting event for dynamic translation
//            dgvInventory.CellFormatting += DgvInventory_CellFormatting;
//        }

//        private void UC_Items_Load(object sender, EventArgs e)
//        {
//            // Apply Font fix for Amharic support (Fixes the "???" issue)
//            if (LangSettings.IsAmharic)
//            {
//                this.dgvInventory.DefaultCellStyle.Font = new Font("Nyala", 11);
//                this.dgvInventory.ColumnHeadersDefaultCellStyle.Font = new Font("Nyala", 11, FontStyle.Bold);
//            }

//            LoadInventoryData();
//        }

//        // --- THE TRANSLATION FIX (Status AND Category) ---
//        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (!LangSettings.IsAmharic || e.Value == null) return;

//            string columnName = dgvInventory.Columns[e.ColumnIndex].Name;
//            string rawValue = e.Value.ToString().Trim();

//            // 1. Translate the Status Column
//            if (columnName == "Status")
//            {
//                switch (rawValue)
//                {
//                    case "Available": e.Value = "ዝግጁ"; break;
//                    case "Low Stock": e.Value = "አነስተኛ ክምችት"; break;
//                    case "Out of Stock": e.Value = "ክምችት አልቋል"; break;
//                }
//                e.FormattingApplied = true;
//            }
//            // 2. Translate the Category Column using the Dictionary
//            else if (columnName == "Category")
//            {
//                if (categoryTranslations.ContainsKey(rawValue))
//                {
//                    e.Value = categoryTranslations[rawValue];
//                    e.FormattingApplied = true;
//                }
//            }
//        }

//        public void LoadInventoryData()
//        {
//            using (SqlConnection conn = new SqlConnection(connString))
//            {
//                try
//                {
//                    conn.Open();

//                    // Determine currency text based on language setting
//                    string currencySymbol = LangSettings.IsAmharic ? " ብር" : " Birr";

//                    string query = $@"SELECT ItemName, Category, Quantity, Unit, 
//                                    FORMAT(UnitPrice, 'N2') + '{currencySymbol}' AS [Unit Price], 
//                                    MinStock, 
//                                    CASE 
//                                        WHEN Quantity <= 0 THEN 'Out of Stock'
//                                        WHEN Quantity <= MinStock THEN 'Low Stock'
//                                        ELSE 'Available'
//                                    END AS Status,
//                                    UnitPrice -- Hidden helper column
//                                    FROM Items";

//                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
//                    DataTable dt = new DataTable();
//                    da.Fill(dt);
//                    dgvInventory.DataSource = dt;

//                    if (dgvInventory.Columns.Contains("UnitPrice"))
//                        dgvInventory.Columns["UnitPrice"].Visible = false;

//                    // --- COLUMN NAME TRANSLATION ---
//                    if (LangSettings.IsAmharic)
//                    {
//                        dgvInventory.Columns["ItemName"].HeaderText = "የዕቃው ስም";
//                        dgvInventory.Columns["Category"].HeaderText = "ዓይነት";
//                        dgvInventory.Columns["Quantity"].HeaderText = "ብዛት";
//                        dgvInventory.Columns["Unit"].HeaderText = "መመዘኛ";
//                        dgvInventory.Columns["Unit Price"].HeaderText = "የአንዱ ዋጋ";
//                        dgvInventory.Columns["MinStock"].HeaderText = "ዝቅተኛ ክምችት";
//                        dgvInventory.Columns["Status"].HeaderText = "ሁኔታ";
//                    }
//                    else
//                    {
//                        dgvInventory.Columns["ItemName"].HeaderText = "Item Name";
//                        dgvInventory.Columns["Category"].HeaderText = "Category";
//                        dgvInventory.Columns["Quantity"].HeaderText = "Quantity";
//                        dgvInventory.Columns["Unit"].HeaderText = "Unit";
//                        dgvInventory.Columns["Unit Price"].HeaderText = "Unit Price";
//                        dgvInventory.Columns["MinStock"].HeaderText = "Min Stock";
//                        dgvInventory.Columns["Status"].HeaderText = "Status";
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error loading inventory: " + ex.Message);
//                }
//            }
//        }

//        private void txtSearch_TextChanged(object sender, EventArgs e)
//        {
//            if (dgvInventory.DataSource is DataTable dt)
//            {
//                try
//                {
//                    dt.DefaultView.RowFilter = string.Format("ItemName LIKE '%{0}%' OR Category LIKE '%{0}%'",
//                        txtSearch.Text.Replace("'", "''"));
//                }
//                catch (Exception ex) { Console.WriteLine("Search error: " + ex.Message); }
//            }
//        }

//        private void btnEdit_Click(object sender, EventArgs e)
//        {
//            if (dgvInventory.SelectedRows.Count > 0)
//            {
//                string name = dgvInventory.SelectedRows[0].Cells["ItemName"].Value.ToString();
//                string cat = dgvInventory.SelectedRows[0].Cells["Category"].Value.ToString();
//                string qty = dgvInventory.SelectedRows[0].Cells["Quantity"].Value.ToString();
//                string unit = dgvInventory.SelectedRows[0].Cells["Unit"].Value.ToString();
//                string price = dgvInventory.SelectedRows[0].Cells["UnitPrice"].Value.ToString();
//                string min = dgvInventory.SelectedRows[0].Cells["MinStock"].Value.ToString();

//                UC_addItems ucAdd = new UC_addItems();
//                ucAdd.FillDataForEdit(name, cat, qty, unit, price, min);

//                Dashboard mainDash = (Dashboard)this.FindForm();
//                if (mainDash != null) mainDash.addUserControl(ucAdd);
//            }
//            else
//            {
//                string msg = LangSettings.IsAmharic ? "እባክዎ ለማስተካከል ረድፍ ይምረጡ" : "Please select a row to edit.";
//                MessageBox.Show(msg, "Selection Required");
//            }
//        }

//        private void btnDelete_Click(object sender, EventArgs e)
//        {
//            if (dgvInventory.SelectedRows.Count > 0)
//            {
//                string itemName = dgvInventory.SelectedRows[0].Cells["ItemName"].Value.ToString();
//                string confirmMsg = LangSettings.IsAmharic ? $"{itemName}ን መሰረዝ እርግጠኛ ነዎት?" : $"Are you sure you want to delete {itemName}?";
//                string confirmTitle = LangSettings.IsAmharic ? "ማረጋገጫ" : "Confirm Delete";

//                if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
//                {
//                    using (SqlConnection conn = new SqlConnection(connString))
//                    {
//                        try
//                        {
//                            conn.Open();
//                            SqlCommand cmd = new SqlCommand("DELETE FROM Items WHERE ItemName=@name", conn);
//                            cmd.Parameters.AddWithValue("@name", itemName);
//                            cmd.ExecuteNonQuery();

//                            LoadInventoryData();
//                            Dashboard mainDash = (Dashboard)this.FindForm();
//                            if (mainDash != null) mainDash.LoadDashboardStatistics();
//                        }
//                        catch (Exception ex) { MessageBox.Show("Error deleting: " + ex.Message); }
//                    }
//                }
//            }
//        }

//        public void button5_Click(object sender, EventArgs e)
//        {
//            Dashboard mainDash = (Dashboard)this.FindForm();
//            if (mainDash != null) mainDash.addUserControl(new UC_addItems());
//        }

//        private void btnEdit_Click_1(object sender, EventArgs e) { btnEdit_Click(sender, e); }
//        private void btnDelete_Click_1(object sender, EventArgs e) { btnDelete_Click(sender, e); }
//        private void txtSearch_TextChanged_1(object sender, EventArgs e) { txtSearch_TextChanged(sender, e); }
//        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
//        private void dgvInventory_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
//        private void label1_Click(object sender, EventArgs e) { }
//        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e) { }
//        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e) { }
//        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
//        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e) { }
//    }
//}


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Tourist_Hotel_Inventory_Management
{
    public partial class UC_Items : UserControl
    {
        // Database Connection String
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;";

        // Dictionary to translate Database Categories to Amharic
        Dictionary<string, string> categoryTranslations = new Dictionary<string, string>
        {
            {"Alcohol", "አልኮል"},
            {"Beverage", "መጠጥ"},
            {"Soft Drink", "ለስላሳ መጠጥ"},
            {"Food & Beverage", "ምግብ እና መጠጥ"},
            {"Cleaning Supplies", "የጽዳት ዕቃዎች"},
            {"Fruits & Vegetables", "አትክልትና ፍራፍሬ"},
            {"Guest Amenities", "የእንግዳ መገልገያዎች"},
            {"Meat & Poultry", "ስጋ እና ዶሮ"}
        };

        public UC_Items()
        {
            InitializeComponent();
            dgvInventory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.MultiSelect = false;

            // Wire up the CellFormatting event for dynamic translation and currency
            dgvInventory.CellFormatting += DgvInventory_CellFormatting;
        }

        private void UC_Items_Load(object sender, EventArgs e)
        {
            // Apply Font fix for Amharic support
            if (LangSettings.IsAmharic)
            {
                this.dgvInventory.DefaultCellStyle.Font = new Font("Nyala", 11);
                this.dgvInventory.ColumnHeadersDefaultCellStyle.Font = new Font("Nyala", 11, FontStyle.Bold);
            }

            LoadInventoryData();
        }

        // --- THE TRANSLATION & CURRENCY FIX ---
        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null) return;

            string columnName = dgvInventory.Columns[e.ColumnIndex].Name;

            // 1. Translate the Status Column
            if (columnName == "Status" && LangSettings.IsAmharic)
            {
                string rawValue = e.Value.ToString().Trim();
                switch (rawValue)
                {
                    case "Available": e.Value = "ዝግጁ"; break;
                    case "Low Stock": e.Value = "አነስተኛ ክምችት"; break;
                    case "Out of Stock": e.Value = "ክምችት አልቋል"; break;
                }
                e.FormattingApplied = true;
            }
            // 2. Translate the Category Column
            else if (columnName == "Category" && LangSettings.IsAmharic)
            {
                string rawValue = e.Value.ToString().Trim();
                if (categoryTranslations.ContainsKey(rawValue))
                {
                    e.Value = categoryTranslations[rawValue];
                    e.FormattingApplied = true;
                }
            }
            // 3. Translate the Price and Currency Symbol (Fixes "???" Birr)
            else if (columnName == "UnitPrice")
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal price))
                {
                    string symbol = LangSettings.IsAmharic ? " ብር" : " Birr";
                    e.Value = price.ToString("N2") + symbol;
                    e.FormattingApplied = true;
                }
            }
        }

        public void LoadInventoryData()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Query simplified: Pull raw UnitPrice instead of formatting in SQL
                    string query = @"SELECT ItemName, Category, Quantity, Unit, 
                                    UnitPrice, 
                                    MinStock, 
                                    CASE 
                                        WHEN Quantity <= 0 THEN 'Out of Stock'
                                        WHEN Quantity <= MinStock THEN 'Low Stock'
                                        ELSE 'Available'
                                    END AS Status
                                    FROM Items";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvInventory.DataSource = dt;

                    // --- COLUMN NAME TRANSLATION ---
                    if (LangSettings.IsAmharic)
                    {
                        dgvInventory.Columns["ItemName"].HeaderText = "የዕቃው ስም";
                        dgvInventory.Columns["Category"].HeaderText = "ዓይነት";
                        dgvInventory.Columns["Quantity"].HeaderText = "ብዛት";
                        dgvInventory.Columns["Unit"].HeaderText = "መመዘኛ";
                        dgvInventory.Columns["UnitPrice"].HeaderText = "የአንዱ ዋጋ";
                        dgvInventory.Columns["MinStock"].HeaderText = "ዝቅተኛ ክምችት";
                        dgvInventory.Columns["Status"].HeaderText = "ሁኔታ";
                    }
                    else
                    {
                        dgvInventory.Columns["ItemName"].HeaderText = "Item Name";
                        dgvInventory.Columns["Category"].HeaderText = "Category";
                        dgvInventory.Columns["Quantity"].HeaderText = "Quantity";
                        dgvInventory.Columns["Unit"].HeaderText = "Unit";
                        dgvInventory.Columns["UnitPrice"].HeaderText = "Unit Price";
                        dgvInventory.Columns["MinStock"].HeaderText = "Min Stock";
                        dgvInventory.Columns["Status"].HeaderText = "Status";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading inventory: " + ex.Message);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dgvInventory.DataSource is DataTable dt)
            {
                try
                {
                    dt.DefaultView.RowFilter = string.Format("ItemName LIKE '%{0}%' OR Category LIKE '%{0}%'",
                        txtSearch.Text.Replace("'", "''"));
                }
                catch (Exception ex) { Console.WriteLine("Search error: " + ex.Message); }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count > 0)
            {
                string name = dgvInventory.SelectedRows[0].Cells["ItemName"].Value.ToString();
                string cat = dgvInventory.SelectedRows[0].Cells["Category"].Value.ToString();
                string qty = dgvInventory.SelectedRows[0].Cells["Quantity"].Value.ToString();
                string unit = dgvInventory.SelectedRows[0].Cells["Unit"].Value.ToString();
                string price = dgvInventory.SelectedRows[0].Cells["UnitPrice"].Value.ToString();
                string min = dgvInventory.SelectedRows[0].Cells["MinStock"].Value.ToString();

                UC_addItems ucAdd = new UC_addItems();
                ucAdd.FillDataForEdit(name, cat, qty, unit, price, min);

                Dashboard mainDash = (Dashboard)this.FindForm();
                if (mainDash != null) mainDash.addUserControl(ucAdd);
            }
            else
            {
                string msg = LangSettings.IsAmharic ? "እባክዎ ለማስተካከል ረድፍ ይምረጡ" : "Please select a row to edit.";
                MessageBox.Show(msg, "Selection Required");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count > 0)
            {
                string itemName = dgvInventory.SelectedRows[0].Cells["ItemName"].Value.ToString();
                string confirmMsg = LangSettings.IsAmharic ? $"{itemName}ን መሰረዝ እርግጠኛ ነዎት?" : $"Are you sure you want to delete {itemName}?";
                string confirmTitle = LangSettings.IsAmharic ? "ማረጋገጫ" : "Confirm Delete";

                if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("DELETE FROM Items WHERE ItemName=@name", conn);
                            cmd.Parameters.AddWithValue("@name", itemName);
                            cmd.ExecuteNonQuery();

                            LoadInventoryData();
                            Dashboard mainDash = (Dashboard)this.FindForm();
                            if (mainDash != null) mainDash.LoadDashboardStatistics();
                        }
                        catch (Exception ex) { MessageBox.Show("Error deleting: " + ex.Message); }
                    }
                }
            }
        }

        public void button5_Click(object sender, EventArgs e)
        {
            Dashboard mainDash = (Dashboard)this.FindForm();
            if (mainDash != null) mainDash.addUserControl(new UC_addItems());
        }

        // Stubs and wired-up events
        private void btnEdit_Click_1(object sender, EventArgs e) { btnEdit_Click(sender, e); }
        private void btnDelete_Click_1(object sender, EventArgs e) { btnDelete_Click(sender, e); }
        private void txtSearch_TextChanged_1(object sender, EventArgs e) { txtSearch_TextChanged(sender, e); }
        private void dgvInventory_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvInventory_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel11_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e) { }
    }
}