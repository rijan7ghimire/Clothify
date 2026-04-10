# Clothify - Mobile-First E-Commerce Web Application

**Module:** CT081-3-3 Mobile and Web Multimedia
**Framework:** ASP.NET Web Forms | HTML5 | CSS3 | JavaScript | Microsoft SQL Server

---

## Introduction

Clothify is a mobile-optimized E-Commerce web application designed as a platform for users to buy clothing and fashion products through an interactive and secure mobile web interface. It demonstrates effective use of interactive multimedia features while meeting the academic requirements of the Mobile and Web Multimedia module.

## Features

### Customer Features
- **Registration & Authentication** – Secure user signup and login with BCrypt password hashing
- **Product Browsing** – Search products, filter by categories, view detailed product pages
- **Shopping Cart** – Add/remove items, update quantities, view subtotal and delivery fee
- **Checkout** – Nepal-specific shipping address (Province, District, Municipality, Ward No., Landmark) with multiple payment methods (Cash on Delivery, eSewa, Khalti, Bank Transfer)
- **Order Management** – View order history, filter by status, track delivery progress
- **Delivery Tracking** – Visual timeline showing order progression (Placed → Processing → Shipped → Delivered)
- **Order Cancellation** – Cancel pending orders with automatic stock restoration
- **Wishlist** – Save products for later, move to cart directly
- **Customer Feedback** – Rate products (1–5 stars) and leave comments
- **Notifications** – View order status updates and alerts
- **Email Notifications** – Receive order confirmation and status update emails
- **Profile Management** – Edit profile details, change password

### Admin Features
- **Dashboard** – Overview with KPIs (Total Orders, Pending, Delivered, Total Users)
- **Manage Users** – View, edit roles, and delete users (GridView with inline editing)
- **Manage Products** – Full CRUD operations with category assignment
- **Manage Categories** – Create, edit, and delete product categories
- **Manage Orders** – Update order status, view order details, shipping info, and items
- **Manage Feedback** – View, filter by rating, and delete customer reviews
- **Sales Reports** – Revenue analytics, monthly comparisons, top-selling products, recent orders

### Technical Features
- **ASP.NET Web Forms User Controls** (.ascx) – ProductCard, OrderStatusBadge, StarRating, NotificationBar
- **Advanced Web Controls** – Repeater, DataList, GridView, DetailsView
- **Data Binding** – Server controls bound to SQL data sources
- **Form Validation** – RequiredFieldValidator, RegularExpressionValidator, CompareValidator
- **Master Pages** – Site.Master (customer), AdminMaster.master (admin)
- **Forms Authentication** – Role-based authorization (Admin/Customer)

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Framework | ASP.NET Web Forms (.NET Framework 4.8) |
| Language | C# |
| Database | Microsoft SQL Server (LocalDB) |
| Data Access | ADO.NET (SqlConnection, SqlCommand, SqlDataAdapter) |
| Authentication | ASP.NET Forms Authentication |
| Frontend | HTML5, CSS3 (Mobile-First), JavaScript |
| Password Security | BCrypt hashing (BCrypt.Net-Next) |
| Email | System.Net.Mail (SMTP) |

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
│   ├── ManageUsers.aspx      # User management (GridView)
│   ├── ManageProducts.aspx   # Product management
│   ├── ManageCategories.aspx # Category management
│   ├── ManageOrders.aspx     # Order management
│   ├── ManageFeedback.aspx   # Feedback management
│   └── Reports.aspx          # Sales reports & analytics
├── App_Code/                 # Shared code classes
│   ├── DBHelper.cs           # Database helper (ADO.NET)
│   ├── CartItem.cs           # Shopping cart item model
│   └── EmailHelper.cs        # Email notification utility
├── App_Data/                 # Database files (.mdf)
├── Controls/                 # ASP.NET User Controls
│   ├── ProductCard.ascx      # Reusable product card
│   ├── OrderStatusBadge.ascx # Order status badge
│   ├── StarRating.ascx       # Star rating display
│   └── NotificationBar.ascx  # Alert/notification bar
├── css/
│   └── style.css             # Mobile-first stylesheet
├── js/
│   └── script.js             # Client-side JavaScript
├── Images/                   # Product images
├── SQL/
│   └── Database.sql          # Database creation script
├── Properties/
│   └── AssemblyInfo.cs       # Assembly metadata
├── Site.Master               # Main layout master page
├── Default.aspx              # Home page
├── Products.aspx             # Product listing (DataList)
├── ProductDetail.aspx        # Product detail (DetailsView)
├── Cart.aspx                 # Shopping cart
├── Checkout.aspx             # Checkout with shipping & payment
├── Orders.aspx               # Order history
├── OrderTracking.aspx        # Delivery tracking timeline
├── Wishlist.aspx             # Product wishlist
├── Notifications.aspx        # Order notifications
├── Feedback.aspx             # Submit product feedback
├── Login.aspx                # User login
├── Register.aspx             # User registration
├── Profile.aspx              # User profile
├── EditProfile.aspx          # Edit profile details
├── ChangePassword.aspx       # Change password
├── ReadMe.html               # Setup guide & credentials
├── Web.config                # Application configuration
└── Global.asax               # Application lifecycle events
```

## Setup Instructions

### Prerequisites
- Visual Studio 2022 (Community or higher)
- "ASP.NET and web development" workload installed
- SQL Server Express LocalDB (included with Visual Studio)

### Steps

1. **Open the Project**
   - Open `Clothify.sln` in Visual Studio 2022
   - Right-click solution → **Restore NuGet Packages**

2. **Create the Database**
   - Right-click `App_Data` folder → **Add** → **SQL Server Database**
   - Name it `ClothifyDB.mdf` and click OK
   - Double-click `ClothifyDB.mdf` to open in Server Explorer
   - Right-click the connection → **New Query**
   - Open `SQL/Database.sql`, paste the script, and execute (Ctrl+Shift+E)

3. **Run the Application**
   - Press `F5` or click **Start** to run with IIS Express
   - The application opens in your default browser

### Default Accounts

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@clothify.com | Admin@123 |
| Customer | *(Register a new account)* | |

### Email Configuration (Optional)
To enable email notifications, update the SMTP settings in `Web.config`:
```xml
<appSettings>
    <add key="SmtpUser" value="your-email@gmail.com" />
    <add key="SmtpPass" value="your-app-password" />
</appSettings>
```

## Currency

All prices are displayed in **Nepalese Rupees (NPR)** formatted as `Rs. X,XXX`.

## Design Approach

- **Mobile-First** – Max-width 480px container optimized for mobile devices
- **Fixed Navigation** – Top header with branding + bottom navigation bar (Home, Shop, Orders, Profile)
- **Responsive** – Scales up to 768px+ for tablet/desktop viewing
- **Accessible** – Clear form labels, validation messages, and status indicators

## Browser Compatibility

- Google Chrome (Recommended)
- Microsoft Edge
- Mozilla Firefox
- Opera
- Safari (macOS/iOS)