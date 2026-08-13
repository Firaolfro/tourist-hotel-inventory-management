

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Data.SqlClient; // Required for Database connection

//namespace Tourist_Hotel_Inventory_Management
//{
//    public partial class UC_Suppliers : UserControl
//    {
//        // Connection string set to match your local database
//        ststring connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";

//        public UC_Suppliers()
//        {
//            InitializeComponent();
//        }

//        private void UC_Suppliers_Load(object sender, EventArgs e)
//        {
//            LoadSuppliers(); // Automatically load data when control opens
//        }

//        // Logic to fetch data from Database and create UI Cards
//        public void LoadSuppliers()
//        {
//            // Clear existing cards from the flowLayoutPanel to prevent duplicates on refresh
//            flowLayoutPanel1.Controls.Clear();

//            try
//            {
//                using (SqlConnection conn = new SqlConnection(connString))
//                {
//                    // Select all relevant supplier columns
//                    //string query = "SELECT SupplierID, SupplierName, Phone, Email, Address FROM Suppliers";
//                    // Change SupplierID to SupplierId to match your SQL screenshot exactly
//                    string query = "SELECT SupplierId, SupplierName, Phone, Email, Address FROM Suppliers";
//                    SqlCommand cmd = new SqlCommand(query, conn);
//                    conn.Open();
//                    SqlDataReader reader = cmd.ExecuteReader();

//                    //while (reader.Read())
//                    //{
//                    //    // Generate a visual card for every row in the database
//                    //    AddSupplierCard(
//                    //        reader["SupplierID"].ToString(),
//                    //        reader["SupplierName"].ToString(),
//                    //        reader["Phone"].ToString(),
//                    //        reader["Email"].ToString(),
//                    //        reader["Address"].ToString()
//                    //    );
//                    //}
//                    while (reader.Read())
//                    {
//                        AddSupplierCard(
//                            reader["SupplierId"].ToString(), // Must match 'SupplierId' exactly
//                            reader["SupplierName"].ToString(),
//                            reader["Phone"].ToString(),
//                            reader["Email"].ToString(),
//                            reader["Address"].ToString()
//                        );
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Error loading suppliers: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Creates a stylized Panel "Card" for each supplier to match your design
//        private void AddSupplierCard(string id, string name, string phone, string email, string address)
//        {
//            // 1. Create Main Card Panel Container
//            Panel card = new Panel();
//            card.Size = new Size(300, 220);
//            card.BackColor = Color.White;
//            card.Margin = new Padding(15);
//            card.BorderStyle = BorderStyle.None; // Set to none for a modern look, or FixedSingle for a border

//            // 2. Add Supplier Name (Header)
//            Label lblName = new Label();
//            lblName.Text = name.ToUpper();
//            lblName.Font = new Font("Segoe UI", 12, FontStyle.Bold);
//            lblName.ForeColor = Color.FromArgb(28, 50, 78); // Dark Blue Theme
//            lblName.Dock = DockStyle.Top;
//            lblName.Height = 40;
//            lblName.TextAlign = ContentAlignment.MiddleLeft;
//            lblName.Padding = new Padding(10, 5, 0, 0);

//            // 3. Add Contact Details (Body)
//            Label lblDetails = new Label();
//            lblDetails.Text = $"\n📞 {phone}\n\n📧 {email}\n\n📍 {address}";
//            lblDetails.Font = new Font("Segoe UI", 10, FontStyle.Regular);
//            lblDetails.ForeColor = Color.Gray;
//            lblDetails.Dock = DockStyle.Fill;
//            lblDetails.Padding = new Padding(10, 0, 10, 10);

//            // 4. Action Buttons Panel (Bottom)
//            FlowLayoutPanel actionPanel = new FlowLayoutPanel();
//            actionPanel.Dock = DockStyle.Bottom;
//            actionPanel.Height = 45;
//            actionPanel.BackColor = Color.FromArgb(245, 245, 245); // Light Gray Footer
//            actionPanel.FlowDirection = FlowDirection.RightToLeft;
//            actionPanel.Padding = new Padding(0, 5, 5, 0);

//            // Delete Button
//            Button btnDelete = new Button();
//            btnDelete.Text = "Delete";
//            btnDelete.ForeColor = Color.White;
//            btnDelete.BackColor = Color.Crimson;
//            btnDelete.FlatStyle = FlatStyle.Flat;
//            btnDelete.Size = new Size(80, 30);
//            btnDelete.Cursor = Cursors.Hand;
//            btnDelete.FlatAppearance.BorderSize = 0;
//            btnDelete.Click += (s, e) => { DeleteSupplier(id); };

//            actionPanel.Controls.Add(btnDelete);

//            // 5. Assemble and add to FlowLayoutPanel
//            card.Controls.Add(lblDetails);
//            card.Controls.Add(lblName);
//            card.Controls.Add(actionPanel);

//            flowLayoutPanel1.Controls.Add(card);
//        }

//        private void DeleteSupplier(string id)
//        {
//            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
//            if (dialogResult == DialogResult.Yes)
//            {
//                try
//                {
//                    using (SqlConnection conn = new SqlConnection(connString))
//                    {
//                        string query = "DELETE FROM Suppliers WHERE SupplierID = @id";
//                        SqlCommand cmd = new SqlCommand(query, conn);
//                        cmd.Parameters.AddWithValue("@id", id);
//                        conn.Open();
//                        cmd.ExecuteNonQuery();

//                        LoadSuppliers(); // Refresh UI to show the updated list
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Could not delete: " + ex.Message);
//                }
//            }
//        }

//        // Navigation to Add Supplier Page
//        private void button5_Click(object sender, EventArgs e)
//        {
//            UC_Add_Supplier addSup = new UC_Add_Supplier();
//            Dashboard mainDash = (Dashboard)this.FindForm();
//            if (mainDash != null)
//            {
//                mainDash.addUserControl(addSup);
//            }
//        }

//        // Empty event handlers kept to prevent designer errors in Visual Studio
//        private void label1_Click(object sender, EventArgs e) { }
//        private void label2_Click(object sender, EventArgs e) { }
//        private void label3_Click(object sender, EventArgs e) { }
//        private void label5_Click(object sender, EventArgs e) { }
//        private void label8_Click(object sender, EventArgs e) { }
//        private void button1_Click(object sender, EventArgs e) { }
//        private void button3_Click(object sender, EventArgs e) { }
//        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
//        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e) { }
//        private void label10_Click(object sender, EventArgs e) { }
//        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
//    }
//}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Required for Database connection

namespace Tourist_Hotel_Inventory_Management
{
    public partial class UC_Suppliers : UserControl
    {
        // Connection string set to match your local database exactly
        string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TouristHotelInventoryDB;Integrated Security=True";

        public UC_Suppliers()
        {
            InitializeComponent();
            // NEW: This manually tells the control to run your code when it loads
            this.Load += new System.EventHandler(this.UC_Suppliers_Load);
         }

        //private void UC_Suppliers_Load(object sender, EventArgs e)
        //{
        //    LoadSuppliers();
        //}
        //private void UC_Suppliers_Load(object sender, EventArgs e)
        //{
        //    // 1. Stress Test: Make it red so we can see where it is
        //    //flowLayoutPanel1.BackColor = Color.Red;
        //    flowLayoutPanel1.BackColor = Color.FromArgb(240, 240, 240); // Light gray

        //    // 2. Bring it to the front just in case something is covering it
        //    flowLayoutPanel1.BringToFront();

        //    // 3. Try to load the cards
        //    LoadSuppliers();
        //}
        private void UC_Suppliers_Load(object sender, EventArgs e)
        {
            // Change Red back to a soft light gray or White
            flowLayoutPanel1.BackColor = Color.FromArgb(245, 245, 245);

            // Ensure the panel is ready to show the cards
            flowLayoutPanel1.AutoScroll = true;

            LoadSuppliers();
        }

        // Logic to fetch data from Database and create UI Cards
        //public void LoadSuppliers()
        //{
        //    // Clear existing cards from the flowLayoutPanel to prevent duplicates on refresh
        //    if (flowLayoutPanel1 != null)
        //    {
        //        flowLayoutPanel1.Controls.Clear();
        //    }

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connString))
        //        {
        //            // Select all relevant supplier columns - matching your SQL schema (SupplierId)
        //            string query = "SELECT SupplierId, SupplierName, Phone, Email, Address FROM Suppliers";
        //            SqlCommand cmd = new SqlCommand(query, conn);
        //            conn.Open();
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                // Generate a visual card for every row in the database
        //                AddSupplierCard(
        //                    reader["SupplierId"].ToString(),
        //                    reader["SupplierName"].ToString(),
        //                    reader["Phone"].ToString(),
        //                    reader["Email"].ToString(),
        //                    reader["Address"].ToString()
        //                );
        //            }
        //        }

        //        // Refresh layout to ensure cards are rendered properly
        //        flowLayoutPanel1.PerformLayout();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error loading suppliers: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        public void LoadSuppliers()
        {

            //MessageBox.Show("LoadSuppliers has started!");
             // 1. Clear existing controls to avoid duplicates
            flowLayoutPanel1.Controls.Clear();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // Using the exact column names from your database screenshot
                    string query = "SELECT SupplierId, SupplierName, Phone, Email, Address FROM Suppliers";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No suppliers found in the database.");
                        return;
                    }

                    while (reader.Read())
                    {
                        // 2. Map data to the card generator
                        AddSupplierCard(
                            reader["SupplierId"].ToString(),
                            reader["SupplierName"].ToString(),
                            reader["Phone"].ToString(),
                            reader["Email"].ToString(),
                            reader["Address"].ToString()
                        );
                    }

                    // Add this at the VERY END of your LoadSuppliers() function, after the while loop
                    flowLayoutPanel1.ResumeLayout();
                    flowLayoutPanel1.PerformLayout();
                    flowLayoutPanel1.Refresh(); // Forces a physical redraw of the red area
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
          
        }

        // Creates a stylized Panel "Card" for each supplier to match your design
        //private void AddSupplierCard(string id, string name, string phone, string email, string address)
        //{
        //    // 1. Create Main Card Panel Container
        //    Panel card = new Panel();
        //    card.Size = new Size(300, 220);
        //    card.BackColor = Color.White;
        //    card.Margin = new Padding(15);
        //    card.BorderStyle = BorderStyle.FixedSingle;

        //    // 2. Add Supplier Name (Header)
        //    Label lblName = new Label();
        //    lblName.Text = name.ToUpper();
        //    lblName.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        //    lblName.ForeColor = Color.FromArgb(28, 50, 78); // Dark Blue Theme
        //    lblName.Dock = DockStyle.Top;
        //    lblName.Height = 40;
        //    lblName.TextAlign = ContentAlignment.MiddleLeft;
        //    lblName.Padding = new Padding(10, 5, 0, 0);

        //    // 3. Add Contact Details (Body)
        //    Label lblDetails = new Label();
        //    lblDetails.Text = $"\n📞 {phone}\n\n📧 {email}\n\n📍 {address}";
        //    lblDetails.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        //    lblDetails.ForeColor = Color.DimGray;
        //    lblDetails.Dock = DockStyle.Fill;
        //    lblDetails.Padding = new Padding(10, 0, 10, 10);

        //    // 4. Action Buttons Panel (Bottom)
        //    FlowLayoutPanel actionPanel = new FlowLayoutPanel();
        //    actionPanel.Dock = DockStyle.Bottom;
        //    actionPanel.Height = 45;
        //    actionPanel.BackColor = Color.FromArgb(245, 245, 245); // Light Gray Footer
        //    actionPanel.FlowDirection = FlowDirection.RightToLeft;
        //    actionPanel.Padding = new Padding(0, 5, 5, 0);

        //    // Delete Button
        //    Button btnDelete = new Button();
        //    btnDelete.Text = "Delete";
        //    btnDelete.ForeColor = Color.White;
        //    btnDelete.BackColor = Color.Crimson;
        //    btnDelete.FlatStyle = FlatStyle.Flat;
        //    btnDelete.Size = new Size(80, 30);
        //    btnDelete.Cursor = Cursors.Hand;
        //    btnDelete.FlatAppearance.BorderSize = 0;
        //    btnDelete.Click += (s, e) => { DeleteSupplier(id); };

        //    actionPanel.Controls.Add(btnDelete);

        //    // 5. Assemble and add to FlowLayoutPanel
        //    card.Controls.Add(lblDetails);
        //    card.Controls.Add(lblName);
        //    card.Controls.Add(actionPanel);

        //    flowLayoutPanel1.Controls.Add(card);
        //}
        //private void AddSupplierCard(string id, string name, string phone, string email, string address)
        //{
        //    // Create Main Card Panel
        //    Panel card = new Panel
        //    {
        //        Size = new Size(350, 220),
        //        BackColor = Color.White,
        //        Margin = new Padding(10),
        //        BorderStyle = BorderStyle.FixedSingle
        //    };

        //    // Use a TableLayoutPanel to recreate your grid-style placeholder
        //    TableLayoutPanel table = new TableLayoutPanel
        //    {
        //        RowCount = 5,
        //        ColumnCount = 1,
        //        Dock = DockStyle.Fill,
        //        CellBorderStyle = TableLayoutPanelCellBorderStyle.Single // This gives the grid lines
        //    };

        //    // Add Data Labels
        //    table.Controls.Add(new Label { Text = name, Font = new Font("Segoe UI", 10, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
        //    table.Controls.Add(new Label { Text = phone, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
        //    table.Controls.Add(new Label { Text = email, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 2);
        //    table.Controls.Add(new Label { Text = address, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 3);

        //    // Add Buttons
        //    FlowLayoutPanel btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
        //    Button btnEdit = new Button { Text = "Edit", ForeColor = Color.Blue, FlatStyle = FlatStyle.Flat, Size = new Size(160, 30) };
        //    Button btnDelete = new Button { Text = "Delete", ForeColor = Color.Red, FlatStyle = FlatStyle.Flat, Size = new Size(160, 30) };

        //    btnDelete.Click += (s, e) => DeleteSupplier(id);

        //    btnPanel.Controls.Add(btnEdit);
        //    btnPanel.Controls.Add(btnDelete);
        //    table.Controls.Add(btnPanel, 0, 4);

        //    card.Controls.Add(table);

        //    // Crucial: Add to flow layout
        //    flowLayoutPanel1.Controls.Add(card);
        //}

        private void AddSupplierCard(string id, string name, string phone, string email, string address)
        {
            // 1. Create the card container
            Panel card = new Panel
            {
                Size = new Size(380, 240), // Slightly taller to fit long Ethiopian company names
                BackColor = Color.White,
                Margin = new Padding(15),
                BorderStyle = BorderStyle.FixedSingle
            };

            // 2. Use a TableLayoutPanel for the clean "Grid" look
            TableLayoutPanel table = new TableLayoutPanel
            {
                RowCount = 5,
                ColumnCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None // Cleaner look
            };

            // 3. Set up the Rows (Name, Category/Phone, Email, Address, Buttons)
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Name
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Phone
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Email
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Address (takes remaining space)
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Buttons

            // 4. Add the Data
            Label lblName = new Label { Text = name, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.MidnightBlue, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            Label lblPhone = new Label { Text = "📞 " + phone, Font = new Font("Segoe UI", 9), Dock = DockStyle.Fill };
            Label lblEmail = new Label { Text = "📧 " + email, Font = new Font("Segoe UI", 9), Dock = DockStyle.Fill };
            Label lblAddress = new Label { Text = "📍 " + address, Font = new Font("Segoe UI", 9), Dock = DockStyle.Fill, AutoSize = false };

            table.Controls.Add(lblName, 0, 0);
            table.Controls.Add(lblPhone, 0, 1);
            table.Controls.Add(lblEmail, 0, 2);
            table.Controls.Add(lblAddress, 0, 3);

            // 5. Action Buttons
            FlowLayoutPanel btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            Button btnDelete = new Button { Text = "Delete", BackColor = Color.Firebrick, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 35), Cursor = Cursors.Hand };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteSupplier(id);

            btnPanel.Controls.Add(btnDelete);
            table.Controls.Add(btnPanel, 0, 4);

            card.Controls.Add(table);

            // 6. Add to the flow panel
            flowLayoutPanel1.Controls.Add(card);
        }




        // Helper to style labels like your placeholder grid
        private Label CreateGridLabel(string text, bool isBold)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BorderStyle = BorderStyle.FixedSingle, // Creates the dashed/solid line look
                Font = new Font("Segoe UI", 10, isBold ? FontStyle.Bold : FontStyle.Regular),
                Padding = new Padding(10, 0, 0, 0)
            };
        }

        private void DeleteSupplier(string id)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        // Fixed: Using SupplierId to match your SQL schema
                        string query = "DELETE FROM Suppliers WHERE SupplierId = @id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        LoadSuppliers(); // Refresh UI to show the updated list
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Navigation to Add Supplier Page
        private void button5_Click(object sender, EventArgs e)
        {
            UC_Add_Supplier addSup = new UC_Add_Supplier();
            Dashboard mainDash = (Dashboard)this.FindForm();
            if (mainDash != null)
            {
                mainDash.addUserControl(addSup);
            }
        }

        // Empty event handlers kept to prevent designer errors in Visual Studio
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel13_Paint(object sender, PaintEventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) {
        //here is flowLayoutPanel1
        }

        private void UC_Suppliers_Load_1(object sender, EventArgs e)
        {

        }
    }
}