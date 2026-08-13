



//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Data.SqlClient;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace Tourist_Hotel_Inventory_Management
//{
//    public partial class UC_Reports : UserControl
//    {
//        // Connection string verified for your local database
//        ststring connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";

//        public UC_Reports()
//        {
//            InitializeComponent();
//        }

//        // --- NEW: Language Translation Logic ---
//        private void ApplyLanguage()
//        {
//            if (LangSettings.IsAmharic)
//            {
//                // Translate Buttons (Ensure these names match your Designer names)
//                button1.Text = "የዕቃ ዝውውር";      // Stock Movement
//                button2.Text = "ያለው ክምችት";      // Current Stock
//                                                // If you added the spending button:
//                                                // button3.Text = "የክፍሎች ወጪ"; 

//                if (btnPrint != null) btnPrint.Text = "አትም"; // Print

//                // Translate Title Label if applicable
//                // label16.Text = "ሪፖርቶች";
//            }
//            else
//            {
//                button1.Text = "Stock Movement";
//                button2.Text = "Current Stock";
//                if (btnPrint != null) btnPrint.Text = "Print";
//            }
//        }

//        // --- Universal Data Loader ---
//        private void LoadReport(string query)
//        {
//            using (SqlConnection conn = new SqlConnection(connString))
//            {
//                try
//                {
//                    conn.Open();
//                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
//                    DataTable dt = new DataTable();
//                    da.Fill(dt);

//                    dgvReports.DataSource = dt;

//                    // Professional styling
//                    dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//                    dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
//                    dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

//                    // Translate Grid Headers dynamically
//                    TranslateGridHeaders();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Report Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }

//        private void TranslateGridHeaders()
//        {
//            if (!LangSettings.IsAmharic) return;

//            // Dictionary for header translations
//            var headers = new Dictionary<string, string>
//            {
//                {"Item Name", "የዕቃው ስም"},
//                {"Category", "ዓይነት"},
//                {"Stock Level", "የክምችት መጠን"},
//                {"Price", "ዋጋ"},
//                {"Status", "ሁኔታ"},
//                {"Item", "ዕቃ"},
//                {"Qty", "ብዛት"},
//                {"Action", "ተግባር"},
//                {"Source/Dest", "መነሻ/መድረሻ"},
//                {"Date", "ቀን"},
//                {"Department", "ክፍል"},
//                {"Total Transactions", "ጠቅላላ ልውውጥ"},
//                {"Total Spent", "ጠቅላላ ወጪ"}
//            };

//            foreach (DataGridViewColumn col in dgvReports.Columns)
//            {
//                if (headers.ContainsKey(col.HeaderText))
//                {
//                    col.HeaderText = headers[col.HeaderText];
//                }
//            }
//        }

//        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
//        {
//            string title = LangSettings.IsAmharic ? "አርባ ምንጭ ቱሪስት ሆቴል - የንብረት ሪፖርት" : "ARBA MINCH TOURIST HOTEL - INVENTORY REPORT";

//            e.Graphics.DrawString(title, new Font("Segoe UI", 14, FontStyle.Bold), Brushes.DarkBlue, new Point(100, 20));
//            e.Graphics.DrawString((LangSettings.IsAmharic ? "የታተመበት ቀን: " : "Printed on: ") + DateTime.Now.ToString(),
//                new Font("Segoe UI", 9), Brushes.Black, new Point(100, 50));

//            int x = 50;
//            int y = 100;
//            int cellHeight = 30;
//            int colWidth = 120;

//            foreach (DataGridViewColumn col in dgvReports.Columns)
//            {
//                e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(x, y, colWidth, cellHeight));
//                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, colWidth, cellHeight));
//                e.Graphics.DrawString(col.HeaderText, new Font("Segoe UI", 10, FontStyle.Bold), Brushes.Black, x + 5, y + 5);
//                x += colWidth;
//            }

//            y += cellHeight;

//            foreach (DataGridViewRow row in dgvReports.Rows)
//            {
//                if (row.IsNewRow) continue;
//                x = 50;

//                foreach (DataGridViewCell cell in row.Cells)
//                {
//                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, colWidth, cellHeight));
//                    Brush cellColor = new SolidBrush(cell.InheritedStyle.ForeColor);
//                    e.Graphics.DrawString(cell.Value?.ToString(), dgvReports.Font, cellColor, x + 5, y + 5);
//                    x += colWidth;
//                }
//                y += cellHeight;
//            }
//        }

//        private void dgvReports_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (e.RowIndex < 0 || e.Value == null) return;
//            string cellValue = e.Value.ToString().ToUpper();

//            if (cellValue.Contains("LOW STOCK") || cellValue.Contains("ዝቅተኛ"))
//            {
//                e.CellStyle.ForeColor = Color.Red;
//                e.CellStyle.Font = new Font(dgvReports.Font, FontStyle.Bold);
//            }
//            else if (cellValue.Contains("AVAILABLE") || cellValue.Contains("አለ"))
//            {
//                e.CellStyle.ForeColor = Color.Green;
//            }

//            if (cellValue.Contains("IN (DELIVERY)") || cellValue.Contains("ገቢ"))
//            {
//                e.CellStyle.ForeColor = Color.Blue;
//            }
//            else if (cellValue.Contains("OUT (USAGE)") || cellValue.Contains("ወጪ"))
//            {
//                e.CellStyle.ForeColor = Color.DarkOrange;
//            }
//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            string currency = LangSettings.IsAmharic ? " ብር" : " Birr";
//            string lowText = LangSettings.IsAmharic ? "🔴 ዝቅተኛ ክምችት" : "🔴 LOW STOCK";
//            string availText = LangSettings.IsAmharic ? "🟢 በበቂ ሁኔታ አለ" : "🟢 AVAILABLE";

//            string query = $@"SELECT 
//                                ItemName AS [Item Name], 
//                                Category, 
//                                Quantity AS [Stock Level], 
//                                FORMAT(UnitPrice, 'N2') + '{currency}' AS [Price], 
//                                CASE 
//                                    WHEN Quantity <= MinStock THEN '{lowText}' 
//                                    ELSE '{availText}' 
//                                END AS [Status]
//                             FROM Items 
//                             ORDER BY Category, ItemName";

//            LoadReport(query);
//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            string inText = LangSettings.IsAmharic ? "ገቢ (ርክክብ)" : "IN (Delivery)";
//            string outText = LangSettings.IsAmharic ? "ወጪ (አጠቃቀም)" : "OUT (Usage)";

//            string query = $@"
//                SELECT 
//                    ItemName AS [Item], 
//                    QuantityReceived AS [Qty], 
//                    '{inText}' AS [Action], 
//                    SupplierName AS [Source/Dest], 
//                    ReceivedDate AS [Date]
//                FROM StockInTransactions
//                UNION ALL
//                SELECT 
//                    ItemName AS [Item], 
//                    QuantityUsed AS [Qty], 
//                    '{outText}' AS [Action], 
//                    Department AS [Source/Dest], 
//                    UsedDate AS [Date]
//                FROM StockOutTransactions
//                ORDER BY [Date] DESC";

//            LoadReport(query);
//        }

//        public void btnDeptSpending_Click(object sender, EventArgs e)
//        {
//            string currency = LangSettings.IsAmharic ? " ብር" : " Birr";

//            string query = $@"SELECT 
//                                Department, 
//                                COUNT(*) AS [Total Transactions],
//                                FORMAT(SUM(s.QuantityUsed * i.UnitPrice), 'N2') + '{currency}' AS [Total Spent]
//                             FROM StockOutTransactions s
//                             JOIN Items i ON s.ItemName = i.ItemName
//                             GROUP BY Department
//                             ORDER BY SUM(s.QuantityUsed * i.UnitPrice) DESC";

//            LoadReport(query);
//        }

//        private void UC_Reports_Load(object sender, EventArgs e)
//        {
//            ApplyLanguage(); // Apply translation to buttons/labels
//            button2_Click(sender, e); // Load initial data with correct language strings
//        }

//        private void btnPrint_Click(object sender, EventArgs e)
//        {
//            if (dgvReports.Rows.Count > 0)
//            {
//                printDialog1.Document = printDocument1;
//                if (printDialog1.ShowDialog() == DialogResult.OK)
//                {
//                    printDocument1.Print();
//                }
//            }
//            else
//            {
//                string msg = LangSettings.IsAmharic ? "የሚታተም መረጃ የለም" : "No data available to print.";
//                MessageBox.Show(msg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//            }
//        }

//        // --- Designer Placeholders ---
//        private void label16_Click(object sender, EventArgs e) { }
//        private void tableLayoutPanel35_Paint(object sender, PaintEventArgs e) { }
//        private void dgvReports_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
//    }
//}




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
    public partial class UC_Reports : UserControl
    {
        // Connection string verified for your local database
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";

        public UC_Reports()
        {
            InitializeComponent();
        }

        // --- NEW: Fix Amharic Encoding & Font ---
        private void ApplyLanguage()
        {
            // Use Nyala for Amharic as it supports Ethiopic characters
            Font amharicFont = new Font("Nyala", 11, FontStyle.Regular);
            Font defaultFont = new Font("Segoe UI", 9, FontStyle.Regular);

            if (LangSettings.IsAmharic)
            {
                button1.Font = amharicFont;
                button2.Font = amharicFont;
                button1.Text = "የዕቃ ዝውውር";
                button2.Text = "ያለው ክምችት";

                if (btnPrint != null)
                {
                    btnPrint.Font = amharicFont;
                    btnPrint.Text = "አትም";
                }

                dgvReports.DefaultCellStyle.Font = amharicFont;
                dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Nyala", 11, FontStyle.Bold);
            }
            else
            {
                button1.Font = defaultFont;
                button2.Font = defaultFont;
                button1.Text = "Stock Movement";
                button2.Text = "Current Stock";

                if (btnPrint != null)
                {
                    btnPrint.Font = defaultFont;
                    btnPrint.Text = "Print";
                }

                dgvReports.DefaultCellStyle.Font = defaultFont;
                dgvReports.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            }
        }

        // --- Universal Data Loader ---
        private void LoadReport(string query)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvReports.DataSource = dt;

                    dgvReports.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvReports.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

                    // Translate Grid Headers dynamically
                    TranslateGridHeaders();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Report Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TranslateGridHeaders()
        {
            if (!LangSettings.IsAmharic) return;

            var headers = new Dictionary<string, string>
            {
                {"Item Name", "የዕቃው ስም"},
                {"Category", "ዓይነት"},
                {"Stock Level", "የክምችት መጠን"},
                {"Price", "ዋጋ"},
                {"Status", "ሁኔታ"},
                {"Item", "ዕቃ"},
                {"Qty", "ብዛት"},
                {"Action", "ተግባር"},
                {"Source/Dest", "መነሻ/መድረሻ"},
                {"Date", "ቀን"},
                {"Department", "ክፍል"},
                {"Total Transactions", "ጠቅላላ ልውውጥ"},
                {"Total Spent", "ጠቅላላ ወጪ"}
            };

            foreach (DataGridViewColumn col in dgvReports.Columns)
            {
                if (headers.ContainsKey(col.HeaderText))
                {
                    col.HeaderText = headers[col.HeaderText];
                }
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Use Nyala for printing Amharic as well
            Font titleFont = LangSettings.IsAmharic ? new Font("Nyala", 16, FontStyle.Bold) : new Font("Segoe UI", 14, FontStyle.Bold);
            Font bodyFont = LangSettings.IsAmharic ? new Font("Nyala", 10) : new Font("Segoe UI", 9);

            string title = LangSettings.IsAmharic ? "አርባ ምንጭ ቱሪስት ሆቴል - የንብረት ሪፖርት" : "ARBA MINCH TOURIST HOTEL - INVENTORY REPORT";

            e.Graphics.DrawString(title, titleFont, Brushes.DarkBlue, new Point(100, 20));
            e.Graphics.DrawString((LangSettings.IsAmharic ? "የታተመበት ቀን: " : "Printed on: ") + DateTime.Now.ToString(),
                bodyFont, Brushes.Black, new Point(100, 50));

            int x = 50;
            int y = 100;
            int cellHeight = 30;
            int colWidth = 120;

            foreach (DataGridViewColumn col in dgvReports.Columns)
            {
                e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(x, y, colWidth, cellHeight));
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, colWidth, cellHeight));
                e.Graphics.DrawString(col.HeaderText, new Font(titleFont.Name, 10, FontStyle.Bold), Brushes.Black, x + 5, y + 5);
                x += colWidth;
            }

            y += cellHeight;

            foreach (DataGridViewRow row in dgvReports.Rows)
            {
                if (row.IsNewRow) continue;
                x = 50;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, colWidth, cellHeight));
                    Brush cellColor = new SolidBrush(cell.InheritedStyle.ForeColor);
                    e.Graphics.DrawString(cell.Value?.ToString(), dgvReports.Font, cellColor, x + 5, y + 5);
                    x += colWidth;
                }
                y += cellHeight;
            }
        }

        private void dgvReports_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string cellValue = e.Value.ToString().ToUpper();

            if (cellValue.Contains("LOW") || cellValue.Contains("ዝቅተኛ"))
            {
                e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.Font = new Font(dgvReports.Font, FontStyle.Bold);
            }
            else if (cellValue.Contains("AVAILABLE") || cellValue.Contains("አለ"))
            {
                e.CellStyle.ForeColor = Color.Green;
            }

            if (cellValue.Contains("IN") || cellValue.Contains("ገቢ"))
            {
                e.CellStyle.ForeColor = Color.Blue;
            }
            else if (cellValue.Contains("OUT") || cellValue.Contains("ወጪ"))
            {
                e.CellStyle.ForeColor = Color.DarkOrange;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Note the N prefix before strings to support Unicode
            string currency = LangSettings.IsAmharic ? " ብር" : " Birr";
            string lowText = LangSettings.IsAmharic ? "🔴 ዝቅተኛ ክምችት" : "🔴 LOW STOCK";
            string availText = LangSettings.IsAmharic ? "🟢 በበቂ ሁኔታ አለ" : "🟢 AVAILABLE";

            string query = $@"SELECT 
                                ItemName AS [Item Name], 
                                Category, 
                                Quantity AS [Stock Level], 
                                FORMAT(UnitPrice, 'N2') + N'{currency}' AS [Price], 
                                CASE 
                                    WHEN Quantity <= MinStock THEN N'{lowText}' 
                                    ELSE N'{availText}' 
                                END AS [Status]
                             FROM Items 
                             ORDER BY Category, ItemName";

            LoadReport(query);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inText = LangSettings.IsAmharic ? "ገቢ (ርክክብ)" : "IN (Delivery)";
            string outText = LangSettings.IsAmharic ? "ወጪ (አጠቃቀም)" : "OUT (Usage)";

            string query = $@"
                SELECT 
                    ItemName AS [Item], 
                    QuantityReceived AS [Qty], 
                    N'{inText}' AS [Action], 
                    SupplierName AS [Source/Dest], 
                    ReceivedDate AS [Date]
                FROM StockInTransactions
                UNION ALL
                SELECT 
                    ItemName AS [Item], 
                    QuantityUsed AS [Qty], 
                    N'{outText}' AS [Action], 
                    Department AS [Source/Dest], 
                    UsedDate AS [Date]
                FROM StockOutTransactions
                ORDER BY [Date] DESC";

            LoadReport(query);
        }

        public void btnDeptSpending_Click(object sender, EventArgs e)
        {
            string currency = LangSettings.IsAmharic ? " ብር" : " Birr";

            string query = $@"SELECT 
                                Department, 
                                COUNT(*) AS [Total Transactions],
                                FORMAT(SUM(s.QuantityUsed * i.UnitPrice), 'N2') + N'{currency}' AS [Total Spent]
                             FROM StockOutTransactions s
                             JOIN Items i ON s.ItemName = i.ItemName
                             GROUP BY Department
                             ORDER BY SUM(s.QuantityUsed * i.UnitPrice) DESC";

            LoadReport(query);
        }

        private void UC_Reports_Load(object sender, EventArgs e)
        {
            ApplyLanguage();
            button2_Click(sender, e);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvReports.Rows.Count > 0)
            {
                printDialog1.Document = printDocument1;
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            else
            {
                string msg = LangSettings.IsAmharic ? "የሚታተም መረጃ የለም" : "No data available to print.";
                MessageBox.Show(msg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label16_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel35_Paint(object sender, PaintEventArgs e) { }
        private void dgvReports_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}