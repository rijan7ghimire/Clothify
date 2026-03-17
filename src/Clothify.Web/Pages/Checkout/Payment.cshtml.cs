using Clothify.Application.Services;
using Clothify.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Checkout;

[Authorize]
public class PaymentModel : PageModel
{
    private readonly IOrderService _orderService;

    public PaymentModel(IOrderService orderService) => _orderService = orderService;

    [BindProperty(SupportsGet = true)] public int OrderId { get; set; }
    [BindProperty(SupportsGet = true)] public string Method { get; set; } = "Khalti";
    public decimal Amount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var order = await _orderService.GetOrderAsync(OrderId);
            Amount = order.Total;
            return Page();
        }
        catch
        {
            return RedirectToPage("/Home/Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(int orderId, string method)
    {
        // Simulate payment success — mark order as Confirmed
        try
        {
            await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Confirmed);
        }
        catch { }

        return RedirectToPage("/Orders/Confirmation", new { id = orderId });
    }
}
