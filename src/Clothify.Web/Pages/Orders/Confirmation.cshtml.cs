using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Orders;

public class ConfirmationModel : PageModel
{
    private readonly IOrderService _orderService;

    public ConfirmationModel(IOrderService orderService) => _orderService = orderService;

    public OrderResponse? Order { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try { Order = await _orderService.GetOrderAsync(id); }
        catch { }
        return Page();
    }
}
