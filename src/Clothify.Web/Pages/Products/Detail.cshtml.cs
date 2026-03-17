using System.Security.Claims;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Clothify.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Products;

public class DetailModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public DetailModel(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public ProductDetailResponse? Product { get; set; }
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Product = await _productService.GetProductDetailAsync(id);
        if (Product == null) return NotFound();
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int id, int variantId, int quantity = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return RedirectToPage("/Auth/Login");

        try
        {
            await _cartService.AddToCartAsync(userId, new Application.DTOs.Request.AddToCartRequest
            {
                ProductVariantId = variantId,
                Quantity = quantity
            });
            SuccessMessage = "Added to cart!";
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }

        Product = await _productService.GetProductDetailAsync(id);
        return Page();
    }

    public async Task<IActionResult> OnPostBuyNowAsync(int id, int variantId, int quantity = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return RedirectToPage("/Auth/Login");

        try
        {
            await _cartService.AddToCartAsync(userId, new Application.DTOs.Request.AddToCartRequest
            {
                ProductVariantId = variantId,
                Quantity = quantity
            });
            return RedirectToPage("/Checkout/Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Product = await _productService.GetProductDetailAsync(id);
            return Page();
        }
    }
}
