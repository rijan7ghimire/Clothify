using Clothify.Core.Entities;

namespace Clothify.Core.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count);
    Task<IEnumerable<Product>> GetNewArrivalsAsync(int count);
    Task<Product?> GetProductWithDetailsAsync(int id);
    Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(
        string? searchTerm,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        string[]? sizes,
        string[]? colors,
        string[]? brands,
        string? sortBy,
        int page,
        int pageSize);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId, int page, int pageSize);
    Task<IEnumerable<Product>> GetRelatedProductsAsync(int productId, int count);
    Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count, DateTime from, DateTime to);
    Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
}
