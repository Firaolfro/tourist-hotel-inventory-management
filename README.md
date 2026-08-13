# 🏨 Tourist Hotel Inventory Management System

A desktop inventory management application developed for **Tourist Hotel (Arba Minch)** to streamline stock tracking, manage hotel inventory categories, and provide administrative oversight of stock levels and inventory movements.

The system helps hotel staff manage inventory efficiently by recording stock entries, usage, transfers, and monitoring current stock levels through a centralized application.

---

## 📌 Project Overview

The **Tourist Hotel Inventory Management System** is a Windows desktop application designed to improve the management of hotel inventory.

It provides staff and administrators with tools to:

- Manage inventory items and categories
- Track stock quantities
- Record inbound and outbound inventory operations
- Monitor stock levels
- Identify low-stock items
- Maintain inventory records and audit information
- Control access through user authentication and roles

The system was developed as a practical software solution for hotel inventory operations.

---

## 🛠️ Technologies Used

| Technology | Purpose |
|------------|---------|
| **C#** | Application programming language |
| **Windows Forms (WinForms)** | Desktop user interface |
| **.NET** | Application framework |
| **Entity Framework Core** | Object-relational mapping (ORM) |
| **Microsoft SQL Server** | Database management |
| **Visual Studio** | Development environment |
| **Git & GitHub** | Version control and source code management |

### Architecture

The application follows a **component-based / layered architecture** to separate application responsibilities and improve maintainability.

---

## ✨ Key Features

### 🔐 User Authentication & Role Management

- Secure login interface
- User authentication
- Role-based access control
- Separate access for staff and administrators

### 📦 Inventory & Stock Management

- Add and manage inventory items
- Organize items into categories
- Track available stock quantities
- Automatically update item stock levels
- Monitor inventory changes

Example inventory categories include:

- Kitchen items
- Beverages
- Room supplies
- Maintenance assets

### 🔄 Inbound & Outbound Operations

The system records inventory movements, including:

- Received items
- Stock additions
- Internal stock transfers
- Item usage
- Stock deductions

This provides a clear record of how inventory enters and leaves the hotel.

### 📊 Dashboard & Reporting

The dashboard provides an overview of inventory information, including:

- Current stock levels
- Inventory status
- Low-stock alerts
- Inventory activity
- Audit information

---

## 🗄️ Database

The application uses:

- **Microsoft SQL Server**
- **Entity Framework Core**

The main database is:

```text
TouristHotelInventoryDB
