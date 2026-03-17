using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Orders;

public class TrackModel : PageModel
{
    private readonly IOrderService _orderService;

    public TrackModel(IOrderService orderService) => _orderService = orderService;

    public OrderTrackingResponse? Tracking { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try { Tracking = await _orderService.GetOrderTrackingAsync(id); }
        catch { }
        return Page();
    }
}
