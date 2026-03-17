namespace Clothify.Application.DTOs.Response;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public List<CategoryResponse> SubCategories { get; set; } = new();
}
