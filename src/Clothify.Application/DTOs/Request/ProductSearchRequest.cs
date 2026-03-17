namespace Clothify.Application.DTOs.Request;

public class ProductSearchRequest
{
    public string? Query { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string[]? Sizes { get; set; }
    public string[]? Colors { get; set; }
    public string[]? Brands { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
