-- ============================================
-- Clothify E-Commerce Database
-- CT081-3-3 Mobile and Web Multimedia
-- ============================================
--
-- SETUP INSTRUCTIONS:
-- 1. In Visual Studio, right-click the App_Data folder -> Add -> SQL Server Database
-- 2. Name it "ClothifyDB.mdf" and click OK
-- 3. Double-click ClothifyDB.mdf in App_Data to open it in Server Explorer
-- 4. Right-click on the database connection -> New Query
-- 5. Paste this entire script and execute it (Ctrl+Shift+E)
--
-- NOTE: Do NOT use CREATE DATABASE or USE statements.
-- The .mdf file is created by Visual Studio and attached via
-- the AttachDbFilename option in Web.config connection string.
-- ============================================

-- ============================================
-- Table: Roles
-- ============================================
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

-- ============================================
-- Table: Categories
-- ============================================
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

-- ============================================
-- Table: Users
-- ============================================
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    PhoneNumber NVARCHAR(20),
    RoleID INT FOREIGN KEY REFERENCES Roles(RoleID),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ============================================
-- Table: Products
-- ============================================
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    ImageURL NVARCHAR(500),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID)
);

-- ============================================
-- Table: Orders
-- ============================================
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderStatus NVARCHAR(50) NOT NULL DEFAULT 'Placed',
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    ShippingFullName NVARCHAR(100),
    ShippingPhone NVARCHAR(20),
    Province NVARCHAR(100),
    District NVARCHAR(100),
    Municipality NVARCHAR(100),
    WardNo NVARCHAR(10),
    Landmark NVARCHAR(200),
    DeliveryFee DECIMAL(10,2) DEFAULT 150.00
);

-- ============================================
-- Table: Feedback
-- ============================================
CREATE TABLE Feedback (
    FeedbackID INT PRIMARY KEY IDENTITY(1,1),
    Rating INT NOT NULL CHECK(Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(MAX),
    FeedbackDate DATETIME DEFAULT GETDATE(),
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID)
);

-- ============================================
-- Table: Payments
-- ============================================
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    PaymentMethod NVARCHAR(50) NOT NULL,
    PaymentStatus NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    PaymentDate DATETIME DEFAULT GETDATE(),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID)
);

-- ============================================
-- Table: OrderItems
-- ============================================
CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID)
);

-- ============================================
-- Seed Data
-- ============================================

-- Insert Roles
INSERT INTO Roles (RoleName) VALUES ('Admin'), ('Customer');

-- Insert Admin User (Password: Admin@123)
INSERT INTO Users (FullName, Email, PasswordHash, PhoneNumber, RoleID, CreatedAt)
VALUES ('Admin User', 'admin@clothify.com',
    'AQAAAAEAACcQAAAAEH0jA2VG5bVHqLOvMz8xPvGrKj3K9VfMn5kNQzXwT4sY7L8MpR2JFHtN6WdG3xKQA==',
    '9800000000', 1, GETDATE());

-- Insert Categories
INSERT INTO Categories (CategoryName, Description) VALUES
('Men''s Clothing', 'Clothing items for men including shirts, pants, and jackets'),
('Women''s Clothing', 'Clothing items for women including dresses, tops, and skirts'),
('Kids'' Clothing', 'Clothing items for children'),
('Accessories', 'Fashion accessories including bags, belts, and hats'),
('Footwear', 'Shoes, sandals, and boots for all');

-- Insert Sample Products
INSERT INTO Products (ProductName, Description, Price, StockQuantity, ImageURL, CategoryID) VALUES
('Classic Cotton T-Shirt', 'Comfortable cotton t-shirt perfect for everyday wear. Available in multiple colors.', 1200.00, 50, '/Images/products/tshirt1.jpg', 1),
('Slim Fit Jeans', 'Modern slim fit denim jeans with stretch fabric for comfort.', 2500.00, 30, '/Images/products/jeans1.jpg', 1),
('Formal Shirt', 'Premium quality formal shirt suitable for office and events.', 1800.00, 25, '/Images/products/shirt1.jpg', 1),
('Summer Dress', 'Light and breezy summer dress with floral pattern.', 3200.00, 20, '/Images/products/dress1.jpg', 2),
('Casual Kurti', 'Traditional yet modern kurti perfect for casual outings.', 1500.00, 35, '/Images/products/kurti1.jpg', 2),
('Denim Jacket', 'Classic denim jacket for a trendy layered look.', 3800.00, 15, '/Images/products/jacket1.jpg', 1),
('Kids Cartoon Tee', 'Fun cartoon printed t-shirt for kids.', 800.00, 40, '/Images/products/kidstee1.jpg', 3),
('Leather Belt', 'Genuine leather belt with metal buckle.', 950.00, 60, '/Images/products/belt1.jpg', 4),
('Canvas Sneakers', 'Lightweight canvas sneakers for daily comfort.', 2200.00, 28, '/Images/products/sneakers1.jpg', 5),
('Wool Sweater', 'Warm wool sweater for cold winter days.', 2800.00, 18, '/Images/products/sweater1.jpg', 1),
('Silk Saree', 'Elegant silk saree with intricate border design.', 5500.00, 12, '/Images/products/saree1.jpg', 2),
('Running Shoes', 'Professional running shoes with cushioned sole.', 4500.00, 22, '/Images/products/running1.jpg', 5);

GO
