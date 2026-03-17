using System.Security.Claims;
using Clothify.Application.DTOs.Request;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService) => _cartService = cartService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetCart()
        => Ok(await _cartService.GetCartAsync(UserId));

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddToCartRequest request)
        => Ok(await _cartService.AddToCartAsync(UserId, request));

    [HttpPut("items/{id:int}")]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] UpdateCartItemRequest request)
        => Ok(await _cartService.UpdateCartItemAsync(UserId, id, request.Quantity));

    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> RemoveItem(int id)
    {
        await _cartService.RemoveCartItemAsync(UserId, id);
        return NoContent();
    }

    [HttpPost("coupon")]
    public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponRequest request)
        => Ok(await _cartService.ApplyCouponAsync(UserId, request.Code));

    [HttpDelete("coupon")]
    public async Task<IActionResult> RemoveCoupon()
        => Ok(await _cartService.RemoveCouponAsync(UserId));
}

public class ApplyCouponRequest
{
    public string Code { get; set; } = string.Empty;
}
