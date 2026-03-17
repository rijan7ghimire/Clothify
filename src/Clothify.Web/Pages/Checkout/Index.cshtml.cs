using System.Security.Claims;
using Clothify.Application.DTOs.Request;
using Clothify.Application.Services;
using Clothify.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Checkout;

[Authorize]
public class CheckoutModel : PageModel
{
    private readonly IOrderService _orderService;

    public CheckoutModel(IOrderService orderService) => _orderService = orderService;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync(
        string fullName, string streetLine1, string? streetLine2,
        string city, string state, string zipCode, string phone,
        string shippingMethod)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var request = new PlaceOrderRequest
        {
            NewAddress = new AddressRequest
            {
                FullName = fullName,
                StreetLine1 = streetLine1,
                StreetLine2 = streetLine2,
                City = city,
                State = state,
                ZipCode = zipCode,
                Country = "US",
                Phone = phone
            },
            ShippingMethod = Enum.Parse<ShippingMethod>(shippingMethod),
            PaymentMethod = PaymentMethod.CreditCard
        };

        var order = await _orderService.PlaceOrderAsync(userId, request);
        return RedirectToPage("/Orders/Confirmation", new { id = order.Id });
    }
}
