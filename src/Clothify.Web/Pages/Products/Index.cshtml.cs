using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Products;

public class ProductListModel : PageModel
{
    private readonly IProductService _productService;

    public ProductListModel(IProductService productService) => _productService = productService;

    public PagedResponse<ProductListResponse> Products { get; set; } = new();
    public string? CategoryName { get; set; }

    public async Task OnGetAsync([FromQuery] string? category, [FromQuery] string? sort,
        [FromQuery] int page = 1, [FromQuery] bool featured = false)
    {
        CategoryName = category;
        Products = await _productService.SearchProductsAsync(new ProductSearchRequest
        {
            Query = category,
            SortBy = sort,
            Page = page,
            PageSize = 20
        });
    }
}
