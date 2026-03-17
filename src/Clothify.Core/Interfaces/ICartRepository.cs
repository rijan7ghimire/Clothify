using Clothify.Core.Entities;

namespace Clothify.Core.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<Cart> GetOrCreateCartAsync(string userId);
    Task<CartItem?> GetCartItemAsync(int cartId, int productVariantId);
    Task AddCartItemAsync(CartItem item);
    Task UpdateCartItemAsync(CartItem item);
    Task RemoveCartItemAsync(int cartItemId);
    Task ClearCartAsync(int cartId);
}
