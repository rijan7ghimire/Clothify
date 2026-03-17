# Clothify

A mobile-first e-commerce clothing platform built with .NET 10, featuring 2,906 real products with images, Nepali Rupee pricing, Khalti/eSewa payment integration, and a full admin dashboard.

![.NET 10](https://img.shields.io/badge/.NET-10.0-purple)
![Razor Pages](https://img.shields.io/badge/Frontend-Razor%20Pages-blue)
![SQLite](https://img.shields.io/badge/Database-SQLite-green)
![Products](https://img.shields.io/badge/Products-2%2C906-orange)

---

## Quick Start

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (only requirement)

### Run in 3 commands
```bash
git clone https://github.com/rijan7ghimire/Clothify.git
cd Clothify
dotnet run --project src/Clothify.Web --urls "http://localhost:5010"
```

Open **http://localhost:5010** in your browser.

> The app automatically creates the SQLite database, runs migrations, and seeds all 2,906 products on first startup. No manual DB setup needed.

### Login Credentials
| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@clothify.com` | `Admin@123!` |
| Customer | `sarah@example.com` | `Customer@123!` |

Or register a new account at `/Auth/Register`.

---

## Install on Phone (PWA)

Clothify is a **Progressive Web App** — it installs on your phone like a native app, no app store needed.

### Step 1: Find your PC's IP

```bash
ipconfig
```

Look for `IPv4 Address` (e.g. `192.168.1.5`). Your phone must be on the **same WiFi**.

### Step 2: Run the app

```bash
dotnet run --project src/Clothify.Web --urls "http://0.0.0.0:5010"
```

### Step 3: Install on your phone

**Android (Chrome):**
1. Open `http://192.168.1.5:5010` in Chrome
2. Tap the **"Install Clothify"** banner at the bottom, OR
3. Chrome menu (3 dots) > **"Add to Home Screen"**

**iOS (Safari):**
1. Open `http://192.168.1.5:5010` in Safari
2. Tap the **Share** button (box with arrow)
3. Scroll down > **"Add to Home Screen"** > **Add**

### What you get
- App icon on home screen (black "C" with gold "CLOTHIFY")
- Opens fullscreen — no browser address bar
- Offline support for cached pages and product images
- App shortcuts: long-press icon for Search, Cart, Orders

---

## Features

### Customer-Facing
- **Home** — Hero banners, category scroll with product images, featured products grid, new arrivals
- **Product Listing** — Filter by category (Boys, Girls, Men, Women), sort by price/newest, pagination
- **Product Detail** — Size/color selection, quantity selector, reviews, Add to Cart + Buy Now
- **Search** — Text search with trending tags, filter bottom sheet (size, color, price range)
- **Cart** — Item list with quantity controls, promo code input, order summary
- **Checkout** — Address form with autofill ("Same as me"), Khalti/eSewa/COD payment options
- **Payment** — Khalti & eSewa payment simulation pages
- **Order Tracking** — 6-step timeline (Placed > Confirmed > Processing > Shipped > Out for Delivery > Delivered)
- **Profile** — Edit personal info, saved addresses (add/delete), change password, order history, help center

### Admin Dashboard (`/Admin`)
- KPI cards (Revenue, Orders, Customers, Conversion)
- Recent orders table with status dropdown (update to Confirmed/Shipped/Delivered)
- Low stock alerts

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core .NET 10, C# |
| Frontend | Razor Pages + Custom CSS Framework (700+ lines) |
| Database | SQLite via Entity Framework Core 10 |
| Auth | ASP.NET Core Identity (cookie auth) |
| Architecture | Clean Architecture (5 projects) |
| API | RESTful with Swagger (`/swagger` on API project) |

---

## Project Structure
```
Clothify.sln
├── src/
│   ├── Clothify.Core/             # Entities, Enums, Interfaces
│   ├── Clothify.Application/      # Services, DTOs, AutoMapper
│   ├── Clothify.Infrastructure/   # EF Core, Repositories, Data Seeder
│   ├── Clothify.API/              # REST API Controllers, JWT Auth
│   └── Clothify.Web/              # Razor Pages (all screens)
│       ├── Pages/
│       │   ├── Home/              # Landing page
│       │   ├── Auth/              # Login, Register
│       │   ├── Products/          # Listing, Detail, Search
│       │   ├── Cart/              # Shopping cart
│       │   ├── Checkout/          # Checkout + Payment
│       │   ├── Orders/            # History, Tracking, Confirmation
│       │   ├── Profile/           # Edit, Addresses, Password, Help
│       │   └── Admin/             # Dashboard
│       └── wwwroot/
│           ├── css/clothify.css   # Mobile-first CSS framework
│           ├── js/clothify.js     # UI interactions
│           └── images/products/   # 2,906 product images
└── data/
    ├── fashion.csv                # Product dataset (2,906 items)
    ├── Apparel/                   # Boys & Girls clothing images
    └── Footwear/                  # Men & Women shoe images
```

---

## Seeded Data

| Data | Count |
|------|-------|
| Products | 2,906 |
| Product Images | 2,906 (local JPGs) |
| Product Variants | ~9,000 (sizes/colors) |
| Categories | 20 (4 top-level + 16 sub) |
| Users | 6 (1 admin + 5 customers) |
| Reviews | ~1,500 |
| Orders | 12 |
| Coupons | 3 (WELCOME20, SAVE500, SPRING30) |

### Categories
- **Boys** — Topwear, Bottomwear, Apparel Sets, Innerwear
- **Girls** — Topwear, Bottomwear, Dresses, Apparel Sets, Innerwear
- **Men** — Shoes, Sandals, Flip Flops
- **Women** — Shoes, Sandals, Flip Flops, Heels

---

## Pricing (Nepali Rupees)

| Category | Price Range |
|----------|------------|
| Men's Shoes | Rs. 3,500 - 18,000 |
| Women's Shoes | Rs. 2,500 - 15,000 |
| Sandals | Rs. 1,500 - 6,000 |
| Kids Tops | Rs. 800 - 4,500 |
| Dresses | Rs. 1,800 - 8,000 |
| Free shipping | Orders over Rs. 5,000 |
| Tax | 13% VAT |

---

## API Endpoints

The REST API runs separately on `https://localhost:7001` with Swagger docs:

```bash
dotnet run --project src/Clothify.API
```

| Endpoint | Description |
|----------|-------------|
| `POST /api/auth/login` | Login (returns JWT) |
| `POST /api/auth/register` | Register new user |
| `GET /api/products` | Search/filter products |
| `GET /api/products/{id}` | Product detail |
| `GET /api/cart` | Get user cart |
| `POST /api/cart/items` | Add to cart |
| `POST /api/orders` | Place order |
| `GET /api/orders` | Order history |
| `GET /api/admin/dashboard` | Admin KPIs |

---

## Screenshots

| Home | Product Detail | Cart | Checkout |
|------|---------------|------|----------|
| Category scroll, banners, product grid | Image, size/color picker, reviews | Items, quantity, promo code | Address, payment (Khalti/eSewa/COD) |

---

## License

MIT
