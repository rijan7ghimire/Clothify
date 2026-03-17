using System.Security.Claims;
using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Mappings;
using Clothify.Application.Services;
using Clothify.Core.Entities;
using Clothify.Core.Enums;
using Clothify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Checkout;

[Authorize]
public class CheckoutModel : PageModel
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutModel(IOrderService orderService, ICartService cartService,
        UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _orderService = orderService;
        _cartService = cartService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public CartResponse? Cart { get; set; }
    public string? ErrorMessage { get; set; }

    // User info for autofill
    public string UserFullName { get; set; } = "";
    public string UserPhone { get; set; } = "";
    public AddressResponse? SavedAddress { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        Cart = await _cartService.GetCartAsync(userId);
        if (Cart == null || !Cart.Items.Any())
            return RedirectToPage("/Cart/Index");

        // Load user info for autofill
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            UserFullName = $"{user.FirstName} {user.LastName}".Trim();
            UserPhone = user.PhoneNumber ?? "";
        }

        // Load saved default address
        var addresses = await _unitOfWork.Repository<Address>().FindAsync(a => a.UserId == userId && a.IsDefault);
        var addr = addresses.FirstOrDefault();
        if (addr != null)
        {
            SavedAddress = new AddressResponse
            {
                FullName = addr.FullName, StreetLine1 = addr.StreetLine1,
                StreetLine2 = addr.StreetLine2, City = addr.City,
                State = addr.State, ZipCode = addr.ZipCode, Phone = addr.Phone
            };
        }

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

            if (paymentMethod is "Khalti" or "eSewa")
                return RedirectToPage("/Checkout/Payment", new { orderId = order.Id, method = paymentMethod });

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
