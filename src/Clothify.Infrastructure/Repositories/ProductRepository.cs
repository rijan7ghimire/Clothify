using Clothify.Core.Entities;
using Clothify.Core.Interfaces;
using Clothify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clothify.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count)
    {
        return await _dbSet
            .Where(p => p.IsFeatured && p.IsActive)
            .Include(p => p.Images.Where(i => i.IsMain))
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetNewArrivalsAsync(int count)
    {
        return await _dbSet
            .Where(p => p.IsActive)
            .Include(p => p.Images.Where(i => i.IsMain))
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
            .Include(p => p.Variants)
            .Include(p => p.Reviews.OrderByDescending(r => r.CreatedAt).Take(3))
                .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchProductsAsync(
        string? searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice,
        string[]? sizes, string[]? colors, string[]? brands,
        string? sortBy, int page, int pageSize)
    {
        var query = _dbSet.Where(p => p.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Brand.Contains(searchTerm)
                || (p.Description != null && p.Description.Contains(searchTerm)));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (minPrice.HasValue)
            query = query.Where(p => (p.DiscountPrice ?? p.BasePrice) >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => (p.DiscountPrice ?? p.BasePrice) <= maxPrice.Value);

        if (sizes?.Length > 0)
            query = query.Where(p => p.Variants.Any(v => sizes.Contains(v.Size)));

        if (colors?.Length > 0)
            query = query.Where(p => p.Variants.Any(v => colors.Contains(v.Color)));

        if (brands?.Length > 0)
            query = query.Where(p => brands.Contains(p.Brand));

        var totalCount = await query.CountAsync();

        query = sortBy switch
        {
            "price_asc" => query.OrderBy(p => p.DiscountPrice ?? p.BasePrice),
            "price_desc" => query.OrderByDescending(p => p.DiscountPrice ?? p.BasePrice),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            "popular" => query.OrderByDescending(p => p.Reviews.Count),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        var products = await query
            .Include(p => p.Images.Where(i => i.IsMain))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId, int page, int pageSize)
    {
        return await _dbSet
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .Include(p => p.Images.Where(i => i.IsMain))
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetRelatedProductsAsync(int productId, int count)
    {
        var product = await _dbSet.FindAsync(productId);
        if (product == null) return Enumerable.Empty<Product>();

        return await _dbSet
            .Where(p => p.CategoryId == product.CategoryId && p.Id != productId && p.IsActive)
            .Include(p => p.Images.Where(i => i.IsMain))
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count, DateTime from, DateTime to)
    {
        return await _context.OrderItems
            .Where(oi => oi.Order.PlacedAt >= from && oi.Order.PlacedAt <= to)
            .GroupBy(oi => oi.ProductVariant.ProductId)
            .OrderByDescending(g => g.Sum(oi => oi.Quantity))
            .Take(count)
            .Select(g => g.First().ProductVariant.Product)
            .Include(p => p.Images.Where(i => i.IsMain))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
    {
        return await _dbSet
            .Where(p => p.IsActive && p.Variants.Any(v => v.StockQuantity <= threshold))
            .Include(p => p.Variants.Where(v => v.StockQuantity <= threshold))
            .ToListAsync();
    }
}
