using Clothify.Application.DTOs.Request;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService) => _productService = productService;

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] ProductSearchRequest request)
    {
        var result = await _productService.SearchProductsAsync(request);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var result = await _productService.GetProductDetailAsync(id);
        return Ok(result);
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured([FromQuery] int count = 8)
    {
        var result = await _productService.GetFeaturedProductsAsync(count);
        return Ok(result);
    }

    [HttpGet("new-arrivals")]
    public async Task<IActionResult> GetNewArrivals([FromQuery] int count = 10)
    {
        var result = await _productService.GetNewArrivalsAsync(count);
        return Ok(result);
    }
}
