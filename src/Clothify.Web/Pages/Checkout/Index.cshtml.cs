using System.Security.Claims;
using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
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
    private readonly ICartService _cartService;

    public CheckoutModel(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }

    public CartResponse? Cart { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        Cart = await _cartService.GetCartAsync(userId);
        if (Cart == null || !Cart.Items.Any())
            return RedirectToPage("/Cart/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        string fullName, string streetLine1, string? streetLine2,
        string city, string state, string zipCode, string phone,
        string shippingMethod, string paymentMethod)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
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
                    Country = "Nepal",
                    Phone = phone,
                    SaveAddress = true
                },
                ShippingMethod = Enum.TryParse<ShippingMethod>(shippingMethod, out var sm) ? sm : ShippingMethod.Standard,
                PaymentMethod = paymentMethod == "Khalti" ? PaymentMethod.PayPal :
                                paymentMethod == "eSewa" ? PaymentMethod.GooglePay :
                                PaymentMethod.CreditCard
            };

            var order = await _orderService.PlaceOrderAsync(userId, request);

            // For Khalti/eSewa, redirect to payment simulation page
            if (paymentMethod is "Khalti" or "eSewa")
            {
                return RedirectToPage("/Checkout/Payment", new { orderId = order.Id, method = paymentMethod });
            }

            // For COD, go directly to confirmation
            return RedirectToPage("/Orders/Confirmation", new { id = order.Id });
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            Cart = await _cartService.GetCartAsync(userId);
            return Page();
        }
    }
}
