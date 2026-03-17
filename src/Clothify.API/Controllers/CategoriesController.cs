using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IProductService _productService;

    public CategoriesController(IProductService productService) => _productService = productService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productService.GetCategoriesAsync();
        return Ok(result);
    }
}
