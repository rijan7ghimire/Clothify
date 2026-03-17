using Clothify.Core.Entities;
using Clothify.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Clothify.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Only seed if products table is empty
        if (await context.Products.AnyAsync()) return;

        // ─── USERS ───
        var admin = new ApplicationUser
        {
            FirstName = "Admin",
            LastName = "Clothify",
            Email = "admin@clothify.com",
            UserName = "admin@clothify.com",
            EmailConfirmed = true,
            PhoneNumber = "+1-555-0100",
            AvatarUrl = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150&h=150&fit=crop&crop=face"
        };
        await userManager.CreateAsync(admin, "Admin@123!");
        await userManager.AddToRoleAsync(admin, "Admin");

        var customers = new List<ApplicationUser>();
        var customerData = new[]
        {
            ("Sarah", "Johnson", "sarah@example.com", "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=150&h=150&fit=crop&crop=face"),
            ("Michael", "Chen", "michael@example.com", "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&h=150&fit=crop&crop=face"),
            ("Emily", "Davis", "emily@example.com", "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=150&h=150&fit=crop&crop=face"),
            ("James", "Wilson", "james@example.com", "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&h=150&fit=crop&crop=face"),
            ("Olivia", "Martinez", "olivia@example.com", "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=150&h=150&fit=crop&crop=face"),
        };

        foreach (var (first, last, email, avatar) in customerData)
        {
            var user = new ApplicationUser
            {
                FirstName = first, LastName = last, Email = email,
                UserName = email, EmailConfirmed = true, AvatarUrl = avatar,
                PhoneNumber = $"+1-555-{Random.Shared.Next(1000, 9999)}"
            };
            await userManager.CreateAsync(user, "Customer@123!");
            await userManager.AddToRoleAsync(user, "Customer");
            customers.Add(user);
        }

        // ─── PRODUCTS ───
        // Using free Unsplash images for clothing

        var products = new List<Product>
        {
            // ══════ MEN (CategoryId: 1) ══════
            new()
            {
                Name = "Classic White Oxford Shirt", Slug = "classic-white-oxford-shirt",
                Description = "A timeless wardrobe essential. This crisp white Oxford shirt is crafted from premium 100% cotton with a button-down collar, offering a clean and polished look perfect for both casual and semi-formal occasions. Features a tailored fit with reinforced stitching.",
                Brand = "Clothify Essentials", BasePrice = 59.99m, CategoryId = 1,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "White Oxford Shirt front view" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1598033129183-c4f50c736c10?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "White Oxford Shirt detail" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1603252109303-2751441dd157?w=600&h=800&fit=crop", DisplayOrder = 2, AltText = "White Oxford Shirt styled" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "MEN-OXF-WHT-S", Size = "S", Color = "White", StockQuantity = 25 },
                    new() { SKU = "MEN-OXF-WHT-M", Size = "M", Color = "White", StockQuantity = 40 },
                    new() { SKU = "MEN-OXF-WHT-L", Size = "L", Color = "White", StockQuantity = 35 },
                    new() { SKU = "MEN-OXF-WHT-XL", Size = "XL", Color = "White", StockQuantity = 20 },
                    new() { SKU = "MEN-OXF-BLU-M", Size = "M", Color = "Blue", StockQuantity = 30 },
                    new() { SKU = "MEN-OXF-BLU-L", Size = "L", Color = "Blue", StockQuantity = 28 },
                }
            },
            new()
            {
                Name = "Slim Fit Chino Trousers", Slug = "slim-fit-chino-trousers",
                Description = "Versatile slim-fit chinos in a soft stretch cotton blend. These modern trousers feature a comfortable mid-rise waist, tapered leg, and clean finish that pairs perfectly with both casual tees and dress shirts.",
                Brand = "Clothify", BasePrice = 79.99m, DiscountPrice = 59.99m, CategoryId = 1,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Slim Fit Chinos" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Chinos detail" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "MEN-CHN-BEG-S", Size = "S", Color = "Beige", StockQuantity = 20 },
                    new() { SKU = "MEN-CHN-BEG-M", Size = "M", Color = "Beige", StockQuantity = 35 },
                    new() { SKU = "MEN-CHN-BEG-L", Size = "L", Color = "Beige", StockQuantity = 30 },
                    new() { SKU = "MEN-CHN-NAV-M", Size = "M", Color = "Navy", StockQuantity = 25 },
                    new() { SKU = "MEN-CHN-NAV-L", Size = "L", Color = "Navy", StockQuantity = 22 },
                    new() { SKU = "MEN-CHN-BLK-M", Size = "M", Color = "Black", StockQuantity = 18 },
                }
            },
            new()
            {
                Name = "Denim Jacket - Vintage Wash", Slug = "denim-jacket-vintage-wash",
                Description = "A rugged yet refined denim jacket featuring a vintage wash finish. Made from heavyweight 12oz denim with copper button closures, dual chest pockets, and an adjustable waist tab for the perfect fit.",
                Brand = "Heritage Denim", BasePrice = 129.99m, CategoryId = 9,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Denim Jacket front" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1576995853123-5a10305d93c0?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Denim Jacket back" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1495105787522-5334e726fa0d?w=600&h=800&fit=crop", DisplayOrder = 2, AltText = "Denim Jacket styled" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "MEN-DNM-BLU-S", Size = "S", Color = "Blue", StockQuantity = 15 },
                    new() { SKU = "MEN-DNM-BLU-M", Size = "M", Color = "Blue", StockQuantity = 25 },
                    new() { SKU = "MEN-DNM-BLU-L", Size = "L", Color = "Blue", StockQuantity = 20 },
                    new() { SKU = "MEN-DNM-BLU-XL", Size = "XL", Color = "Blue", StockQuantity = 10 },
                }
            },
            new()
            {
                Name = "Premium Cotton Crew Neck Tee", Slug = "premium-cotton-crew-neck-tee",
                Description = "Super soft 100% organic cotton t-shirt with a relaxed crew neck fit. Pre-shrunk fabric ensures lasting comfort wash after wash. A daily essential in every color.",
                Brand = "Clothify Basics", BasePrice = 29.99m, CategoryId = 1,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "White Crew Tee" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Black Crew Tee" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "MEN-TEE-WHT-S", Size = "S", Color = "White", StockQuantity = 50 },
                    new() { SKU = "MEN-TEE-WHT-M", Size = "M", Color = "White", StockQuantity = 60 },
                    new() { SKU = "MEN-TEE-WHT-L", Size = "L", Color = "White", StockQuantity = 55 },
                    new() { SKU = "MEN-TEE-BLK-S", Size = "S", Color = "Black", StockQuantity = 45 },
                    new() { SKU = "MEN-TEE-BLK-M", Size = "M", Color = "Black", StockQuantity = 58 },
                    new() { SKU = "MEN-TEE-BLK-L", Size = "L", Color = "Black", StockQuantity = 50 },
                    new() { SKU = "MEN-TEE-GRY-M", Size = "M", Color = "Grey", StockQuantity = 40 },
                }
            },
            new()
            {
                Name = "Wool Blend Overcoat", Slug = "wool-blend-overcoat",
                Description = "Sophisticated single-breasted overcoat in a luxurious wool-cashmere blend. Features notch lapels, two flap pockets, and a clean silhouette that elevates any winter outfit.",
                Brand = "Clothify Premium", BasePrice = 249.99m, DiscountPrice = 189.99m, CategoryId = 9,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1544923246-77307dd270b5?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Wool Overcoat" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1539533018447-63fcce2678e3?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Overcoat detail" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "MEN-COT-BLK-M", Size = "M", Color = "Black", StockQuantity = 12 },
                    new() { SKU = "MEN-COT-BLK-L", Size = "L", Color = "Black", StockQuantity = 15 },
                    new() { SKU = "MEN-COT-CAM-M", Size = "M", Color = "Beige", StockQuantity = 10 },
                    new() { SKU = "MEN-COT-CAM-L", Size = "L", Color = "Beige", StockQuantity = 8 },
                }
            },
            new()
            {
                Name = "Stretch Slim Fit Jeans", Slug = "stretch-slim-fit-jeans",
                Description = "Modern slim fit jeans with 2% elastane for all-day comfort. Dark indigo wash with subtle whiskering for a worn-in look. Five-pocket styling with branded rivets.",
                Brand = "Heritage Denim", BasePrice = 89.99m, CategoryId = 1,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Slim Fit Jeans" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Jeans detail" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "MEN-JNS-DRK-S", Size = "S", Color = "Blue", StockQuantity = 20 },
                    new() { SKU = "MEN-JNS-DRK-M", Size = "M", Color = "Blue", StockQuantity = 35 },
                    new() { SKU = "MEN-JNS-DRK-L", Size = "L", Color = "Blue", StockQuantity = 30 },
                    new() { SKU = "MEN-JNS-DRK-XL", Size = "XL", Color = "Blue", StockQuantity = 15 },
                    new() { SKU = "MEN-JNS-BLK-M", Size = "M", Color = "Black", StockQuantity = 25 },
                }
            },

            // ══════ WOMEN - TOPS (CategoryId: 6) ══════
            new()
            {
                Name = "Silk Blend Blouse", Slug = "silk-blend-blouse",
                Description = "Elegant silk-blend blouse with a relaxed fit and delicate V-neckline. The fluid drape and subtle sheen make this piece perfect for office to evening transitions.",
                Brand = "Clothify Luxe", BasePrice = 89.99m, CategoryId = 6,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1564257631407-4deb1f99d992?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Silk Blouse" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1551163943-3f6a855d1153?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Blouse styled" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "WMN-BLS-WHT-XS", Size = "XS", Color = "White", StockQuantity = 18 },
                    new() { SKU = "WMN-BLS-WHT-S", Size = "S", Color = "White", StockQuantity = 25 },
                    new() { SKU = "WMN-BLS-WHT-M", Size = "M", Color = "White", StockQuantity = 30 },
                    new() { SKU = "WMN-BLS-BLK-S", Size = "S", Color = "Black", StockQuantity = 20 },
                    new() { SKU = "WMN-BLS-BLK-M", Size = "M", Color = "Black", StockQuantity = 22 },
                    new() { SKU = "WMN-BLS-PNK-S", Size = "S", Color = "Pink", StockQuantity = 15 },
                }
            },
            new()
            {
                Name = "Oversized Knit Sweater", Slug = "oversized-knit-sweater",
                Description = "Cozy oversized sweater in a chunky knit pattern. Made from a soft wool-acrylic blend with dropped shoulders and ribbed cuffs. Perfect for layering on cool days.",
                Brand = "Clothify", BasePrice = 69.99m, DiscountPrice = 49.99m, CategoryId = 6,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1576871337632-b9aef4c17ab9?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Oversized Sweater" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1631541909061-71e349d1f203?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Sweater detail" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "WMN-SWT-CRM-S", Size = "S", Color = "Beige", StockQuantity = 22 },
                    new() { SKU = "WMN-SWT-CRM-M", Size = "M", Color = "Beige", StockQuantity = 28 },
                    new() { SKU = "WMN-SWT-CRM-L", Size = "L", Color = "Beige", StockQuantity = 20 },
                    new() { SKU = "WMN-SWT-GRY-M", Size = "M", Color = "Grey", StockQuantity = 18 },
                    new() { SKU = "WMN-SWT-BRN-M", Size = "M", Color = "Brown", StockQuantity = 15 },
                }
            },
            new()
            {
                Name = "Cropped Linen Tank Top", Slug = "cropped-linen-tank-top",
                Description = "Lightweight and breathable linen tank top with a flattering cropped length. Square neckline with adjustable straps for a customizable fit. Ideal for summer styling.",
                Brand = "Clothify Basics", BasePrice = 34.99m, CategoryId = 6,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1586790170083-2f9ceadc732d?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Linen Tank" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "WMN-TNK-WHT-XS", Size = "XS", Color = "White", StockQuantity = 30 },
                    new() { SKU = "WMN-TNK-WHT-S", Size = "S", Color = "White", StockQuantity = 40 },
                    new() { SKU = "WMN-TNK-WHT-M", Size = "M", Color = "White", StockQuantity = 35 },
                    new() { SKU = "WMN-TNK-BLK-S", Size = "S", Color = "Black", StockQuantity = 25 },
                }
            },

            // ══════ WOMEN - DRESSES (CategoryId: 8) ══════
            new()
            {
                Name = "Floral Midi Wrap Dress", Slug = "floral-midi-wrap-dress",
                Description = "Stunning midi wrap dress in a hand-painted floral print. Features a flattering V-neck wrap front, flutter sleeves, and a self-tie belt that cinches the waist beautifully.",
                Brand = "Clothify", BasePrice = 119.99m, DiscountPrice = 89.99m, CategoryId = 8,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1572804013309-59a88b7e92f1?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Floral Wrap Dress" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1595777457583-95e059d581b8?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Dress side view" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1568252542512-9fe8fe9c87bb?w=600&h=800&fit=crop", DisplayOrder = 2, AltText = "Dress detail" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "WMN-DRS-FLR-XS", Size = "XS", Color = "Pink", StockQuantity = 12 },
                    new() { SKU = "WMN-DRS-FLR-S", Size = "S", Color = "Pink", StockQuantity = 20 },
                    new() { SKU = "WMN-DRS-FLR-M", Size = "M", Color = "Pink", StockQuantity = 25 },
                    new() { SKU = "WMN-DRS-FLR-L", Size = "L", Color = "Pink", StockQuantity = 15 },
                }
            },
            new()
            {
                Name = "Little Black Dress", Slug = "little-black-dress",
                Description = "The ultimate LBD. A sleek, fitted silhouette with a subtle scoop neck and knee-length hem. Crafted from premium stretch crepe for effortless movement and a flawless fit.",
                Brand = "Clothify Luxe", BasePrice = 149.99m, CategoryId = 8,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1550639525-c97d455acf70?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Little Black Dress" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1566174053879-31528523f8ae?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "LBD styled" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "WMN-LBD-BLK-XS", Size = "XS", Color = "Black", StockQuantity = 10 },
                    new() { SKU = "WMN-LBD-BLK-S", Size = "S", Color = "Black", StockQuantity = 18 },
                    new() { SKU = "WMN-LBD-BLK-M", Size = "M", Color = "Black", StockQuantity = 22 },
                    new() { SKU = "WMN-LBD-BLK-L", Size = "L", Color = "Black", StockQuantity = 14 },
                }
            },
            new()
            {
                Name = "Summer Maxi Dress", Slug = "summer-maxi-dress",
                Description = "Effortlessly elegant maxi dress perfect for warm-weather occasions. Lightweight fabric with a tiered skirt, adjustable spaghetti straps, and a smocked bodice for easy wear.",
                Brand = "Clothify", BasePrice = 99.99m, DiscountPrice = 74.99m, CategoryId = 8,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Maxi Dress" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1515372039744-b8f02a3ae446?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Maxi Dress styled" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "WMN-MAX-WHT-S", Size = "S", Color = "White", StockQuantity = 16 },
                    new() { SKU = "WMN-MAX-WHT-M", Size = "M", Color = "White", StockQuantity = 20 },
                    new() { SKU = "WMN-MAX-BLU-S", Size = "S", Color = "Blue", StockQuantity = 14 },
                    new() { SKU = "WMN-MAX-BLU-M", Size = "M", Color = "Blue", StockQuantity = 18 },
                }
            },

            // ══════ WOMEN - BOTTOMS (CategoryId: 7) ══════
            new()
            {
                Name = "High Waist Wide Leg Trousers", Slug = "high-waist-wide-leg-trousers",
                Description = "Tailored wide-leg trousers with a flattering high waist. The fluid drape creates an elegant silhouette. Features side zip closure, front pleats, and pressed creases.",
                Brand = "Clothify", BasePrice = 89.99m, CategoryId = 7,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1594633312681-425c7b97ccd1?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Wide Leg Trousers" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "WMN-WLG-BLK-XS", Size = "XS", Color = "Black", StockQuantity = 15 },
                    new() { SKU = "WMN-WLG-BLK-S", Size = "S", Color = "Black", StockQuantity = 22 },
                    new() { SKU = "WMN-WLG-BLK-M", Size = "M", Color = "Black", StockQuantity = 20 },
                    new() { SKU = "WMN-WLG-BEG-S", Size = "S", Color = "Beige", StockQuantity = 18 },
                    new() { SKU = "WMN-WLG-BEG-M", Size = "M", Color = "Beige", StockQuantity = 16 },
                }
            },

            // ══════ KIDS (CategoryId: 3) ══════
            new()
            {
                Name = "Kids Colorful Striped Tee", Slug = "kids-colorful-striped-tee",
                Description = "Fun and vibrant striped t-shirt for active kids. Made from 100% soft organic cotton that's gentle on skin. Pre-washed for extra softness.",
                Brand = "Clothify Kids", BasePrice = 24.99m, CategoryId = 3,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1519238263530-99bdd11df2ea?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Kids Striped Tee" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "KID-TEE-STR-4", Size = "S", Color = "Red", StockQuantity = 30 },
                    new() { SKU = "KID-TEE-STR-6", Size = "M", Color = "Red", StockQuantity = 35 },
                    new() { SKU = "KID-TEE-STR-8", Size = "L", Color = "Red", StockQuantity = 28 },
                    new() { SKU = "KID-TEE-BLU-6", Size = "M", Color = "Blue", StockQuantity = 25 },
                }
            },
            new()
            {
                Name = "Kids Denim Overalls", Slug = "kids-denim-overalls",
                Description = "Adorable denim overalls made for everyday adventures. Durable cotton denim with adjustable straps, multiple pockets, and snap leg openings for easy changes.",
                Brand = "Clothify Kids", BasePrice = 44.99m, DiscountPrice = 34.99m, CategoryId = 3,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1543854589-fdd4d8e6b68e?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Kids Overalls" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "KID-OVR-BLU-4", Size = "S", Color = "Blue", StockQuantity = 20 },
                    new() { SKU = "KID-OVR-BLU-6", Size = "M", Color = "Blue", StockQuantity = 25 },
                    new() { SKU = "KID-OVR-BLU-8", Size = "L", Color = "Blue", StockQuantity = 18 },
                }
            },

            // ══════ ACCESSORIES (CategoryId: 4) ══════
            new()
            {
                Name = "Leather Crossbody Bag", Slug = "leather-crossbody-bag",
                Description = "Minimalist crossbody bag in genuine full-grain leather. Features an adjustable strap, magnetic snap closure, interior card slots, and a zip pocket for essentials.",
                Brand = "Clothify Accessories", BasePrice = 139.99m, CategoryId = 4,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Leather Crossbody Bag" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1590874103328-eac38a683ce7?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Bag interior" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "ACC-BAG-BRN-OS", Size = "OS", Color = "Brown", StockQuantity = 20 },
                    new() { SKU = "ACC-BAG-BLK-OS", Size = "OS", Color = "Black", StockQuantity = 25 },
                    new() { SKU = "ACC-BAG-TAN-OS", Size = "OS", Color = "Beige", StockQuantity = 15 },
                }
            },
            new()
            {
                Name = "Classic Aviator Sunglasses", Slug = "classic-aviator-sunglasses",
                Description = "Timeless aviator sunglasses with UV400 protection lenses and a lightweight metal frame. Includes a premium leather case and microfiber cleaning cloth.",
                Brand = "Clothify Accessories", BasePrice = 59.99m, DiscountPrice = 44.99m, CategoryId = 4,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1511499767150-a48a237f0083?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Aviator Sunglasses" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "ACC-SUN-GLD-OS", Size = "OS", Color = "Gold", StockQuantity = 40 },
                    new() { SKU = "ACC-SUN-SLV-OS", Size = "OS", Color = "Grey", StockQuantity = 35 },
                    new() { SKU = "ACC-SUN-BLK-OS", Size = "OS", Color = "Black", StockQuantity = 30 },
                }
            },
            new()
            {
                Name = "Wool Blend Fedora Hat", Slug = "wool-blend-fedora-hat",
                Description = "Stylish wool-blend fedora with a grosgrain ribbon band. The structured brim and pinched crown add a sophisticated touch to any outfit.",
                Brand = "Clothify Accessories", BasePrice = 49.99m, CategoryId = 4,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1514327605112-b887c0e61c0a?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Fedora Hat" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "ACC-HAT-BLK-SM", Size = "S", Color = "Black", StockQuantity = 20 },
                    new() { SKU = "ACC-HAT-BLK-ML", Size = "M", Color = "Black", StockQuantity = 25 },
                    new() { SKU = "ACC-HAT-BEG-SM", Size = "S", Color = "Beige", StockQuantity = 15 },
                    new() { SKU = "ACC-HAT-BEG-ML", Size = "M", Color = "Beige", StockQuantity = 18 },
                }
            },

            // ══════ SHOES (CategoryId: 10) ══════
            new()
            {
                Name = "White Leather Sneakers", Slug = "white-leather-sneakers",
                Description = "Clean and minimal white leather sneakers with a cushioned insole for all-day comfort. Features a rubber cupsole, padded collar, and subtle logo debossing.",
                Brand = "Clothify Footwear", BasePrice = 119.99m, CategoryId = 10,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1549298916-b41d501d3772?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "White Sneakers" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1525966222134-fcfa99b8ae77?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Sneakers side" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "SHO-SNK-WHT-7", Size = "S", Color = "White", StockQuantity = 15 },
                    new() { SKU = "SHO-SNK-WHT-8", Size = "M", Color = "White", StockQuantity = 25 },
                    new() { SKU = "SHO-SNK-WHT-9", Size = "L", Color = "White", StockQuantity = 30 },
                    new() { SKU = "SHO-SNK-WHT-10", Size = "XL", Color = "White", StockQuantity = 20 },
                    new() { SKU = "SHO-SNK-BLK-8", Size = "M", Color = "Black", StockQuantity = 18 },
                    new() { SKU = "SHO-SNK-BLK-9", Size = "L", Color = "Black", StockQuantity = 22 },
                }
            },
            new()
            {
                Name = "Chelsea Boots - Suede", Slug = "chelsea-boots-suede",
                Description = "Classic Chelsea boots in premium suede with elastic side panels and a pull tab for easy on/off. Leather-lined interior with a durable rubber outsole.",
                Brand = "Clothify Footwear", BasePrice = 159.99m, DiscountPrice = 129.99m, CategoryId = 10,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1638247025967-b4e38f787b76?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Chelsea Boots" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1608256246200-53e635b5b65f?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Boots detail" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "SHO-CHE-BRN-8", Size = "M", Color = "Brown", StockQuantity = 12 },
                    new() { SKU = "SHO-CHE-BRN-9", Size = "L", Color = "Brown", StockQuantity = 18 },
                    new() { SKU = "SHO-CHE-BRN-10", Size = "XL", Color = "Brown", StockQuantity = 10 },
                    new() { SKU = "SHO-CHE-BLK-9", Size = "L", Color = "Black", StockQuantity = 14 },
                    new() { SKU = "SHO-CHE-BLK-10", Size = "XL", Color = "Black", StockQuantity = 8 },
                }
            },

            // ══════ SALE (CategoryId: 5) ══════
            new()
            {
                Name = "Linen Blazer - Relaxed Fit", Slug = "linen-blazer-relaxed-fit",
                Description = "Breathable linen blazer with a relaxed, unstructured silhouette. Patch pockets, natural shell buttons, and a half-lined interior make this the perfect summer layering piece.",
                Brand = "Clothify Premium", BasePrice = 179.99m, DiscountPrice = 99.99m, CategoryId = 5,
                IsFeatured = true, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1507679799987-c73779587ccf?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Linen Blazer" },
                    new() { ImageUrl = "https://images.unsplash.com/photo-1593030761757-71fae45fa0e7?w=600&h=800&fit=crop", DisplayOrder = 1, AltText = "Blazer styled" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "SAL-BLZ-BEG-S", Size = "S", Color = "Beige", StockQuantity = 8 },
                    new() { SKU = "SAL-BLZ-BEG-M", Size = "M", Color = "Beige", StockQuantity = 12 },
                    new() { SKU = "SAL-BLZ-BEG-L", Size = "L", Color = "Beige", StockQuantity = 10 },
                    new() { SKU = "SAL-BLZ-NAV-M", Size = "M", Color = "Navy", StockQuantity = 6 },
                    new() { SKU = "SAL-BLZ-NAV-L", Size = "L", Color = "Navy", StockQuantity = 4 },
                }
            },
            new()
            {
                Name = "Cashmere Scarf", Slug = "cashmere-scarf",
                Description = "Luxuriously soft 100% cashmere scarf in a generous size. Lightweight yet warm, with delicately fringed edges. A versatile accessory for all seasons.",
                Brand = "Clothify Luxe", BasePrice = 129.99m, DiscountPrice = 79.99m, CategoryId = 5,
                IsFeatured = false, IsActive = true,
                Images = new List<ProductImage>
                {
                    new() { ImageUrl = "https://images.unsplash.com/photo-1520903920243-00d872a2d1c9?w=600&h=800&fit=crop", DisplayOrder = 0, IsMain = true, AltText = "Cashmere Scarf" },
                },
                Variants = new List<ProductVariant>
                {
                    new() { SKU = "SAL-SCF-GRY-OS", Size = "OS", Color = "Grey", StockQuantity = 3 },
                    new() { SKU = "SAL-SCF-BEG-OS", Size = "OS", Color = "Beige", StockQuantity = 2 },
                    new() { SKU = "SAL-SCF-BLK-OS", Size = "OS", Color = "Black", StockQuantity = 5 },
                }
            },
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // ─── REVIEWS ───
        var allProducts = await context.Products.ToListAsync();
        var reviewTexts = new[]
        {
            ("Perfect fit!", "Exactly what I was looking for. Great quality fabric and the fit is spot on. Will definitely order more.", 5),
            ("Love the quality", "The material feels premium and holds up well after multiple washes. Very happy with this purchase.", 5),
            ("Good value", "Nice piece for the price. Slightly larger than expected but still looks great.", 4),
            ("Exceeded expectations", "The color is even better in person. Shipping was fast and packaging was eco-friendly.", 5),
            ("Decent purchase", "Good quality overall but the color was slightly different from the photos. Still keeping it.", 3),
            ("My new favorite", "I've worn this almost every week since buying it. So comfortable and versatile.", 5),
            ("Nice but runs small", "Beautiful item but I'd recommend sizing up. The fabric doesn't stretch much.", 4),
            ("Great everyday piece", "Simple, clean design that goes with everything. Exactly what my wardrobe needed.", 4),
            ("Impressive quality", "For this price point, the construction and materials are outstanding. No loose threads.", 5),
            ("Would buy again", "Comfortable, stylish, and well-made. Already eyeing more colors.", 4),
        };

        var reviews = new List<Review>();
        foreach (var product in allProducts)
        {
            var numReviews = Random.Shared.Next(2, 5);
            for (int i = 0; i < numReviews; i++)
            {
                var (title, body, rating) = reviewTexts[Random.Shared.Next(reviewTexts.Length)];
                var customer = customers[Random.Shared.Next(customers.Count)];
                reviews.Add(new Review
                {
                    UserId = customer.Id,
                    ProductId = product.Id,
                    Rating = rating,
                    Title = title,
                    Body = body,
                    IsVerifiedPurchase = Random.Shared.Next(2) == 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 90))
                });
            }
        }
        await context.Reviews.AddRangeAsync(reviews);

        // ─── SAMPLE ORDERS ───
        var addresses = new List<Address>();
        var addressData = new[]
        {
            ("Sarah Johnson", "742 Fashion Ave", "Apt 4B", "New York", "NY", "10018"),
            ("Michael Chen", "1250 Market St", null, "San Francisco", "CA", "94103"),
            ("Emily Davis", "820 Michigan Ave", "Suite 100", "Chicago", "IL", "60611"),
        };

        for (int i = 0; i < addressData.Length; i++)
        {
            var (name, street1, street2, city, state, zip) = addressData[i];
            addresses.Add(new Address
            {
                UserId = customers[i].Id,
                FullName = name,
                StreetLine1 = street1,
                StreetLine2 = street2,
                City = city,
                State = state,
                ZipCode = zip,
                Country = "US",
                Phone = "+1-555-0200",
                IsDefault = true
            });
        }
        await context.Addresses.AddRangeAsync(addresses);
        await context.SaveChangesAsync();

        var savedAddresses = await context.Addresses.ToListAsync();
        var allVariants = await context.ProductVariants.Include(v => v.Product).ToListAsync();

        var orders = new List<Order>();
        var statuses = new[] { OrderStatus.Delivered, OrderStatus.Shipped, OrderStatus.Processing, OrderStatus.Delivered, OrderStatus.OutForDelivery };

        for (int i = 0; i < 8; i++)
        {
            var customerIdx = i % customers.Count;
            var customer = customers[customerIdx];
            var address = savedAddresses.FirstOrDefault(a => a.UserId == customer.Id) ?? savedAddresses.First();

            var orderVariants = allVariants.OrderBy(_ => Random.Shared.Next()).Take(Random.Shared.Next(1, 4)).ToList();
            var items = orderVariants.Select(v => new OrderItem
            {
                ProductVariantId = v.Id,
                ProductName = v.Product.Name,
                Size = v.Size,
                Color = v.Color,
                ImageUrl = v.Product.Images.FirstOrDefault()?.ImageUrl,
                Quantity = Random.Shared.Next(1, 3),
                UnitPrice = v.PriceOverride ?? v.Product.DiscountPrice ?? v.Product.BasePrice,
                Total = (v.PriceOverride ?? v.Product.DiscountPrice ?? v.Product.BasePrice) * Random.Shared.Next(1, 3)
            }).ToList();

            var subtotal = items.Sum(x => x.Total);
            var status = statuses[i % statuses.Length];
            var placedDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 45));

            orders.Add(new Order
            {
                UserId = customer.Id,
                OrderNumber = $"CLT-{placedDate:yyyyMMdd}-{(i + 1):D4}",
                Status = status,
                ShippingAddressId = address.Id,
                ShippingMethod = ShippingMethod.Standard,
                PaymentMethod = PaymentMethod.CreditCard,
                Subtotal = subtotal,
                ShippingCost = subtotal >= 50 ? 0 : 5.99m,
                Discount = 0,
                Tax = subtotal * 0.08m,
                Total = subtotal + (subtotal >= 50 ? 0 : 5.99m) + (subtotal * 0.08m),
                PlacedAt = placedDate,
                ShippedAt = status >= OrderStatus.Shipped ? placedDate.AddDays(2) : null,
                DeliveredAt = status == OrderStatus.Delivered ? placedDate.AddDays(6) : null,
                EstimatedDeliveryDate = placedDate.AddDays(7),
                TrackingNumber = status >= OrderStatus.Shipped ? $"1Z{Random.Shared.Next(100000000, 999999999)}" : null,
                CarrierName = status >= OrderStatus.Shipped ? "UPS" : null,
                Items = items
            });
        }

        await context.Orders.AddRangeAsync(orders);

        // ─── COUPONS ───
        var coupons = new List<Coupon>
        {
            new() { Code = "WELCOME20", DiscountType = DiscountType.Percentage, DiscountValue = 20, MinOrderAmount = 50, MaxUses = 1000, ExpiresAt = DateTime.UtcNow.AddMonths(3) },
            new() { Code = "SAVE10", DiscountType = DiscountType.Fixed, DiscountValue = 10, MinOrderAmount = 75, MaxUses = 500, ExpiresAt = DateTime.UtcNow.AddMonths(1) },
            new() { Code = "SPRING30", DiscountType = DiscountType.Percentage, DiscountValue = 30, MinOrderAmount = 100, MaxUses = 200, ExpiresAt = DateTime.UtcNow.AddDays(14) },
        };
        await context.Coupons.AddRangeAsync(coupons);

        await context.SaveChangesAsync();
    }
}
