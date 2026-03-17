using System.ComponentModel.DataAnnotations;

namespace Clothify.Application.DTOs.Request;

public class AddToCartRequest
{
    [Required]
    public int ProductVariantId { get; set; }

    [Required, Range(1, 10)]
    public int Quantity { get; set; } = 1;
}
