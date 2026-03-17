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

        if (await context.Products.AnyAsync()) return;

        // ─── USERS ───
        var admin = new ApplicationUser
        {
            FirstName = "Admin", LastName = "Clothify",
            Email = "admin@clothify.com", UserName = "admin@clothify.com",
            EmailConfirmed = true, PhoneNumber = "+1-555-0100"
        };
        await userManager.CreateAsync(admin, "Admin@123!");
        await userManager.AddToRoleAsync(admin, "Admin");

        var customers = new List<ApplicationUser>();
        var customerData = new[]
        {
            ("Sarah", "Johnson", "sarah@example.com"),
            ("Michael", "Chen", "michael@example.com"),
            ("Emily", "Davis", "emily@example.com"),
            ("James", "Wilson", "james@example.com"),
            ("Olivia", "Martinez", "olivia@example.com"),
        };
        foreach (var (first, last, email) in customerData)
        {
            var user = new ApplicationUser
            {
                FirstName = first, LastName = last, Email = email,
                UserName = email, EmailConfirmed = true,
                PhoneNumber = $"+1-555-{Random.Shared.Next(1000, 9999)}"
            };
            await userManager.CreateAsync(user, "Customer@123!");
            await userManager.AddToRoleAsync(user, "Customer");
            customers.Add(user);
        }

        // ─── FIND CSV ───
        var csvPath = FindCsvPath();
        if (csvPath == null)
        {
            Console.WriteLine("WARNING: fashion.csv not found. Skipping product seeding.");
            return;
        }
        Console.WriteLine($"Seeding products from: {csvPath}");

        // Category mapping: (Gender, SubCategory) → CategoryId
        var categoryMap = new Dictionary<(string Gender, string SubCategory), int>
        {
            [("Boys", "Topwear")] = 10, [("Boys", "Bottomwear")] = 11,
            [("Boys", "Apparel Set")] = 12, [("Boys", "Innerwear")] = 13, [("Boys", "Socks")] = 13,
            [("Girls", "Topwear")] = 20, [("Girls", "Bottomwear")] = 21,
            [("Girls", "Dress")] = 22, [("Girls", "Apparel Set")] = 23,
            [("Girls", "Innerwear")] = 24, [("Girls", "Socks")] = 24,
            [("Men", "Shoes")] = 30, [("Men", "Sandal")] = 31, [("Men", "Flip Flops")] = 32,
            [("Women", "Shoes")] = 40, [("Women", "Sandal")] = 41, [("Women", "Flip Flops")] = 42,
        };

        // Map Women Heels into Women Shoes (Id 40) — or use 43 if you added a Heels category
        // We'll catch "Shoes" subcategory which includes Heels, Flats, etc.

        var lines = await File.ReadAllLinesAsync(csvPath);
        var products = new List<Product>();
        var usedSlugs = new HashSet<string>();
        var rand = Random.Shared;
        int skipped = 0;

        foreach (var line in lines.Skip(1))
        {
            var parts = ParseCsvLine(line);
            if (parts.Length < 9) { skipped++; continue; }

            var productId = parts[0].Trim();
            var gender = parts[1].Trim();
            var subCategory = parts[3].Trim();
            var productType = parts[4].Trim();
            var colour = parts[5].Trim();
            var usage = parts[6].Trim();
            var productTitle = parts[7].Trim();
            var imageFile = parts[8].Trim();

            if (!categoryMap.TryGetValue((gender, subCategory), out var categoryId))
            { skipped++; continue; }

            // Unique slug
            var baseSlug = GenerateSlug(productTitle);
            var slug = baseSlug;
            int sc = 1;
            while (usedSlugs.Contains(slug)) slug = $"{baseSlug}-{sc++}";
            usedSlugs.Add(slug);

            // Price in Nepali Rupees (NPR)
            var basePrice = (gender, subCategory) switch
            {
                ("Men", "Shoes") => rand.Next(3500, 18000),
                ("Women", "Shoes") => rand.Next(2500, 15000),
                (_, "Sandal") => rand.Next(1500, 6000),
                (_, "Flip Flops") => rand.Next(500, 3000),
                (_, "Dress") => rand.Next(1800, 8000),
                (_, "Topwear") => rand.Next(800, 4500),
                (_, "Bottomwear") => rand.Next(1200, 5500),
                (_, "Apparel Set") => rand.Next(2500, 7000),
                _ => rand.Next(1000, 5000),
            } + 0m;

            decimal? discountPrice = rand.NextDouble() < 0.3
                ? Math.Round(basePrice * (1 - rand.Next(10, 40) / 100m), 2)
                : null;

            var product = new Product
            {
                Name = productTitle,
                Slug = slug,
                Description = $"Stylish {colour.ToLower()} {productType.ToLower()} designed for {gender.ToLower()}. " +
                              $"This {usage.ToLower()} wear piece features premium quality materials and a comfortable fit. " +
                              $"Perfect for everyday wear with a modern, trendy look.",
                Brand = ExtractBrand(productTitle),
                BasePrice = basePrice,
                DiscountPrice = discountPrice,
                CategoryId = categoryId,
                IsFeatured = rand.NextDouble() < 0.10,
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-rand.Next(1, 180)),
                Images = new List<ProductImage>
                {
                    new()
                    {
                        ImageUrl = $"/images/products/{imageFile}",
                        DisplayOrder = 0, IsMain = true, AltText = productTitle
                    }
                },
                Variants = GenerateVariants(productId, colour, gender, subCategory, rand)
            };
            products.Add(product);
        }

        // Batch insert
        for (int i = 0; i < products.Count; i += 200)
        {
            var batch = products.Skip(i).Take(200).ToList();
            await context.Products.AddRangeAsync(batch);
            await context.SaveChangesAsync();
            Console.WriteLine($"  Seeded {Math.Min(i + 200, products.Count)}/{products.Count} products...");
        }
        Console.WriteLine($"Seeded {products.Count} products ({skipped} skipped).");

        // ─── REVIEWS (for first 500 products) ───
        var reviewProducts = await context.Products.Take(500).ToListAsync();
        var reviewTexts = new[]
        {
            ("Perfect fit!", "Exactly what I was looking for. Great quality and fit.", 5),
            ("Love it!", "Material feels premium. Color matches the photo perfectly.", 5),
            ("Good value", "Nice piece for the price. Slightly larger than expected.", 4),
            ("Exceeded expectations", "Even better in person. Fast shipping too!", 5),
            ("My new favorite", "Been wearing this non-stop. So comfortable!", 5),
            ("Runs small", "Beautiful item but recommend sizing up one size.", 4),
            ("Great everyday piece", "Simple design that goes with everything.", 4),
            ("Impressive quality", "Outstanding construction for this price point.", 5),
            ("Would buy again", "Comfortable, stylish, and well-made.", 4),
            ("Not bad", "Average quality. Expected a bit more for the price.", 3),
            ("Fantastic!", "Best purchase this season. Gets so many compliments!", 5),
            ("Decent", "Good quality overall. Color slightly different from photos.", 3),
        };
        var reviews = new List<Review>();
        foreach (var p in reviewProducts)
        {
            for (int i = 0; i < rand.Next(1, 5); i++)
            {
                var (title, body, rating) = reviewTexts[rand.Next(reviewTexts.Length)];
                reviews.Add(new Review
                {
                    UserId = customers[rand.Next(customers.Count)].Id,
                    ProductId = p.Id, Rating = rating, Title = title, Body = body,
                    IsVerifiedPurchase = rand.Next(2) == 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-rand.Next(1, 120))
                });
            }
        }
        await context.Reviews.AddRangeAsync(reviews);

        // ─── ADDRESSES ───
        await context.Addresses.AddRangeAsync(
            new Address { UserId = customers[0].Id, FullName = "Sarah Johnson", StreetLine1 = "742 Fashion Ave", StreetLine2 = "Apt 4B", City = "New York", State = "NY", ZipCode = "10018", Country = "US", Phone = "+1-555-0200", IsDefault = true },
            new Address { UserId = customers[1].Id, FullName = "Michael Chen", StreetLine1 = "1250 Market St", City = "San Francisco", State = "CA", ZipCode = "94103", Country = "US", Phone = "+1-555-0300", IsDefault = true },
            new Address { UserId = customers[2].Id, FullName = "Emily Davis", StreetLine1 = "820 Michigan Ave", City = "Chicago", State = "IL", ZipCode = "60611", Country = "US", Phone = "+1-555-0400", IsDefault = true }
        );
        await context.SaveChangesAsync();

        // ─── ORDERS ───
        var savedAddresses = await context.Addresses.ToListAsync();
        var sampleVariants = await context.ProductVariants.Include(v => v.Product).ThenInclude(p => p.Images).Take(50).ToListAsync();
        var statuses = new[] { OrderStatus.Delivered, OrderStatus.Shipped, OrderStatus.Processing, OrderStatus.Delivered, OrderStatus.OutForDelivery };

        for (int i = 0; i < 12; i++)
        {
            var customer = customers[i % customers.Count];
            var address = savedAddresses.FirstOrDefault(a => a.UserId == customer.Id) ?? savedAddresses.First();
            var orderVariants = sampleVariants.OrderBy(_ => rand.Next()).Take(rand.Next(1, 4)).ToList();
            var items = orderVariants.Select(v => {
                var qty = rand.Next(1, 3);
                var price = v.PriceOverride ?? v.Product.DiscountPrice ?? v.Product.BasePrice;
                return new OrderItem { ProductVariantId = v.Id, ProductName = v.Product.Name, Size = v.Size, Color = v.Color, ImageUrl = v.Product.Images.FirstOrDefault()?.ImageUrl, Quantity = qty, UnitPrice = price, Total = price * qty };
            }).ToList();
            var subtotal = items.Sum(x => x.Total);
            var status = statuses[i % statuses.Length];
            var placedDate = DateTime.UtcNow.AddDays(-rand.Next(1, 60));

            context.Orders.Add(new Order
            {
                UserId = customer.Id, OrderNumber = $"CLT-{placedDate:yyyyMMdd}-{(i + 1):D4}",
                Status = status, ShippingAddressId = address.Id,
                ShippingMethod = ShippingMethod.Standard, PaymentMethod = PaymentMethod.CreditCard,
                Subtotal = subtotal, ShippingCost = subtotal >= 5000 ? 0 : 150m, Discount = 0,
                Tax = Math.Round(subtotal * 0.13m, 2),
                Total = Math.Round(subtotal + (subtotal >= 5000 ? 0 : 150m) + subtotal * 0.13m, 2),
                PlacedAt = placedDate,
                ShippedAt = status >= OrderStatus.Shipped ? placedDate.AddDays(2) : null,
                DeliveredAt = status == OrderStatus.Delivered ? placedDate.AddDays(6) : null,
                EstimatedDeliveryDate = placedDate.AddDays(7),
                TrackingNumber = status >= OrderStatus.Shipped ? $"1Z{rand.Next(100000000, 999999999)}" : null,
                CarrierName = status >= OrderStatus.Shipped ? "UPS" : null,
                Items = items
            });
        }

        // ─── COUPONS ───
        await context.Coupons.AddRangeAsync(
            new Coupon { Code = "WELCOME20", DiscountType = DiscountType.Percentage, DiscountValue = 20, MinOrderAmount = 3000, MaxUses = 1000, ExpiresAt = DateTime.UtcNow.AddMonths(3) },
            new Coupon { Code = "SAVE500", DiscountType = DiscountType.Fixed, DiscountValue = 500, MinOrderAmount = 5000, MaxUses = 500, ExpiresAt = DateTime.UtcNow.AddMonths(1) },
            new Coupon { Code = "SPRING30", DiscountType = DiscountType.Percentage, DiscountValue = 30, MinOrderAmount = 8000, MaxUses = 200, ExpiresAt = DateTime.UtcNow.AddDays(14) }
        );
        await context.SaveChangesAsync();
        Console.WriteLine("Seeding complete!");
    }

    private static string? FindCsvPath()
    {
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, "data", "fashion.csv");
            if (File.Exists(candidate)) return candidate;
            candidate = Path.Combine(dir.FullName, "fashion.csv");
            if (File.Exists(candidate)) return candidate;
            dir = dir.Parent;
        }
        return null;
    }

    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        var current = new System.Text.StringBuilder();
        foreach (char c in line)
        {
            if (c == '"') inQuotes = !inQuotes;
            else if (c == ',' && !inQuotes) { result.Add(current.ToString()); current.Clear(); }
            else current.Append(c);
        }
        result.Add(current.ToString());
        return result.ToArray();
    }

    private static string GenerateSlug(string title)
    {
        var slug = title.ToLower().Replace("&", "and").Replace("'", "").Replace("\"", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-").Trim('-');
        return slug.Length > 100 ? slug[..100] : slug;
    }

    private static string ExtractBrand(string title)
    {
        var brands = new[] { "Gini and Jony", "Allen Solly", "Palm Tree", "Disney Kids", "Doodle Kids", "Do u speak", "Little Miss", "Nike", "Adidas", "Puma", "Reebok", "Sparx", "Bata", "Catwalk", "Metro", "Woodland", "Red Tape", "Lee Cooper", "Lotto", "Fila", "Skechers", "Crocs", "United Colors of Benetton", "612 League", "Lilliput", "Beebay", "Mothercare" };
        foreach (var b in brands)
            if (title.StartsWith(b, StringComparison.OrdinalIgnoreCase)) return b;
        var words = title.Split(' ');
        return words.Length >= 2 ? $"{words[0]} {words[1]}".Replace("Girls", "").Replace("Boys", "").Replace("Men's", "").Replace("Women's", "").Trim() : "Clothify";
    }

    private static List<ProductVariant> GenerateVariants(string productId, string colour, string gender, string subCategory, Random rand)
    {
        var variants = new List<ProductVariant>();
        var isFootwear = gender is "Men" or "Women";
        var sizeSet = isFootwear ? new[] { "6", "7", "8", "9", "10", "11" } : new[] { "S", "M", "L", "XL" };
        var selectedSizes = sizeSet.OrderBy(_ => rand.Next()).Take(rand.Next(2, Math.Min(5, sizeSet.Length + 1))).ToArray();
        var prefix = $"{gender[..1]}{subCategory[..Math.Min(2, subCategory.Length)]}".ToUpper();

        foreach (var size in selectedSizes)
        {
            variants.Add(new ProductVariant
            {
                SKU = $"{prefix}-{productId}-{colour[..Math.Min(3, colour.Length)].ToUpper()}-{size}",
                Size = size, Color = colour,
                StockQuantity = rand.Next(3, 50)
            });
        }
        return variants;
    }
}
