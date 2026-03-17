namespace Clothify.Core.Entities;

public class Wishlist
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}
