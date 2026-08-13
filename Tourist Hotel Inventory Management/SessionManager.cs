using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourist_Hotel_Inventory_Management
{
    public static class SessionManager
    {
        // This will store the username of the person who just logged in
        public static string CurrentUsername { get; set; }

        // You can also store their role (Admin/Staff) for permission checks
        public static string CurrentRole { get; set; }
    }
}