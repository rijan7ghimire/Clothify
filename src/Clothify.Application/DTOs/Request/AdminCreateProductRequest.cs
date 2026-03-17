using System.ComponentModel.DataAnnotations;

namespace Clothify.Application.DTOs.Request;

public class AdminCreateProductRequest
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required, MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Required, Range(0.01, 100000)]
    public decimal BasePrice { get; set; }

    public decimal? DiscountPrice { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public bool IsFeatured { get; set; }

    public List<VariantRequest> Variants { get; set; } = new();
    public List<string> ImageUrls { get; set; } = new();
}

public class VariantRequest
{
    [Required] public string SKU { get; set; } = string.Empty;
    [Required] public string Size { get; set; } = string.Empty;
    [Required] public string Color { get; set; } = string.Empty;
    [Required, Range(0, 100000)] public int StockQuantity { get; set; }
    public decimal? PriceOverride { get; set; }
}
