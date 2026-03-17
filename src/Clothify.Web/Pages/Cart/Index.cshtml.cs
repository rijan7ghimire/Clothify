using System.Security.Claims;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Cart;

public class CartModel : PageModel
{
    private readonly ICartService _cartService;

    public CartModel(ICartService cartService) => _cartService = cartService;

    public CartResponse? Cart { get; set; }

    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            Cart = await _cartService.GetCartAsync(userId);
        }
    }
}
