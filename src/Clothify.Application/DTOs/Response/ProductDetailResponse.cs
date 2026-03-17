namespace Clothify.Application.DTOs.Response;

public class ProductDetailResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public bool IsOnSale { get; set; }
    public int? DiscountPercent { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public List<ProductImageResponse> Images { get; set; } = new();
    public List<ProductVariantResponse> Variants { get; set; } = new();
    public List<ReviewResponse> RecentReviews { get; set; } = new();
}

public class ProductImageResponse
{
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public bool IsMain { get; set; }
}

public class ProductVariantResponse
{
    public int Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal? PriceOverride { get; set; }
}

public class ReviewResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public bool IsVerifiedPurchase { get; set; }
    public DateTime CreatedAt { get; set; }
}
