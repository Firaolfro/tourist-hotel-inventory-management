using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourist_Hotel_Inventory_Management
{
    internal class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }
        public int MinStock { get; set; }
        public string Status => Quantity <= MinStock ? "Low Stock" : "In Stock";
    }
}
