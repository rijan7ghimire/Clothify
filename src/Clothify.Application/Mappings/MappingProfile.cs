using AutoMapper;
using Clothify.Application.DTOs.Response;
using Clothify.Core.Entities;

namespace Clothify.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<ApplicationUser, UserResponse>();

        // Category
        CreateMap<Category, CategoryResponse>();

        // Product List
        CreateMap<Product, ProductListResponse>()
            .ForMember(d => d.Price, o => o.MapFrom(s => s.DiscountPrice ?? s.BasePrice))
            .ForMember(d => d.OriginalPrice, o => o.MapFrom(s => s.DiscountPrice.HasValue ? s.BasePrice : (decimal?)null))
            .ForMember(d => d.IsOnSale, o => o.MapFrom(s => s.DiscountPrice.HasValue))
            .ForMember(d => d.DiscountPercent, o => o.MapFrom(s =>
                s.DiscountPrice.HasValue ? (int)((1 - s.DiscountPrice.Value / s.BasePrice) * 100) : (int?)null))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s =>
                s.Images.FirstOrDefault(i => i.IsMain) != null ? s.Images.First(i => i.IsMain).ImageUrl : null))
            .ForMember(d => d.Rating, o => o.MapFrom(s =>
                s.Reviews.Any() ? s.Reviews.Average(r => r.Rating) : (double?)null))
            .ForMember(d => d.ReviewCount, o => o.MapFrom(s => s.Reviews.Count));

        // Product Detail
        CreateMap<Product, ProductDetailResponse>()
            .ForMember(d => d.Price, o => o.MapFrom(s => s.DiscountPrice ?? s.BasePrice))
            .ForMember(d => d.OriginalPrice, o => o.MapFrom(s => s.DiscountPrice.HasValue ? s.BasePrice : (decimal?)null))
            .ForMember(d => d.IsOnSale, o => o.MapFrom(s => s.DiscountPrice.HasValue))
            .ForMember(d => d.DiscountPercent, o => o.MapFrom(s =>
                s.DiscountPrice.HasValue ? (int)((1 - s.DiscountPrice.Value / s.BasePrice) * 100) : (int?)null))
            .ForMember(d => d.AverageRating, o => o.MapFrom(s =>
                s.Reviews.Any() ? s.Reviews.Average(r => r.Rating) : 0))
            .ForMember(d => d.ReviewCount, o => o.MapFrom(s => s.Reviews.Count))
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.RecentReviews, o => o.MapFrom(s => s.Reviews));

        CreateMap<ProductImage, ProductImageResponse>();
        CreateMap<ProductVariant, ProductVariantResponse>();
        CreateMap<Review, ReviewResponse>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"));

        // Cart
        CreateMap<CartItem, CartItemResponse>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductVariant.Product.Name))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s =>
                s.ProductVariant.Product.Images.FirstOrDefault(i => i.IsMain) != null
                    ? s.ProductVariant.Product.Images.First(i => i.IsMain).ImageUrl : null))
            .ForMember(d => d.Size, o => o.MapFrom(s => s.ProductVariant.Size))
            .ForMember(d => d.Color, o => o.MapFrom(s => s.ProductVariant.Color))
            .ForMember(d => d.UnitPrice, o => o.MapFrom(s =>
                s.ProductVariant.PriceOverride ?? s.ProductVariant.Product.DiscountPrice ?? s.ProductVariant.Product.BasePrice))
            .ForMember(d => d.Subtotal, o => o.MapFrom(s =>
                (s.ProductVariant.PriceOverride ?? s.ProductVariant.Product.DiscountPrice ?? s.ProductVariant.Product.BasePrice) * s.Quantity));

        // Order
        CreateMap<Order, OrderResponse>()
            .ForMember(d => d.StatusDisplay, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.ItemCount, o => o.MapFrom(s => s.Items.Sum(i => i.Quantity)));

        CreateMap<OrderItem, OrderItemResponse>();

        // Address
        CreateMap<Address, AddressResponse>();
    }
}

public class AddressResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string StreetLine1 { get; set; } = string.Empty;
    public string? StreetLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsDefault { get; set; }
}
