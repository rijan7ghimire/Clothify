using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Clothify.Core.Entities;
using Clothify.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Products;

public class ProductListModel : PageModel
{
    private readonly IProductService _productService;
    private readonly IUnitOfWork _unitOfWork;

    public ProductListModel(IProductService productService, IUnitOfWork unitOfWork)
    {
        _productService = productService;
        _unitOfWork = unitOfWork;
    }

    public PagedResponse<ProductListResponse> Products { get; set; } = new();
    public string? CategoryName { get; set; }
    [BindProperty(SupportsGet = true)] public string? Sort { get; set; }
    [BindProperty(SupportsGet = true)] public decimal? MinPrice { get; set; }
    [BindProperty(SupportsGet = true)] public decimal? MaxPrice { get; set; }
    [BindProperty(SupportsGet = true)] public string[]? Sizes { get; set; }
    [BindProperty(SupportsGet = true)] public string[]? Colors { get; set; }

    public async Task OnGetAsync([FromQuery] string? category, [FromQuery] string? q,
        [FromQuery] string? sort, [FromQuery] int page = 1)
    {
        Sort = sort;
        int? categoryId = null;

        // Resolve category slug to ID
        if (!string.IsNullOrEmpty(category))
        {
            var cats = await _unitOfWork.Repository<Category>().FindAsync(c =>
                c.Slug == category || c.Name.ToLower().Contains(category.ToLower()));
            var cat = cats.FirstOrDefault();
            if (cat != null)
            {
                categoryId = cat.Id;
                CategoryName = cat.Name;

                // Also include child categories
                var children = await _unitOfWork.Repository<Category>()
                    .FindAsync(c => c.ParentCategoryId == cat.Id);
                var childIds = children.Select(c => c.Id).ToList();
                if (childIds.Any())
                {
                    // Search across parent + all children
                    var allResults = new List<ProductListResponse>();
                    int totalCount = 0;

                    foreach (var id in childIds.Prepend(cat.Id))
                    {
                        var result = await _productService.SearchProductsAsync(new ProductSearchRequest
                        {
                            CategoryId = id,
                            Query = q,
                            SortBy = sort,
                            MinPrice = MinPrice,
                            MaxPrice = MaxPrice,
                            Sizes = Sizes,
                            Colors = Colors,
                            Page = 1,
                            PageSize = 200
                        });
                        allResults.AddRange(result.Items);
                        totalCount += result.TotalCount;
                    }

                    Products = new PagedResponse<ProductListResponse>
                    {
                        Items = allResults.Skip((page - 1) * 20).Take(20).ToList(),
                        TotalCount = totalCount,
                        Page = page,
                        PageSize = 20
                    };
                    return;
                }
            }
        }

        Products = await _productService.SearchProductsAsync(new ProductSearchRequest
        {
            CategoryId = categoryId,
            Query = q,
            SortBy = sort,
            MinPrice = MinPrice,
            MaxPrice = MaxPrice,
            Sizes = Sizes,
            Colors = Colors,
            Page = page,
            PageSize = 20
        });
    }
}
