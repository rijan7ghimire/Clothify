using System.ComponentModel.DataAnnotations;
using Clothify.Core.Enums;

namespace Clothify.Application.DTOs.Request;

public class PlaceOrderRequest
{
    public int? ShippingAddressId { get; set; }
    public AddressRequest? NewAddress { get; set; }

    [Required]
    public ShippingMethod ShippingMethod { get; set; }

    [Required]
    public PaymentMethod PaymentMethod { get; set; }

    public string? CouponCode { get; set; }
    public string? PaymentToken { get; set; }
}

public class AddressRequest
{
    [Required] public string FullName { get; set; } = string.Empty;
    [Required] public string StreetLine1 { get; set; } = string.Empty;
    public string? StreetLine2 { get; set; }
    [Required] public string City { get; set; } = string.Empty;
    [Required] public string State { get; set; } = string.Empty;
    [Required] public string ZipCode { get; set; } = string.Empty;
    [Required] public string Country { get; set; } = string.Empty;
    [Required] public string Phone { get; set; } = string.Empty;
    public bool SaveAddress { get; set; }
    public bool IsDefault { get; set; }
}
