
//using System.Data.SqlClient;
//using System;
//using System.Data;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting; // Required for Charting

//namespace Tourist_Hotel_Inventory_Management
//{
//    public partial class UC_Dashboard : UserControl
//    {
//        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;TrustServerCertificate=True";

//        // Constructor to accept the real-time numbers passed from the Dashboard Form
//        public UC_Dashboard(int totalItems, int lowStock)
//        {
//            InitializeComponent();

//            // FIX: Set the text immediately when the dashboard is created
//            SetInitialLabels();

//            lblTotalItems.Text = totalItems.ToString();
//            lblLowStock.Text = lowStock.ToString();

//            RefreshDashboardData();
//        }

//        public UC_Dashboard()
//        {
//            InitializeComponent();
//            // FIX: Also set labels here for the designer/default view
//            SetInitialLabels();
//        }

//        // NEW HELPER: This ensures code is not empty on startup
//        private void SetInitialLabels()
//        {
//            // Set Welcome Text
//            if (!string.IsNullOrEmpty(SessionManager.CurrentUsername))
//            {
//                lblWelcome.Text = "Welcome, " + SessionManager.CurrentUsername;
//            }
//            else
//            {
//                lblWelcome.Text = "Welcome, Guest";
//            }

//            // Set Date Text
//            lblDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
//        }

//        private void UC_Dashboard_Load(object sender, EventArgs e)
//        {
//            // Ensures data is fresh when the control is displayed
//            SetInitialLabels();
//            RefreshDashboardData();
//        }

//        // --- REFRESH BUTTON LOGIC ---
//        private void button1_Click(object sender, EventArgs e)
//        {
//            RefreshDashboardData();
//            SetInitialLabels();

//            MessageBox.Show("Dashboard Data Refreshed!", "System Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        public void RefreshDashboardData()
//        {
//            using (SqlConnection conn = new SqlConnection(connString))
//            {
//                try
//                {
//                    if (conn.State == ConnectionState.Closed) conn.Open();

//                    // 1. Update Numeric Labels
//                    SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Items", conn);
//                    lblTotalItems.Text = cmd1.ExecuteScalar()?.ToString() ?? "0";

//                    SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Items WHERE Quantity <= MinStock", conn);
//                    lblLowStock.Text = cmd2.ExecuteScalar()?.ToString() ?? "0";

//                    SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM Suppliers", conn);
//                    lblTotalSuppliers.Text = cmd3.ExecuteScalar()?.ToString() ?? "0";

//                    // 2. Update Chart Data
//                    LoadChartData(conn);
//                }
//                catch (Exception ex)
//                {
//                    // Fail silently or log error for debugging
//                }
//            }
//        }

//        private void LoadChartData(SqlConnection conn)
//        {
//            try
//            {
//                // Query to group items by category
//                string query = "SELECT Category, COUNT(*) as Total FROM Items GROUP BY Category";
//                SqlDataAdapter da = new SqlDataAdapter(query, conn);
//                DataTable dt = new DataTable();
//                da.Fill(dt);

//                // Assuming your chart name is chart1
//                chart1.Series.Clear();
//                Series series = new Series("Categories");
//                series.ChartType = SeriesChartType.Pie; // Sets it to Pie Chart

//                foreach (DataRow row in dt.Rows)
//                {
//                    series.Points.AddXY(row["Category"].ToString(), row["Total"]);
//                }

//                chart1.Series.Add(series);

//                // Optional: Make it look modern
//                chart1.Series["Categories"]["PieLabelStyle"] = "Outside";
//                chart1.ChartAreas[0].Area3DStyle.Enable3D = true; // 3D Effect
//            }
//            catch { }
//        }

//        // --- NAVIGATION & EVENT HANDLERS ---

//        private void lblWelcome_Click(object sender, EventArgs e)
//        {
//            SetInitialLabels();
//        }

//        private void lblDate_Click(object sender, EventArgs e)
//        {
//            lblDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
//        }

//        private void btnViewItems_Click(object sender, EventArgs e)
//        {
//            Dashboard mainDash = (Dashboard)this.FindForm();
//            if (mainDash != null) mainDash.addUserControl(new UC_Items());
//        }

//        private void btnViewSuppliers_Click(object sender, EventArgs e)
//        {
//            Dashboard mainDash = (Dashboard)this.FindForm();
//            if (mainDash != null) mainDash.addUserControl(new UC_Suppliers());
//        }

//        private void btnEditItem_Click(object sender, EventArgs e)
//        {
//            Dashboard mainDash = (Dashboard)this.FindForm();
//            if (mainDash != null) mainDash.addUserControl(new UC_addItems());
//        }

//        private void btnDeleteItem_Click(object sender, EventArgs e)
//        {
//            DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
//            if (result == DialogResult.Yes)
//            {
//                MessageBox.Show("Delete functionality ready to be linked to Database.");
//            }
//        }

//        private void tableLayoutPanel20_Paint(object sender, PaintEventArgs e) { }
//        private void label35_Click(object sender, EventArgs e) { }
//        private void tableLayoutPanel38_Paint(object sender, PaintEventArgs e) { }
//        private void tableLayoutPanel49_Paint(object sender, PaintEventArgs e) { }
//        private void label29_Click(object sender, EventArgs e) { }
//        private void label30_Click(object sender, EventArgs e) { }
//        private void tableLayoutPanel50_Paint(object sender, PaintEventArgs e) { }
//        private void tableLayoutPanel21_Paint(object sender, PaintEventArgs e) { }

//        private void chart1_Click(object sender, EventArgs e)
//        {
//            // Refresh chart data if clicked
//            RefreshDashboardData();
//        }
//    }
//}

using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Tourist_Hotel_Inventory_Management
{
    public partial class UC_Dashboard : UserControl
    {
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True;TrustServerCertificate=True";

        // Mapping for Chart Category Translations
        //Dictionary<string, string> categoryTranslations = new Dictionary<string, string>
        //{
        //    {"Beverages", "መጠጦች"},
        //    {"Alcoholic Drinks", "አልኮል መጠጦች"},
        //    {"Food & Ingredients", "ምግብ እና ቅመማ ቅመሞች"},
        //    {"Meat & Poultry", "ስጋ እና ዶሮ"},
        //    {"Fruits & Vegetables", "አትክልትና ፍራፍሬ"},
        //    {"Dairy Products", "የወተት ተዋጽኦዎች"},
        //    {"Cleaning Supplies", "የጽዳት ዕቃዎች"},
        //    {"Guest Amenities", "የእንግዳ መገልገያዎች"},
        //    {"Kitchenware", "የወጥ ቤት ዕቃዎች"},
        //    {"Electrical Supplies", "የኤሌክትሪክ ዕቃዎች"},
        //    {"Plumbing Materials", "የቧንቧ ዕቃዎች"},
        //    {"Bedding & Linens", "አልጋ ልብስ እና አንሶላ"},
        //    {"Uniforms", "የሰራተኛ ዩኒፎርም"},
        //    {"Stationery & Office Supplies", "የቢሮ መሣሪያዎች"},
        //    {"Hardware & Tools", "መሣሪያዎች እና ሃርድዌር"},
        //    {"Laundry Chemicals", "የልብስ ማጠቢያ ኬሚካሎች"},
        //    {"IT Accessories", "የኮምፒውተር መለዋወጫዎች"},
        //    {"Event & Banquet Decor", "የዝግጅት ማስጌጫዎች"}
        //};

        Dictionary<string, string> categoryTranslations = new Dictionary<string, string>
{
    {"Beverages", "መጠጦች"},
    {"Alcoholic Drinks", "አልኮል መጠጦች"},
    {"Food & Ingredients", "ምግብ እና ቅመማ ቅመሞች"},
    {"Meat & Poultry", "ስጋ እና ዶሮ"},
    {"Fruits & Vegetables", "አትክልትና ፍራፍሬ"},
    {"Dairy Products", "የወተት ተዋጽኦዎች"},
    {"Cleaning Supplies", "የጽዳት ዕቃዎች"},
    {"Guest Amenities", "የእንግዳ መገልገያዎች"},
    {"Kitchenware", "የወጥ ቤት ዕቃዎች"},
    {"Electrical Supplies", "የኤሌክትሪክ ዕቃዎች"},
    {"Plumbing Materials", "የቧንቧ ዕቃዎች"},
    {"Bedding & Linens", "አልጋ ልብስ እና አንሶላ"},
    {"Uniforms", "የሰራተኛ ዩኒፎርም"},
    {"Stationery & Office Supplies", "የቢሮ መሣሪያዎች"},
    {"Hardware & Tools", "መሣሪያዎች እና ሃርድዌር"},
    {"Laundry Chemicals", "የልብስ ማጠቢያ ኬሚካሎች"},
    {"IT Accessories", "የኮምፒውተር መለዋወጫዎች"},
    {"Event & Banquet Decor", "የዝግጅት ማስጌጫዎች"}
};

        public UC_Dashboard(int totalItems, int lowStock)
        {
            InitializeComponent();
            SetInitialLabels();
            lblTotalItems.Text = totalItems.ToString();
            lblLowStock.Text = lowStock.ToString();
            RefreshDashboardData();
        }

        public UC_Dashboard()
        {
            InitializeComponent();
            SetInitialLabels();
        }

        public void SetInitialLabels()
        {
            bool isAmharic = LangSettings.IsAmharic;
            string welcomePrefix = isAmharic ? "እንኳን ደህና መጡ፣ " : "Welcome, ";
            string guestName = isAmharic ? "እንግዳ" : "Guest";

            lblWelcome.Text = welcomePrefix + (SessionManager.CurrentUsername ?? guestName);

            if (isAmharic)
            {
                lblDate.Font = new Font("Nyala", 11);
                lblDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy", new System.Globalization.CultureInfo("am-ET"));
            }
            else
            {
                lblDate.Font = new Font("Segoe UI", 10);
                lblDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            }
        }

        private void UC_Dashboard_Load(object sender, EventArgs e)
        {
            SetInitialLabels();
            RefreshDashboardData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RefreshDashboardData();
            SetInitialLabels();
            string msg = LangSettings.IsAmharic ? "ዳሽቦርዱ ታድሷል!" : "Dashboard Data Refreshed!";
            MessageBox.Show(msg, "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void RefreshDashboardData()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Items", conn);
                    lblTotalItems.Text = cmd1.ExecuteScalar()?.ToString() ?? "0";

                    SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Items WHERE Quantity <= MinStock", conn);
                    lblLowStock.Text = cmd2.ExecuteScalar()?.ToString() ?? "0";

                    SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM Suppliers", conn);
                    lblTotalSuppliers.Text = cmd3.ExecuteScalar()?.ToString() ?? "0";

                    LoadChartData(conn);
                }
                catch { }
            }
        }

        private void LoadChartData(SqlConnection conn)
        {
            try
            {
                string query = "SELECT Category, COUNT(*) as Total FROM Items GROUP BY Category";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Clear and Setup Series
                chart1.Series.Clear();
                Series series = new Series("Categories") { ChartType = SeriesChartType.Pie };

                // Handle Legend Font
                if (chart1.Legends.Count > 0)
                {
                    chart1.Legends[0].Font = LangSettings.IsAmharic ? new Font("Nyala", 10) : new Font("Segoe UI", 9);
                }

                foreach (DataRow row in dt.Rows)
                {
                    string categoryName = row["Category"].ToString();

                    if (LangSettings.IsAmharic && categoryTranslations.ContainsKey(categoryName))
                    {
                        categoryName = categoryTranslations[categoryName];
                    }

                    int pointIndex = series.Points.AddXY(categoryName, row["Total"]);

                    if (LangSettings.IsAmharic)
                    {
                        series.Points[pointIndex].Font = new Font("Nyala", 9);
                    }
                }

                chart1.Series.Add(series);
                chart1.Series["Categories"]["PieLabelStyle"] = "Outside";
                chart1.ChartAreas[0].Area3DStyle.Enable3D = true;

                // CRITICAL FIX: Force the chart to repaint after data change
                chart1.Invalidate();
                chart1.Update();
            }
            catch { }
        }

        // Sidebar Navigation Helper Methods
        private void btnViewItems_Click(object sender, EventArgs e) => ((Dashboard)this.FindForm())?.addUserControl(new UC_Items());
        private void btnViewSuppliers_Click(object sender, EventArgs e) => ((Dashboard)this.FindForm())?.addUserControl(new UC_Suppliers());
        private void btnEditItem_Click(object sender, EventArgs e) => ((Dashboard)this.FindForm())?.addUserControl(new UC_addItems());
        private void chart1_Click(object sender, EventArgs e) => RefreshDashboardData();

        // Stubs
        private void lblWelcome_Click(object sender, EventArgs e) { SetInitialLabels(); }
        private void lblDate_Click(object sender, EventArgs e) { SetInitialLabels(); }
        private void tableLayoutPanel20_Paint(object sender, PaintEventArgs e) { }
        private void label35_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel38_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel49_Paint(object sender, PaintEventArgs e) { }
        private void label29_Click(object sender, EventArgs e) { }
        private void label30_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel50_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel21_Paint(object sender, PaintEventArgs e) { }
    }
}