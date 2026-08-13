


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourist_Hotel_Inventory_Management
{
    public partial class Dashboard : Form
    {
        // The "Master List" for the whole project
        Dictionary<string, string> amharicDict = new Dictionary<string, string>
        {
            // System & Sidebar
            {"Arba Minch Tourist Hotel", "አርባ ምንጭ ቱሪስት ሆቴል"},
            {"Tourist Hotel", "አርባ ምንጭ ቱሪስት ሆቴል"},
            //{"Inventory Management System", "የንብረት አስተዳደር ስርዓት"},
            {"Inventory Management System", "የንብረት አስተዳደር ሲስተም"},
            {"Dashboard", "ዋና ገጽ"},
            {"Items", "የዕቃዎች ዝርዝር"},
            {"Stock In", "ገቢ ዕቃ"},
            {"Stock Out", "ወጪ ዕቃ"},
            {"Suppliers", "አቅራቢዎች"},
            {"Reports", "ሪፖርት"},
            {"Settings", "ማስተካከያ"},
            {"About", "ስለ እኛ"},
            {"Logout", "ውጣ"},

            // Categories (Merged List)
            //{"Beverages", "መጠጦች"},
            //{"Alcoholic Drinks", "አልኮል መጠጦች"},
            //{"Food & Ingredients", "ምግብ እና ቅመማ ቅመሞች"},
            //{"Meat & Poultry", "ስጋ እና ዶሮ"},
            //{"Fruits & Vegetables", "አትክልትና ፍራፍሬ"},
            //{"Dairy Products", "የወተት ተዋጽኦዎች"},
            //{"Cleaning Supplies", "የጽዳት ዕቃዎች"},
            //{"Guest Amenities", "የእንግዳ መገልገያዎች"},
            //{"Kitchenware", "የወጥ ቤት ዕቃዎች"},
            //{"Electrical Supplies", "የኤሌክትሪክ ዕቃዎች"},
            //{"Plumbing Materials", "የቧንቧ ዕቃዎች"},
            //{"Bedding & Linens", "አልጋ ልብስ እና አንሶላ"},
            //{"Uniforms", "የሰራተኛ ዩኒፎርም"},
            //{"Stationery & Office Supplies", "የቢሮ መሣሪያዎች"},
            //{"Hardware & Tools", "መሣሪያዎች እና ሃርድዌር"},
            //{"Laundry Chemicals", "የልብስ ማጠቢያ ኬሚካሎች"},
            //{"IT Accessories", "የኮምፒውተር መለዋወጫዎች"},
            //{"Event & Banquet Decor", "የዝግጅት ማስጌጫዎች"},

            // Category Translations for Pie Chart
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
{"Event & Banquet Decor", "የዝግጅት ማስጌጫዎች"},

            // Common Form Labels
            {"Item Name", "የዕቃው ስም"},
            {"Quantity", "ብዛት"},
            {"Unit", "መመዘኛ"},
            {"Unit Price", "የአንዱ ዋጋ"},
            {"Min Stock", "ዝቅተኛ ክምችት"},
            {"Status", "ሁኔታ"},
            {"Category", "ዓይነት"},
            {"Search", "ፈልግ"},
            {"Save", "መዝግብ"},
            {"Edit", "አስተካክል"},
            {"Delete", "ሰርዝ"},
            {"Department", "ክፍል"},

            // Departments for Stock Out
            {"Front Office", "የፊት ቢሮ"},
            {"Housekeeping", "የቤት አያያዝ"},
            {"Kitchen", "ወጥ ቤት"},
            {"Restaurant", "ምግብ ቤት"},
            {"Bar", "ባር"},
            {"Maintenance", "ጥገና"},
            {"Administration", "አስተዳደር"},
            {"Security", "ጥበቃ"},
            {"Banquet", "ግብዣ እና ዝግጅት"},
            {"Staff Canteen", "የሰራተኞች ካፌ"},

            // Common Units
            {"Pcs", "ፍሬ"},
            {"Kg", "ኪሎግራም"},
            {"Ltr", "ሊትር"},
            {"Box", "ሳጥን"},
            {"Pack", "ጥቅል"},

            // Add these to your dictionary
            {"Birr", "ብር"},
            {"Price (Birr)", "ዋጋ (በብር)"},
            {"Total Value", "ጠቅላላ ዋጋ"},
            {"ETB", "ብር"},
            // Inside amharicDict in Dashboard.cs
{"Total Items", "ጠቅላላ ዕቃዎች"},
{"Low Stock Items", "ዝቅተኛ ክምችት ያላቸው"},
{"Total Suppliers", "ጠቅላላ አቅራቢዎች"},
{"Refresh Data", "መረጃን አድስ"},
{"Items by Category", "ዕቃዎች በዓይነት"}
        };

        // Connection string verified for your TouristHotelInventoryDB
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";

        // Variables to hold real-time data to pass to UC_Dashboard
        int totalItemsCount = 0;
        int lowStockCount = 0;

        public void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add(userControl);

            // FORCED REFRESH: This tells the UserControl to recalculate its children (like the FlowPanel)
            userControl.BringToFront();
            userControl.Invalidate();
            userControl.Update();
        }

        public Dashboard()
        {
            InitializeComponent();
        }

        // FETCH INITIAL STATS: Calculates real hotel data from SQL
        public void LoadDashboardStatistics()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // 1. Get Total Inventory Value (The sum of all stock)
                    SqlCommand cmdVal = new SqlCommand("SELECT SUM(Quantity * UnitPrice) FROM Items", conn);
                    object totalVal = cmdVal.ExecuteScalar();

                    // 2. Get Total Unique Items count
                    SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM Items", conn);
                    totalItemsCount = Convert.ToInt32(cmdCount.ExecuteScalar());

                    // 3. Get Low Stock Alert count (Quantity <= MinStock)
                    SqlCommand cmdLow = new SqlCommand("SELECT COUNT(*) FROM Items WHERE Quantity <= MinStock", conn);
                    lowStockCount = Convert.ToInt32(cmdLow.ExecuteScalar());
                }
                catch (Exception) { /* Maintains design stability if DB is empty */ }
            }
        }

        private void ApplyTranslation(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                // 1. Check if this control's text is in our master list
                if (amharicDict.ContainsKey(c.Text))
                {
                    c.Text = amharicDict[c.Text];
                }

                // 2. If it's a container (like a Panel or Table), scan inside it too
                if (c.HasChildren)
                {
                    ApplyTranslation(c);
                }
            }
        }

        private void tableLayoutPanel22_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel54_Paint(object sender, PaintEventArgs e) { }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // 1. SET INITIAL LANGUAGE STATE
            LangSettings.IsAmharic = false;
            btnLanguageToggle.Text = "አማርኛ";

            // 2. DYNAMIC USERNAME LOGIC
            if (!string.IsNullOrEmpty(SessionManager.CurrentUsername))
            {
                lblUsername.Text = SessionManager.CurrentUsername;
            }

            // 3. FETCH DATA & SHOW INITIAL PAGE
            LoadDashboardStatistics();

            // Explicitly load the Dashboard UserControl
            UC_Dashboard initialDash = new UC_Dashboard(totalItemsCount, lowStockCount);
            addUserControl(initialDash);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadDashboardStatistics();
            addUserControl(new UC_Dashboard(totalItemsCount, lowStockCount));
        }

        // --- NAVIGATION ---
        private void button3_Click(object sender, EventArgs e) { addUserControl(new UC_Items()); }
        private void button4_Click(object sender, EventArgs e) { addUserControl(new Stock_in()); }
        private void button5_Click(object sender, EventArgs e) { addUserControl(new UC_Suppliers()); }
        private void button7_Click(object sender, EventArgs e) { addUserControl(new UC_Reports()); }

        private void button8_Click(object sender, EventArgs e)
        {
            UC_Settings_user_acc settings = new UC_Settings_user_acc();
            addUserControl(settings);
            settings.LoadUserList();
        }

        private void pnlContent_Paint(object sender, PaintEventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            // Bilingual Logout Message
            string message = (btnLanguageToggle.Text == "English") ? "በእርግጠኝነት መውጣት ይፈልጋሉ?" : "Are you sure you want to logout?";
            string title = (btnLanguageToggle.Text == "English") ? "ማረጋገጫ" : "Logout Confirmation";

            if (MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Clear session on logout
                SessionManager.CurrentUsername = "";

                LoginForm2 login = new LoginForm2();
                login.Show();
                this.Hide();
                this.Close();
                this.Dispose();
            }
        }

        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e) { }
        private void button10_Click(object sender, EventArgs e) { addUserControl(new Stock_Out()); }
        private void button5_Click_1(object sender, EventArgs e) { addUserControl(new UC_Suppliers()); }
        private void button9_Click(object sender, EventArgs e) { addUserControl(new UC_About()); }

        private void tableLayoutPanel17_Paint(object sender, PaintEventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) { }

        private void lblUsername_Click(object sender, EventArgs e) { /* Event maintained */ }
        private void label1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel18_Paint(object sender, PaintEventArgs e) { }

        private void btnLanguageToggle_Click(object sender, EventArgs e)
        {
            // Update the Global setting
            LangSettings.IsAmharic = (btnLanguageToggle.Text == "አማርኛ");

            if (LangSettings.IsAmharic)
            {
                // --- SYSTEM NAMES ---
                label8.Text = "አርባ ምንጭ ቱሪስት ሆቴል";
                label9.Text = "የንብረት አስተዳደር ሲስተም";

                // --- SIDEBAR BUTTONS ---
                button2.Text = "ዋና ገጽ";      // Dashboard
                button3.Text = "የዕቃዎች ዝርዝር";         // Items/Category
                button4.Text = "ገቢ ዕቃ";        // Stock In
                button10.Text = "ወጪ ዕቃ";       // Stock Out
                button5.Text = "አቅራቢዎች";      // Suppliers
                button7.Text = "ሪፖርት";         // Reports
                button8.Text = "ማስተካከያ";       // Settings
                button9.Text = "ስለ እኛ";        // About
                button1.Text = "ውጣ";           // Logout

                btnLanguageToggle.Text = "English";
            }
            else
            {
                // --- RESET TO ENGLISH ---
                label8.Text = "Arba Minch Tourist Hotel";
                label9.Text = "Inventory Management System";

                button2.Text = "Dashboard";
                button3.Text = "Items";
                button4.Text = "Stock In";
                button10.Text = "Stock Out";
                button5.Text = "Suppliers";
                button7.Text = "Reports";
                button8.Text = "Settings";
                button9.Text = "About";
                button1.Text = "Logout";

                btnLanguageToggle.Text = "አማርኛ";
            }

            // --- REFRESH ACTIVE CONTENT WITH PIE CHART FIX ---
            if (pnlContent.Controls.Count > 0)
            {
                Control currentControl = pnlContent.Controls[0];

                // If the current control is the Dashboard, use the constructor that accepts stats
                if (currentControl is UC_Dashboard)
                {
                    LoadDashboardStatistics(); // Refresh counts from DB first
                    UC_Dashboard freshDash = new UC_Dashboard(totalItemsCount, lowStockCount);
                    addUserControl(freshDash);
                }
                else
                {
                    // For all other pages, use the default constructor
                    UserControl freshPage = (UserControl)Activator.CreateInstance(currentControl.GetType());
                    addUserControl(freshPage);
                }
            }
        }

        private void label8_Click(object sender, EventArgs e) { }
    }
}