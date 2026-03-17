using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Products;

public class DetailModel : PageModel
{
    private readonly IProductService _productService;

    public DetailModel(IProductService productService) => _productService = productService;

    public ProductDetailResponse? Product { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Product = await _productService.GetProductDetailAsync(id);
            return Page();
        }
        catch
        {
            return RedirectToPage("/Products/Index");
        }
    }
}
