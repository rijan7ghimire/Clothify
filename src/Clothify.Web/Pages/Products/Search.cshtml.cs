using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Products;

public class SearchModel : PageModel
{
    private readonly IProductService _productService;

    public SearchModel(IProductService productService) => _productService = productService;

    [BindProperty(SupportsGet = true)] public string? Query { get; set; }
    public PagedResponse<ProductListResponse> Results { get; set; } = new();

    public async Task OnGetAsync([FromQuery(Name = "q")] string? q)
    {
        Query = q;
        if (!string.IsNullOrWhiteSpace(q))
        {
            Results = await _productService.SearchProductsAsync(new ProductSearchRequest { Query = q });
        }
    }
}
