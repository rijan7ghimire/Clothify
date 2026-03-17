using System.Security.Claims;
using Clothify.Application.DTOs.Request;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService) => _orderService = orderService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        var result = await _orderService.PlaceOrderAsync(UserId, request);
        return Created($"/api/orders/{result.Id}", result);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _orderService.GetUserOrdersAsync(UserId, page, pageSize));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrder(int id)
        => Ok(await _orderService.GetOrderAsync(id));

    [HttpGet("{id:int}/tracking")]
    public async Task<IActionResult> GetTracking(int id)
        => Ok(await _orderService.GetOrderTrackingAsync(id));

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        await _orderService.CancelOrderAsync(id, UserId);
        return Ok(new { message = "Order cancelled successfully" });
    }
}
