using Clothify.Core.Entities;
using Clothify.Core.Interfaces;
using Clothify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clothify.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context) => _context = context;

    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
        return await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.ProductVariant)
                    .ThenInclude(pv => pv.Product)
                        .ThenInclude(p => p.Images.Where(i => i.IsMain))
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart> GetOrCreateCartAsync(string userId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart != null) return cart;

        cart = new Cart { UserId = userId };
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int productVariantId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductVariantId == productVariantId);
    }

    public async Task AddCartItemAsync(CartItem item)
    {
        await _context.CartItems.AddAsync(item);
    }

    public Task UpdateCartItemAsync(CartItem item)
    {
        _context.CartItems.Update(item);
        return Task.CompletedTask;
    }

    public async Task RemoveCartItemAsync(int cartItemId)
    {
        var item = await _context.CartItems.FindAsync(cartItemId);
        if (item != null) _context.CartItems.Remove(item);
    }

    public async Task ClearCartAsync(int cartId)
    {
        var items = await _context.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
        _context.CartItems.RemoveRange(items);
    }
}
