using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Home;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService) => _productService = productService;

    public List<CategoryResponse> Categories { get; set; } = new();
    public List<ProductListResponse> FeaturedProducts { get; set; } = new();
    public List<ProductListResponse> NewArrivals { get; set; } = new();

    public async Task OnGetAsync()
    {
        Categories = await _productService.GetCategoriesAsync();
        FeaturedProducts = await _productService.GetFeaturedProductsAsync(8);
        NewArrivals = await _productService.GetNewArrivalsAsync(10);
    }
}
