# Clothify - Mobile-First E-Commerce Web Application

**Module:** CT081-3-3 Mobile and Web Multimedia
**Framework:** ASP.NET Web Forms | HTML5 | CSS3 | JavaScript | Microsoft SQL Server

---

## Introduction

Clothify is a mobile-optimized E-Commerce web application designed as a platform for users to buy clothing and fashion products through an interactive and secure mobile web interface. It demonstrates effective use of interactive multimedia features while meeting the academic requirements of the Mobile and Web Multimedia module.

## Features

### Customer Features
- **Registration & Authentication** – Secure user signup and login with password hashing
- **Product Browsing** – Search products, filter by categories, view detailed product pages
- **Shopping Cart** – Add/remove items, update quantities, view subtotal and delivery fee
- **Checkout** – Nepal-specific shipping address (Province, District, Municipality, Ward No., Landmark) with multiple payment methods (Cash on Delivery, eSewa, Khalti, Bank Transfer)
- **Order Management** – View order history, filter by status, track delivery progress
- **Delivery Tracking** – Visual timeline showing order progression (Placed → Processing → Shipped → Delivered)
- **Customer Feedback** – Rate products (1–5) and leave comments

### Admin Features
- **Dashboard** – Overview with KPIs (Total Orders, Pending, Delivered, Total Users)
- **Manage Users** – View, edit roles, and delete users
- **Manage Products** – Full CRUD operations with category assignment
- **Manage Categories** – Create, edit, and delete product categories
- **Manage Orders** – Update order status, view order details and shipping info
- **Manage Feedback** – View and moderate customer reviews

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Framework | ASP.NET Web Forms (.NET Framework 4.8) |
| Language | C# |
| Database | Microsoft SQL Server |
| Data Access | ADO.NET (SqlConnection, SqlCommand, SqlDataAdapter) |
| Authentication | ASP.NET Forms Authentication |
| Frontend | HTML5, CSS3 (Mobile-First), JavaScript |
| Password Security | BCrypt hashing |

## Database Schema

The application uses 8 database tables:

| Table | Description |
|-------|-------------|
| **Roles** | User roles (Admin, Customer) |
| **Users** | User accounts with hashed passwords |
| **Categories** | Product categories |
| **Products** | Product catalog with pricing and stock |
| **Orders** | Customer orders with Nepal shipping addresses |
| **OrderItems** | Individual items within each order |
| **Payments** | Payment records (COD, eSewa, Khalti, Bank Transfer) |
| **Feedback** | Customer ratings and comments |

## Project Structure

```
Clothify/
├── Admin/                    # Admin panel pages
│   ├── AdminMaster.master    # Admin layout master page
│   ├── Dashboard.aspx        # Admin dashboard
│   ├── ManageUsers.aspx      # User management
│   ├── ManageProducts.aspx   # Product management
│   ├── ManageCategories.aspx # Category management
│   ├── ManageOrders.aspx     # Order management
│   └── ManageFeedback.aspx   # Feedback management
├── App_Code/                 # Shared code classes
│   ├── DBHelper.cs           # Database helper (ADO.NET)
│   └── CartItem.cs           # Shopping cart item model
├── css/
│   └── style.css             # Mobile-first stylesheet
├── js/
│   └── script.js             # Client-side JavaScript
├── Images/                   # Product images
├── SQL/
│   └── Database.sql          # Database creation script
├── Site.Master               # Main layout master page
├── Default.aspx              # Home page
├── Products.aspx             # Product listing & search
├── ProductDetail.aspx        # Product detail with reviews
├── Cart.aspx                 # Shopping cart
├── Checkout.aspx             # Checkout with shipping & payment
├── Orders.aspx               # Order history
├── OrderTracking.aspx        # Delivery tracking
├── Feedback.aspx             # Submit product feedback
├── Login.aspx                # User login
├── Register.aspx             # User registration
├── Profile.aspx              # User profile
├── EditProfile.aspx          # Edit profile details
├── ChangePassword.aspx       # Change password
├── Web.config                # Application configuration
└── Global.asax               # Application lifecycle events
```

## Setup Instructions

### Prerequisites
- Visual Studio 2019 or later
- Microsoft SQL Server (or SQL Server Express)
- .NET Framework 4.8

### Steps

1. **Create the Database**
   - Open SQL Server Management Studio (SSMS)
   - Execute the script at `SQL/Database.sql` to create the database, tables, and seed data

2. **Configure Connection String**
   - Open `Web.config`
   - Update the `ClothifyDB` connection string to match your SQL Server instance:
     ```xml
     <add name="ClothifyDB" connectionString="Server=YOUR_SERVER;Database=ClothifyDB;Trusted_Connection=True;" />
     ```

3. **Run the Application**
   - Open `Clothify.sln` in Visual Studio
   - Press `F5` or click **Start** to run

### Default Admin Account
- **Email:** admin@clothify.com
- **Password:** Admin@123

## Currency

All prices are displayed in **Nepalese Rupees (NPR)** formatted as `Rs. X,XXX`.

## Design Approach

- **Mobile-First** – Max-width 480px container optimized for mobile devices
- **Fixed Navigation** – Top header with branding + bottom navigation bar (Home, Shop, Orders, Profile)
- **Responsive** – Scales up to 768px+ for tablet/desktop viewing
- **Accessible** – Clear form labels, validation messages, and status indicators
