using System.Security.Claims;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Orders;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IOrderService _orderService;

    public IndexModel(IOrderService orderService) => _orderService = orderService;

    public PagedResponse<OrderResponse> Orders { get; set; } = new();

    public async Task OnGetAsync([FromQuery] int page = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        Orders = await _orderService.GetUserOrdersAsync(userId, page, 10);
    }
}
