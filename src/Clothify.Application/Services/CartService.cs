using AutoMapper;
using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Core.Entities;
using Clothify.Core.Exceptions;
using Clothify.Core.Interfaces;

namespace Clothify.Application.Services;

public interface ICartService
{
    Task<CartResponse> GetCartAsync(string userId);
    Task<CartResponse> AddToCartAsync(string userId, AddToCartRequest request);
    Task<CartResponse> UpdateCartItemAsync(string userId, int itemId, int quantity);
    Task RemoveCartItemAsync(string userId, int itemId);
    Task ClearCartAsync(string userId);
    Task<CartResponse> ApplyCouponAsync(string userId, string code);
    Task<CartResponse> RemoveCouponAsync(string userId);
}

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CartService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CartResponse> GetCartAsync(string userId)
    {
        var cart = await _unitOfWork.Cart.GetOrCreateCartAsync(userId);
        return MapCartResponse(cart);
    }

    public async Task<CartResponse> AddToCartAsync(string userId, AddToCartRequest request)
    {
        var cart = await _unitOfWork.Cart.GetOrCreateCartAsync(userId);

        var variant = await _unitOfWork.Repository<ProductVariant>().GetByIdAsync(request.ProductVariantId);
        if (variant == null) throw new NotFoundException("ProductVariant", request.ProductVariantId);
        if (variant.StockQuantity < request.Quantity)
            throw new BadRequestException("Insufficient stock");

        var existingItem = await _unitOfWork.Cart.GetCartItemAsync(cart.Id, request.ProductVariantId);
        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
            await _unitOfWork.Cart.UpdateCartItemAsync(existingItem);
        }
        else
        {
            await _unitOfWork.Cart.AddCartItemAsync(new CartItem
            {
                CartId = cart.Id,
                ProductVariantId = request.ProductVariantId,
                Quantity = request.Quantity
            });
        }

        await _unitOfWork.SaveChangesAsync();
        return await GetCartAsync(userId);
    }

    public async Task<CartResponse> UpdateCartItemAsync(string userId, int itemId, int quantity)
    {
        var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
        if (cart == null) throw new NotFoundException("Cart", userId);

        var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null) throw new NotFoundException("CartItem", itemId);

        item.Quantity = quantity;
        await _unitOfWork.Cart.UpdateCartItemAsync(item);
        await _unitOfWork.SaveChangesAsync();
        return await GetCartAsync(userId);
    }

    public async Task RemoveCartItemAsync(string userId, int itemId)
    {
        await _unitOfWork.Cart.RemoveCartItemAsync(itemId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
        if (cart != null)
        {
            await _unitOfWork.Cart.ClearCartAsync(cart.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<CartResponse> ApplyCouponAsync(string userId, string code)
    {
        var coupon = (await _unitOfWork.Repository<Coupon>()
            .FindAsync(c => c.Code == code && c.IsActive && c.ExpiresAt > DateTime.UtcNow))
            .FirstOrDefault();

        if (coupon == null) throw new BadRequestException("Invalid coupon code");
        if (coupon.CurrentUses >= coupon.MaxUses) throw new BadRequestException("Coupon has been fully redeemed");

        return await GetCartAsync(userId);
    }

    public async Task<CartResponse> RemoveCouponAsync(string userId)
    {
        return await GetCartAsync(userId);
    }

    private CartResponse MapCartResponse(Cart cart)
    {
        var items = _mapper.Map<List<CartItemResponse>>(cart.Items);
        var subtotal = items.Sum(i => i.Subtotal);

        return new CartResponse
        {
            Id = cart.Id,
            Items = items,
            Subtotal = subtotal,
            ShippingCost = subtotal >= 50 ? 0 : 5.99m,
            Tax = subtotal * 0.08m,
            Total = subtotal + (subtotal >= 50 ? 0 : 5.99m) + (subtotal * 0.08m),
            ItemCount = items.Sum(i => i.Quantity)
        };
    }
}
