namespace Clothify.Application.DTOs.Response;

public class ProductListResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsOnSale { get; set; }
    public int? DiscountPercent { get; set; }
    public double? Rating { get; set; }
    public int ReviewCount { get; set; }
}
