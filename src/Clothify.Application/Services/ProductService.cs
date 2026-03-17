using AutoMapper;
using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Core.Entities;
using Clothify.Core.Exceptions;
using Clothify.Core.Interfaces;

namespace Clothify.Application.Services;

public interface IProductService
{
    Task<ProductDetailResponse> GetProductDetailAsync(int id);
    Task<List<ProductListResponse>> GetFeaturedProductsAsync(int count = 8);
    Task<List<ProductListResponse>> GetNewArrivalsAsync(int count = 10);
    Task<PagedResponse<ProductListResponse>> SearchProductsAsync(ProductSearchRequest request);
    Task<List<CategoryResponse>> GetCategoriesAsync();
    Task<ProductDetailResponse> CreateProductAsync(AdminCreateProductRequest request);
    Task UpdateProductAsync(int id, AdminCreateProductRequest request);
    Task DeleteProductAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDetailResponse> GetProductDetailAsync(int id)
    {
        var product = await _unitOfWork.Products.GetProductWithDetailsAsync(id);
        if (product == null) throw new NotFoundException(nameof(Product), id);
        return _mapper.Map<ProductDetailResponse>(product);
    }

    public async Task<List<ProductListResponse>> GetFeaturedProductsAsync(int count = 8)
    {
        var products = await _unitOfWork.Products.GetFeaturedProductsAsync(count);
        return _mapper.Map<List<ProductListResponse>>(products);
    }

    public async Task<List<ProductListResponse>> GetNewArrivalsAsync(int count = 10)
    {
        var products = await _unitOfWork.Products.GetNewArrivalsAsync(count);
        return _mapper.Map<List<ProductListResponse>>(products);
    }

    public async Task<PagedResponse<ProductListResponse>> SearchProductsAsync(ProductSearchRequest request)
    {
        var (products, totalCount) = await _unitOfWork.Products.SearchProductsAsync(
            request.Query, request.CategoryId, request.MinPrice, request.MaxPrice,
            request.Sizes, request.Colors, request.Brands,
            request.SortBy, request.Page, request.PageSize);

        return new PagedResponse<ProductListResponse>
        {
            Items = _mapper.Map<List<ProductListResponse>>(products),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<List<CategoryResponse>> GetCategoriesAsync()
    {
        var categories = await _unitOfWork.Repository<Category>()
            .FindAsync(c => c.ParentCategoryId == null);
        return _mapper.Map<List<CategoryResponse>>(categories);
    }

    public async Task<ProductDetailResponse> CreateProductAsync(AdminCreateProductRequest request)
    {
        var slug = request.Name.ToLower().Replace(" ", "-");
        var product = new Product
        {
            Name = request.Name,
            Slug = slug,
            Description = request.Description,
            Brand = request.Brand,
            BasePrice = request.BasePrice,
            DiscountPrice = request.DiscountPrice,
            CategoryId = request.CategoryId,
            IsFeatured = request.IsFeatured
        };

        foreach (var v in request.Variants)
        {
            product.Variants.Add(new ProductVariant
            {
                SKU = v.SKU, Size = v.Size, Color = v.Color,
                StockQuantity = v.StockQuantity, PriceOverride = v.PriceOverride
            });
        }

        for (int i = 0; i < request.ImageUrls.Count; i++)
        {
            product.Images.Add(new ProductImage
            {
                ImageUrl = request.ImageUrls[i],
                DisplayOrder = i,
                IsMain = i == 0
            });
        }

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ProductDetailResponse>(product);
    }

    public async Task UpdateProductAsync(int id, AdminCreateProductRequest request)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) throw new NotFoundException(nameof(Product), id);

        product.Name = request.Name;
        product.Slug = request.Name.ToLower().Replace(" ", "-");
        product.Description = request.Description;
        product.Brand = request.Brand;
        product.BasePrice = request.BasePrice;
        product.DiscountPrice = request.DiscountPrice;
        product.CategoryId = request.CategoryId;
        product.IsFeatured = request.IsFeatured;
        product.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null) throw new NotFoundException(nameof(Product), id);
        product.IsActive = false;
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }
}
